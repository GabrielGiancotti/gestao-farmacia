using Dados.Repositorio.Base;
using Interface.Repositorio.Base;
using Microsoft.Extensions.DependencyInjection;

namespace IOC
{
    public static class DependecyInjectionsExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            #region INegocio

            #endregion

            #region IRepositorio

            #endregion

            #region IRepositorioBase
            services.AddTransient<IRepositorioBase<Dados.Entidade.Usuario>, RepositorioBase<Dados.Entidade.Usuario>>();
            services.AddTransient<IRepositorioBase<Dados.Entidade.Endereco>, RepositorioBase<Dados.Entidade.Endereco>>();
            services.AddTransient<IRepositorioBase<Dados.Entidade.Sessao>, RepositorioBase<Dados.Entidade.Sessao>>();
            #endregion

            return services;
        }
    }
}
