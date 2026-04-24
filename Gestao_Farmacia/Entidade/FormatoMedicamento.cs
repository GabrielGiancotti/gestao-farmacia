using Interface.Repositorio.Base;

namespace Entidade
{
    public class FormatoMedicamento : EntidadeBase
    {
        public override int Codigo { get; set; }
        public required string Descricao { get; set; }
        public override int Codigo_Usuario_Criacao { get; set; }
        public override DateTime Data_Criacao { get; set; }
        public override int? Codigo_Usuario_Modificacao { get; set; }
        public override DateTime? Data_Modificacao { get; set; }
        public override int? Codigo_Usuario_Delecao { get; set; }
        public override DateTime? Data_Delecao { get; set; }
        public override bool Deletado { get; set; }
    }
}
