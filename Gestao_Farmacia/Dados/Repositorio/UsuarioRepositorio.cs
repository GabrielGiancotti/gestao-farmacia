using AutoMapper;
using Dados.Contexto;
using Dominio;
using Dominio.Enum;
using Interface.Repositorio;
using Interface.Repositorio.Base;

namespace Dados.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private GestaoFarmaciaContexto _contexto;
        private readonly IMapper _mapper;
        private readonly IRepositorioBase<Entidade.Usuario> _usuarioReposBase;

        public UsuarioRepositorio(GestaoFarmaciaContexto contexto, IMapper mapper, IRepositorioBase<Entidade.Usuario> usuarioReposBase)
        {
            _contexto = contexto;
            _mapper = mapper;
            _usuarioReposBase = usuarioReposBase;
        }

        #region Inserção
        public async Task<int> CadastrarExternoAsync(Usuario usuario, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Entidade.Usuario dadoUsuario = _mapper.Map<Entidade.Usuario>(usuario);
            dadoUsuario.Codigo_Perfil = (int)EnumPerfilUsuario.Usuario;
            dadoUsuario.Ativo = true;
            dadoUsuario.Tentativas_Login = 0;
            dadoUsuario.Codigo_Usuario_Criacao = (int)EnumUsuarioSistema.Sistema;

            int codigoNovoUsuario = await _usuarioReposBase.InserirAssincrono(dadoUsuario, _contexto);
            return codigoNovoUsuario;
        }
        #endregion

        #region Validações
        public async Task<bool> ExisteEmailAsync(int? codigo, string email, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            try
            {
                bool retorno = true;

                if (codigo != null && codigo.Value > 0)
                    retorno = await _usuarioReposBase.BuscarFiltradoUnicoAssincrono(u =>  u.Codigo != codigo && string.Equals(u.Email_Hash, email), _contexto) != null;
                else
                    retorno = await _usuarioReposBase.BuscarFiltradoUnicoAssincrono(u => string.Equals(u.Email_Hash, email), _contexto) != null;

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ExisteCpfAsync(int? codigo, string cpf, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            try
            {
                bool retorno = true;

                if (codigo != null && codigo.Value > 0)
                    retorno = await _usuarioReposBase.BuscarFiltradoUnicoAssincrono(u => u.Codigo != codigo && string.Equals(u.Cpf_Hash, cpf), _contexto) != null;
                else
                    retorno = await _usuarioReposBase.BuscarFiltradoUnicoAssincrono(u => string.Equals(u.Cpf_Hash, cpf), _contexto) != null;

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
