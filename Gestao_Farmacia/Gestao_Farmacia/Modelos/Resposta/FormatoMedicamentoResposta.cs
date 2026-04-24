namespace Gestao_Farmacia.Modelos.Resposta
{
    /// <summary>
    /// Classe responsável pela resposta do formato do medicamento.
    /// </summary>
    public class FormatoMedicamentoResposta
    {
        /// <summary>
        /// Código do formato do medicamento.
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Descrição do formato do medicamento.
        /// </summary>
        public required string Descricao { get; set; }
        /// <summary>
        /// Data em que o formato de medicamento foi criado.
        /// </summary>
        public DateTime Data_Criacao { get; set; }
    }
}
