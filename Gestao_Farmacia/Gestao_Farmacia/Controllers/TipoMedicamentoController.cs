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
    /// Controller responsável por armazenar todos os endpoints relacionado a Tipo de Medicamento.
    /// </summary>
    [ApiController, Route("api/[controller]")]
    public class TipoMedicamentoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITipoMedicamentoNegocio _tipoMedicamentoNegocio;
        private readonly IAutenticacaoNegocio _autenticacaoNegocio;

        public TipoMedicamentoController(IMapper mapper, ITipoMedicamentoNegocio tipoMedicamentoNegocio, IAutenticacaoNegocio autenticacaoNegocio)
        {
            _mapper = mapper;
            _tipoMedicamentoNegocio = tipoMedicamentoNegocio;
            _autenticacaoNegocio = autenticacaoNegocio;
        }

        /// <summary>
        /// Buscar todos os tipos de medicamentos que já foram cadastrados.
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

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Tipo Medicamento", "Buscar Todos");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                var lstTiposMedicamentos = await _tipoMedicamentoNegocio.BuscarTodosAsync();

                return StatusCode(StatusCodes.Status200OK, new Resposta<object>(StatusCodes.Status200OK, lstTiposMedicamentos));
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
        /// Buscar o tipo de medicamento pelo código.
        /// </summary>
        /// <param name="codigo">Código do tipo de medicamento.</param>
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

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Tipo Medicamento", "Buscar Pelo Código");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                var tipoMedicamento = await _tipoMedicamentoNegocio.BuscarPeloCodigoAsync(codigo);

                return StatusCode(StatusCodes.Status200OK, new Resposta<object>(StatusCodes.Status200OK, tipoMedicamento));
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
        /// Cadastrar um novo tipo de medicamento.
        /// </summary>
        /// <param name="tipoMedicamento">Objeto que contém todos os campos necessários para realizar o cadastro de um novo tipo de medicamento.</param>
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
        public async Task<ActionResult<Resposta<object>>> CadastrarAsync([FromBody] CreateTipoMedicamento tipoMedicamento)
        {
            try
            {
                if (tipoMedicamento == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Dados de cadastro do tipo de medicamento não informados."));

                if (!Request.Headers.TryGetValue("Authorization", out var header))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                var token = header.ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                bool retornoValidacaoToken = await _autenticacaoNegocio.ValidarTokenAsync(token);
                if (!retornoValidacaoToken)
                    return StatusCode(StatusCodes.Status401Unauthorized, new Resposta<object>(StatusCodes.Status401Unauthorized, "Não foi possível validar o token de validação informado."));

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Tipo Medicamento", "Cadastrar");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                int codigoTipoMedicamento = await _tipoMedicamentoNegocio.CadastrarAsync(tipoMedicamento);
                if (codigoTipoMedicamento > 0)
                    return StatusCode(StatusCodes.Status201Created, new Resposta<object>(StatusCodes.Status201Created, codigoTipoMedicamento));

                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Algo deu errado durante a inserção do tipo de medicamento."));
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
        /// Atualizar um tipo de medicamento já existente no sistema.
        /// </summary>
        /// <param name="tipoMedicamento">Objeto que contém todos os campos necessários para realizar a atualização de um tipo de medicamento já existente no sistema.</param>
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
        public async Task<ActionResult<Resposta<object>>> AtualizarAsync([FromBody] UpdateTipoMedicamento tipoMedicamento)
        {
            try
            {
                if (tipoMedicamento == null)
                    return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Dados de atualização do tipo de medicamento não informados."));

                if (!Request.Headers.TryGetValue("Authorization", out var header))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                var token = header.ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                    return BadRequest(new Resposta<object>(StatusCodes.Status400BadRequest, "Token de validação não informado."));

                bool retornoValidacaoToken = await _autenticacaoNegocio.ValidarTokenAsync(token);
                if (!retornoValidacaoToken)
                    return StatusCode(StatusCodes.Status401Unauthorized, new Resposta<object>(StatusCodes.Status401Unauthorized, "Não foi possível validar o token de validação informado."));

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Tipo Medicamento", "Atualizar");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                bool retornoAtualizacaoTipoMedicamento = await _tipoMedicamentoNegocio.AtualizarAsync(tipoMedicamento);
                if (!retornoAtualizacaoTipoMedicamento)
                    return StatusCode(StatusCodes.Status200OK, new Resposta<object>(StatusCodes.Status200OK, retornoAtualizacaoTipoMedicamento));

                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Algo deu errado durante a atualização do tipo de medicamento."));
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
        /// Deleta um tipo de medicamento já existente no sistema.
        /// </summary>
        /// <param name="codigo">Código do tipo de medicamento que será excluído.</param>
        /// <param name="codigoUsuario">Código do usuário que está excluindo o tipo de medicamento.</param>
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
                    return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Código do tipo de medicamento não informado."));

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

                bool retornoVerificacaoPermissao = await _autenticacaoNegocio.VerificarPermissaoUsuarioAsync(token, "Tipo Medicamento", "Deletar");
                if (!retornoVerificacaoPermissao)
                    return StatusCode(StatusCodes.Status403Forbidden, new Resposta<object>(StatusCodes.Status403Forbidden, "O usuário vinculado ao token não possui permissão para acessar o recurso informado."));

                bool retornoExclusaoTipoMedicamento = await _tipoMedicamentoNegocio.DeletarAsync(codigo, codigoUsuario);
                if (!retornoExclusaoTipoMedicamento)
                    return StatusCode(StatusCodes.Status204NoContent, new Resposta<object>(StatusCodes.Status204NoContent, string.Empty));

                return StatusCode(StatusCodes.Status400BadRequest, new Resposta<object>(StatusCodes.Status400BadRequest, "Algo deu errado durante a exclusão do tipo de medicamento."));
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
