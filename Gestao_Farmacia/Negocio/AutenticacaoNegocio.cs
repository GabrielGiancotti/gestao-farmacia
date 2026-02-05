using AutoMapper;
using Dados.Contexto;
using Dominio.Enum;
using Dominio.Excecoes;
using Interface.Negocio;
using Interface.Repositorio;
using Interface.Repositorio.Base;
using Interface.Util;
using Microsoft.Extensions.Logging;

namespace Negocio
{
    public class AutenticacaoNegocio : IAutenticacaoNegocio
    {
        private GestaoFarmaciaContexto _contexto;
        private readonly IMapper _mapper;
        private readonly ILogger<AutenticacaoNegocio> _logger;
        private readonly IAutenticacaoRepositorio _autenticacaoRepositorio;
        private readonly ICriptografiaServico _criptografiaServico;
        private readonly IValidacaoServico _validacaoServico;
        private readonly IRepositorioBase<Entidade.Sessao> _sessaoReposBase;

        public AutenticacaoNegocio(GestaoFarmaciaContexto contexto, IMapper mapper, ILogger<AutenticacaoNegocio> logger, IAutenticacaoRepositorio autenticacaoRepositorio, ICriptografiaServico criptografiaServico, IValidacaoServico validacaoServico, 
            IRepositorioBase<Entidade.Sessao> sessaoReposBase)
        {
            _contexto = contexto;
            _mapper = mapper;
            _logger = logger;
            _autenticacaoRepositorio = autenticacaoRepositorio;
            _criptografiaServico = criptografiaServico;
            _validacaoServico = validacaoServico;
            _sessaoReposBase = sessaoReposBase;
        }

        #region Inserção
        public async Task<object> LoginAsync<TInput>(TInput input) where TInput : class
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "O objeto de entrada não pode ser nulo.");

            Dominio.UsuarioLogin dadosLogin = _mapper.Map<Dominio.UsuarioLogin>(input);
            if (dadosLogin == null)
                throw new ArgumentNullException(nameof(dadosLogin), "As informações para realizar o login não foram informadas.");

            object retornoValidacao = ValidarModeloLogin(dadosLogin, _contexto);
            if (retornoValidacao is string mensagemErro)
                throw new NegocioException(mensagemErro);

            try
            {
                string emailCriptografado = _criptografiaServico.GerarHash(dadosLogin.Email);
                string senhaCriptografada = _criptografiaServico.GerarHash(dadosLogin.Senha);

                var respostaLogin = await _autenticacaoRepositorio.LoginAsync(emailCriptografado, senhaCriptografada, _contexto);
                if (respostaLogin != null)
                {
                    Entidade.Sessao dadoNovaSessaoUsuario = new Entidade.Sessao()
                    {
                        Codigo = 0,
                        Codigo_Usuario = respostaLogin.Codigo,
                        Chave = Guid.NewGuid().ToString(),
                        Ip = dadosLogin.Ip,
                        Data_Expiracao = DateTime.Now.AddHours(1),
                        Codigo_Usuario_Criacao = (int)EnumUsuarioSistema.Sistema,
                        Data_Criacao = DateTime.Now
                    };

                    int codigoNovaSessaoUsuario = await _sessaoReposBase.InserirAssincrono(dadoNovaSessaoUsuario, _contexto);
                    if (codigoNovaSessaoUsuario <= 0)
                        throw new NegocioException("Ocorreu um problema durante a geração de uma nova chave da API, tente novamente mais tarde!");

                    respostaLogin.Nome = _criptografiaServico.DescriptografarTexto(respostaLogin.Nome);
                    respostaLogin.Email = _criptografiaServico.DescriptografarTexto(respostaLogin.Email);
                    respostaLogin.Chave = dadoNovaSessaoUsuario.Chave;
                    respostaLogin.Data_Expiracao_Chave = dadoNovaSessaoUsuario.Data_Expiracao;

                    return respostaLogin;
                }

                throw new NegocioException("Email e/ou senha inválidos.");
            }
            catch (NegocioException ex)
            {
                _logger.LogError(ex, $"Mensagem de erro tratado para o e-mail: {dadosLogin.Email}.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado ao realizar o login do e-mail: {dadosLogin.Email}.");
                throw new NegocioException($"Erro inesperado ao realizar o login do e-mail: {dadosLogin.Email}.", ex);
            }
            finally
            {
                _contexto.ChangeTracker.Clear();
            }
        }
        #endregion

        #region Validações
        public async Task<bool> ValidarTokenExternoAsync(string token)
        {
            try
            {
                bool resultado = await _autenticacaoRepositorio.ValidarTokenExternoAsync(token, _contexto);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new NegocioException(ex.Message, ex);
            }
        }

        private object ValidarModeloLogin(Dominio.UsuarioLogin dadosLogin, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            if (string.IsNullOrWhiteSpace(dadosLogin.Email))
                return "O atributo email não pode ser nulo ou vazio.";

            bool emailValido = _validacaoServico.EmailValido(dadosLogin.Email);
            if (!emailValido)
                return "O atributo email informado não é válido.";

            if (string.IsNullOrWhiteSpace(dadosLogin.Senha))
                return "O atributo senha não pode ser nulo ou vazio.";

            bool senhaValida = _validacaoServico.SenhaValida(dadosLogin.Senha);
            if (!senhaValida)
                return "O atributo senha deve conter no mínimo 8 caracteres, com uma letra maiúscula, uma minúscula e um número.";

            return true;
        }
        #endregion
    }
}
