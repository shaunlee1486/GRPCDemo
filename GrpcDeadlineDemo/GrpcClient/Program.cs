using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceApp1;

namespace GrpcClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var number1 = 100;
            var number2 = 30;

            Console.WriteLine($"Number1: {number1}");
            Console.WriteLine($"Number1: {number2}");

            var channel = GrpcChannel.ForAddress("http://localhost:5146");
            var calculationClient = new Calculation.CalculationClient(channel);

            await Sum(calculationClient, number1, number2);
            await Subtract(calculationClient, number1, number2);

            await channel.ShutdownAsync();
            Console.ReadLine();
        }

        private static async Task Subtract(Calculation.CalculationClient calculationClient, int number1, int number2)
        {
            try
            {
                var sumResult = await calculationClient.SumAsync(new CalcRequest
                {
                    Number1 = number1,
                    Number2 = number2
                }, deadline: DateTime.UtcNow.AddSeconds(5));

                Console.WriteLine($"Sum result: {sumResult.Result}");
            }
            catch (RpcException ex)
            {
                if (ex.StatusCode == StatusCode.DeadlineExceeded)
                {
                    Console.WriteLine($"Sum result: Request Timedout");
                }
            }
        }

        private static async Task Sum(Calculation.CalculationClient calculationClient, int number1, int number2)
        {
            try
            {
                var subtractResult = await calculationClient.SubtractAsync(new CalcRequest { Number1 = number1, Number2 = number2 }, deadline: DateTime.UtcNow.AddSeconds(5));

                Console.WriteLine($"Subtraction result: {subtractResult.Result}");
            }
            catch (RpcException ex)
            {
                if (ex.StatusCode == StatusCode.DeadlineExceeded)
                {
                    Console.WriteLine($"Subtraction result: Request Timedout");
                }
            }
        }
    }
}