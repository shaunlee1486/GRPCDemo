using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcService;

namespace GrpcClient
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            await GetFullName(channel);

            await SaveProduct(channel);

            await GetProducts(channel);

            await channel.ShutdownAsync();

            Console.ReadKey();
        }

        static async Task GetFullName(GrpcChannel channel)
        {
            var client = new Sample.SampleClient(channel);
            var response = await client.GetFullNameAsync(new SampleRequest { FirstName = "shaun", LastName = "lee" });

            Console.Out.WriteLine(response.FullName);
        }


        static async Task SaveProduct(GrpcChannel channel)
        {
            var client = new Product.ProductClient(channel);

            var stockDate = DateTime.SpecifyKind(new DateTime(2022, 2, 1), DateTimeKind.Utc);
            var response = await client.SaveProductAsync(new ProductModel
            {
                ProductName = "Legion pro 5",
                ProductCode = "LG005",
                Price = 3200,
                StockDate = Timestamp.FromDateTime(stockDate),
            });

            Console.Out.WriteLine($"{response.IsSuccessful} | {response.StatusCode}");
        }

        static async Task GetProducts(GrpcChannel channel)
        {
            var client = new Product.ProductClient(channel);
            var response = await client.GetProductsAsync(new Google.Protobuf.WellKnownTypes.Empty());

            foreach (var product in response.Products)
            {
                Console.Out.WriteLine($"{product.ProductName} | {product.ProductCode} | {product.Price} | {product.StockDate.ToDateTime().ToString("dd/MM/yyyy")}");
            }
        }
    }
}