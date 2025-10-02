using Dados.Contexto;
using Interface.Repositorio.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dados.Repositorio.Base
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : EntidadeBase
    {
        private GestaoFarmaciaContexto _contexto;

        public RepositorioBase(GestaoFarmaciaContexto contexto) 
        {
            _contexto = contexto;
        }

        public T BuscarPeloCodigo(int codigo, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            T dadosRetorno = _contexto.Set<T>().AsNoTracking().FirstOrDefault(f => f.Codigo == codigo);

            return dadosRetorno;
        }

        public async Task<T> BuscarPeloCodigoAssincrono(int codigo, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            T dadosRetorno = await _contexto.Set<T>().AsNoTracking().FirstOrDefaultAsync(f => f.Codigo == codigo);

            return dadosRetorno;
        }

        public List<T> BuscarFiltrado(T objeto, string filtro, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            string consulta = $"{objeto.Consulta} AND {filtro}";
            List<T> dadosRetorno = _contexto.Set<T>().FromSqlRaw(consulta).AsNoTracking().ToList();

            return dadosRetorno;
        }

        public async Task<List<T>> BuscarFiltradoAssincrono(T objeto, string filtro, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            string consulta = $"{objeto.Consulta} AND {filtro}";
            List<T> dadosRetorno = await _contexto.Set<T>().FromSqlRaw(consulta).AsNoTracking().ToListAsync();

            return dadosRetorno;
        }

        public T BuscarFiltradoUnico(T objeto, string filtro, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            string consulta = $"{objeto.Consulta} AND {filtro}";
            T dadosRetorno = _contexto.Set<T>().FromSqlRaw(consulta).AsNoTracking().FirstOrDefault();

            return dadosRetorno;
        }

        public async Task<T> BuscarFiltradoUnicoAssincrono(T objeto, string filtro, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            string consulta = $"{objeto.Consulta} AND {filtro}";
            T dadosRetorno = await _contexto.Set<T>().FromSqlRaw(consulta).AsNoTracking().FirstOrDefaultAsync();

            return dadosRetorno;
        }

        public List<T> BuscarTodos(object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            List<T> dadosRetorno = _contexto.Set<T>().AsNoTracking().Where(w => !w.Deletado).ToList();

            return dadosRetorno;
        }

        public async Task<List<T>> BuscarTodosAssincrono(object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            List<T> dadosRetorno = await _contexto.Set<T>().AsNoTracking().Where(w => !w.Deletado).ToListAsync();

            return dadosRetorno;
        }

        public bool Atualizar(T objeto, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            objeto.Data_Modificacao = DateTime.Now;

            _contexto.Set<T>().Update(objeto);
            _contexto.SaveChanges();

            return true;
        }

        public async Task<bool> AtualizarAssincrono(T objeto, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            objeto.Data_Modificacao = DateTime.Now;

            _contexto.Set<T>().Update(objeto);
            await _contexto.SaveChangesAsync();

            return true;
        }

        public bool Deletar(T objeto, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            objeto.Data_Delecao = DateTime.Now;
            objeto.Deletado = true;

            _contexto.Set<T>().Update(objeto);
            _contexto.SaveChanges();

            return true;
        }

        public async Task<bool> DeletarAssincrono(T objeto, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            objeto.Data_Delecao = DateTime.Now;
            objeto.Deletado = true;

            _contexto.Set<T>().Update(objeto);
            await _contexto.SaveChangesAsync();

            return true;
        }

        public int Inserir(T objeto, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            objeto.Data_Delecao = DateTime.Now;
            objeto.Deletado = true;

            _contexto.Set<T>().Add(objeto);
            _contexto.SaveChanges();

            return objeto.Codigo;
        }

        public async Task<int> InserirAssincrono(T objeto, object contexto)
        {
            if (contexto != null)
                _contexto = (GestaoFarmaciaContexto)contexto;

            objeto.Data_Criacao = DateTime.Now;

            _contexto.Set<T>().Add(objeto);
            await _contexto.SaveChangesAsync();

            return objeto.Codigo;
        }
    }
}
