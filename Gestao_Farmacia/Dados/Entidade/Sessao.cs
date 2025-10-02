using Interface.Repositorio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dados.Entidade
{
    public class Sessao : EntidadeBase
    {
        public override int Codigo { get; set; }
        public int Codigo_Usuario { get; set; }
        public required string Chave { get; set; }
        public string? Ip { get; set; }
        public DateTime Data_Expiracao { get; set; }
        public override DateTime Data_Criacao { get; set; }

        [NotMapped]
        public override int Codigo_Usuario_Criacao { get; set; }
        [NotMapped]
        public override int Codigo_Usuario_Modificacao { get; set; }
        [NotMapped]
        public override DateTime Data_Modificacao { get; set; }
        [NotMapped]
        public override int Codigo_Usuario_Delecao { get; set; }
        [NotMapped]
        public override DateTime Data_Delecao { get; set; }
        [NotMapped]
        public override bool Deletado { get; set; }
        [NotMapped]
        public override string Consulta => "SELECT S.* FROM F001_Sessao S WHERE 1 = 1";
    }
}
