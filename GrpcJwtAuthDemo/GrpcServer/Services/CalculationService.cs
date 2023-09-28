using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace GrpcServer.Services
{
    //[Authorize]
    public class CalculationService : Calculation.CalculationBase
    {
        [Authorize(Roles = "Administrator")]
        public override Task<CalculationResult> Add(InputNubers request, ServerCallContext context)
        {
            return Task.FromResult(new CalculationResult { Result = request.Number1 + request.Number2 });
        }

        [Authorize(Roles = "Administrator,User")]
        public override Task<CalculationResult> Subtract(InputNubers request, ServerCallContext context)
        {
            return Task.FromResult(new CalculationResult { Result = request.Number1 - request.Number2 });
        }

        [AllowAnonymous]
        public override Task<CalculationResult> Multiply(InputNubers request, ServerCallContext context)
        {
            return Task.FromResult(new CalculationResult { Result = request.Number1 * request.Number2 });
        }
    }
}
