using CostaRicaMusicDAL.Data;
using CostaRicaMusicDAL.Entidades;

namespace CostaRicaMusicDAL.Repositories;

public class AlbumRepository : IAlbumRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public AlbumRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<Album> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT IdAlbum, Titulo, IdArtista, Anio, PortadaUrl FROM Albumes ORDER BY Titulo;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new Album
            {
                IdAlbum = reader.GetInt32(0),
                Titulo = reader.GetString(1),
                IdArtista = reader.GetInt32(2),
                Anio = reader.GetInt32(3),
                PortadaUrl = reader.GetString(4)
            };
        }
    }

    public Album? GetById(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT IdAlbum, Titulo, IdArtista, Anio, PortadaUrl FROM Albumes WHERE IdAlbum = $id LIMIT 1;";
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        return new Album
        {
            IdAlbum = reader.GetInt32(0),
            Titulo = reader.GetString(1),
            IdArtista = reader.GetInt32(2),
            Anio = reader.GetInt32(3),
            PortadaUrl = reader.GetString(4)
        };
    }
}
