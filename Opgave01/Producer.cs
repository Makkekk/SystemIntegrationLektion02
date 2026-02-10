using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Opgave01;

public class Producer {

    public async Task SendPerson(Person person) {
        var factory = new ConnectionFactory() {
            HostName = "localhost"
        };
        
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        
        await channel.QueueDeclareAsync(queue: "person_queue", durable: false, exclusive: false, autoDelete: false,
            arguments: null);
        
        string json = JsonSerializer.Serialize(person);
        var body = Encoding.UTF8.GetBytes(json);
        
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "person_queue", body: body);
        Console.WriteLine($"[Producer] Sendt og serialiseret: {json}");
    }
}