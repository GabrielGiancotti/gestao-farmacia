using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Perfil
    {
        public  int Codigo { get; set; }
        public required string Nome { get; set; }
        public int Codigo_Usuario_Criacao { get; set; }
        public DateTime Data_Criacao { get; set; }
        public int? Codigo_Usuario_Modificacao { get; set; }
        public DateTime? Data_Modificacao { get; set; }
        public int? Codigo_Usuario_Delecao { get; set; }
        public DateTime? Data_Delecao { get; set; }
        public bool Deletado { get; set; }
    }
}
