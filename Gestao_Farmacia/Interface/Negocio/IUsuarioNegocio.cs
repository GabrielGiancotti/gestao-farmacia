using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Negocio
{
    public interface IUsuarioNegocio
    {
        #region Inserção
        Task<int> CadastrarExternoAsync<TInput>(TInput input) where TInput : class;
        #endregion

        #region Validação
        Task<bool> ExisteEmailAsync(int? codigo, string email);
        Task<bool> ExisteCpfAsync(int? codigo, string cpf);
        #endregion
    }
}