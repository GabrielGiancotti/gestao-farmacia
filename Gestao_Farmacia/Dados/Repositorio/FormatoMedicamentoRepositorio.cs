using AutoMapper;
using Dados.Contexto;
using Dominio;
using Interface.Repositorio;
using Interface.Repositorio.Base;
using Interface.Util;
using Microsoft.EntityFrameworkCore;

namespace Dados.Repositorio
{
    public class FormatoMedicamentoRepositorio : IFormatoMedicamentoRepositorio
    {
        private GestaoFarmaciaContexto _contexto;
        private readonly IRepositorioBase<Entidade.FormatoMedicamento> _formatoMedicamentoReposBase;
        private readonly IMapper _mapper;

        public FormatoMedicamentoRepositorio(GestaoFarmaciaContexto contexto, IRepositorioBase<Entidade.FormatoMedicamento> formatoMedicamentoReposBase, IMapper mapper)
        {
            _contexto = contexto;
            _formatoMedicamentoReposBase = formatoMedicamentoReposBase;
            _mapper = mapper;
        }

        #region Inserção
        public async Task<List<FormatoMedicamento>> BuscarTodosAsync(object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            List<FormatoMedicamento> lstFormatosMedicamentos = await (from fp in _contexto.FormatoMedicamento
                                                                      where !fp.Deletado
                                                                      select new FormatoMedicamento()
                                                                      {
                                                                          Codigo = fp.Codigo,
                                                                          Descricao = fp.Descricao,
                                                                          Codigo_Usuario_Criacao = fp.Codigo_Usuario_Criacao,
                                                                          Data_Criacao = fp.Data_Criacao,
                                                                          Codigo_Usuario_Modificacao = fp.Codigo_Usuario_Modificacao,
                                                                          Data_Modificacao = fp.Data_Modificacao,
                                                                          Codigo_Usuario_Delecao = fp.Codigo_Usuario_Delecao,
                                                                          Data_Delecao = fp.Data_Delecao,
                                                                          Deletado = fp.Deletado
                                                                      }).ToListAsync();

            return lstFormatosMedicamentos;
        }

        public async Task<FormatoMedicamento> BuscarPeloCodigoAsync(int codigo, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            FormatoMedicamento formatoMedicamento = await (from fp in _contexto.FormatoMedicamento
                                                                   where !fp.Deletado
                                                                   select new FormatoMedicamento()
                                                                   {
                                                                       Codigo = fp.Codigo,
                                                                       Descricao = fp.Descricao,
                                                                       Codigo_Usuario_Criacao = fp.Codigo_Usuario_Criacao,
                                                                       Data_Criacao = fp.Data_Criacao,
                                                                       Codigo_Usuario_Modificacao = fp.Codigo_Usuario_Modificacao,
                                                                       Data_Modificacao = fp.Data_Modificacao,
                                                                       Codigo_Usuario_Delecao = fp.Codigo_Usuario_Delecao,
                                                                       Data_Delecao = fp.Data_Delecao,
                                                                       Deletado = fp.Deletado
                                                                   }).FirstOrDefaultAsync();

            return formatoMedicamento;
        }
        #endregion

        #region Inserção
        public async Task<int> CadastrarAsync(FormatoMedicamento formatoMedicamento, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Entidade.FormatoMedicamento dadoFormatoMedicamento = _mapper.Map<FormatoMedicamento, Entidade.FormatoMedicamento>(formatoMedicamento);

            int codigoFormatoMedicamentoRetorno = await _formatoMedicamentoReposBase.InserirAssincrono(dadoFormatoMedicamento, _contexto);

            return codigoFormatoMedicamentoRetorno;
        }
        #endregion

        #region Atualização
        public async Task<bool> AtualizarAsync(FormatoMedicamento formatoMedicamento, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Entidade.FormatoMedicamento dadoFormatoMedicamento = _mapper.Map<FormatoMedicamento, Entidade.FormatoMedicamento>(formatoMedicamento);

            bool atualizacaoFormatoMedicamentoRetorno = await _formatoMedicamentoReposBase.AtualizarAssincrono(dadoFormatoMedicamento, _contexto);

            return atualizacaoFormatoMedicamentoRetorno;
        }
        #endregion

        #region Atualização
        public async Task<bool> DeletarAsync(int codigo, int codigoUsuario, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Entidade.FormatoMedicamento dadoFormatoMedicamento = await _formatoMedicamentoReposBase.BuscarPeloCodigoAssincrono(codigo, _contexto);
            dadoFormatoMedicamento.Codigo_Usuario_Delecao = codigoUsuario;

            bool delecaoFormatoMedicamentoRetorno = await _formatoMedicamentoReposBase.DeletarAssincrono(dadoFormatoMedicamento, _contexto);

            return delecaoFormatoMedicamentoRetorno;
        }
        #endregion
    }
}
