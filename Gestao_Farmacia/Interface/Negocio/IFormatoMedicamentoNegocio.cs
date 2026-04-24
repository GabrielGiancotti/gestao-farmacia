using Dominio;

namespace Interface.Negocio
{
    public interface IFormatoMedicamentoNegocio
    {
        #region Consulta
        Task<List<FormatoMedicamento>> BuscarTodosAsync();
        Task<FormatoMedicamento> BuscarPeloCodigoAsync(int codigo);
        #endregion

        #region Inserção
        Task<int> CadastrarAsync<TInput>(TInput input) where TInput : class;
        #endregion

        #region Atualização
        Task<bool> AtualizarAsync<TInput>(TInput input) where TInput : class;
        #endregion

        #region Exclusão
        Task<bool> DeletarAsync(int codigo, int codigoUsuario);
        #endregion
    }
}