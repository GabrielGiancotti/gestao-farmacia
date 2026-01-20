namespace Interface.Repositorio
{
    public interface IAutenticacaoRepositorio
    {
        #region Inserção
        Task<Dominio.UsuarioLoginResposta?> LoginAsync(string email, string senha, object? contexto = null);
        #endregion

        #region Validações
        Task<bool> ValidarTokenExternoAsync(string token, object? contexto = null);
        #endregion
    }
}
