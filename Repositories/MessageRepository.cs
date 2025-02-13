using Npgsql;
using SMSService.Interfaces;
using SMSService.Models;

public class MessageRepository : IMessageRepository
{
    private readonly string _connectionString;

    public MessageRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync()
    {
        var messages = new List<Message>();
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new NpgsqlCommand("SELECT Id, Text, OrderNumber, CreatedAt FROM Messages", connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    messages.Add(new Message
                    {
                        Id = reader.GetInt32(0),
                        OrderNumber = reader.GetInt32(0),
                        Text = reader.GetString(1),
                        CreatedAt = reader.GetDateTime(2)
                    });
                }
            }
        }
        return messages;
    }

    public async Task<Message> GetMessageByIdAsync(int id)
    {
        Message message = null;
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand("SELECT Id, Text, OrderNumber, CreatedAt FROM Messages WHERE Id = @id", connection))
            {
                command.Parameters.AddWithValue("id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        message = new Message
                        {
                            Id = reader.GetInt32(0),
                            OrderNumber = reader.GetInt32(0),
                            Text = reader.GetString(1),
                            CreatedAt = reader.GetDateTime(2)
                        };
                    }
                }
            }
        }
        return message;
    }

    public async Task AddMessageAsync(Message message)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand("INSERT INTO Messages (Text, CreatedAt) VALUES (@text, @createdAt)", connection))
            {
                command.Parameters.AddWithValue("text", message.Text);
                command.Parameters.AddWithValue("createdAt", message.CreatedAt);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}