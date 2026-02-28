using Interface.Repositorio.Base;

namespace Entidade
{
    public class Permissao : EntidadeBase
    {
        public override int Codigo { get; set; }
        public int Codigo_Perfil { get; set; }
        public required string Recurso { get; set; }
        public required string Acao { get; set; }
        public bool Permitido { get; set; }
        public override int Codigo_Usuario_Criacao { get; set; }
        public override DateTime Data_Criacao { get; set; }
        public override int? Codigo_Usuario_Modificacao { get; set; }
        public override DateTime? Data_Modificacao { get; set; }
        public override int? Codigo_Usuario_Delecao { get; set; }
        public override DateTime? Data_Delecao { get; set; }
        public override bool Deletado { get; set; }
    }
}
