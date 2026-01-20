using Aplicacao.Modelos.Criacao;
using AutoMapper;

namespace Aplicacao.Mappings
{
    public class Mapping : Profile
    {
        public Mapping() 
        {
            #region Dados -> Dominio
            CreateMap<Entidade.Usuario, Dominio.Usuario>();
            CreateMap<Entidade.Endereco, Dominio.Endereco>();
            CreateMap<Entidade.Sessao, Dominio.Sessao>();
            CreateMap<Entidade.TokenApi, Dominio.TokenApi>();
            #endregion

            #region Dominio -> Dados
            CreateMap<Dominio.Usuario, Entidade.Usuario>();
            CreateMap<Dominio.Endereco, Entidade.Endereco>();
            CreateMap<Dominio.Sessao, Entidade.Sessao>();
            CreateMap<Dominio.TokenApi, Entidade.TokenApi>();
            #endregion

            #region Dominio -> Resposta

            #endregion

            #region Criacao -> Dominio
            CreateMap<CreateUsuario, Dominio.Usuario>();
            CreateMap<CreateUsuarioExterno, Dominio.Usuario>();
            CreateMap<CreateUsuarioLogin, Dominio.UsuarioLogin>();
            #endregion

            #region Atualizacao -> Dominio

            #endregion
        }
    }
}
