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

1. **Registro**: se pide usuario y contraseña → se genera `salt` aleatorio y se calcula el hash iterado con SHA512.
2. Se almacena el `salt` y `hash` en variables globales.
3. **Login**: se introduce la contraseña → se recalcula el hash con el `salt` almacenado.
4. Si coincide con el hash guardado, el login es exitoso.
5. Una vez logueado, se procede con la simulación de cifrado híbrido.

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