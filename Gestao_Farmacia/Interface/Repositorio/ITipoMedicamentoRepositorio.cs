using Dominio;

namespace Interface.Repositorio
{
    public interface ITipoMedicamentoRepositorio
    {
        #region Consulta
        Task<List<TipoMedicamento>> BuscarTodosAsync(object? contexto = null);
        Task<TipoMedicamento> BuscarPeloCodigoAsync(int codigo, object? contexto = null);
        #endregion

        #region Inserção
        Task<int> CadastrarAsync(TipoMedicamento tipoMedicamento, object? contexto = null);
        #endregion

        #region Atualização
        Task<bool> AtualizarAsync(TipoMedicamento tipoMedicamento, object? contexto = null);
        #endregion

        #region Exclusão
        Task<bool> DeletarAsync(int codigo, int codigoUsuario, object? contexto = null);
        #endregion
    }
}
