using System.ComponentModel.DataAnnotations;

namespace Gestao_Farmacia.Modelos.Atualizacao
{
    public class UpdateTipoMedicamento
    {
        [Required(ErrorMessage = "O atributo descrição é obrigatório.")]
        [MaxLength(250, ErrorMessage = "O atributo descricao deve ter no máximo 250 caracteres.")]
        public required string Descricao { get; set; }
        [Required(ErrorMessage = "O atributo código do usuário de modificação é obrigatório.")]
        public int Codigo_Usuario_Modificacao { get; set; }
    }
}
