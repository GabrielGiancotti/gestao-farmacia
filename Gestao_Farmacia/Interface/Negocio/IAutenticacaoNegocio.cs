using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Negocio
{
    public interface IAutenticacaoNegocio
    {
        #region Inserção
        Task<object> LoginAsync<TInput>(TInput input) where TInput : class;
        #endregion

        #region Validação
        Task<bool> ValidarTokenExternoAsync(string token);
        #endregion
    }
}
