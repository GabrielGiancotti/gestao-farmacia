using AutoMapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;
using Dados.Contexto;
using IOC;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Farmacia
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISOptions>(o =>
            {
                o.ForwardClientCertificate = false;
                o.AutomaticAuthentication = false;
            });

            services.AddHttpContextAccessor();
            services.AddControllers();

            services.AddDbContext<GestaoFarmaciaContexto>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Database"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);





            }, ServiceLifetime.Transient);

            services.AddDependencyInjection();

            services.AddAutoMapper(typeof(Startup));

            //services.AddSingleton(new MapperConfiguration(config =>
            //{
            //    #region Dados -> Dominio
            //    config.CreateMap<Dados.Entidade.Usuario, Dominio.Usuario>();
            //    config.CreateMap<Dados.Entidade.Endereco, Dominio.Endereco>();
            //    config.CreateMap<Dados.Entidade.Sessao, Dominio.Sessao>();
            //    #endregion

            //    #region Dominio -> Dados
            //    config.CreateMap<Dominio.Usuario, Dados.Entidade.Usuario>();
            //    config.CreateMap<Dominio.Endereco, Dados.Entidade.Endereco>();
            //    config.CreateMap<Dominio.Sessao, Dados.Entidade.Sessao>();
            //    #endregion

            //    #region Dominio -> Resposta

            //    #endregion

            //    #region Criacao -> Dominio

            //    #endregion

            //    #region Atualizacao -> Dominio

            //    #endregion

            //}).CreateMapper());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SistemaGestaoFarmacia", Version = "v1" });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "SistemaGestaoFarmacia v1"));

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var locale = Configuration["Local:SiteLocale"];
            if (string.IsNullOrEmpty(locale))
                locale = "pt-BR";

            RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = new List<CultureInfo> { new CultureInfo(locale) },
                SupportedUICultures = new List<CultureInfo> { new CultureInfo(locale) },
                DefaultRequestCulture = new RequestCulture(locale)
            };
            app.UseRequestLocalization(localizationOptions);
        }
    }
}
