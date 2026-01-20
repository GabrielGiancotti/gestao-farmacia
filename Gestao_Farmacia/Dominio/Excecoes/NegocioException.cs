using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Excecoes
{
    public class NegocioException : Exception
    {
        public NegocioException(string mensagem) : base(mensagem)
        {

        }

        public NegocioException(string mensagem, Exception exception) : base(mensagem, exception)
        {

        }
    }
}
