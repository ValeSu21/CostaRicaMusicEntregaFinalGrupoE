using CostaRicaMusicDAL.Entidades;

namespace CostaRicaMusicDAL.Data;

public static class SeedData
{
    public static List<Artista> Artistas => new()
    {
        new Artista { IdArtista = 1, Nombre = "Harry Styles", Genero = "Pop Rock", Pais = "Reino Unido", Biografia = "Cantautor británico reconocido por combinar pop melódico, estética elegante y un sonido emocionalmente directo.", ImagenUrl = "/media/images/artistas/harry-styles.svg" },
        new Artista { IdArtista = 2, Nombre = "System of a Down", Genero = "Metal Alternativo", Pais = "Estados Unidos / Armenia", Biografia = "Banda icónica de metal alternativo con riffs agresivos, cambios impredecibles y una identidad política muy marcada.", ImagenUrl = "/media/images/artistas/system-of-a-down.svg" },
        new Artista { IdArtista = 3, Nombre = "Soda Stereo", Genero = "Rock en Español", Pais = "Argentina", Biografia = "Trío fundamental del rock latino, célebre por su sonido sofisticado, himnos generacionales y enorme legado cultural.", ImagenUrl = "/media/images/artistas/soda-stereo.svg" },
        new Artista { IdArtista = 4, Nombre = "Beyoncé", Genero = "Pop / R&B", Pais = "Estados Unidos", Biografia = "Artista global de pop y R&B conocida por su potencia vocal, dirección visual impecable y propuestas escénicas ambiciosas.", ImagenUrl = "/media/images/artistas/beyonce.svg" },
        new Artista { IdArtista = 5, Nombre = "Tyler, The Creator", Genero = "Hip-Hop Alternativo", Pais = "Estados Unidos", Biografia = "Rapper, productor y director creativo que mezcla beats experimentales, storytelling íntimo y una estética visual muy definida.", ImagenUrl = "/media/images/artistas/tyler-the-creator.svg" },
        new Artista { IdArtista = 6, Nombre = "Bad Bunny", Genero = "Reggaetón / Latin Trap", Pais = "Puerto Rico", Biografia = "Figura clave de la música urbana latina, con una propuesta versátil que cruza reggaetón, trap y experimentación pop.", ImagenUrl = "/media/images/artistas/bad-bunny.svg" }
    };

    public static List<Album> Albumes => new()
    {
        new Album { IdAlbum = 1, Titulo = "Harry Styles", IdArtista = 1, Anio = 2017, PortadaUrl = "/media/images/albums/harry-styles.svg" },
        new Album { IdAlbum = 2, Titulo = "Toxicity", IdArtista = 2, Anio = 2001, PortadaUrl = "/media/images/albums/toxicity.svg" },
        new Album { IdAlbum = 3, Titulo = "Canción Animal", IdArtista = 3, Anio = 1990, PortadaUrl = "/media/images/albums/cancion-animal.svg" },
        new Album { IdAlbum = 4, Titulo = "I Am... Sasha Fierce", IdArtista = 4, Anio = 2008, PortadaUrl = "/media/images/albums/i-am-sasha-fierce.svg" },
        new Album { IdAlbum = 5, Titulo = "Flower Boy", IdArtista = 5, Anio = 2017, PortadaUrl = "/media/images/albums/flower-boy.svg" },
        new Album { IdAlbum = 6, Titulo = "Un Verano Sin Ti", IdArtista = 6, Anio = 2022, PortadaUrl = "/media/images/albums/un-verano-sin-ti.svg" }
    };

    public static List<Cancion> Canciones => new()
    {
        new Cancion { IdCancion = 1, Nombre = "Sign of the Times", IdArtista = 1, IdAlbum = 1, Duracion = "05:41", AudioUrl = "/media/audio/aurora-lenta.wav", ImagenUrl = "/media/images/canciones/sign-of-the-times.svg" },
        new Cancion { IdCancion = 2, Nombre = "Chop Suey!", IdArtista = 2, IdAlbum = 2, Duracion = "03:30", AudioUrl = "/media/audio/cafe-y-codigo.wav", ImagenUrl = "/media/images/canciones/chop-suey.svg" },
        new Cancion { IdCancion = 3, Nombre = "De Música Ligera", IdArtista = 3, IdAlbum = 3, Duracion = "03:27", AudioUrl = "/media/audio/brisa-en-san-jose.wav", ImagenUrl = "/media/images/canciones/de-musica-ligera.svg" },
        new Cancion { IdCancion = 4, Nombre = "Halo", IdArtista = 4, IdAlbum = 4, Duracion = "04:21", AudioUrl = "/media/audio/kilometro-27.wav", ImagenUrl = "/media/images/canciones/halo.svg" },
        new Cancion { IdCancion = 5, Nombre = "See You Again", IdArtista = 5, IdAlbum = 5, Duracion = "03:00", AudioUrl = "/media/audio/neblina-azul.wav", ImagenUrl = "/media/images/canciones/see-you-again.svg" },
        new Cancion { IdCancion = 6, Nombre = "Tití Me Preguntó", IdArtista = 6, IdAlbum = 6, Duracion = "04:03", AudioUrl = "/media/audio/pausa.wav", ImagenUrl = "/media/images/canciones/titi-me-pregunto.svg" }
    };

    public static List<Usuario> Usuarios => new()
    {
        new Usuario { IdUsuario = 1, Nombre = "Administrador", Email = "admin@crmusic.com", Password = "Admin123!", Rol = "Administrador" },
        new Usuario { IdUsuario = 2, Nombre = "Usuario Demo", Email = "user@crmusic.com", Password = "User123!", Rol = "Usuario" }
    };

    public static List<Playlist> Playlists => new()
    {
        new Playlist { IdPlaylist = 1, Nombre = "Hail Mary Mix", Descripcion = "Selección demo con artistas internacionales y estética sci-fi.", UsuarioEmail = "user@crmusic.com", CancionesIds = new List<int> { 1, 3, 5 } }
    };
}
