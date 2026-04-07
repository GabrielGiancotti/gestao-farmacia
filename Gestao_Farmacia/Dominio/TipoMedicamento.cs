namespace Dominio
{
    public class TipoMedicamento
    {
        public  int Codigo { get; set; }
        public required string Descricao { get; set; }
        public int Codigo_Usuario_Criacao { get; set; }
        public DateTime Data_Criacao { get; set; }
        public int? Codigo_Usuario_Modificacao { get; set; }
        public DateTime? Data_Modificacao { get; set; }
        public int? Codigo_Usuario_Delecao { get; set; }
        public DateTime? Data_Delecao { get; set; }
        public bool Deletado { get; set; }
    }
}
