using Dominio;

namespace Interface.Repositorio
{
    public interface IFormatoMedicamentoRepositorio
    {
        #region Consulta
        Task<List<FormatoMedicamento>> BuscarTodosAsync(object? contexto = null);
        Task<FormatoMedicamento> BuscarPeloCodigoAsync(int codigo, object? contexto = null);
        #endregion

        #region Inserção
        Task<int> CadastrarAsync(FormatoMedicamento formatoMedicamento, object? contexto = null);
        #endregion

        #region Atualização
        Task<bool> AtualizarAsync(FormatoMedicamento formatoMedicamento, object? contexto = null);
        #endregion

        #region Exclusão
        Task<bool> DeletarAsync(int codigo, int codigoUsuario, object? contexto = null);
        #endregion
    }
}
