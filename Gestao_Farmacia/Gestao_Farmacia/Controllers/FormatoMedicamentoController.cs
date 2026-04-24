using AutoMapper;
using Dominio.Excecoes;
using Gestao_Farmacia.Modelos.Atualizacao;
using Gestao_Farmacia.Modelos.Criacao;
using Gestao_Farmacia.Modelos.Resposta.Base;
using Interface.Negocio;
using Microsoft.AspNetCore.Mvc;
using Negocio;

namespace Gestao_Farmacia.Controllers
{
    /// <summary>
    /// Controller responsável por armazenar todos os endpoints relacionado a Formato de Medicamento.
    /// </summary>
    [ApiController, Route("api/[controller]")]
    public class FormatoMedicamentoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFormatoMedicamentoNegocio _formatoMedicamentoNegocio;
        private readonly IAutenticacaoNegocio _autenticacaoNegocio;

        public FormatoMedicamentoController(IMapper mapper, IFormatoMedicamentoNegocio formatoMedicamentoNegocio, IAutenticacaoNegocio autenticacaoNegocio)
        {
            _mapper = mapper;
            _formatoMedicamentoNegocio = formatoMedicamentoNegocio;
            _autenticacaoNegocio = autenticacaoNegocio;
        }

        /// <summary>
        /// Buscar todos os formatos de medicamentos que já foram cadastrados.
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <response code="200">Caso os dados sejam buscados com sucesso.</response>
        /// <response code="400">Caso tenha tido algum problema durante a busca dos dados.</response>
        /// <response code="401">Caso tenha tido algum problema de autenticação durante a busca dos dados.</response>
        /// <response code="500">Caso tenha tido algum problema interno no servidor durante a busca dos dados.</response>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resposta<object>>> BuscarTodosAsync()
        {
            try
            {
                if (!Request.Headers.TryGetValue("Authorization", out var header))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                var token = header.ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                bool retornoValidacaoToken = await _autenticacaoNegocio.ValidarTokenAsync(token);
                if (!retornoValidacaoToken)
                    return StatusCode(StatusCodes.Status401Unauthorized, new Resposta<object>(StatusCodes.Status401Unauthorized, "Não foi possível validar o token de validação informado."));

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Formato Medicamento", "Buscar Todos");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                var lstFormatosMedicamentos = await _formatoMedicamentoNegocio.BuscarTodosAsync();

                return StatusCode(StatusCodes.Status200OK, new Resposta<object>(StatusCodes.Status200OK, lstFormatosMedicamentos));
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

        /// <summary>
        /// Buscar o formato de medicamento pelo código.
        /// </summary>
        /// <param name="codigo">Código do formato de medicamento.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Caso os dados sejam buscados com sucesso.</response>
        /// <response code="400">Caso tenha tido algum problema durante a busca dos dados.</response>
        /// <response code="401">Caso tenha tido algum problema de autenticação durante a busca dos dados.</response>
        /// <response code="500">Caso tenha tido algum problema interno no servidor durante a busca dos dados.</response>
        [HttpGet("{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resposta<object>>> BuscarPeloCodigoAsync([FromRoute] int codigo)
        {
            try
            {
                if (codigo <= 0)
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Código não informado."));

                if (!Request.Headers.TryGetValue("Authorization", out var header))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                var token = header.ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                bool retornoValidacaoToken = await _autenticacaoNegocio.ValidarTokenAsync(token);
                if (!retornoValidacaoToken)
                    return StatusCode(StatusCodes.Status401Unauthorized, new Resposta<object>(StatusCodes.Status401Unauthorized, "Não foi possível validar o token de validação informado."));

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Formato Medicamento", "Buscar Pelo Código");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                var formatoMedicamento = await _formatoMedicamentoNegocio.BuscarPeloCodigoAsync(codigo);

                return StatusCode(StatusCodes.Status200OK, new Resposta<object>(StatusCodes.Status200OK, formatoMedicamento));
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

        /// <summary>
        /// Cadastrar um novo formato de medicamento.
        /// </summary>
        /// <param name="formatoMedicamento">Objeto que contém todos os campos necessários para realizar o cadastro de um novo formato de medicamento.</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso os dados sejam inseridos com sucesso.</response>
        /// <response code="400">Caso tenha tido algum problema durante a inserção dos dados.</response>
        /// <response code="401">Caso tenha tido algum problema de autenticação durante a inserção dos dados.</response>
        /// <response code="500">Caso tenha tido algum problema interno no servidor durante a inserção dos dados.</response>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resposta<object>>> CadastrarAsync([FromBody] CreateFormatoMedicamento formatoMedicamento)
        {
            try
            {
                if (formatoMedicamento == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Dados de cadastro do formato de medicamento não informados."));

                if (!Request.Headers.TryGetValue("Authorization", out var header))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                var token = header.ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                bool retornoValidacaoToken = await _autenticacaoNegocio.ValidarTokenAsync(token);
                if (!retornoValidacaoToken)
                    return StatusCode(StatusCodes.Status401Unauthorized, new Resposta<object>(StatusCodes.Status401Unauthorized, "Não foi possível validar o token de validação informado."));

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Formato Medicamento", "Cadastrar");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                int codigoFormatoMedicamento = await _formatoMedicamentoNegocio.CadastrarAsync(formatoMedicamento);
                if (codigoFormatoMedicamento > 0)
                    return StatusCode(StatusCodes.Status201Created, new Resposta<object>(StatusCodes.Status201Created, codigoFormatoMedicamento));

                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Algo deu errado durante a inserção do formato de medicamento."));
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

        /// <summary>
        /// Atualizar um formato de medicamento já existente no sistema.
        /// </summary>
        /// <param name="formatoMedicamento">Objeto que contém todos os campos necessários para realizar a atualização de um formato de medicamento já existente no sistema.</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Caso os dados sejam atualizados com sucesso.</response>
        /// <response code="400">Caso tenha tido algum problema durante a atualização dos dados.</response>
        /// <response code="401">Caso tenha tido algum problema de autenticação durante a atualização dos dados.</response>
        /// <response code="500">Caso tenha tido algum problema interno no servidor durante a atualização dos dados.</response>
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resposta<object>>> AtualizarAsync([FromBody] UpdateFormatoMedicamento formatoMedicamento)
        {
            try
            {
                if (formatoMedicamento == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Dados de atualização do formato de medicamento não informados."));

                if (!Request.Headers.TryGetValue("Authorization", out var header))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                var token = header.ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                bool retornoValidacaoToken = await _autenticacaoNegocio.ValidarTokenAsync(token);
                if (!retornoValidacaoToken)
                    return StatusCode(StatusCodes.Status401Unauthorized, new Resposta<object>(StatusCodes.Status401Unauthorized, "Não foi possível validar o token de validação informado."));

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Formato Medicamento", "Atualizar");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                bool retornoAtualizacaoFormatoMedicamento = await _formatoMedicamentoNegocio.AtualizarAsync(formatoMedicamento);
                if (!retornoAtualizacaoFormatoMedicamento)
                    return StatusCode(StatusCodes.Status200OK, new Resposta<object>(StatusCodes.Status200OK, retornoAtualizacaoFormatoMedicamento));

                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Algo deu errado durante a atualização do formato de medicamento."));
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

        /// <summary>
        /// Deleta um formato de medicamento já existente no sistema.
        /// </summary>
        /// <param name="codigo">Código do formato de medicamento que será excluído.</param>
        /// <param name="codigoUsuario">Código do usuário que está excluindo o formato de medicamento.</param>
        /// <returns>IActionResult</returns>
        /// <response code="204">Caso os dados sejam excluídos com sucesso.</response>
        /// <response code="400">Caso tenha tido algum problema durante a exclusão dos dados.</response>
        /// <response code="401">Caso tenha tido algum problema de autenticação durante a exclusão dos dados.</response>
        /// <response code="500">Caso tenha tido algum problema interno no servidor durante a exclusão dos dados.</response>
        [HttpDelete("{codigo}/{codigoUsuario}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Resposta<object>>> DeletarAsync([FromRoute] int codigo, [FromRoute] int codigoUsuario)
        {
            try
            {
                if (codigo <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Código do formato de medicamento não informado."));

                if (codigoUsuario <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Código do usuário não informado."));

                if (!Request.Headers.TryGetValue("Authorization", out var header))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                var token = header.ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                bool retornoValidacaoToken = await _autenticacaoNegocio.ValidarTokenAsync(token);
                if (!retornoValidacaoToken)
                    return StatusCode(StatusCodes.Status401Unauthorized, new Resposta<object>(StatusCodes.Status401Unauthorized, "Não foi possível validar o token de validação informado."));

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Formato Medicamento", "Deletar");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                bool retornoExclusaoFormatoMedicamento = await _formatoMedicamentoNegocio.DeletarAsync(codigo, codigoUsuario);
                if (!retornoExclusaoFormatoMedicamento)
                    return StatusCode(StatusCodes.Status204NoContent, new Resposta<object>(StatusCodes.Status204NoContent, string.Empty));

                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Algo deu errado durante a exclusão do formato de medicamento."));
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
