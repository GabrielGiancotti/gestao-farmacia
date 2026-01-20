using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Util
{
    public interface IValidacaoServico
    {
        bool CpfValido(string cpf);
        bool EmailValido(string email);
        bool DataNascimentoValida(DateTime dataNascimento);
        bool TelefoneValido(string telefone);
        bool SenhaValida(string senha);
        bool ValorEnumValido<TEnum>(int valor) where TEnum : struct, Enum;
    }
}
