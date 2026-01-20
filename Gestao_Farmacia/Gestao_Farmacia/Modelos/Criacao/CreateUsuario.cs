using System.ComponentModel.DataAnnotations;

namespace Aplicacao.Modelos.Criacao
{
    public class CreateUsuario
    {
        public required string Nome { get; set; }
        public string? Data_Nascimento { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public int Codigo_Perfil { get; set; }
        public bool Ativo { get; set; }
        public string? Genero { get; set; }
        public string? Crm { get; set; }
        public int Codigo_Usuario_Criacao { get; set; }
    }

    public class CreateUsuarioExterno
    {
        [Required(ErrorMessage = "O atributo nome é obrigatório.")]
        [MaxLength(250, ErrorMessage = "O atributo nome deve ter no máximo 250 caracteres.")]
        public required string Nome { get; set; }
        public string? Data_Nascimento { get; set; }
        [MaxLength(14, ErrorMessage = "O atributo cpf deve ter no máximo 14 caracteres.")]
        public string? Cpf { get; set; }
        [MaxLength(14, ErrorMessage = "O atributo telefone deve ter no máximo 14 caracteres.")]
        public string? Telefone { get; set; }
        [Required(ErrorMessage = "O atributo email é obrigatório.")]
        [MaxLength(250, ErrorMessage = "O atributo email deve ter no máximo 250 caracteres.")]
        [EmailAddress(ErrorMessage = "O atributo email informado é inválido.")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "O atributo senha é obrigatório.")]
        [MaxLength(250, ErrorMessage = "O atributo senha deve ter no máximo 250 caracteres.")]
        [MinLength(8, ErrorMessage = "O atributo senha deve ter no mínimo 8 caracteres.")]
        public required string Senha { get; set; }
        public int? Genero { get; set; }
    }

    public class CreateUsuarioLogin
    {
        [Required(ErrorMessage = "O atributo e-mail é obrigatório.")]
        [MaxLength(ErrorMessage = "O atributo e-mail deve ter no máximo 250 caracteres.")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "O atributo senha é obrigatório.")]
        [MaxLength(ErrorMessage = "O atributo senha deve ter no máximo 250 caracteres.")]
        public required string Senha { get; set; }
        [MaxLength(32, ErrorMessage = "O atributo nome deve ter no máximo 32 caracteres.")]
        public string? Ip { get; set; }
    }
}
