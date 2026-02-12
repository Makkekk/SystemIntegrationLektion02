using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Opgave02_WeatherApp.Controllers;
[ApiController]
[Route("api/[controller]")]

public class GateInfoController : ControllerBase {
    
    private readonly ILogger<GateInfoController> _logger;
    
    public GateInfoController(ILogger<GateInfoController> logger) {
        _logger = logger;
    }

    [HttpGet(Name = "gateno")]

    public async Task<ActionResult<bool>> Get([FromHeader] Airline airline, [FromHeader] int gateNumber) {
        var info = new GateInfo() {
            GateNumber = gateNumber,
            FlightNumber = $"{airline.ToString()}{new Random().Next(100, 999)}"
        };

        try {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "gate_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonSerializer.Serialize(info);
            var body = Encoding.UTF8.GetBytes(message);

            // 3. Send besked
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "gate_queue", body: body);

            // 4. Logning
            _logger.LogInformation($"Besked sendt til RabbitMQ: Gate {info.GateNumber} for {airline}");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved afsendelse til RabbitMQ");
            return false;
        }
    }
}