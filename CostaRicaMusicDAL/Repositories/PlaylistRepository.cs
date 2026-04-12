using CostaRicaMusicDAL.Data;
using CostaRicaMusicDAL.Entidades;

namespace CostaRicaMusicDAL.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public PlaylistRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<Playlist> GetByUser(string email)
    {
        var playlists = new List<Playlist>();

        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
SELECT p.IdPlaylist, p.Nombre, p.Descripcion, u.Email
FROM Playlists p
INNER JOIN Usuarios u ON u.IdUsuario = p.IdUsuario
WHERE lower(u.Email) = lower($email)
ORDER BY p.Nombre;";
        command.Parameters.AddWithValue("$email", email);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            playlists.Add(new Playlist
            {
                IdPlaylist = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Descripcion = reader.GetString(2),
                UsuarioEmail = reader.GetString(3),
                CancionesIds = new List<int>()
            });
        }

        foreach (var playlist in playlists)
        {
            playlist.CancionesIds = GetSongIdsByPlaylist(connection, playlist.IdPlaylist);
        }

        return playlists;
    }

    public Playlist? GetById(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
SELECT p.IdPlaylist, p.Nombre, p.Descripcion, u.Email
FROM Playlists p
INNER JOIN Usuarios u ON u.IdUsuario = p.IdUsuario
WHERE p.IdPlaylist = $id
LIMIT 1;";
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var playlist = new Playlist
        {
            IdPlaylist = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            Descripcion = reader.GetString(2),
            UsuarioEmail = reader.GetString(3),
            CancionesIds = new List<int>()
        };

        playlist.CancionesIds = GetSongIdsByPlaylist(connection, playlist.IdPlaylist);
        return playlist;
    }

    public Playlist Create(Playlist playlist)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        var userId = GetUserIdByEmail(connection, playlist.UsuarioEmail);
        using var transaction = connection.BeginTransaction();

        using (var command = connection.CreateCommand())
        {
            command.Transaction = transaction;
            command.CommandText = @"
INSERT INTO Playlists (Nombre, Descripcion, IdUsuario)
VALUES ($nombre, $descripcion, $idUsuario);
SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$nombre", playlist.Nombre);
            command.Parameters.AddWithValue("$descripcion", playlist.Descripcion);
            command.Parameters.AddWithValue("$idUsuario", userId);
            playlist.IdPlaylist = Convert.ToInt32(command.ExecuteScalar());
        }

        foreach (var songId in playlist.CancionesIds.Distinct())
        {
            InsertPlaylistSong(connection, transaction, playlist.IdPlaylist, songId);
        }

        transaction.Commit();
        return playlist;
    }

    public void Update(Playlist playlist)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();

        using (var updateCommand = connection.CreateCommand())
        {
            updateCommand.Transaction = transaction;
            updateCommand.CommandText = @"
UPDATE Playlists
SET Nombre = $nombre, Descripcion = $descripcion
WHERE IdPlaylist = $id;";
            updateCommand.Parameters.AddWithValue("$nombre", playlist.Nombre);
            updateCommand.Parameters.AddWithValue("$descripcion", playlist.Descripcion);
            updateCommand.Parameters.AddWithValue("$id", playlist.IdPlaylist);
            updateCommand.ExecuteNonQuery();
        }

        using (var deleteSongsCommand = connection.CreateCommand())
        {
            deleteSongsCommand.Transaction = transaction;
            deleteSongsCommand.CommandText = "DELETE FROM PlaylistCanciones WHERE IdPlaylist = $id;";
            deleteSongsCommand.Parameters.AddWithValue("$id", playlist.IdPlaylist);
            deleteSongsCommand.ExecuteNonQuery();
        }

        foreach (var songId in playlist.CancionesIds.Distinct())
        {
            InsertPlaylistSong(connection, transaction, playlist.IdPlaylist, songId);
        }

        transaction.Commit();
    }

    public void Delete(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Playlists WHERE IdPlaylist = $id;";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    private static int GetUserIdByEmail(Microsoft.Data.Sqlite.SqliteConnection connection, string email)
    {
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT IdUsuario FROM Usuarios WHERE lower(Email) = lower($email) LIMIT 1;";
        command.Parameters.AddWithValue("$email", email);
        var result = command.ExecuteScalar();
        if (result is null)
        {
            throw new InvalidOperationException("No se encontró el usuario de la playlist.");
        }

        return Convert.ToInt32(result);
    }

    private static List<int> GetSongIdsByPlaylist(Microsoft.Data.Sqlite.SqliteConnection connection, int playlistId)
    {
        var songIds = new List<int>();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT IdCancion FROM PlaylistCanciones WHERE IdPlaylist = $playlistId ORDER BY IdCancion;";
        command.Parameters.AddWithValue("$playlistId", playlistId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            songIds.Add(reader.GetInt32(0));
        }

        return songIds;
    }

    private static void InsertPlaylistSong(Microsoft.Data.Sqlite.SqliteConnection connection, Microsoft.Data.Sqlite.SqliteTransaction transaction, int playlistId, int songId)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = @"
INSERT OR IGNORE INTO PlaylistCanciones (IdPlaylist, IdCancion)
VALUES ($playlistId, $songId);";
        command.Parameters.AddWithValue("$playlistId", playlistId);
        command.Parameters.AddWithValue("$songId", songId);
        command.ExecuteNonQuery();
    }
}
