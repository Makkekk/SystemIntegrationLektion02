using Opgave01;

namespace Opgave01 {
    class Program {
        static async Task Main(string[] args) {
            Console.WriteLine("Vælg 1 for at sende en person eller 2 for at modtage personer:");
            var choice = Console.ReadLine();

            if (choice == "1") {
                var producer = new Producer();
                var person = new Person("Alice", 30, "alice@email.com");
                await producer.SendPerson(person);
            }
            else if (choice == "2") {
                var consumer = new Consumer();
                await consumer.StartListening();
            }
            else {
                Console.WriteLine("Ugyldigt valg.");
            }
        }
    }
}