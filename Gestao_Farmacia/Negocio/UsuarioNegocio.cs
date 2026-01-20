using AutoMapper;
using Dados.Contexto;
using Dominio.Enum;
using Dominio.Excecoes;
using Interface.Negocio;
using Interface.Repositorio;
using Interface.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class UsuarioNegocio : IUsuarioNegocio
    {
        private GestaoFarmaciaContexto _contexto;
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioNegocio> _logger;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ICriptografiaServico _criptografiaServico;
        private readonly IValidacaoServico _validacaoServico;

        public UsuarioNegocio(GestaoFarmaciaContexto contexto, IMapper mapper, ILogger<UsuarioNegocio> logger, IUsuarioRepositorio usuarioRepositorio, ICriptografiaServico criptografiaServico, IValidacaoServico validacaoServico)
        {
            _contexto = contexto;
            _mapper = mapper;
            _logger = logger;
            _usuarioRepositorio = usuarioRepositorio;
            _criptografiaServico = criptografiaServico;
            _validacaoServico = validacaoServico;
        }

        #region Inserção
        public async Task<int> CadastrarExternoAsync<TInput>(TInput input) where TInput : class
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "O objeto de entrada não pode ser nulo.");

            Dominio.Usuario usuario = _mapper.Map<Dominio.Usuario>(input);

            object retornoValidacao = await ValidarModeloExternoAsync(usuario, _contexto);
            if (retornoValidacao is string mensagemErro)
                throw new NegocioException(mensagemErro);

            Dominio.Usuario usuarioTratado = TratarDadosUsuario(usuario);
            using var transacao = _contexto.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            bool sucesso = false;
            try
            {
                int codigoUsuario = await _usuarioRepositorio.CadastrarExternoAsync(usuarioTratado, _contexto);
                if (codigoUsuario > 0)
                {
                    transacao.Commit();
                    _contexto.ChangeTracker.Clear();
                    sucesso = true;
                    return codigoUsuario;
                }

                throw new NegocioException("Falha ao inserir os dados no banco.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Erro ao acessar o banco de dados durante o cadastro do usuário externo com e-mail: {usuario.Email}.");
                throw new NegocioException($"Erro ao acessar o banco de dados durante o cadastro do usuário externo com e-mail: {usuario.Email}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado ao cadastrar usuário externo com e-mail: {usuario.Email}.");
                throw new NegocioException($"Erro inesperado durante o cadastro do usuário com e-mail: {usuario.Email}.", ex);
            }
            finally
            {
                if (!sucesso)
                {
                    transacao.Rollback();
                    _contexto.ChangeTracker.Clear();
                }
            }
        }
        #endregion

        #region Validações
        public async Task<bool> ExisteEmailAsync(int? codigo, string email)
        {
            try
            {
                string emailCriptografado = _criptografiaServico.GerarHash(email);

                bool resultado = await _usuarioRepositorio.ExisteEmailAsync(codigo, emailCriptografado, _contexto);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new NegocioException(ex.Message, ex);
            }
        }

        public async Task<bool> ExisteCpfAsync(int? codigo, string cpf)
        {
            try
            {
                string cpfCriptografado = _criptografiaServico.GerarHash(cpf);

                bool resultado = await _usuarioRepositorio.ExisteCpfAsync(codigo, cpfCriptografado, _contexto);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new NegocioException(ex.Message, ex);
            }
        }

        private async Task<object> ValidarModeloExternoAsync(Dominio.Usuario usuario, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                return "O atributo nome não pode ser nulo ou vazio.";

            if (usuario.Nome.Length > 250)
                return "O atributo nome não pode ser maior que 250 caracteres.";

            if (!string.IsNullOrWhiteSpace(usuario.Data_Nascimento))
            {
                bool dataNascimentoValida = _validacaoServico.DataNascimentoValida(Convert.ToDateTime(usuario.Data_Nascimento));
                if (!dataNascimentoValida)
                    return $"Não é possível inserir o cadastro de um usuário que possui menos de {(int)EnumValidacaoIdadeUsuario.Minimo} anos ou mais que {(int)EnumValidacaoIdadeUsuario.Maximo} anos.";
            }

            if (!string.IsNullOrWhiteSpace(usuario.Cpf))
            {
                bool cpfValido = _validacaoServico.CpfValido(usuario.Cpf);
                if (!cpfValido)
                    return "O atributo cpf informado não é válido.";

                bool existeCpfUsuario = await ExisteCpfAsync(null, usuario.Cpf);
                if (existeCpfUsuario)
                    return "Não foi possível realizar o cadastro. Verifique os dados informados e tente novamente!";
            }

            if (!string.IsNullOrWhiteSpace(usuario.Telefone))
            {
                bool telefoneValido = _validacaoServico.TelefoneValido(usuario.Telefone);
                if (!telefoneValido)
                    return "O atributo telefone informado não é válido.";
            }

            if (string.IsNullOrWhiteSpace(usuario.Email))
                return "O atributo email não pode ser nulo ou vazio.";

            bool emailValido = _validacaoServico.EmailValido(usuario.Email);
            if (!emailValido)
                return "O atributo email informado não é válido.";

            bool existeEmailUsuario = await ExisteEmailAsync(null, usuario.Email);
            if (existeEmailUsuario)
                return "Não foi possível realizar o cadastro. Verifique os dados informados e tente novamente!";

            if (string.IsNullOrWhiteSpace(usuario.Senha))
                return "O atributo senha não pode ser nulo ou vazio.";

            bool senhaValida = _validacaoServico.SenhaValida(usuario.Senha);
            if (!senhaValida)
                return "O atributo senha deve conter no mínimo 8 caracteres, com uma letra maiúscula, uma minúscula e um número.";

            if (usuario.Genero != null)
            {
                bool valorEnumValido = _validacaoServico.ValorEnumValido<EnumGeneroUsuario>(usuario.Genero.Value);
                if (!valorEnumValido)
                    return "O atributo genero informado não é válido.";
            }

            return true;
        }

        private Dominio.Usuario TratarDadosUsuario(Dominio.Usuario usuario)
        {
            Dominio.Usuario usuarioTratado = new Dominio.Usuario()
            {
                Codigo = usuario.Codigo,
                Nome = _criptografiaServico.CriptografarTexto(usuario.Nome),
                Data_Nascimento = string.IsNullOrEmpty(usuario.Data_Nascimento) ? null : _criptografiaServico.CriptografarTexto(usuario.Data_Nascimento),
                Cpf = string.IsNullOrEmpty(usuario.Cpf) ? null : _criptografiaServico.CriptografarTexto(usuario.Cpf),
                Telefone = string.IsNullOrEmpty(usuario.Telefone) ? null : _criptografiaServico.CriptografarTexto(usuario.Telefone),
                Email = _criptografiaServico.CriptografarTexto(usuario.Email),
                Senha = _criptografiaServico.GerarHash(usuario.Senha),
                Codigo_Perfil = usuario.Codigo_Perfil,
                Ativo = usuario.Ativo,
                Genero = usuario.Genero,
                Tentativas_Login = usuario.Tentativas_Login,
                Data_Ultimo_Login = usuario.Data_Ultimo_Login,
                Crm = usuario.Crm,
                Codigo_Usuario_Criacao = usuario.Codigo_Usuario_Criacao,
                Data_Criacao = usuario.Data_Criacao,
                Codigo_Usuario_Modificacao = usuario.Codigo_Usuario_Modificacao,
                Data_Modificacao = usuario.Data_Modificacao,
                Codigo_Usuario_Delecao = usuario.Codigo_Usuario_Modificacao,
                Data_Delecao = usuario.Data_Delecao,
                Deletado = usuario.Deletado,
            };

            return usuarioTratado;
        }
        #endregion
    }
}
