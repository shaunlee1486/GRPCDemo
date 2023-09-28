using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceApp2;

namespace GrpcServiceApp1.Services
{
    public class CalculationService : Calculation.CalculationBase
    {
        public override async Task<CalcResponse> Sum(CalcRequest request, ServerCallContext context)
        {
            await Task.Delay(10000);
            return new CalcResponse { Result = request.Number1 + request.Number2 };
        }

        public override async Task<CalcResponse> Subtract(CalcRequest request, ServerCallContext context)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5149");
            var subtractClient = new Subtract.SubtractClient(channel);
            var subtractResponse = await subtractClient.SubtractAsync(new SubtractRequest
            {
                Number1 = request.Number1,
                Number2 = request.Number2
            }, deadline: context.Deadline);

            await channel.ShutdownAsync();
            return new CalcResponse { Result = subtractResponse.Result };
        }
    }
}
