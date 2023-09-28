using Grpc.Core;

namespace GrpcServer.Services
{
    public class StreamDemoService : StreamDemo.StreamDemoBase
    {
        private Random random;

        public StreamDemoService()
        {
            random = new Random();
        }
        public override async Task ServerStreamingDemo(Test request, IServerStreamWriter<Test> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                await responseStream.WriteAsync(new Test { TestMessage = $"Message {i + 1}" });
                var randomNumber = random.Next(1, 5);
                await Task.Delay(randomNumber * 1000);
            }
        }

        public override async Task<Test> ClientStreamingDemo(IAsyncStreamReader<Test> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                Console.WriteLine(requestStream.Current.TestMessage);
            }

            Console.WriteLine("Client streaming completed");

            return new Test { TestMessage = "Sample" };
        }

        public override async Task BidirectionalStreamingDemo(IAsyncStreamReader<Test> requestStream, IServerStreamWriter<Test> responseStream, ServerCallContext context)
        {
            var tasks = new List<Task>();

            while(await requestStream.MoveNext())
            {
                Console.WriteLine($"Received request: {requestStream.Current.TestMessage}");

                var task = Task.Run(async () =>
                {
                    var message = requestStream.Current.TestMessage;
                    var randomNumber = random.Next(1, 5);
                    await Task.Delay (randomNumber * 1000);
                    await responseStream.WriteAsync(new Test { TestMessage = message });
                    Console.WriteLine($"Sent response: {message}");
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("Bidirectional streaming completed");
        }
    }
}
