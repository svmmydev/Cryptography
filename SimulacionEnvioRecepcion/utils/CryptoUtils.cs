using System.Security.Cryptography;
using System.Text;

namespace Crypto.Utils
{
    public static class CryptoUtils
    {
        const int iteraciones = 10000;
        public static Dictionary<string, string> GenerarHashConSaltIterado(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(32);

            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), salt, iteraciones, HashAlgorithmName.SHA512);
            byte[] hashBytesConSaltIterado = k1.GetBytes(32);
            
            return new Dictionary<string, string>
            {
                { "salt", BytesToStringHex(salt) },
                { "hash", BytesToStringHex(hashBytesConSaltIterado) }
            };
        }

        public static bool VerificarPassword(string inputPassword, string hashGuardado, string saltGuardado)
        {
            byte[] saltAComparar = StringHexToBytes(saltGuardado);
            byte[] hashAComparar = StringHexToBytes(hashGuardado);

            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(inputPassword), saltAComparar, iteraciones, HashAlgorithmName.SHA512);
            byte[] hashBytesConSaltIterado = k1.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(hashBytesConSaltIterado, hashAComparar);
        }

        public static string BytesToStringHex (byte[] result)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in result)
                stringBuilder.AppendFormat("{0:x2}", b);

            return stringBuilder.ToString();
        }

        public static byte[] StringHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }
    }
}
