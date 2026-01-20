using Interface.Repositorio.Base;

namespace Entidade
{
    public class Endereco : EntidadeBase
    {
        public override int Codigo { get; set; }
        public required string Logradouro { get; set; }
        public required string Numero { get; set; }
        public required string Bairro { get; set; }
        public required string Cidade { get; set; }
        public required string Estado { get; set; }
        public required string Cep { get; set; }
        public string? Complemento { get; set; }
        public override int Codigo_Usuario_Criacao { get; set; }
        public override DateTime Data_Criacao { get; set; }
        public override int? Codigo_Usuario_Modificacao { get; set; }
        public override DateTime? Data_Modificacao { get; set; }
        public override int? Codigo_Usuario_Delecao { get; set; }
        public override DateTime? Data_Delecao { get; set; }
        public override bool Deletado { get; set; }
    }
}
