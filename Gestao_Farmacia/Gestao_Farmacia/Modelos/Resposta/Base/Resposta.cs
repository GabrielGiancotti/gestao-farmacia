namespace Aplicacao.Modelos.Resposta.Base
{
    public class Resposta<T>
    {
        public int Status_Http { get; set; }
        public string? Mensagem_Erro { get; set; }
        public T? Dado { get; set; }

        public Resposta(int statusHttp, T dado)
        {
            Dado = dado;
            Status_Http = statusHttp;
            Mensagem_Erro = null;
        }

        public Resposta(int statusHttp, string mensagemErro) 
        {
            Status_Http = statusHttp;
            Mensagem_Erro = mensagemErro;
        }
    }
}
