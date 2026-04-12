# Costa Rica Music

## Integrantes del grupo
- Completar por el grupo

## Repositorio
- Completar con el enlace del repositorio

## Arquitectura del proyecto
La solución está separada en 3 capas:
- **CostaRicaMusicProyectoFinal**: proyecto web ASP.NET Core MVC con controladores, vistas, autenticación por cookies y reproductor.
- **CostaRicaMusicBLL**: lógica de negocio, DTOs, interfaces y servicios.
- **CostaRicaMusicDAL**: entidades, repositorios y acceso a datos con SQLite.

## Base de datos
Se utiliza **SQLite** con creación automática de la base de datos al iniciar la aplicación.
El archivo se genera en `CostaRicaMusicProyectoFinal/App_Data/costaricamusic.db`.

## Librerías o paquetes NuGet
- `Microsoft.Data.Sqlite`
- `Microsoft.AspNetCore.Authentication.Cookies`

## Principios SOLID y patrones aplicados
- **Responsabilidad Única**: cada capa separa UI, negocio y acceso a datos.
- **Inversión de Dependencias**: los servicios consumen interfaces de repositorios.
- **Open/Closed**: la lógica se puede extender sin modificar la estructura principal.
- **Repository Pattern**: encapsula el acceso a datos por entidad.
- **DTO Pattern**: evita exponer directamente las entidades de datos a la UI.

## Credenciales de prueba
- **Administrador**: `admin@crmusic.com` / `Admin123!`
- **Usuario**: `user@crmusic.com` / `User123!`

## Funcionalidades cubiertas
- Login contra base de datos
- Listado paginado de canciones
- Búsqueda por nombre
- Ver información del artista
- Ver álbum y sus canciones
- Crear, editar y eliminar playlists
- Agregar y quitar canciones de playlists
- Reproductor con botones de anterior, play/detener y siguiente
- Persistencia del reproductor mientras el usuario navega por la aplicación

## Cómo ejecutar
1. Abrir la solución `CostaRicaMusicProyectoFinal.sln`.
2. Restaurar paquetes NuGet.
3. Ejecutar el proyecto web `CostaRicaMusicProyectoFinal`.
4. La base de datos y sus datos iniciales se crean automáticamente en el primer inicio.
