using AutoMapper;

namespace Gestao_Farmacia.Mappings
{
    public class Mapping : Profile
    {
        public Mapping() 
        {
            #region Dados -> Dominio
            CreateMap<Dados.Entidade.Usuario, Dominio.Usuario>();
            CreateMap<Dados.Entidade.Endereco, Dominio.Endereco>();
            CreateMap<Dados.Entidade.Sessao, Dominio.Sessao>();
            #endregion

            #region Dominio -> Dados
            CreateMap<Dominio.Usuario, Dados.Entidade.Usuario>();
            CreateMap<Dominio.Endereco, Dados.Entidade.Endereco>();
            CreateMap<Dominio.Sessao, Dados.Entidade.Sessao>();
            #endregion

            #region Dominio -> Resposta

            #endregion

            #region Criacao -> Dominio

            #endregion

            #region Atualizacao -> Dominio

            #endregion
        }
    }
}
