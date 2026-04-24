using System.ComponentModel.DataAnnotations;

namespace Gestao_Farmacia.Modelos.Criacao
{
    public class CreateFormatoMedicamento
    {
        [Required(ErrorMessage = "O atributo descrição é obrigatório.")]
        [MaxLength(250, ErrorMessage = "O atributo descrição deve ter no máximo 250 caracteres.")]
        public required string Descricao { get; set; }
        [Required(ErrorMessage = "O atributo código do usuário de criação é obrigatório.")]
        public int Codigo_Usuario_Criacao { get; set; }
    }
}
