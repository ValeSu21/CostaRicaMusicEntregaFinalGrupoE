using Microsoft.Data.Sqlite;

namespace CostaRicaMusicDAL.Data;

public class DatabaseInitializer
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public DatabaseInitializer(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public void Initialize()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        var createScript = @"
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Usuarios (
    IdUsuario INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL,
    Rol TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Artistas (
    IdArtista INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Genero TEXT NOT NULL,
    Pais TEXT NOT NULL,
    Biografia TEXT NOT NULL,
    ImagenUrl TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Albumes (
    IdAlbum INTEGER PRIMARY KEY AUTOINCREMENT,
    Titulo TEXT NOT NULL,
    IdArtista INTEGER NOT NULL,
    Anio INTEGER NOT NULL,
    PortadaUrl TEXT NOT NULL,
    FOREIGN KEY (IdArtista) REFERENCES Artistas(IdArtista)
);

CREATE TABLE IF NOT EXISTS Canciones (
    IdCancion INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    IdArtista INTEGER NOT NULL,
    IdAlbum INTEGER NOT NULL,
    Duracion TEXT NOT NULL,
    AudioUrl TEXT NOT NULL,
    ImagenUrl TEXT NOT NULL,
    FOREIGN KEY (IdArtista) REFERENCES Artistas(IdArtista),
    FOREIGN KEY (IdAlbum) REFERENCES Albumes(IdAlbum)
);

CREATE TABLE IF NOT EXISTS Playlists (
    IdPlaylist INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Descripcion TEXT NOT NULL,
    IdUsuario INTEGER NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE IF NOT EXISTS PlaylistCanciones (
    IdPlaylist INTEGER NOT NULL,
    IdCancion INTEGER NOT NULL,
    PRIMARY KEY (IdPlaylist, IdCancion),
    FOREIGN KEY (IdPlaylist) REFERENCES Playlists(IdPlaylist) ON DELETE CASCADE,
    FOREIGN KEY (IdCancion) REFERENCES Canciones(IdCancion)
);
";

        using (var createCommand = connection.CreateCommand())
        {
            createCommand.CommandText = createScript;
            createCommand.ExecuteNonQuery();
        }

        EnsureDemoUsers(connection);

        if (!TableHasData(connection, "Artistas") || NeedsCatalogRefresh(connection))
        {
            ReseedCatalog(connection);
        }
    }

    private static void EnsureDemoUsers(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = @"
INSERT OR IGNORE INTO Usuarios (IdUsuario, Nombre, Email, Password, Rol) VALUES
(1, 'Administrador', 'admin@crmusic.com', 'Admin123!', 'Administrador'),
(2, 'Usuario Demo', 'user@crmusic.com', 'User123!', 'Usuario');";
        command.ExecuteNonQuery();
    }

    private static bool NeedsCatalogRefresh(SqliteConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Artistas WHERE Nombre = 'Harry Styles';";
        var result = Convert.ToInt32(command.ExecuteScalar());
        return result == 0;
    }

    private static void ReseedCatalog(SqliteConnection connection)
    {
        var seedScript = @"
PRAGMA foreign_keys = OFF;
DELETE FROM PlaylistCanciones;
DELETE FROM Playlists;
DELETE FROM Canciones;
DELETE FROM Albumes;
DELETE FROM Artistas;
DELETE FROM sqlite_sequence WHERE name IN ('Artistas', 'Albumes', 'Canciones', 'Playlists');

INSERT INTO Artistas (IdArtista, Nombre, Genero, Pais, Biografia, ImagenUrl) VALUES
(1, 'Harry Styles', 'Pop Rock', 'Reino Unido', 'Cantautor británico reconocido por combinar pop melódico, estética elegante y un sonido emocionalmente directo.', '/media/images/artistas/harry-styles.svg'),
(2, 'System of a Down', 'Metal Alternativo', 'Estados Unidos / Armenia', 'Banda icónica de metal alternativo con riffs agresivos, cambios impredecibles y una identidad política muy marcada.', '/media/images/artistas/system-of-a-down.svg'),
(3, 'Soda Stereo', 'Rock en Español', 'Argentina', 'Trío fundamental del rock latino, célebre por su sonido sofisticado, himnos generacionales y enorme legado cultural.', '/media/images/artistas/soda-stereo.svg'),
(4, 'Beyoncé', 'Pop / R&B', 'Estados Unidos', 'Artista global de pop y R&B conocida por su potencia vocal, dirección visual impecable y propuestas escénicas ambiciosas.', '/media/images/artistas/beyonce.svg'),
(5, 'Tyler, The Creator', 'Hip-Hop Alternativo', 'Estados Unidos', 'Rapper, productor y director creativo que mezcla beats experimentales, storytelling íntimo y una estética visual muy definida.', '/media/images/artistas/tyler-the-creator.svg'),
(6, 'Bad Bunny', 'Reggaetón / Latin Trap', 'Puerto Rico', 'Figura clave de la música urbana latina, con una propuesta versátil que cruza reggaetón, trap y experimentación pop.', '/media/images/artistas/bad-bunny.svg');

INSERT INTO Albumes (IdAlbum, Titulo, IdArtista, Anio, PortadaUrl) VALUES
(1, 'Harry Styles', 1, 2017, '/media/images/albums/harry-styles.svg'),
(2, 'Toxicity', 2, 2001, '/media/images/albums/toxicity.svg'),
(3, 'Canción Animal', 3, 1990, '/media/images/albums/cancion-animal.svg'),
(4, 'I Am... Sasha Fierce', 4, 2008, '/media/images/albums/i-am-sasha-fierce.svg'),
(5, 'Flower Boy', 5, 2017, '/media/images/albums/flower-boy.svg'),
(6, 'Un Verano Sin Ti', 6, 2022, '/media/images/albums/un-verano-sin-ti.svg');

INSERT INTO Canciones (IdCancion, Nombre, IdArtista, IdAlbum, Duracion, AudioUrl, ImagenUrl) VALUES
(1, 'Sign of the Times', 1, 1, '05:41', '/media/audio/aurora-lenta.wav', '/media/images/canciones/sign-of-the-times.svg'),
(2, 'Chop Suey!', 2, 2, '03:30', '/media/audio/cafe-y-codigo.wav', '/media/images/canciones/chop-suey.svg'),
(3, 'De Música Ligera', 3, 3, '03:27', '/media/audio/brisa-en-san-jose.wav', '/media/images/canciones/de-musica-ligera.svg'),
(4, 'Halo', 4, 4, '04:21', '/media/audio/kilometro-27.wav', '/media/images/canciones/halo.svg'),
(5, 'See You Again', 5, 5, '03:00', '/media/audio/neblina-azul.wav', '/media/images/canciones/see-you-again.svg'),
(6, 'Tití Me Preguntó', 6, 6, '04:03', '/media/audio/pausa.wav', '/media/images/canciones/titi-me-pregunto.svg');

INSERT INTO Playlists (IdPlaylist, Nombre, Descripcion, IdUsuario) VALUES
(1, 'Hail Mary Mix', 'Selección demo con artistas internacionales y estética sci-fi.', 2);

INSERT INTO PlaylistCanciones (IdPlaylist, IdCancion) VALUES
(1, 1),
(1, 3),
(1, 5);
PRAGMA foreign_keys = ON;";

        using var seedCommand = connection.CreateCommand();
        seedCommand.CommandText = seedScript;
        seedCommand.ExecuteNonQuery();
    }

    private static bool TableHasData(SqliteConnection connection, string tableName)
    {
        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT EXISTS(SELECT 1 FROM {tableName} LIMIT 1);";
        var result = Convert.ToInt32(command.ExecuteScalar());
        return result == 1;
    }
}
