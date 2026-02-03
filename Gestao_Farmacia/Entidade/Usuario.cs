using Interface.Repositorio.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidade
{
    [Table("F001_Usuario")]
    public class Usuario : EntidadeBase
    {
        public override int Codigo { get; set; }
        public required string Nome { get; set; }
        public string? Data_Nascimento { get; set; }
        public string? Cpf { get; set; }
        public string? Cpf_Hash { get; set; }
        public string? Telefone { get; set; }
        public required string Email { get; set; }
        public required string Email_Hash { get; set; }
        public required string Senha { get; set; }
        public int Codigo_Perfil { get; set; }
        public bool Ativo { get; set; }
        public int? Genero { get; set; }
        public int Tentativas_Login { get; set; }
        public DateTime? Data_Ultimo_Login { get; set; }
        public string? Crm { get; set; }
        public override int Codigo_Usuario_Criacao { get; set; }
        public override DateTime Data_Criacao { get; set; }
        public override int? Codigo_Usuario_Modificacao { get; set; }
        public override DateTime? Data_Modificacao { get; set; }
        public override int? Codigo_Usuario_Delecao { get; set; }
        public override DateTime? Data_Delecao { get; set; }
        public override bool Deletado { get; set; }
    }
}
