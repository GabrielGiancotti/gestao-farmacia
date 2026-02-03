using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Usuario
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public string? Data_Nascimento { get; set; }
        public string? Cpf { get; set; }
        public string? Cpf_Hash { get; set; }
        public string? Telefone { get; set; }
        public required string Email { get; set; }
        public required string Email_Hash { get; set; }
        public required string Senha { get; set; }
        public int Codigo_Perfil { get; set; }
        public bool Ativo { get; set; }
        public int? Genero { get; set; }
        public int Tentativas_Login { get; set; }
        public DateTime? Data_Ultimo_Login { get; set; }
        public string? Crm { get; set; }
        public int Codigo_Usuario_Criacao { get; set; }
        public DateTime Data_Criacao { get; set; }
        public int? Codigo_Usuario_Modificacao { get; set; }
        public DateTime? Data_Modificacao { get; set; }
        public int? Codigo_Usuario_Delecao { get; set; }
        public DateTime? Data_Delecao { get; set; }
        public bool Deletado { get; set; }
    }

    public class UsuarioLogin
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public string? Ip { get; set; }
    }

    public class UsuarioLoginResposta
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public int Codigo_Perfil { get; set; }
        public bool Ativo { get; set; }
        public DateTime? Data_Ultimo_Login { get; set; }
        public string? Crm { get; set; }
        public string? Chave { get; set; }
        public DateTime? Data_Expiracao_Chave { get; set; }
    }
}
