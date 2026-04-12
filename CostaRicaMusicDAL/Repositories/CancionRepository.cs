using CostaRicaMusicDAL.Data;
using CostaRicaMusicDAL.Entidades;

namespace CostaRicaMusicDAL.Repositories;

public class CancionRepository : ICancionRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public CancionRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<Cancion> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT IdCancion, Nombre, IdArtista, IdAlbum, Duracion, AudioUrl, ImagenUrl FROM Canciones ORDER BY Nombre;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new Cancion
            {
                IdCancion = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                IdArtista = reader.GetInt32(2),
                IdAlbum = reader.GetInt32(3),
                Duracion = reader.GetString(4),
                AudioUrl = reader.GetString(5),
                ImagenUrl = reader.GetString(6)
            };
        }
    }

    public Cancion? GetById(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT IdCancion, Nombre, IdArtista, IdAlbum, Duracion, AudioUrl, ImagenUrl FROM Canciones WHERE IdCancion = $id LIMIT 1;";
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        return new Cancion
        {
            IdCancion = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            IdArtista = reader.GetInt32(2),
            IdAlbum = reader.GetInt32(3),
            Duracion = reader.GetString(4),
            AudioUrl = reader.GetString(5),
            ImagenUrl = reader.GetString(6)
        };
    }
}
