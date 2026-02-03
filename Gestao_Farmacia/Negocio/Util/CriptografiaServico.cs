using Interface.Util;
using System.Security.Cryptography;
using System.Text;

namespace Negocio.Util
{
    /// <summary>
    /// Serviço responsável pela criptografia e validação dos dados.
    /// </summary>
    public class CriptografiaServico : ICriptografiaServico
    {
        private const string ChaveBase = "G3sta0F4rm4c14";
        private const int Iteracoes = 100_000;

        public string GerarHash(string texto)
        {
            var textoBytes = Encoding.UTF8.GetBytes(texto);

            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(textoBytes);
                var hashBase64 = Convert.ToBase64String(hashBytes);
                
                return hashBase64;
            }
        }

        public string CriptografarTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            // Gera salt e IV(Initialization Vector) aleatórios
            byte[] salt = new byte[16];
            byte[] iv = new byte[16];
            RandomNumberGenerator.Fill(salt);
            RandomNumberGenerator.Fill(iv);

            using var pbkdf2 = new Rfc2898DeriveBytes(ChaveBase, salt, Iteracoes, HashAlgorithmName.SHA256);
            byte[] chave = pbkdf2.GetBytes(32);

            using var aes = Aes.Create();
            aes.Key = chave;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(texto);
            }

            // concatena salt + IV + ciphertext e converte em Base64
            byte[] resultado = new byte[salt.Length + iv.Length + memoryStream.ToArray().Length];
            Buffer.BlockCopy(salt, 0, resultado, 0, salt.Length);
            Buffer.BlockCopy(iv, 0, resultado, salt.Length, iv.Length);
            Buffer.BlockCopy(memoryStream.ToArray(), 0, resultado, salt.Length + iv.Length, memoryStream.ToArray().Length);

            return Convert.ToBase64String(resultado);
        }

        public string DescriptografarTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            byte[] dados = Convert.FromBase64String(texto);

            byte[] salt = new byte[16];
            byte[] iv = new byte[16];
            byte[] textoCriptografado = new byte[dados.Length - salt.Length - iv.Length];

            Buffer.BlockCopy(dados, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(dados, salt.Length, iv, 0, iv.Length);
            Buffer.BlockCopy(dados, salt.Length + iv.Length, textoCriptografado, 0, textoCriptografado.Length);

            using var pbkdf2 = new Rfc2898DeriveBytes(ChaveBase, salt, Iteracoes, HashAlgorithmName.SHA256);
            byte[] chave = pbkdf2.GetBytes(32);

            using var aes = Aes.Create();
            aes.Key = chave;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            using var memoryStream = new MemoryStream(textoCriptografado);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            
            return streamReader.ReadToEnd();
        }
    }
}
