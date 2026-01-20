namespace Interface.Repositorio.Base
{
    public abstract class EntidadeBase
    {
        public abstract int Codigo { get; set; }
        public abstract int Codigo_Usuario_Criacao { get; set; }
        public abstract DateTime Data_Criacao { get; set; }
        public abstract int? Codigo_Usuario_Modificacao { get; set; }
        public abstract DateTime? Data_Modificacao { get; set; }
        public abstract int? Codigo_Usuario_Delecao { get; set; }
        public abstract DateTime? Data_Delecao { get; set; }
        public abstract bool Deletado { get; set; }
    }
}
