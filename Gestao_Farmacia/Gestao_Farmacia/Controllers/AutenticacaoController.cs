using AutoMapper;
using Dominio.Excecoes;
using Aplicacao.Modelos.Criacao;
using Aplicacao.Modelos.Resposta;
using Aplicacao.Modelos.Resposta.Base;
using Interface.Negocio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Gestao_Farmacia.Controllers
{
    /// <summary>
    /// Controller responsável por armazenar todos os endpoints relacionado a Autenticação.
    /// </summary>
    [ApiController, Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAutenticacaoNegocio _autenticacaoNegocio;

        public AutenticacaoController(IMapper mapper, IAutenticacaoNegocio autenticacaoNegocio)
        {
            _mapper = mapper;
            _autenticacaoNegocio = autenticacaoNegocio;
        }

        /// <summary>
        /// Válida os dados de acesso do usuário, e caso os dados estejam corretos, o login é realizado.
        /// </summary>
        /// <param name="token">Token necessário para validação.</param>
        /// <param name="dadosLogin">Objeto que contém todos os campos necessário para realizar o login de um usuário já existente.</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso os dados sejam inseridos com sucesso.</response>
        /// <response code="400">Caso tenha tido algum problema durante a inserção dos dados.</response>
        /// <response code="401">Caso tenha tido algum problema de autenticação durante a inserção dos dados.</response>
        /// <response code="500">Caso tenha tido algum problema interno no servidor durante a inserção dos dados.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resposta<object>>> LoginAsync([FromHeader] string token, [FromBody] CreateUsuarioLogin dadosLogin)
        {
            try
            {
                if (dadosLogin == null)
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Dados de login do usuário não informados."));

                var retornoLogin = await _autenticacaoNegocio.LoginAsync(dadosLogin);
                if (retornoLogin != null)
                {
                    UsuarioLoginResposta loginResposta = _mapper.Map<UsuarioLoginResposta>(retornoLogin);
                    return StatusCode(StatusCodes.Status200OK, new Resposta<object>(StatusCodes.Status200OK, loginResposta));
                }

                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Algo deu errado durante a tentativa de login."));
            }
            catch (NegocioException ex)
            {
                return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Resposta<object>(StatusCodes.Status500InternalServerError, "Tivemos um problema interno durante a solicitação! Favor tentar novamente."));
            }
        }

        /// <summary>
        /// Válida se o token externo informado é válido.
        /// </summary>
        /// <param name="token">Token necessário para validação.</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso os dados sejam inseridos com sucesso.</response>
        /// <response code="400">Caso tenha tido algum problema durante a inserção dos dados.</response>
        /// <response code="401">Caso tenha tido algum problema de autenticação durante a inserção dos dados.</response>
        /// <response code="500">Caso tenha tido algum problema interno no servidor durante a inserção dos dados.</response>
        [HttpGet("externo/validar/token")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resposta<object>>> ValidarTokenExternoAsync([FromHeader] string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                bool retornoLogin = await _autenticacaoNegocio.ValidarTokenExternoAsync(token);
                if (retornoLogin)
                    return StatusCode(StatusCodes.Status200OK, new Resposta<object>(StatusCodes.Status200OK, retornoLogin));

                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação informado não é válido."));
            }
            catch (NegocioException ex)
            {
                return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Resposta<object>(StatusCodes.Status500InternalServerError, "Tivemos um problema interno durante a solicitação! Favor tentar novamente."));
            }
        }
    }
}
