# SystemIntegration Lektion 2

### Opgave 1: JsonSerilizer og JsonDeserialize

JSON er det mest brugte format til dataudveksling mellem systemer. I denne opgave skal du implementere en simpel JSON-serialisering og deserialisering i C# ved hjælp af `System.Text.Json` biblioteket.

1. Opret en C#-klasse kaldet `Person` med følgende egenskaber:
   - `string Name`
   - `int Age`
   - `string Email`

Du er velkommen til at være kreativ og lave en anden klasse, hvis du foretrækker det.

2. Brug Hello World eksemplet fra sidste lektion og udvid det til at inkludere følgende funktionalitet:
   - Opret en instans af `Person` klassen og initialiser den med nogle værdier.
   - Serialiser `Person` objektet til en JSON-streng ved hjælp af `JsonSerializer`.
   - Send det serialiserede JSON som en besked via RabbitMQ (du kan genbruge koden fra lektion 1 til at sende beskeder).
   - Deserialiser JSON-strengen tilbage til et `Person` objekt på consumer siden.
   - Print egenskaberne for det deserialiserede objekt til konsollen for at bekræfte, at dataene er bevaret korrekt.

### Opgave 2: GateInfo

I denne opgave skal du lave en løsning til håndtering af gate information for en lufthavn. 

I stedet for at bruge en Console Applikation, vil vi bruge et api. 

Start med at oprette et nyt ASP.NET Core Web API projekt. Dette project kommer med en WeatherForecast controller.
Brug NuGet Package Manager til at installere Scalar.ASPNetCore nuget pakken.
i Program.cs filen, tilføj Scalar middleware til pipelinen ved at kalde app.MapScalarApiReference() lige efter app.MapOpenAPI() kaldet.

I filen Properties/launchSettings.json, kan du ændre propertien "LaunchBrowser" til true for at åbne browseren automatisk, når du kører applikationen.
Til en property "launchUrl": "scalar" for at navigere direkte til Scalar API dokumentationen.

Start applikationen og test weatherforecast api'et.

Tilføj en ny controller kaldet GateInfoController. 

Tilføj følgende endpoint til controlleren:

```csharp

[HttpGet(Name = "gateno")]
        public async Task<bool> get([FromHeader] models.Airline airline, [FromHeader] int gateNumber)

```

hvor Airline er en enum med følgende værdier:
    SAS, KLM, NORWEGIAN

Du skal oprette en klasse kaldet `GateInfo`, der indeholder følgende egenskaber:

- `int GateNumber`
- `string flightNumber`

På baggrund af hvad du får ind som parametre, skal du oprette et passende GateInfo objekt og sende dit via RabbitMQ til en consumer, der modtager beskeden og printer gate informationen til konsollen.

Det kan være rart at skrive logning i din api, så du kan se at beskeden er sendt.

Tilføj dette til Controlleren. 

```csharp

       private ILogger<AirportController> _logger;

        public AirportController(ILogger<AirportController> logger)
        {
            _logger = logger;
        }
```

for at få dependency injection af en logger. Brug

```csharp

_logger.LogInformation("Your log message here");

```

til at logge en besked til consollen.

---


Lav en ConsolApp til Consumeren, lige som i sidste lektion. Brug args til at sætte hvilken kø der skal lyttes på.

Du kan sende en parameter til din consol app ved at køre dotnet run -- queueName i terminalen fra den folder hvor din consol app ligger.

Eksempel:

```bash
dotnet run -- sas
```

### Opgave 3: Competing Consumers

I denne opgave skal du implementere competing consumers mønsteret ved hjælp af RabbitMQ i C#. Competing consumers betyder, at flere forbrugere (consumers) lytter på den samme kø, og beskederne fordeles mellem dem. Dette kan hjælpe med at forbedre ydeevnen og skalerbarheden af din applikation.

Følg denne https://www.rabbitmq.com/tutorials/tutorial-two-dotnet tutorial for at implementere competing consumers.

De kalder disse køer for "work queues", kært barn har åbenbart mange navne.
