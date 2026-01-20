using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Util
{
    public interface ICriptografiaServico
    {
        string GerarHash(string texto);
        bool VerificarHash(string texto, string hash);
        public string CriptografarTexto(string texto);
        public string DescriptografarTexto(string texto);
    }
}
