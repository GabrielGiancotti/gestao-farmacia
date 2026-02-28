using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Permissao
    {
        public  int Codigo { get; set; }
        public int Codigo_Perfil { get; set; }
        public required string Recurso { get; set; }
        public required string Acao { get; set; }
        public bool Permitido { get; set; }
        public int Codigo_Usuario_Criacao { get; set; }
        public DateTime Data_Criacao { get; set; }
        public int? Codigo_Usuario_Modificacao { get; set; }
        public DateTime? Data_Modificacao { get; set; }
        public int? Codigo_Usuario_Delecao { get; set; }
        public DateTime? Data_Delecao { get; set; }
        public bool Deletado { get; set; }
    }
}
