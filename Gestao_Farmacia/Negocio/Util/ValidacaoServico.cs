using Interface.Util;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Dominio.Enum;

namespace Negocio.Util
{
    /// <summary>
    /// Serviço responsável por validar dados de entrada.
    /// </summary>
    public class ValidacaoServico : IValidacaoServico
    {
        /// <summary>
        /// Método responsável por validar o CPF informado (formato e dígitos verificadores).
        /// </summary>
        public bool CpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = Regex.Replace(cpf, "[^0-9]", "");

            if (cpf.Length != 11)
                return false;

            if (new string(cpf[0], cpf.Length) == cpf)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        /// <summary>
        /// Método responsável por validar se o e-mail informado está em um formato correto.
        /// </summary>
        public bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Método responsável por validar se a data de nascimento informada está dentro da aceitável pelo sistema.
        /// </summary>
        public bool DataNascimentoValida(DateTime dataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;

            if (dataNascimento.Date > hoje.AddYears(-idade))
                idade--;

            return idade >= (int)EnumValidacaoIdadeUsuario.Minimo && idade <= (int)EnumValidacaoIdadeUsuario.Maximo;
        }

        /// <summary>
        /// Método responsável por validar se o telefone informado está em um formato correto.
        /// </summary>
        public bool TelefoneValido(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return false;

            // Permite apenas números e DDD entre parênteses, opcionalmente com traço
            const string pattern = @"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$";
            return Regex.IsMatch(telefone, pattern);
        }

        /// <summary>
        /// Método responsável por validar a senha informada (quantidade mínima de caracteres).
        /// </summary>
        public bool SenhaValida(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha) || senha.Length < (int)EnumValidacaoSenhaUsuario.MinimoCaracteres)
                return false;

            // Pelo menos uma letra maiúscula, uma minúscula, um número e um caractere especial
            const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$";
            return Regex.IsMatch(senha, pattern);
        }

        public bool ValorEnumValido<TEnum>(int valor) where TEnum : struct, System.Enum
        {
            return System.Enum.IsDefined(typeof(TEnum), valor);
        }
    }
}
