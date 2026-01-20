using System.ComponentModel;

namespace Dominio.Enum
{
    public enum EnumValidacaoIdadeUsuario
    {
        [Description("Mínimo")]
        Minimo = 16,
        [Description("Máximo")]
        Maximo = 100
    }

    public enum EnumValidacaoSenhaUsuario
    {
        [Description("Minimo Caracteres")]
        MinimoCaracteres = 8
    }

    public enum EnumGeneroUsuario
    {
        [Description("Masculino")]
        Masculino = 1,
        [Description("Feminino")]
        Feminino = 2,
        [Description("Outro")]
        Outro = 3,
        [Description("Não informar")]
        NaoInformar = 4
    }

    public enum EnumPerfilUsuario
    {
        [Description("Usuário")]
        Usuario = 1,
        [Description("Administrador")]
        Administrador = 2,
        [Description("Farmacêutico")]
        Farmaceutico = 3
    }

    public enum EnumUsuarioSistema
    {
        [Description("Sistema")]
        Sistema = 1
    }
}
