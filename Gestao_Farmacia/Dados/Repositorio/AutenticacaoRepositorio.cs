using AutoMapper;
using Dados.Contexto;
using Dominio;
using Dominio.Enum;
using Interface.Repositorio;
using Interface.Repositorio.Base;
using Interface.Util;
using Microsoft.EntityFrameworkCore;

namespace Dados.Repositorio
{
    public class AutenticacaoRepositorio : IAutenticacaoRepositorio
    {
        private GestaoFarmaciaContexto _contexto;
        private readonly IRepositorioBase<Entidade.TokenApi> _tokenApiReposBase;
        private readonly IRepositorioBase<Entidade.Usuario> _usuarioReposBase;
        private readonly IRepositorioBase<Entidade.Sessao> _sessaoReposBase;
        private readonly IRepositorioBase<Entidade.Permissao> _permissaoReposBase;

        public AutenticacaoRepositorio(GestaoFarmaciaContexto contexto, IRepositorioBase<Entidade.TokenApi> tokenApiReposBase, IRepositorioBase<Entidade.Usuario> usuarioReposBase, IRepositorioBase<Entidade.Sessao> sessaoReposBase,
            IRepositorioBase<Entidade.Permissao> permissaoReposBase)
        {
            _contexto = contexto;
            _tokenApiReposBase = tokenApiReposBase;
            _usuarioReposBase = usuarioReposBase;
            _sessaoReposBase = sessaoReposBase;
            _permissaoReposBase = permissaoReposBase;
        }

        #region Inserção
        public async Task<Dominio.UsuarioLoginResposta?> LoginAsync(string email, string senha, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Dominio.UsuarioLoginResposta usuarioRetorno = null;

            Entidade.Usuario dadoUsuario = await (from u in _contexto.Usuario
                                                  where u.Email_Hash == email && u.Senha == senha && u.Ativo && !u.Deletado
                                                  select u).FirstOrDefaultAsync();

            if (dadoUsuario == null)
                return null;

            dadoUsuario.Data_Ultimo_Login = DateTime.Now;

            bool retornoAtualizacaoUsuario = await _usuarioReposBase.AtualizarAssincrono(dadoUsuario, _contexto);
            if (!retornoAtualizacaoUsuario)
                return null;

            usuarioRetorno = new Dominio.UsuarioLoginResposta
            {
                Codigo = dadoUsuario.Codigo,
                Nome = dadoUsuario.Nome,
                Email = dadoUsuario.Email,
                Codigo_Perfil = dadoUsuario.Codigo_Perfil,
                Ativo = dadoUsuario.Ativo,
                Data_Ultimo_Login = dadoUsuario.Data_Ultimo_Login,
                Crm = dadoUsuario.Crm,
                Chave = null,
                Data_Expiracao_Chave = null
            };

            return usuarioRetorno;
        }
        #endregion

        #region Validações
        public async Task<bool> ValidarTokenExternoAsync(string token, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            try
            {
                bool retorno = false;

                var tokenValidacaoExterno = await _tokenApiReposBase.BuscarFiltradoUnicoAssincrono(t => t.Descricao.Equals("Cadastro Usuario Externo"), _contexto);
                if (tokenValidacaoExterno != null && tokenValidacaoExterno.Chave.Equals(token))
                    retorno = true;

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ValidarTokenAsync(string token, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            try
            {
                bool retorno = false;

                var tokenValidacao = await _sessaoReposBase.BuscarFiltradoUnicoAssincrono(s => s.Chave.Equals(token) && DateTime.Now <= s.Data_Expiracao, _contexto);
                if (tokenValidacao != null)
                    retorno = true;

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> VerificarPermissaoUsuarioAsync(string token, string recurso, string acao, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            try
            {
                bool retorno = false;

                var tokenValidacao = await _sessaoReposBase.BuscarFiltradoUnicoAssincrono(s => s.Chave.Equals(token) && DateTime.Now <= s.Data_Expiracao, _contexto);
                if (tokenValidacao != null)
                {
                    var usuario = await _usuarioReposBase.BuscarFiltradoUnicoAssincrono(u => u.Codigo == tokenValidacao.Codigo_Usuario, _contexto);
                    if (usuario != null)
                    {
                        var permissao = await _permissaoReposBase.BuscarFiltradoUnicoAssincrono(p => p.Codigo_Perfil == usuario.Codigo_Perfil && p.Recurso.Equals(recurso) && p.Acao.Equals(acao), _contexto);
                        if (permissao != null && permissao.Permitido)
                            retorno = true;
                    }
                }

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
