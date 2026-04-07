namespace Gestao_Farmacia.Modelos.Resposta
{
    /// <summary>
    /// Classe responsável pela resposta do tipo do medicamento.
    /// </summary>
    public class TipoMedicamentoResposta
    {
        /// <summary>
        /// Código do tipo do medicamento.
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Descrição do tipo do medicamento.
        /// </summary>
        public required string Descricao { get; set; }
        /// <summary>
        /// Data em que o tipo de medicamento foi criado.
        /// </summary>
        public DateTime Data_Criacao { get; set; }
    }
}
