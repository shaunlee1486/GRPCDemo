using Grpc.Core;
using GrpcService;

namespace GrpcService.Services
{
    public class SampleService : Sample.SampleBase
    {
        public override Task<SampleReply> GetFullName(SampleRequest request, ServerCallContext context)
        {
            var result = $"{request.FirstName} {request.LastName}";
            return Task.FromResult( new SampleReply { FullName = result });
        }
    }
}