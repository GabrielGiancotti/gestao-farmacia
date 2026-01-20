using Interface.Repositorio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidade
{
    public class TokenApi : EntidadeBase
    {
        public override int Codigo { get; set; }
        public required string Descricao { get; set; }
        public required string Chave { get; set; }
        public DateTime Data_Expiracao { get; set; }
        public override DateTime Data_Criacao { get; set; }

        [NotMapped]
        public override int Codigo_Usuario_Criacao { get; set; }
        [NotMapped]
        public override int? Codigo_Usuario_Modificacao { get; set; }
        [NotMapped]
        public override DateTime? Data_Modificacao { get; set; }
        [NotMapped]
        public override int? Codigo_Usuario_Delecao { get; set; }
        [NotMapped]
        public override DateTime? Data_Delecao { get; set; }
        [NotMapped]
        public override bool Deletado { get; set; }
    }
}
