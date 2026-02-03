using Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dados.Contexto
{
    public partial class GestaoFarmaciaContexto : DbContext, IDisposable
    {
        private static IConfiguration Configuration;
        //private static IConfigurationSection _Ambiente;
        private static string _Ambiente;

        public static bool Producao
        {
            get
            {
                if (_Ambiente != null)
                    return !_Ambiente.Contains("HML");
                else
                {
                    string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
                    //Configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build(); //Versão antiga
                    Configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

                    var stringConexao = Configuration.GetConnectionString("Database");

                    if (string.IsNullOrWhiteSpace(stringConexao))
                        throw new InvalidOperationException("Connection string 'Database' não encontrada no appsettings.json.");

                    return !stringConexao.Contains("HML", StringComparison.OrdinalIgnoreCase) && !stringConexao.Contains("Testes", StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        public GestaoFarmaciaContexto()
        {
            //string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            string projectPath = Directory.GetCurrentDirectory();
            Configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            var stringConexao = Configuration.GetConnectionString("Database");
            if (string.IsNullOrWhiteSpace(stringConexao))
                throw new InvalidOperationException("Connection string 'Database' não encontrada no appsettings.json.");

            _Ambiente = stringConexao;
        }

        public GestaoFarmaciaContexto(DbContextOptions<GestaoFarmaciaContexto> options)
            : base(options)
        {
        }

        #region Tabelas
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Endereco> Endereco { get; set; }
        public virtual DbSet<Sessao> Sessao { get; set; }
        public virtual DbSet<TokenApi> TokenApi { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer(_Ambiente, opt =>
                    {
                        opt.CommandTimeout(360);
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("F001_Usuario");

                entity.Property(e => e.Codigo).HasColumnType("int").HasColumnName("Codigo");
                entity.HasKey("Codigo");
                entity.Property(e => e.Nome).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Nome");
                entity.Property(e => e.Data_Nascimento).HasMaxLength(250).IsUnicode(false).HasColumnName("Data_Nascimento");
                entity.Property(e => e.Cpf).HasMaxLength(250).IsUnicode(false).HasColumnName("Cpf");
                entity.Property(e => e.Cpf_Hash).HasMaxLength(250).IsUnicode(false).HasColumnName("Cpf_Hash");
                entity.Property(e => e.Telefone).HasMaxLength(250).IsUnicode(false).HasColumnName("Telefone");
                entity.Property(e => e.Email).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Email");
                entity.Property(e => e.Email_Hash).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Email_Hash");
                entity.Property(e => e.Senha).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Senha");
                entity.Property(e => e.Codigo_Perfil).HasColumnType("int").IsRequired().HasColumnName("Codigo_Perfil");
                entity.Property(e => e.Ativo).HasColumnType("bit").IsRequired().HasColumnName("Ativo");
                entity.Property(e => e.Genero).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Genero");
                entity.Property(e => e.Tentativas_Login).HasColumnType("int").IsRequired().HasColumnName("Tentativas_Login");
                entity.Property(e => e.Data_Ultimo_Login).HasColumnType("datetime2(7)").HasColumnName("Data_Ultimo_Login");
                entity.Property(e => e.Crm).HasMaxLength(250).IsUnicode(false).HasColumnName("Crm");
                entity.Property(e => e.Codigo_Usuario_Criacao).HasColumnType("int").IsRequired().HasColumnName("Codigo_Usuario_Criacao");
                entity.Property(e => e.Data_Criacao).HasColumnType("datetime2(7)").IsRequired().HasColumnName("Data_Criacao").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Codigo_Usuario_Modificacao).HasColumnType("int").HasColumnName("Codigo_Usuario_Modificacao");
                entity.Property(e => e.Data_Modificacao).HasColumnType("datetime2(7)").HasColumnName("Data_Modificacao");
                entity.Property(e => e.Codigo_Usuario_Delecao).HasColumnType("int").HasColumnName("Codigo_Usuario_Delecao");
                entity.Property(e => e.Data_Delecao).HasColumnType("datetime2(7)").HasColumnName("Data_Delecao");
                entity.Property(e => e.Deletado).HasColumnType("bit").HasColumnName("Deletado");
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.ToTable("F002_Endereco");

                entity.Property(e => e.Codigo).HasColumnType("int").HasColumnName("Codigo");
                entity.HasKey("Codigo");
                entity.Property(e => e.Logradouro).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Logradouro");
                entity.Property(e => e.Numero).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Numero");
                entity.Property(e => e.Bairro).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Bairro");
                entity.Property(e => e.Cidade).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Cidade");
                entity.Property(e => e.Estado).HasMaxLength(250).IsUnicode(false).IsRequired().HasColumnName("Estado");
                entity.Property(e => e.Cep).HasMaxLength(9).IsUnicode(false).IsRequired().HasColumnName("Cep");
                entity.Property(e => e.Complemento).HasMaxLength(250).IsUnicode(false).HasColumnName("Complemento");
                entity.Property(e => e.Codigo_Usuario_Criacao).HasColumnType("int").IsRequired().HasColumnName("Codigo_Usuario_Criacao");
                entity.Property(e => e.Data_Criacao).HasColumnType("datetime2(7)").IsRequired().HasColumnName("Data_Criacao").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Codigo_Usuario_Modificacao).HasColumnType("int").HasColumnName("Codigo_Usuario_Modificacao");
                entity.Property(e => e.Data_Modificacao).HasColumnType("datetime2(7)").HasColumnName("Data_Modificacao");
                entity.Property(e => e.Codigo_Usuario_Delecao).HasColumnType("int").HasColumnName("Codigo_Usuario_Delecao");
                entity.Property(e => e.Data_Delecao).HasColumnType("datetime2(7)").HasColumnName("Data_Delecao");
                entity.Property(e => e.Deletado).HasColumnType("bit").HasColumnName("Deletado");
            });

            modelBuilder.Entity<Sessao>(entity =>
            {
                entity.ToTable("F003_Sessao");

                entity.Property(e => e.Codigo).HasColumnType("int").HasColumnName("Codigo");
                entity.HasKey("Codigo");
                entity.Property(e => e.Codigo_Usuario).HasColumnType("int").IsRequired().HasColumnName("Codigo_Usuario");
                entity.Property(e => e.Chave).HasMaxLength(36).IsUnicode(false).IsRequired().HasColumnName("Chave");
                entity.Property(e => e.Ip).HasMaxLength(32).IsUnicode(false).HasColumnName("Ip");
                entity.Property(e => e.Data_Expiracao).HasColumnType("datetime2(7)").IsRequired().HasColumnName("Data_Expiracao");
                entity.Property(e => e.Data_Criacao).HasColumnType("datetime2(7)").HasColumnName("Data_Criacao").HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TokenApi>(entity =>
            {
                entity.ToTable("F011_Token_Api");

                entity.Property(e => e.Codigo).HasColumnType("int").HasColumnName("Codigo");
                entity.HasKey("Codigo");
                entity.Property(e => e.Descricao).HasMaxLength(250).IsRequired().HasColumnName("Descricao");
                entity.Property(e => e.Chave).HasMaxLength(36).IsUnicode(false).IsRequired().HasColumnName("Chave");
                entity.Property(e => e.Data_Expiracao).HasColumnType("datetime2(7)").IsRequired().HasColumnName("Data_Expiracao");
                entity.Property(e => e.Data_Criacao).HasColumnType("datetime2(7)").HasColumnName("Data_Criacao").HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        bool _disposed;

        public override void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}
