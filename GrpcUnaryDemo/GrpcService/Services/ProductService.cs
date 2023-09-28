using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService;

namespace GrpcService.Services
{
    public class ProductService : Product.ProductBase
    {
        public override Task<ProductSaveResponse> SaveProduct(ProductModel request, ServerCallContext context)
        {
            // insert method to the database

            Console.WriteLine($"{request.ProductName} | {request.ProductCode} | {request.Price} | {request.StockDate.ToDateTime().ToString("dd/MM/yyyy")}");

            var result = new ProductSaveResponse
            {
                StatusCode = 200,
                IsSuccessful = true,
            };

            return Task.FromResult(result);
        }

        public override Task<ProductList> GetProducts(Empty request, ServerCallContext context)
        {
            var result = new ProductList();
            result.Products.AddRange(new List<ProductModel>()
            {
                new ProductModel
                {
                    ProductName = "Legion pro 5",
                    ProductCode = "LG005",
                    Price = 35000,
                    StockDate = Timestamp.FromDateTime(DateTime.SpecifyKind(new DateTime(2022, 4, 2), DateTimeKind.Utc))
                },
                new ProductModel
                {
                    ProductName = "Acer nitro 5",
                    ProductCode = "AC005",
                    Price = 35000,
                    StockDate = Timestamp.FromDateTime(DateTime.SpecifyKind(new DateTime(2022, 5, 3), DateTimeKind.Utc))
                },
                new ProductModel
                {
                    ProductName = "Zenbook 5 pro",
                    ProductCode = "AB005",
                    Price = 35000,
                    StockDate = Timestamp.FromDateTime(DateTime.SpecifyKind(new DateTime(2022, 6, 4), DateTimeKind.Utc))
                },
            }); ;

            return Task.FromResult(result);
        }
    }
}
