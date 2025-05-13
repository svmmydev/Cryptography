using System.Security.Cryptography;

namespace ClaveSimetricaClass
{
    public class ClaveSimetrica
    {
        public byte[] Key {get;set;}
        public byte[] IV {get;set;}

        /// <summary>
        /// Constructor: al instanciar la clase, genera una nueva clave e IV aleatorios con AES.
        /// </summary>
        public ClaveSimetrica()
        {
            Aes aesAlg = Aes.Create();
            this.Key = aesAlg.Key;
            this.IV = aesAlg.IV;
        }

        /// <summary>
        /// Cifra un mensaje de texto utilizando una clave y IV específicos proporcionados como argumentos.
        /// </summary>
        /// <param name="Mensaje">Texto plano a cifrar</param>
        /// <param name="Key">Clave AES a usar</param>
        /// <param name="IV">Vector de inicialización a usar</param>
        /// <returns>Mensaje cifrado en forma de array de bytes</returns>
        public byte[] CifrarMensaje (string Mensaje, byte[] Key, byte[] IV)
        {            
            Aes aesAlg = Aes.Create();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(Key, IV); 

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                { 
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {                            
                        streamWriter.Write(Mensaje);  
                    }
                    return memoryStream.ToArray();                                          
                }
            }
        }

        /// <summary>
        /// Cifra un mensaje utilizando la clave y IV propios de la instancia.
        /// </summary>
        /// <param name="Mensaje">Texto plano a cifrar</param>
        /// <returns>Mensaje cifrado como array de bytes</returns>
        public byte[] CifrarMensaje (string Mensaje)
        {            
            Aes aesAlg = Aes.Create();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(this.Key, this.IV); 

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                { 
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {                            
                        streamWriter.Write(Mensaje);  
                    }
                    return memoryStream.ToArray();                                          
                }
            }
        }

        /// <summary>
        /// Descifra un mensaje cifrado con una clave y IV específicos proporcionados como argumentos.
        /// </summary>
        /// <param name="MensajeCifrado">Array de bytes con el mensaje cifrado</param>
        /// <param name="Key">Clave AES a usar</param>
        /// <param name="IV">Vector de inicialización a usar</param>
        /// <returns>Texto plano resultante del descifrado</returns>
        public string DescifrarMensaje (byte[] MensajeCifrado, byte[] Key, byte[] IV)
        {
            Aes aesAlg = Aes.Create();
            
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(Key, IV); 
            using (MemoryStream memoryStreamd = new MemoryStream(MensajeCifrado))
            {
                using (CryptoStream cryptoStreamd = new CryptoStream((Stream)memoryStreamd, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStreamd))
                    { 
                        return streamReader.ReadToEnd();                             
                    }
                }
            }   

        }

        /// <summary>
        /// Descifra un mensaje utilizando la clave y IV propios de la instancia.
        /// </summary>
        /// <param name="MensajeCifrado">Array de bytes con el mensaje cifrado</param>
        /// <returns>Texto plano resultante del descifrado</returns>
        public string DescifrarMensaje (byte[] MensajeCifrado)
        {
            Aes aesAlg = Aes.Create();
            
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(this.Key, this.IV); 
            using (MemoryStream memoryStreamd = new MemoryStream(MensajeCifrado))
            {
                using (CryptoStream cryptoStreamd = new CryptoStream((Stream)memoryStreamd, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStreamd))
                    { 
                        return streamReader.ReadToEnd();                             
                    }
                }
            }   
        }
    }
}
