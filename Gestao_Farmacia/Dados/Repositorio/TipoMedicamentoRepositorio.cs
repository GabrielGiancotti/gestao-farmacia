using AutoMapper;
using Dados.Contexto;
using Dominio;
using Dominio.Enum;
using Interface.Repositorio;
using Interface.Repositorio.Base;
using Interface.Util;
using Microsoft.EntityFrameworkCore;

namespace Dados.Repositorio
{
    public class TipoMedicamentoRepositorio : ITipoMedicamentoRepositorio
    {
        private GestaoFarmaciaContexto _contexto;
        private readonly IRepositorioBase<Entidade.TipoMedicamento> _tipoMedicamentoReposBase;
        private readonly IMapper _mapper;

        public TipoMedicamentoRepositorio(GestaoFarmaciaContexto contexto, IRepositorioBase<Entidade.TipoMedicamento> tipoMedicamentoReposBase, IMapper mapper)
        {
            _contexto = contexto;
            _tipoMedicamentoReposBase = tipoMedicamentoReposBase;
            _mapper = mapper;
        }

        #region Inserção
        public async Task<List<TipoMedicamento>> BuscarTodosAsync(object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            List<Dominio.TipoMedicamento> lstTiposMedicamentos = await (from tp in _contexto.TipoMedicamento
                                                                        where !tp.Deletado
                                                                        select new Dominio.TipoMedicamento()
                                                                        {
                                                                            Codigo = tp.Codigo,
                                                                            Descricao = tp.Descricao,
                                                                            Codigo_Usuario_Criacao = tp.Codigo_Usuario_Criacao,
                                                                            Data_Criacao = tp.Data_Criacao,
                                                                            Codigo_Usuario_Modificacao = tp.Codigo_Usuario_Modificacao,
                                                                            Data_Modificacao = tp.Data_Modificacao,
                                                                            Codigo_Usuario_Delecao = tp.Codigo_Usuario_Delecao,
                                                                            Data_Delecao = tp.Data_Delecao,
                                                                            Deletado = tp.Deletado
                                                                        }).ToListAsync();

            return lstTiposMedicamentos;
        }

        public async Task<TipoMedicamento> BuscarPeloCodigoAsync(int codigo, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Dominio.TipoMedicamento tipoMedicamento = await (from tp in _contexto.TipoMedicamento
                                                             where !tp.Deletado
                                                             select new Dominio.TipoMedicamento()
                                                             {
                                                                 Codigo = tp.Codigo,
                                                                 Descricao = tp.Descricao,
                                                                 Codigo_Usuario_Criacao = tp.Codigo_Usuario_Criacao,
                                                                 Data_Criacao = tp.Data_Criacao,
                                                                 Codigo_Usuario_Modificacao = tp.Codigo_Usuario_Modificacao,
                                                                 Data_Modificacao = tp.Data_Modificacao,
                                                                 Codigo_Usuario_Delecao = tp.Codigo_Usuario_Delecao,
                                                                 Data_Delecao = tp.Data_Delecao,
                                                                 Deletado = tp.Deletado
                                                             }).FirstOrDefaultAsync();

            return tipoMedicamento;
        }
        #endregion

        #region Inserção
        public async Task<int> CadastrarAsync(Dominio.TipoMedicamento tipoMedicamento, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Entidade.TipoMedicamento dadoTipoMedicamento = _mapper.Map<Dominio.TipoMedicamento, Entidade.TipoMedicamento>(tipoMedicamento);

            int codigoTipoMedicamentoRetorno = await _tipoMedicamentoReposBase.InserirAssincrono(dadoTipoMedicamento, _contexto);

            return codigoTipoMedicamentoRetorno;
        }
        #endregion

        #region Atualização
        public async Task<bool> AtualizarAsync(Dominio.TipoMedicamento tipoMedicamento, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Entidade.TipoMedicamento dadoTipoMedicamento = _mapper.Map<Dominio.TipoMedicamento, Entidade.TipoMedicamento>(tipoMedicamento);

            bool atualizacaoTipoMedicamentoRetorno = await _tipoMedicamentoReposBase.AtualizarAssincrono(dadoTipoMedicamento, _contexto);

            return atualizacaoTipoMedicamentoRetorno;
        }
        #endregion

        #region Atualização
        public async Task<bool> DeletarAsync(int codigo, int codigoUsuario, object? contexto = null)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            Entidade.TipoMedicamento dadoTipoMedicamento = await _tipoMedicamentoReposBase.BuscarPeloCodigoAssincrono(codigo, _contexto);
            dadoTipoMedicamento.Codigo_Usuario_Delecao = codigoUsuario;

            bool delecaoTipoMedicamentoRetorno = await _tipoMedicamentoReposBase.DeletarAssincrono(dadoTipoMedicamento, _contexto);

            return delecaoTipoMedicamentoRetorno;
        }
        #endregion
    }
}
