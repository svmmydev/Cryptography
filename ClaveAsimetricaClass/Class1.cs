using System.Security.Cryptography;
using System.Text;

namespace ClaveAsimetricaClass
{
    public class ClaveAsimetrica
    {
        public RSACryptoServiceProvider RSA {get; set;} 
        public RSAParameters PublicKey = new RSAParameters();

        /// <summary>
        /// Constructor: genera un nuevo par de claves (pública/privada).
        /// </summary>
        public ClaveAsimetrica()
        {
            RSA = new RSACryptoServiceProvider();
            PublicKey = RSA.ExportParameters(false);
        }

        /// <summary>
        /// Firma un mensaje usando la clave privada interna.
        /// </summary>
        public byte[] FirmarMensaje (byte[] MensajeBytes)
        {
            return RSA.SignData(MensajeBytes,0,MensajeBytes.Length,SHA512.Create());
        } 


        //Método inválido: No tiene sentido firmar con una clave pública
        /**
        public byte[] FirmarMensaje (byte[] MensajeBytes, RSAParameters ClavePublicaExterna)
        {
            RSACryptoServiceProvider RSA_Externo = new RSACryptoServiceProvider();
            RSA_Externo.ImportParameters (ClavePublicaExterna);
            return RSA_Externo.SignData(MensajeBytes,0,MensajeBytes.Length,SHA512.Create());
        }
        */


        /// <summary>
        /// Verifica una firma usando la clave pública propia.
        /// </summary>
        public bool ComprobarFirma (byte[] FirmaBytes, byte[] textoDescifradoBytes)
        {
            return RSA.VerifyData(textoDescifradoBytes,SHA512.Create(),FirmaBytes);
        } 

        /// <summary>
        /// Verifica una firma usando una clave pública externa.
        /// </summary>
        public bool ComprobarFirma (byte[] FirmaBytes, byte[] textoDescifradoBytes, RSAParameters ClavePublicaExterna)
        {
            RSACryptoServiceProvider RSA_Externo = new RSACryptoServiceProvider();
            RSA_Externo.ImportParameters (ClavePublicaExterna);            
            return RSA_Externo.VerifyData(textoDescifradoBytes,SHA512.Create(),FirmaBytes);
        } 


        //Método especial para testeo: Firmar con clave privada para descifrar con la privada propia
        /**
        public byte[] CifrarMensaje (byte[] MensajeBytes)
        {
            return RSA.Encrypt(MensajeBytes,false);
        }
        */


        /// <summary>
        /// Cifra un mensaje con una clave pública externa (del receptor).
        /// </summary>
        public byte[] CifrarMensaje (byte[] MensajeBytes, RSAParameters ClavePublicaExterna)
        {
            RSACryptoServiceProvider RSA_Externo = new RSACryptoServiceProvider();
            RSA_Externo.ImportParameters (ClavePublicaExterna);

            return RSA_Externo.Encrypt(MensajeBytes,false);
        }

        /// <summary>
        /// Descifra un mensaje con la clave privada propia.
        /// </summary>
        public byte[] DescifrarMensaje (byte[] MensajeCifradoBytes)
        {
            return RSA.Decrypt(MensajeCifradoBytes,false);
        }


        //Método poco útil: No tiene sentido descifrar con una clave pública
        /**
        public byte[] DescifrarMensaje (byte[] MensajeCifradoBytes, RSAParameters ClavePublicaExterna)
        {
            RSACryptoServiceProvider RSA_Externo = new RSACryptoServiceProvider();
            RSA_Externo.ImportParameters (ClavePublicaExterna);
            return RSA_Externo.Decrypt(MensajeCifradoBytes,false);            
        }
        */
    }
}
