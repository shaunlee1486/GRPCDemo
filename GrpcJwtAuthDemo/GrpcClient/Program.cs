using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5162");
            var accessToken = await Authenticate(channel, "admin", "admin");

            if (accessToken == null) return;

            await Calculator(channel, accessToken);

            await channel.ShutdownAsync();
            Console.ReadLine();
        }

        private static Task<string> Authenticate(GrpcChannel channel, string userName, string password)
        {
            try
            {
                var authenticationClient = new Authentication.AuthenticationClient(channel);
                var authenticationResponse = authenticationClient.Authenticate(new AuthenticationRequest
                {
                    UserName = userName,
                    Password = password
                });

                Console.WriteLine($"Received auth response | token: {authenticationResponse.AccessToken} | Expires In: {authenticationResponse.ExpiresIn}");
                return Task.FromResult(authenticationResponse.AccessToken);
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"Status code: {ex.StatusCode} | Error: {ex.Message}");
                return null;
            }
        }

        private static async Task Calculator(GrpcChannel channel, string accessToken)
        {
            try
            {
                var calculationClient = new Calculation.CalculationClient(channel);
                var headers = new Metadata();
                headers.Add("Authorization", $"Bearer {accessToken}");
                //var sumResult = await calculationClient.AddAsync(new InputNubers { Number1 = 1, Number2 = 2 }, headers);
                //Console.WriteLine($"Sum result: 1 + 2 = {sumResult.Result}");

                //var subtractResult = await calculationClient.SubtractAsync(new InputNubers { Number1 = 20, Number2 = 5}, headers);
                //Console.WriteLine($"Subtract result: 20 - 5 = {subtractResult.Result}");

                var multiplyResult = await calculationClient.MultiplyAsync(new InputNubers { Number1 = 5, Number2 = 5});
                Console.WriteLine($"Subtract result: 5*5 = {multiplyResult.Result}");
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"Status code: {ex.StatusCode} | Error: {ex.Message}");
            }
        }
    }
}