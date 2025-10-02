using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Endereco
    {
        public int Codigo { get; set; }
        public required string Logradouro { get; set; }
        public required string Numero { get; set; }
        public required string Bairro { get; set; }
        public required string Cidade { get; set; }
        public required string Estado { get; set; }
        public required string Cep { get; set; }
        public string? Complemento { get; set; }
        public int Codigo_Usuario_Criacao { get; set; }
        public DateTime Data_Criacao { get; set; }
        public int Codigo_Usuario_Modificacao { get; set; }
        public DateTime Data_Modificacao { get; set; }
        public int Codigo_Usuario_Delecao { get; set; }
        public DateTime Data_Delecao { get; set; }
        public bool Deletado { get; set; }
    }
}
