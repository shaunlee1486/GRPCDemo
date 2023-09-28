using Grpc.Core;

namespace GrpcServiceApp2.Services
{
    public class SubtractService : Subtract.SubtractBase
    {
        public override async Task<SubtractResponse> Subtract(SubtractRequest request, ServerCallContext context)
        {
            await Task.Delay(10000);
            return new SubtractResponse { Result = request.Number1 - request.Number2};
        }
    }
}
