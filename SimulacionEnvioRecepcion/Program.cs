
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
        static Dictionary<string, string> DataHash;
        static ClaveAsimetrica Emisor = new ClaveAsimetrica();
        static ClaveAsimetrica Receptor = new ClaveAsimetrica();
        static ClaveSimetrica ClaveSimetricaEmisor = new ClaveSimetrica();
        static ClaveSimetrica ClaveSimetricaReceptor = new ClaveSimetrica();

        static string TextoAEnviar = "Me he dado cuenta que incluso las personas que dicen que todo está predestinado y que no podemos hacer nada para cambiar nuestro destino igual miran antes de cruzar la calle. Stephen Hawking.";
        
        static void Main(string[] args)
        {

            /****PARTE 1****/
            
            //Login / Registro
            Console.WriteLine ("¿Deseas registrarte? (S/N)");
            string? registro = Console.ReadLine();

            if (registro?.ToUpper() == "S")
            {
                //Realizar registro del cliente
                Registro();
            }

            //Realizar login
            bool login = Login();

            /***FIN PARTE 1***/

            if (login)
            {                  
                byte[] TextoAEnviar_Bytes = Encoding.UTF8.GetBytes(TextoAEnviar); 
                Console.WriteLine("Texto a enviar bytes: {0}", CryptoUtils.BytesToStringHex(TextoAEnviar_Bytes));    
                
                //LADO EMISOR

                //Firmar mensaje


                //Cifrar mensaje con la clave simétrica


                //Cifrar clave simétrica con la clave pública del receptor

                //LADO RECEPTOR

                //Descifrar clave simétrica

                
                //Descifrar clave simétrica
 

                //Descifrar mensaje con la clave simétrica


                //Comprobar firma

            }
        }

        public static void Registro()
        {
            Console.WriteLine ("\nIndica tu nombre de usuario:");
            UserName = Console.ReadLine();
            //Una vez obtenido el nombre de usuario lo guardamos en la variable UserName y este ya no cambiará 

            Console.WriteLine ("\nIndica tu password:");
            string? passwordRegister = Console.ReadLine();
            //Una vez obtenido el passoword de registro debemos tratarlo como es debido para almacenarlo correctamente a la variable SecurePass

            /***PARTE 1***/

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

                /***PARTE 1***/

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
