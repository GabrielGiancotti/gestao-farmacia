namespace Aplicacao.Modelos.Resposta
{
    /// <summary>
    /// Classe responsável pela resposta do usuário.
    /// </summary>
    public class UsuarioResposta
    {
        /// <summary>
        /// Código do usuário.
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        public required string Nome { get; set; }
        /// <summary>
        /// Data de nascimento do usuário.
        /// </summary>
        public string? Data_Nascimento { get; set; }
        /// <summary>
        /// Cpf do usuário.
        /// </summary>
        public string? Cpf { get; set; }
        /// <summary>
        /// Telefone do usuário.
        /// </summary>
        public string? Telefone { get; set; }
        /// <summary>
        /// Email do usuário.
        /// </summary>
        public required string Email { get; set; }
        /// <summary>
        /// Senha do usuário.
        /// </summary>
        public required string Senha { get; set; }
        /// <summary>
        /// Código perfil do usuário.
        /// </summary>
        public int Codigo_Perfil { get; set; }
        /// <summary>
        /// Situação do usuário no sistema.
        /// </summary>
        public bool Ativo { get; set; }
        /// <summary>
        /// Código do gênero do usuário.
        /// </summary>
        public int? Genero { get; set; }
        /// <summary>
        /// Crm do usuário.
        /// </summary>
        public string? Crm { get; set; }
        /// <summary>
        /// Data em que o usuário foi criado.
        /// </summary>
        public DateTime Data_Criacao { get; set; }
    }

    /// <summary>
    /// Objeto responsável pela resposta de login do usuário.
    /// </summary>
    public class UsuarioLoginResposta
    {
        /// <summary>
        /// Código do usuário que realizou o login.
        /// </summary>
        public int Codigo_Usuario { get; set; }
        /// <summary>
        /// Nome do usuário que realizou o login.
        /// </summary>
        public required string Nome_Usuario { get; set; }
        /// <summary>
        /// Chave(GUID) gerada na conclusão do login do usuário.
        /// </summary>
        public required string Chave { get; set; }
        /// <summary>
        /// Data de criação da chave que será utilizada pelo usuário.
        /// </summary>
        public required DateTime Data_Criacao { get; set; }
        /// <summary>
        /// Data de expiração da chave que será utilizada pelo usuário.
        /// </summary>
        public required DateTime Data_Expiracao { get; set; }
    }
}
