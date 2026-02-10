using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Opgave01;

public class Consumer {
    public async Task StartListening() {
        var factory = new ConnectionFactory() {
            HostName = "localhost"
        };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "person_queue", durable: false, exclusive: false, autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) => {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            Person? modtaget = JsonSerializer.Deserialize<Person>(json);

            Console.WriteLine($"[Consumer] Modtaget og deserialiseret: {json}");
            await Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queue: "person_queue", autoAck: true, consumer: consumer);

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }
}