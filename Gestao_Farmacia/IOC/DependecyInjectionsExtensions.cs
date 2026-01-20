using Dados.Repositorio.Base;
using Interface.Negocio;
using Interface.Repositorio;
using Interface.Util;
using Interface.Repositorio.Base;
using Microsoft.Extensions.DependencyInjection;
using Negocio;
using Negocio.Util;
using Dados.Repositorio;

namespace IOC
{
    public static class DependecyInjectionsExtensions
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            #region INegocio
            services.AddScoped<IUsuarioNegocio, UsuarioNegocio>();
            services.AddScoped<IAutenticacaoNegocio, AutenticacaoNegocio>();
            #endregion

            #region IRepositorio
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IAutenticacaoRepositorio, AutenticacaoRepositorio>();
            #endregion

            #region IUtil
            services.AddSingleton<ICriptografiaServico, CriptografiaServico>();
            services.AddSingleton<IValidacaoServico, ValidacaoServico>();
            #endregion

            #region IRepositorioBase
            services.AddScoped<IRepositorioBase<Entidade.Usuario>, RepositorioBase<Entidade.Usuario>>();
            services.AddScoped<IRepositorioBase<Entidade.Endereco>, RepositorioBase<Entidade.Endereco>>();
            services.AddScoped<IRepositorioBase<Entidade.Sessao>, RepositorioBase<Entidade.Sessao>>();
            services.AddScoped<IRepositorioBase<Entidade.TokenApi>, RepositorioBase<Entidade.TokenApi>>();
            #endregion

            return services;
        }
    }
}
