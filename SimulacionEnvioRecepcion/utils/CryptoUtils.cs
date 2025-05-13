using System.Security.Cryptography;
using System.Text;

namespace Crypto.Utils
{
    public static class CryptoUtils
    {
        const int iteraciones = 10000;
        
        /// <summary>
        /// Genera un hash de la contraseña combinándola con un salt aleatorio
        /// utilizando PBKDF2 (Rfc2898DeriveBytes) con SHA-512.
        /// Devuelve un diccionario con el 'salt' y el 'hash', ambos en formato hexadecimal.
        /// </summary>
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

        /// <summary>
        /// Verifica si una contraseña introducida es correcta comparando su hash derivado
        /// con el hash almacenado, utilizando el mismo salt.
        /// </summary>
        public static bool VerificarPassword(string inputPassword, string hashGuardado, string saltGuardado)
        {
            byte[] saltAComparar = StringHexToBytes(saltGuardado);
            byte[] hashAComparar = StringHexToBytes(hashGuardado);

            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(inputPassword), saltAComparar, iteraciones, HashAlgorithmName.SHA512);
            byte[] hashBytesConSaltIterado = k1.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(hashBytesConSaltIterado, hashAComparar);
        }

        /// <summary>
        /// Convierte un array de bytes a un string hexadecimal en minúsculas.
        /// Ejemplo: [0x4f, 0xa2] → "4fa2"
        /// </summary>
        public static string BytesToStringHex (byte[] result)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in result)
                stringBuilder.AppendFormat("{0:x2}", b);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Convierte un string hexadecimal (como "4fa2") a un array de bytes.
        /// </summary>
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
