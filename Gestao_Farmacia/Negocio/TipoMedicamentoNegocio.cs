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
    public class TipoMedicamentoNegocio : ITipoMedicamentoNegocio
    {
        private GestaoFarmaciaContexto _contexto;
        private readonly IMapper _mapper;
        private readonly ILogger<TipoMedicamentoNegocio> _logger;
        private readonly ITipoMedicamentoRepositorio _tipoMedicamentoRepositorio;
        private readonly IRepositorioBase<Entidade.TipoMedicamento> _tipoMedicamentoReposBase;

        public TipoMedicamentoNegocio(GestaoFarmaciaContexto contexto, IMapper mapper, ILogger<TipoMedicamentoNegocio> logger, ITipoMedicamentoRepositorio tipoMedicamentoRepositorio,
            IRepositorioBase<Entidade.TipoMedicamento> tipoMedicamentoReposBase)
        {
            _contexto = contexto;
            _mapper = mapper;
            _logger = logger;
            _tipoMedicamentoRepositorio = tipoMedicamentoRepositorio;
            _tipoMedicamentoReposBase = tipoMedicamentoReposBase;
        }

        #region Consulta
        public async Task<List<Dominio.TipoMedicamento>> BuscarTodosAsync()
        {
            try
            {
                List<Dominio.TipoMedicamento> lstTiposMedicamentos = await _tipoMedicamentoRepositorio.BuscarTodosAsync(_contexto);

                return lstTiposMedicamentos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado ao buscar todos os tipos de medicamento.");
                throw new NegocioException($"Erro inesperado ao buscar todos os tipos de medicamento.", ex);
            }
        }

        public async Task<Dominio.TipoMedicamento> BuscarPeloCodigoAsync(int codigo)
        {
            try
            {
                Dominio.TipoMedicamento tipoMedicamento = await _tipoMedicamentoRepositorio.BuscarPeloCodigoAsync(codigo, _contexto);

                return tipoMedicamento;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado ao buscar o tipo de medicamento pelo código.");
                throw new NegocioException($"Erro inesperado ao buscar o tipo de medicamento pelo código.", ex);
            }
        }
        #endregion

        #region Inserção
        public async Task<int> CadastrarAsync<TInput>(TInput input) where TInput : class
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "O objeto de cadastro não pode ser nulo.");

            Dominio.TipoMedicamento tipoMedicamento = _mapper.Map<Dominio.TipoMedicamento>(input);

            object retornoValidacao = await ValidarModeloAsync(tipoMedicamento, true, _contexto);
            if (retornoValidacao is string mensagemErro)
                throw new NegocioException(mensagemErro);

            using var transacao = _contexto.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            bool sucesso = false;
            try
            {
                int codigoTipoMedicamento = await _tipoMedicamentoRepositorio.CadastrarAsync(tipoMedicamento, _contexto);
                if (codigoTipoMedicamento > 0)
                {
                    transacao.Commit();
                    _contexto.ChangeTracker.Clear();
                    sucesso = true;
                    return codigoTipoMedicamento;
                }

                throw new NegocioException("Falha ao inserir os dados do tipo de medicamento no banco.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Erro ao acessar o banco de dados durante o cadastro do tipo de medicamento.");
                throw new NegocioException($"Erro ao acessar o banco de dados durante o cadastro do tipo de medicamento.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado durante o cadastro do tipo de medicamento.");
                throw new NegocioException($"Erro inesperado durante o cadastro do tipo de medicamento.", ex);
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

            Dominio.TipoMedicamento tipoMedicamento = _mapper.Map<Dominio.TipoMedicamento>(input);

            object retornoValidacao = await ValidarModeloAsync(tipoMedicamento, false, _contexto);
            if (retornoValidacao is string mensagemErro)
                throw new NegocioException(mensagemErro);

            using var transacao = _contexto.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            bool sucesso = false;
            try
            {
                bool retornoAtualizacao = await _tipoMedicamentoRepositorio.AtualizarAsync(tipoMedicamento, _contexto);
                if (retornoAtualizacao)
                {
                    transacao.Commit();
                    _contexto.ChangeTracker.Clear();
                    sucesso = true;
                    return retornoAtualizacao;
                }

                throw new NegocioException("Falha ao atualizar os dados do tipo de medicamento no banco.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Erro ao acessar o banco de dados durante a atualização do tipo de medicamento.");
                throw new NegocioException($"Erro ao acessar o banco de dados durante a atualização do tipo de medicamento.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado durante a atualização do tipo de medicamento.");
                throw new NegocioException($"Erro inesperado durante a atualização do tipo de medicamento.", ex);
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
        private async Task<object> ValidarModeloAsync(Dominio.TipoMedicamento tipoMedicamento, bool isInsert, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            if (string.IsNullOrWhiteSpace(tipoMedicamento.Descricao))
                return "O atributo descrição não pode ser nulo ou vazio.";

            if (tipoMedicamento.Descricao.Length > 250)
                return "O atributo descrição não pode ser maior que 250 caracteres.";

            if (!isInsert)
            {
                if (tipoMedicamento.Codigo <= 0)
                    return "O atributo código não pode ser menor ou igual a 0.";

                var dadoTipoMedicamento = await _tipoMedicamentoReposBase.BuscarPeloCodigoAssincrono(tipoMedicamento.Codigo, _contexto);
                if (dadoTipoMedicamento == null)
                    return "Não foi possível encontrar um tipo de medicamento em nossa base de dados pelo código informado.";
            }

            return true;
        }

        private async Task<object> ValidarModeloDelecaoAsync(int codigo, int codigoUsuario, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            if (codigo <= 0)
                return "O atributo código não pode ser menor ou igual a 0.";

            var dadoTipoMedicamento = await _tipoMedicamentoReposBase.BuscarPeloCodigoAssincrono(codigo, _contexto);
            if (dadoTipoMedicamento == null)
                return "Não foi possível encontrar um tipo de medicamento em nossa base de dados pelo código informado.";

            if (codigoUsuario <= 0)
                return "O atributo código do usuário não pode ser menor ou igual a 0.";

            var dadoUsuario = await _tipoMedicamentoReposBase.BuscarPeloCodigoAssincrono(codigo, _contexto);
            if (dadoUsuario == null)
                return "Não foi possível encontrar um tipo de medicamento em nossa base de dados pelo código informado.";

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
                bool retornoAtualizacao = await _tipoMedicamentoRepositorio.DeletarAsync(codigo, codigoUsuario, _contexto);
                if (retornoAtualizacao)
                {
                    transacao.Commit();
                    _contexto.ChangeTracker.Clear();
                    sucesso = true;
                    return retornoAtualizacao;
                }

                throw new NegocioException("Falha ao deletar os dados do tipo de medicamento no banco.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Erro ao acessar o banco de dados durante a deleção do tipo de medicamento.");
                throw new NegocioException($"Erro ao acessar o banco de dados durante a deleção do tipo de medicamento.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado durante a deleção do tipo de medicamento.");
                throw new NegocioException($"Erro inesperado durante a deleção do tipo de medicamento.", ex);
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
