# Sistema de Gestión de Planillas de Pago

Este proyecto es una aplicación web MVC desarrollada con .NET 8 para gestionar planillas de pago. La aplicación permite registrar descuentos y bonificaciones diarios en una bitácora y, al finalizar el periodo, calcular los descuentos de ley en El Salvador y generar una planilla de pago y boletas de pago.

## Características

- **Registro de descuentos y bonificaciones**: Permite registrar diariamente descuentos y bonificaciones en una bitácora.
- **Cálculo de planillas**: Suma los descuentos y bonificaciones al finalizar el periodo y calcula los descuentos de ley para generar la planilla de pago.
- **Generación de boletas de pago**: Crea boletas de pago individuales para cada empleado.
- **Autenticación y autorización**: Implementado utilizando Microsoft Identity Core.

## Tecnologías Utilizadas

- **.NET 8**: Framework principal de desarrollo.
- **Bootstrap 5.3**: Framework de CSS para un diseño responsivo y moderno.
- **MySQL**: Sistema de gestión de bases de datos.
- **Entity Framework**: ORM para el manejo de la base de datos.
- **Patrón Repositorio y Unidad de Trabajo**: Para la abstracción y manejo de datos.
- **Microsoft Identity Core**: Para la autenticación y autorización de usuarios.

## Instalación

1. Clona el repositorio a tu máquina local:

    ```bash
      git clone https://github.com/merg8511/SistemaPlanilla.git
    ```

2. Abre el proyecto en Visual Studio o tu editor de código preferido.

3. Asegúrate de tener instalado .NET Core SDK.

4. Configura la base de datos:
   
   Puedes solicitar la base de datos a `merg8511@gmail.com`

## Uso

1. Ejecuta la aplicación en Visual Studio.
2. Accede a través de tu navegador web a `https://localhost:puerto/` para interactuar con el sistema.

## Contribución

Si quieres contribuir a este proyecto, sigue estos pasos:

1. Haz un fork del proyecto.
2. Crea una nueva rama (`git checkout -b feature/nueva-funcionalidad`).
3. Haz tus cambios y haz commit (`git commit -am 'Agrega nueva funcionalidad'`).
4. Haz push a la rama (`git push origin feature/nueva-funcionalidad`).
5. Crea un nuevo Pull Request.

## Créditos

Desarrollado por Mario Rodríguez.
