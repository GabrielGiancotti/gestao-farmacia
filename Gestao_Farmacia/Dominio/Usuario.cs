﻿using System;
using System.Collections.Generic;
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
        public string? Telefone { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public int Codigo_Perfil { get; set; }
        public bool Ativo { get; set; }
        public string? Genero { get; set; }
        public int Tentativas_Login { get; set; }
        public DateTime? Data_Ultimo_Login { get; set; }
        public string? Crm { get; set; }
        public int Codigo_Usuario_Criacao { get; set; }
        public DateTime Data_Criacao { get; set; }
        public int Codigo_Usuario_Modificacao { get; set; }
        public DateTime Data_Modificacao { get; set; }
        public int Codigo_Usuario_Delecao { get; set; }
        public DateTime Data_Delecao { get; set; }
        public bool Deletado { get; set; }
    }
}
