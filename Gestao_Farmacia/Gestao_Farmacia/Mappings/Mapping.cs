using Gestao_Farmacia.Modelos.Resposta;
using AutoMapper;
using Gestao_Farmacia.Modelos.Criacao;
using Gestao_Farmacia.Modelos.Atualizacao;

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
            CreateMap<Entidade.TipoMedicamento, Dominio.TipoMedicamento>();
            CreateMap<Entidade.FormatoMedicamento, Dominio.FormatoMedicamento>();
            CreateMap<Entidade.TokenApi, Dominio.TokenApi>();
            CreateMap<Entidade.Perfil, Dominio.Perfil>();
            CreateMap<Entidade.Permissao, Dominio.Permissao>();
            #endregion

            #region Dominio -> Dados
            CreateMap<Dominio.Usuario, Entidade.Usuario>();
            CreateMap<Dominio.Endereco, Entidade.Endereco>();
            CreateMap<Dominio.Sessao, Entidade.Sessao>();
            CreateMap<Dominio.TipoMedicamento, Entidade.TipoMedicamento>();
            CreateMap<Dominio.FormatoMedicamento, Entidade.FormatoMedicamento>();
            CreateMap<Dominio.TokenApi, Entidade.TokenApi>();
            CreateMap<Dominio.Perfil, Entidade.Perfil>();
            CreateMap<Dominio.Permissao, Entidade.Permissao>();
            #endregion

            #region Dominio -> Resposta
            CreateMap<Dominio.UsuarioLoginResposta, UsuarioLoginResposta>();
            CreateMap<Dominio.TipoMedicamento, TipoMedicamentoResposta>();
            CreateMap<Dominio.FormatoMedicamento, FormatoMedicamentoResposta>();
            #endregion

            #region Criacao -> Dominio
            CreateMap<CreateUsuario, Dominio.Usuario>();
            CreateMap<CreateUsuarioExterno, Dominio.Usuario>();
            CreateMap<CreateUsuarioLogin, Dominio.UsuarioLogin>();
            CreateMap<CreateTipoMedicamento, Dominio.TipoMedicamento>();
            CreateMap<CreateFormatoMedicamento, Dominio.FormatoMedicamento>();
            #endregion

            #region Atualizacao -> Dominio
            CreateMap<UpdateTipoMedicamento, Dominio.TipoMedicamento>();
            CreateMap<UpdateFormatoMedicamento, Dominio.FormatoMedicamento>();
            #endregion
        }
    }
}
