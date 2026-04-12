using CostaRicaMusicDAL.Data;
using CostaRicaMusicDAL.Entidades;

namespace CostaRicaMusicDAL.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public UsuarioRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Usuario? GetByCredentials(string email, string password)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = @"
SELECT IdUsuario, Nombre, Email, Password, Rol
FROM Usuarios
WHERE lower(Email) = lower($email) AND Password = $password
LIMIT 1;";
        command.Parameters.AddWithValue("$email", email);
        command.Parameters.AddWithValue("$password", password);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new Usuario
        {
            IdUsuario = reader.GetInt32(0),
            Nombre = reader.GetString(1),
            Email = reader.GetString(2),
            Password = reader.GetString(3),
            Rol = reader.GetString(4)
        };
    }
}
