
using System.Text;
using ClaveSimetricaClass;
using ClaveAsimetricaClass;
using Crypto.Utils;

namespace SimuladorEnvioRecepcion
{
    class Program
    {   
        //Credenciales
        static string? UserName;
        static string? SecurePass;
        static string? Salt;
        static bool login;
        static Dictionary<string, string>? DataHash;


        //Claves y objetos
        static ClaveAsimetrica Emisor = new ClaveAsimetrica();
        static ClaveAsimetrica Receptor = new ClaveAsimetrica();
        static ClaveSimetrica ClaveSimetricaEmisor = new ClaveSimetrica();
        static ClaveSimetrica ClaveSimetricaReceptor = new ClaveSimetrica();


        //Datos a enviar originales
        static string TextoAEnviar = "Me he dado cuenta que incluso las personas que dicen que todo " +
            "está predestinado y que no podemos hacer nada para cambiar nuestro destino igual miran antes " + 
            "de cruzar la calle. Stephen Hawking.";
        static byte[]? TextoAEnviar_Bytes;


        //Resultados Emisor
        static byte[]? FirmaEmisor;
        static byte[]? MensajeCifradoSimetrico;
        static byte[]? ClaveSimetricaKEYCifrada;
        static byte[]? ClaveSimetricaIVCifrada;
        

        //Resultados Receptor
        static byte[]? ClaveSimetricaKEYDescifrada;
        static byte[]? ClaveSimetricaIVDescifrada;
        static string? MensajeDescifradoSimetrico;
        static byte[]? MensajeDescifradoSimetrico_Bytes;
        static bool FirmaValida;


        static void Main(string[] args)
        {
            /****INICIO PARTE 1****/
            
            //Login - Registro
            Console.WriteLine ("# ¿Deseas registrarte? (S/N)");
            string? registro = Console.ReadLine();

            if (registro?.ToUpper() == "S")
            {
                //Realizar registro del cliente
                Registro();
            }

            if (UserName != null)
            {
                //Realizar login
                login = Login();
            }
            else
            {
                Console.WriteLine("\n# No hay ningún usuario almacenado");
            }

            /****FIN PARTE 1****/

            /****INICIO PARTE 2****/

            if (login)
            {
                TextoAEnviar_Bytes = Encoding.UTF8.GetBytes(TextoAEnviar);
                Console.WriteLine($"\n\n# Texto a enviar bytes:\n{CryptoUtils.BytesToStringHex(TextoAEnviar_Bytes)}");
                
                
                /// -------------
                /// LADO EMISOR |
                /// -------------
                
                Console.WriteLine("\n\n##################");
                Console.WriteLine("INICIO LADO EMISOR");
                Console.WriteLine("##################");

                //Firmar mensaje
                FirmaEmisor = Emisor.FirmarMensaje(TextoAEnviar_Bytes);
                Console.WriteLine($"\n# Firma hasheada:\n{CryptoUtils.BytesToStringHex(FirmaEmisor)}");

                //Cifrar mensaje con la clave simétrica
                MensajeCifradoSimetrico = ClaveSimetricaEmisor.CifrarMensaje(TextoAEnviar);
                Console.WriteLine($"\n# Texto cifrado con clave simétrica:\n{CryptoUtils.BytesToStringHex(MensajeCifradoSimetrico)}");

                //Cifrar clave simétrica con la clave pública del receptor
                ClaveSimetricaKEYCifrada = Emisor.CifrarMensaje(ClaveSimetricaEmisor.Key, Receptor.PublicKey);
                Console.WriteLine($"\n# Clave simétrica cifrada (KEY):\n{CryptoUtils.BytesToStringHex(ClaveSimetricaKEYCifrada)}");
                ClaveSimetricaIVCifrada = Emisor.CifrarMensaje(ClaveSimetricaEmisor.IV, Receptor.PublicKey);
                Console.WriteLine($"\n# Clave simétrica cifrada (IV):\n{CryptoUtils.BytesToStringHex(ClaveSimetricaIVCifrada)}");

                //PLUS: KEY - IV emisor para comparar
                Console.WriteLine($"\n# Clave KEY original del emisor:\n{CryptoUtils.BytesToStringHex(ClaveSimetricaEmisor.Key)}");
                Console.WriteLine($"\n# Clave IV original del emisor:\n{CryptoUtils.BytesToStringHex(ClaveSimetricaEmisor.IV)}");


                /// ---------------
                /// LADO RECEPTOR |
                /// ---------------
                
                Console.WriteLine("\n\n####################");
                Console.WriteLine("INICIO LADO RECEPTOR");
                Console.WriteLine("####################");

                //Descifrar clave simétrica
                ClaveSimetricaKEYDescifrada = Receptor.DescifrarMensaje(ClaveSimetricaKEYCifrada);
                Console.WriteLine($"\n# Clave simétrica del emisor descifrada (KEY):\n{CryptoUtils.BytesToStringHex(ClaveSimetricaKEYDescifrada)}");
                ClaveSimetricaIVDescifrada = Receptor.DescifrarMensaje(ClaveSimetricaIVCifrada);
                Console.WriteLine($"\n# Clave simétrica del emisor descifrada (IV):\n{CryptoUtils.BytesToStringHex(ClaveSimetricaIVDescifrada)}");
 
                //Descifrar mensaje con la clave simétrica
                MensajeDescifradoSimetrico = ClaveSimetricaReceptor.DescifrarMensaje(
                    MensajeCifradoSimetrico,
                    ClaveSimetricaKEYDescifrada,
                    ClaveSimetricaIVDescifrada);
                
                //Pasar mensaje descifrado a bytes
                MensajeDescifradoSimetrico_Bytes = Encoding.UTF8.GetBytes(MensajeDescifradoSimetrico);
                Console.WriteLine($"\n# Texto descifrado a recibir en bytes:\n{CryptoUtils.BytesToStringHex(MensajeDescifradoSimetrico_Bytes)}");

                //Comprobar firma
                FirmaValida = Receptor.ComprobarFirma(FirmaEmisor, MensajeDescifradoSimetrico_Bytes, Emisor.PublicKey);

                if (FirmaValida)
                {
                    Console.WriteLine($"\n### FIRMA VALIDA ###");
                    Console.WriteLine($"\n# Texto recibido y descifrado:\n{MensajeDescifradoSimetrico}");
                }
                else
                {
                    Console.WriteLine($"\n\n### FIRMA NO VALIDA ###");
                    Console.WriteLine("\nLa comunicación podría haber sido interceptada por un agente externo");
                }
            }

            Console.WriteLine("\n\n#############################");
            Console.WriteLine("FIN DE LA COMUNICACIÓN SEGURA");
            Console.WriteLine("#############################");

            /****FIN PARTE 2****/
        }

