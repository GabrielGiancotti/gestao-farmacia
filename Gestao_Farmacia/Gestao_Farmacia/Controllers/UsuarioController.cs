using Aplicacao.Modelos.Criacao;
using Aplicacao.Modelos.Resposta.Base;
using AutoMapper;
using Dominio.Excecoes;
using Interface.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Farmacia.Controllers
{
    /// <summary>
    /// Controller responsável por armazenar todos os endpoints relacionado a Usuário.
    /// </summary>
    [ApiController, Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IAutenticacaoNegocio _autenticacaoNegocio;

        public UsuarioController(IMapper mapper, IUsuarioNegocio usuarioNegocio, IAutenticacaoNegocio autenticacaoNegocio)
        {
            _mapper = mapper;
            _usuarioNegocio = usuarioNegocio;
            _autenticacaoNegocio = autenticacaoNegocio;
        }

        /// <summary>
        /// Cadastrar um novo usuário sem a necessidade de estar autenticado no sistema.
        /// </summary>
        /// <param name="token">Token necessário para validação.</param>
        /// <param name="usuario">Objeto que contém todos os campos necessário para realizar o cadastro externo de um novo usuário.</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso os dados sejam inseridos com sucesso.</response>
        /// <response code="400">Caso tenha tido algum problema durante a inserção dos dados.</response>
        /// <response code="401">Caso tenha tido algum problema de autenticação durante a inserção dos dados.</response>
        /// <response code="500">Caso tenha tido algum problema interno no servidor durante a inserção dos dados.</response>
        [HttpPost("externo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resposta<object>>> CadastrarExternoAsync([FromHeader] string token, [FromBody] CreateUsuarioExterno usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Token não informado."));

                if (usuario == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Dados de cadastro do usuário não informados."));

                bool respValidarSessao = await _autenticacaoNegocio.ValidarTokenExternoAsync(token);
                if (!respValidarSessao)
                    return StatusCode(StatusCodes.Status401Unauthorized, new Resposta<object>(StatusCodes.Status401Unauthorized, "Sessão não autorizada."));

                int codigoUsuarioCadastrado = await _usuarioNegocio.CadastrarExternoAsync(usuario);
                if (codigoUsuarioCadastrado > 0)
                    return StatusCode(StatusCodes.Status201Created, new Resposta<object>(StatusCodes.Status201Created, codigoUsuarioCadastrado));

                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Algo deu errado durante a inserção do usuário."));
            }
            catch (NegocioException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Resposta<object>(StatusCodes.Status500InternalServerError, "Tivemos um problema interno durante a solicitação! Favor tentar novamente."));
            }
        }
    }
}
