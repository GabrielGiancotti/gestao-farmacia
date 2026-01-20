using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Repositorio.Base
{
    public interface IRepositorioBase<T> where T : EntidadeBase
    {
        T BuscarPeloCodigo(int codigo, object contexto);
        Task<T> BuscarPeloCodigoAssincrono(int codigo, object contexto);
        List<T> BuscarTodos(object contexto);
        Task<List<T>> BuscarTodosAssincrono(object contexto);
        List<T> BuscarFiltrado(Expression<Func<T, bool>> filtro, object contexto);
        Task<List<T>> BuscarFiltradoAssincrono(Expression<Func<T, bool>> filtro, object contexto);
        T BuscarFiltradoUnico(Expression<Func<T, bool>> filtro, object contexto);
        Task<T> BuscarFiltradoUnicoAssincrono(Expression<Func<T, bool>> filtro, object contexto);
        int Inserir(T objeto, object contexto);
        Task<int> InserirAssincrono(T objeto, object contexto);
        bool Atualizar(T objeto, object contexto);
        Task<bool> AtualizarAssincrono(T objeto, object contexto);
        bool Deletar(T objeto, object contexto);
        Task<bool> DeletarAssincrono(T objeto, object contexto);
    }
}
