using CostaRicaMusicDAL.Data;
using CostaRicaMusicDAL.Entidades;

namespace CostaRicaMusicDAL.Repositories;

public class ArtistaRepository : IArtistaRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public ArtistaRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<Artista> GetAll()
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT IdArtista, Nombre, Genero, Pais, Biografia, ImagenUrl FROM Artistas ORDER BY Nombre;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            yield return new Artista
            {
                IdArtista = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Genero = reader.GetString(2),
                Pais = reader.GetString(3),
                Biografia = reader.GetString(4),
                ImagenUrl = reader.GetString(5)
            };
        }
    }

    public Artista? GetById(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"SELECT IdArtista, Nombre, Genero, Pais, Biografia, ImagenUrl FROM Artistas WHERE IdArtista = $id LIMIT 1;";
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();
        if (!reader.Read()) return null;

        return new Artista
        {
            IdArtista = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            Genero = reader.GetString(2),
            Pais = reader.GetString(3),
            Biografia = reader.GetString(4),
            ImagenUrl = reader.GetString(5)
        };
    }
}
