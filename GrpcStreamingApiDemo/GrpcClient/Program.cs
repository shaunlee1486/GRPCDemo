using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    internal class Program
    {
        private static Random random = new Random();

        private static async Task Main(string[] args)
        {
            //await SeverStreamingDemo();
            //await ClientStreamingDemo();
            await BidirectionStreamingDemo();
            Console.WriteLine("Hello, World!");
            Console.ReadLine();
        }

        private static async Task BidirectionStreamingDemo()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5147");
            var client = new StreamDemo.StreamDemoClient(channel);
            var stream = client.BidirectionalStreamingDemo();

            var requestTask = Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var randomNumber = random.Next(1, 5);
                    await Task.Delay(randomNumber * 1000);
                    await stream.RequestStream.WriteAsync(new Test { TestMessage = $"Message {i + 1}" });
                    Console.WriteLine($"Send request: {i+1}");
                }
                await stream.RequestStream.CompleteAsync();
            });

            var responseTask = Task.Run(async () =>
            {
                while (await stream.ResponseStream.MoveNext(CancellationToken.None))
                {
                    Console.WriteLine($"Received response: {stream.ResponseStream.Current.TestMessage}");
                }

                Console.WriteLine("Response stream completed");
            });

            await Task.WhenAll(requestTask, responseTask);
            await channel.ShutdownAsync();
        }

        private static async Task ClientStreamingDemo()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5147");
            var client = new StreamDemo.StreamDemoClient(channel);
            var stream = client.ClientStreamingDemo();
            for (int i = 0; i < 10; i++)
            {
                await stream.RequestStream.WriteAsync(new Test { TestMessage = $"Message {i+1}" });
                var randomNumber = random.Next(1, 5);
                await Task.Delay(randomNumber*1000);
            }

            await stream.RequestStream.CompleteAsync();

            await channel.ShutdownAsync();
        }



        private static async Task SeverStreamingDemo()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5147");
            var client = new StreamDemo.StreamDemoClient(channel);

            var response = client.ServerStreamingDemo(new Test { TestMessage = "Hello test" });
            while (await response.ResponseStream.MoveNext(CancellationToken.None))
            {
                var value = response.ResponseStream.Current.TestMessage;
                Console.Out.WriteLine(value);
            }

            Console.Out.WriteLine("Server streaming completed");
            await channel.ShutdownAsync();
        }
    }
}