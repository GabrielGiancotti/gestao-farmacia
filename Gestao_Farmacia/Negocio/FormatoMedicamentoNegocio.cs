using AutoMapper;
using Dados.Contexto;
using Dados.Repositorio;
using Dominio.Enum;
using Dominio.Excecoes;
using Interface.Negocio;
using Interface.Repositorio;
using Interface.Repositorio.Base;
using Interface.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Negocio
{
    public class FormatoMedicamentoNegocio : IFormatoMedicamentoNegocio
    {
        private GestaoFarmaciaContexto _contexto;
        private readonly IMapper _mapper;
        private readonly ILogger<FormatoMedicamentoNegocio> _logger;
        private readonly IFormatoMedicamentoRepositorio _formatoMedicamentoRepositorio;
        private readonly IRepositorioBase<Entidade.FormatoMedicamento> _formatoMedicamentoReposBase;

        public FormatoMedicamentoNegocio(GestaoFarmaciaContexto contexto, IMapper mapper, ILogger<FormatoMedicamentoNegocio> logger, IFormatoMedicamentoRepositorio formatoMedicamentoRepositorio,
            IRepositorioBase<Entidade.FormatoMedicamento> formatoMedicamentoReposBase)
        {
            _contexto = contexto;
            _mapper = mapper;
            _logger = logger;
            _formatoMedicamentoRepositorio = formatoMedicamentoRepositorio;
            _formatoMedicamentoReposBase = formatoMedicamentoReposBase;
        }

        #region Consulta
        public async Task<List<Dominio.FormatoMedicamento>> BuscarTodosAsync()
        {
            try
            {
                List<Dominio.FormatoMedicamento> lstFormatosMedicamentos = await _formatoMedicamentoRepositorio.BuscarTodosAsync(_contexto);

                return lstFormatosMedicamentos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado ao buscar todos os formatos de medicamento.");
                throw new NegocioException($"Erro inesperado ao buscar todos os formatos de medicamento.", ex);
            }
        }

        public async Task<Dominio.FormatoMedicamento> BuscarPeloCodigoAsync(int codigo)
        {
            try
            {
                Dominio.FormatoMedicamento formatoMedicamento = await _formatoMedicamentoRepositorio.BuscarPeloCodigoAsync(codigo, _contexto);

                return formatoMedicamento;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado ao buscar o formato de medicamento pelo código.");
                throw new NegocioException($"Erro inesperado ao buscar o formato de medicamento pelo código.", ex);
            }
        }
        #endregion

        #region Inserção
        public async Task<int> CadastrarAsync<TInput>(TInput input) where TInput : class
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "O objeto de cadastro não pode ser nulo.");

            Dominio.FormatoMedicamento formatoMedicamento = _mapper.Map<Dominio.FormatoMedicamento>(input);

            object retornoValidacao = await ValidarModeloAsync(formatoMedicamento, true, _contexto);
            if (retornoValidacao is string mensagemErro)
                throw new NegocioException(mensagemErro);

            using var transacao = _contexto.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            bool sucesso = false;
            try
            {
                int codigoFormatoMedicamento = await _formatoMedicamentoRepositorio.CadastrarAsync(formatoMedicamento, _contexto);
                if (codigoFormatoMedicamento > 0)
                {
                    transacao.Commit();
                    _contexto.ChangeTracker.Clear();
                    sucesso = true;
                    return codigoFormatoMedicamento;
                }

                throw new NegocioException("Falha ao inserir os dados do formato de medicamento no banco.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Erro ao acessar o banco de dados durante o cadastro do formato de medicamento.");
                throw new NegocioException($"Erro ao acessar o banco de dados durante o cadastro do formato de medicamento.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado durante o cadastro do formato de medicamento.");
                throw new NegocioException($"Erro inesperado durante o cadastro do formato de medicamento.", ex);
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

        #region Atualização
        public async Task<bool> AtualizarAsync<TInput>(TInput input) where TInput : class
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "O objeto de atualização não pode ser nulo.");

            Dominio.FormatoMedicamento formatoMedicamento = _mapper.Map<Dominio.FormatoMedicamento>(input);

            object retornoValidacao = await ValidarModeloAsync(formatoMedicamento, false, _contexto);
            if (retornoValidacao is string mensagemErro)
                throw new NegocioException(mensagemErro);

            using var transacao = _contexto.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            bool sucesso = false;
            try
            {
                bool retornoAtualizacao = await _formatoMedicamentoRepositorio.AtualizarAsync(formatoMedicamento, _contexto);
                if (retornoAtualizacao)
                {
                    transacao.Commit();
                    _contexto.ChangeTracker.Clear();
                    sucesso = true;
                    return retornoAtualizacao;
                }

                throw new NegocioException("Falha ao atualizar os dados do formato de medicamento no banco.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Erro ao acessar o banco de dados durante a atualização do formato de medicamento.");
                throw new NegocioException($"Erro ao acessar o banco de dados durante a atualização do formato de medicamento.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado durante a atualização do formato de medicamento.");
                throw new NegocioException($"Erro inesperado durante a atualização do formato de medicamento.", ex);
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
        private async Task<object> ValidarModeloAsync(Dominio.FormatoMedicamento formatoMedicamento, bool isInsert, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            if (string.IsNullOrWhiteSpace(formatoMedicamento.Descricao))
                return "O atributo descrição não pode ser nulo ou vazio.";

            if (formatoMedicamento.Descricao.Length > 250)
                return "O atributo descrição não pode ser maior que 250 caracteres.";

            if (!isInsert)
            {
                if (formatoMedicamento.Codigo <= 0)
                    return "O atributo código não pode ser menor ou igual a 0.";

                var dadoFormatoMedicamento = await _formatoMedicamentoReposBase.BuscarPeloCodigoAssincrono(formatoMedicamento.Codigo, _contexto);
                if (dadoFormatoMedicamento == null)
                    return "Não foi possível encontrar um formato de medicamento em nossa base de dados pelo código informado.";
            }

            return true;
        }

        private async Task<object> ValidarModeloDelecaoAsync(int codigo, int codigoUsuario, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            if (codigo <= 0)
                return "O atributo código não pode ser menor ou igual a 0.";

            var dadoFormatoMedicamento = await _formatoMedicamentoReposBase.BuscarPeloCodigoAssincrono(codigo, _contexto);
            if (dadoFormatoMedicamento == null)
                return "Não foi possível encontrar um formato de medicamento em nossa base de dados pelo código informado.";

            if (codigoUsuario <= 0)
                return "O atributo código do usuário não pode ser menor ou igual a 0.";

            var dadoUsuario = await _formatoMedicamentoReposBase.BuscarPeloCodigoAssincrono(codigo, _contexto);
            if (dadoUsuario == null)
                return "Não foi possível encontrar um formato de medicamento em nossa base de dados pelo código informado.";

            return true;
        }
        #endregion

        #region Deleção
        public async Task<bool> DeletarAsync(int codigo, int codigoUsuario)
        {
            object retornoValidacao = await ValidarModeloDelecaoAsync(codigo, codigoUsuario, _contexto);
            if (retornoValidacao is string mensagemErro)
                throw new NegocioException(mensagemErro);

            using var transacao = _contexto.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            bool sucesso = false;
            try
            {
                bool retornoAtualizacao = await _formatoMedicamentoRepositorio.DeletarAsync(codigo, codigoUsuario, _contexto);
                if (retornoAtualizacao)
                {
                    transacao.Commit();
                    _contexto.ChangeTracker.Clear();
                    sucesso = true;
                    return retornoAtualizacao;
                }

                throw new NegocioException("Falha ao deletar os dados do formato de medicamento no banco.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Erro ao acessar o banco de dados durante a deleção do formato de medicamento.");
                throw new NegocioException($"Erro ao acessar o banco de dados durante a deleção do formato de medicamento.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado durante a deleção do formato de medicamento.");
                throw new NegocioException($"Erro inesperado durante a deleção do formato de medicamento.", ex);
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
    }
}