        public static void Registro()
        {
            //Una vez obtenido el nombre de usuario lo guardamos en la variable UserName y este ya no cambiará 
            Console.WriteLine ("\n# Indica tu nombre de usuario:");
            UserName = Console.ReadLine();

            //Una vez obtenido el passoword de registro debemos tratarlo como es debido para almacenarlo correctamente a la variable SecurePass
            Console.WriteLine ("\n# Indica tu password:");
            string? passwordRegister = Console.ReadLine();

            /****PARTE 1****/

            //Se calcula un 'Hash' (SHA512) iterado 10000 veces y se almacena junto a su 'salt' generado aleatoriamente
            DataHash = CryptoUtils.GenerarHashConSaltIterado(passwordRegister!);
            Console.WriteLine($"\n# Prueba:\nSALT: {DataHash["salt"]}\nHASH CON SALT: {DataHash["hash"]} (SHA512 iterado)");

            Salt = DataHash["salt"];
            SecurePass = DataHash["hash"];
        }

        public static bool Login()
        {
            bool auxlogin = false;

            do
            {
                Console.WriteLine ("\n### ACCESO A LA APLICACIÓN ###");
                Console.WriteLine ("\n# Usuario: ");
                string userName = Console.ReadLine()!;

                Console.WriteLine ("\n# Password: ");
                string Password = Console.ReadLine()!;

                /****PARTE 1****/

                //Se calcula el Hash iterado con el 'salt' y el 'Hash' obtenidos previamente y se procesa su comprobación
                if (UserName == userName && CryptoUtils.VerificarPassword(Password!, SecurePass!, Salt!))
                {
                    Console.WriteLine("\n### Login correcto ###");
                    auxlogin = true;
                }
                else if (UserName != userName)
                {
                    Console.WriteLine("\n# Usuario inexistente");
                }
                else
                {
                    Console.WriteLine("\n# Contraseña incorrecta");
                }
            }while (!auxlogin);

            return auxlogin;
        }
    }
}
