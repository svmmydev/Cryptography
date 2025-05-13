<div align="center">

# Respuestas a las preguntas del enunciado

</div>

<br>

## Registro/Login

Se ha implementado un registro/login mediante un hasheo  iterativo (de 10.000 veces) a través de la clase ‘Rfc2898DeriveBytes’. A su vez, a esta, se le inyecta la función ‘Hash’ SHA512.
Una vez se ha calculado, se almacena tanto el ‘Hash’ generado como el ‘salt’ aleatorio utilizado.
Finalmente, se utiliza el mismo método con la password del input del login, al cual se le pasa el ‘salt’ almacenado previamente. Si la comparación de ambos es exitosa se logea.

<br>

## Explicación del procedimiento

- Conversión del mensaje: el texto plano se convierte en un array de bytes (UTF-8) para poder firmarlo y cifrarlo correctamente.
  
- Firma digital: el emisor firma el mensaje original usando su clave privada mediante SHA-512, lo que garantiza autenticidad e integridad del mensaje.
  
- Cifrado simétrico del mensaje: el mensaje se cifra con AES, utilizando una clave y IV generados aleatoriamente, lo que asegura la confidencialidad del contenido.
  
- Cifrado asimétrico de la clave e IV: tanto la clave como el IV del cifrado simétrico se cifran con la clave pública del receptor, lo que garantiza que solo el receptor pueda descifrarlos con su clave privada.
  
- Receptor descifra y verifica: el receptor descifra la clave e IV, luego el mensaje, y finalmente verifica la firma usando la clave pública del emisor. Si es válida, se asegura de que el contenido no fue alterado y proviene del emisor.

<br>

## ¿Métodos prescindibles de ClaveAsimetrica.cs?

- Sobre los métodos de la clase **‘ClaveAsimetrica’** que no han sido empleados, quizás el único que se podría dejar es el método **‘CifrarMensaje(byte[] MensajeBytes)’** para pruebas. Para testeos locales, como por ejemplo un sistema donde nosotros mismos enviamos un mensaje cifrado por nosotros, para luego descifrar con nuestra propia calve privada. Nunca para proyectos reales.

- Con el método **‘FirmarMensaje(byte[], RSAParameters clavePublicaExterna)’** no conseguiríamos nada, esta definición es completamente errónea. No tiene ningún sentido que firmemos con una clave pública porque cualquiera tiene acceso a ella. Como concepto, a nivel criptográfico, este método está mal programado.
- Como tercero y último, el método **‘DescifrarMensaje(byte[], RSAParameters ClavePublicaExterna)’** también es confuso y no concuerda con lo que realmente debería ser para descifrar un mensaje. Lo ideal es poder descifrar con una clave privada, no con una clave pública, esto también es erróneo conceptualmente.
- Además, hay una línea **inútil** dentro del método de cifrado externo. Me he tomado la libertad de eliminarla:

<div align="center">
    
```byte[] textoPlanoBytes = Encoding.UTF8.GetBytes("hola");```

</div>

<br>
