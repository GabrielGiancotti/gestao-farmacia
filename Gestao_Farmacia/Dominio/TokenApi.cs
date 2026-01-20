using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class TokenApi
    {
        public  int Codigo { get; set; }
        public required string Descricao { get; set; }
        public required string Chave { get; set; }
        public DateTime Data_Expiracao { get; set; }
        public DateTime Data_Criacao { get; set; }
    }
}
