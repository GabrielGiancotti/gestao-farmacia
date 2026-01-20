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

        public AutenticacaoRepositorio(GestaoFarmaciaContexto contexto, IRepositorioBase<Entidade.TokenApi> tokenApiReposBase)
        {
            _contexto = contexto;
            _tokenApiReposBase = tokenApiReposBase;
        }

        #region Inserção
        public async Task<Dominio.UsuarioLoginResposta?> LoginAsync(string email, string senha, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Dominio.UsuarioLoginResposta usuarioRetorno = await (from u in _contexto.Usuario
                                                    where u.Email == email && u.Senha == senha && u.Ativo && u.Deletado
                                                    select new Dominio.UsuarioLoginResposta
                                                    {
                                                        Codigo = u.Codigo,
                                                        Nome = u.Nome,
                                                        Email = u.Email,
                                                        Codigo_Perfil = u.Codigo_Perfil,
                                                        Ativo = u.Ativo,
                                                        Data_Ultimo_Login = u.Data_Ultimo_Login,
                                                        Crm = u.Crm,
                                                        Chave = null,
                                                        Data_Expiracao_Chave = null
                                                    }).FirstOrDefaultAsync();

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
        #endregion
    }
}
