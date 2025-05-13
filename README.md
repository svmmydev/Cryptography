<div align="center">

# SIMULADOR DE ENVÍO Y RECEPCIÓN CON CIFRADO HÍBRIDO

</div>

Este proyecto simula un sistema de envío seguro de mensajes entre dos partes (Emisor y Receptor), utilizando criptografía híbrida: cifrado simétrico (AES) para el contenido y cifrado asimétrico (RSA) para el intercambio de claves, junto con firma digital (RSA + SHA-512) para garantizar la autenticidad.

<br>

<div align="center">

![Simulación de carretera](Assets/Images/10-mostrar-carretera-en-cliente.png)

</div>

<br>

## Índice

- [Características](#características)
- [Arquitectura del Sistema](#arquitectura-del-sistema)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Estructura del Proyecto](#estructura-del-proyecto)

<br>

## Características

- **Mensaje firmado digitalmente** con SHA-512 y clave privada RSA del emisor.
- **Cifrado del mensaje** usando AES con clave y vector IV generados aleatoriamente.
- **Cifrado de la clave simétrica y IV** con la clave pública RSA del receptor.
- **Verificación de firma** en el lado del receptor con la clave pública del emisor.
- **Visualización paso a paso** de todo el proceso en consola.
- **Hash y salt iterado (10000x)** para el proceso de login seguro.

<br>

## Arquitectura del Sistema

El sistema está completamente basado en consola y simula una comunicación punto a punto:

1. **Registro/Login**:

   - El usuario se registra con un nombre y contraseña.
   - Se genera un salt y un hash seguro con 10.000 iteraciones.

2. **Lado Emisor**:

   - Se firma digitalmente el mensaje original.
   - Se cifra el mensaje con AES.
   - Se cifra la clave AES (key + IV) con la clave pública del receptor.

3. **Lado Receptor**:

   - Descifra la clave AES (key + IV) con su clave privada.
   - Descifra el mensaje con AES.
   - Verifica la firma del emisor.

<br>

## Tecnologías Utilizadas

- **Lenguaje:** C#
- **Criptografía simétrica:** AES (`Aes.Create()`)
- **Criptografía asimétrica:** RSA (`RSACryptoServiceProvider`)
- **Firma digital:** `SignData` y `VerifyData` con SHA-512
- **Derivación de clave:** `Rfc2898DeriveBytes`
- **Comparación segura:** `CryptographicOperations.FixedTimeEquals`
- **Codificación:** UTF-8 / HEX para consola

<br>

## Estructura del Proyecto

<table align="center" border="6px">
    <tr>
        <td>
            <pre>
                SEGURIDAD-CRYPTOGRAFIA
                │
                ├── 🔐 ClaveAsimetricaClass
                │   ├── 📄 Class1.cs
                │   └── 📄 ClaveAsimetricaClass.cs
                │
                ├── 🔐 ClaveSimetricaClass
                │   ├── 📄 Class1.cs
                │   └── 📄 ClaveSimetricaClass.cs
                │
                ├── 💬 SimulacionEnvioRecepcion
                │   ├── 📁 utils
                │   │   └── 🛠️ CryptoUtils.cs
                │   └── 📄 Program.cs
                │
                ├── 📄 CifradoSolution.sln
                ├── 📄 README.md
                └── 📄 seguridad-criptografia.csproj
            </pre>
        </td>
    </tr>
    </table>

<br>

<div align="center">

###### © Sammy

</div>
