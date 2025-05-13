
using System.Text;
using ClaveSimetricaClass;
using ClaveAsimetricaClass;
using Crypto.Utils;

namespace SimuladorEnvioRecepcion
{
    class Program
    {   
        static string? UserName;
        static string? SecurePass;
        static string? Salt;
        static bool login;
        static Dictionary<string, string> DataHash;
        static ClaveAsimetrica Emisor = new ClaveAsimetrica();
        static ClaveAsimetrica Receptor = new ClaveAsimetrica();
        static ClaveSimetrica ClaveSimetricaEmisor = new ClaveSimetrica();
        static ClaveSimetrica ClaveSimetricaReceptor = new ClaveSimetrica();

        static string TextoAEnviar = "Me he dado cuenta que incluso las personas que dicen que todo está predestinado y que no podemos hacer nada para cambiar nuestro destino igual miran antes de cruzar la calle. Stephen Hawking.";
        static byte[] TextoAEnviar_Bytes;

        static byte[] FirmaEmisor;
        static byte[] MensajeCifradoSimetrico;
        static byte[] ClaveSimetricaKEYCifrada;
        static byte[] ClaveSimetricaIVCifrada;

        static void Main(string[] args)
        {
            /****INICIO PARTE 1****/
            
            //Login - Registro
            Console.WriteLine ("¿Deseas registrarte? (S/N)");
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
                Console.WriteLine("\nNo hay ningún usuario almacenado");
            }

            /****FIN PARTE 1****/

            /****INICIO PARTE 2****/

            if (login)
            {
                TextoAEnviar_Bytes = Encoding.UTF8.GetBytes(TextoAEnviar);
                Console.WriteLine($"\nTexto a enviar bytes:\n{CryptoUtils.BytesToStringHex(TextoAEnviar_Bytes)}");
                
                /// -------------
                /// LADO EMISOR |
                /// -------------

                //Firmar mensaje

                //Cifrar mensaje con la clave simétrica

                //Cifrar clave simétrica con la clave pública del receptor


                /// ---------------
                /// LADO RECEPTOR |
                /// ---------------

                //Descifrar clave simétrica
 
                //Descifrar mensaje con la clave simétrica

                //Comprobar firma
            }

            /****FIN PARTE 2****/
        }

        public static void Registro()
        {
            Console.WriteLine ("\nIndica tu nombre de usuario:");
            UserName = Console.ReadLine();
            //Una vez obtenido el nombre de usuario lo guardamos en la variable UserName y este ya no cambiará 

            Console.WriteLine ("\nIndica tu password:");
            string? passwordRegister = Console.ReadLine();
            //Una vez obtenido el passoword de registro debemos tratarlo como es debido para almacenarlo correctamente a la variable SecurePass

            /****PARTE 1****/

            //Se calcula un 'Hash' (SHA512) iterado 10000 veces y se almacena junto a su 'salt' generado aleatoriamente.
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
                Console.WriteLine ("\nACCESO A LA APLICACIÓN");
                Console.WriteLine ("\nUsuario: ");
                string userName = Console.ReadLine();

                Console.WriteLine ("\nPassword: ");
                string Password = Console.ReadLine();

                /****PARTE 1****/

                //Se calcula el Hash iterado con el 'salt' y el 'Hash' obtenidos previamente y se procesa su comprobación
                if (UserName == userName && CryptoUtils.VerificarPassword(Password!, SecurePass!, Salt!))
                {
                    Console.WriteLine("\nLogin correcto");
                    auxlogin = true;
                }
                else if (UserName != userName)
                {
                    Console.WriteLine("\n# Usuario inexistente #");
                }
                else
                {
                    Console.WriteLine("\n# Contraseña incorrecta #");
                }
            }while (!auxlogin);

            return auxlogin;
        }
    }
}
