namespace Interface.Repositorio
{
    public interface IUsuarioRepositorio
    {
        #region Inserção
        Task<int> CadastrarExternoAsync(Dominio.Usuario usuario, object? contexto = null);
        #endregion

        #region Validações
        Task<bool> ExisteEmailAsync(int? codigo, string email, object? contexto = null);
        Task<bool> ExisteCpfAsync(int? codigo, string cpf, object? contexto = null);
        #endregion
    }
}
