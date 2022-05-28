using System.Text;
using MediatR;
using Web.Extensions;

namespace Web.Handlers.Home.Queries;

public class IndexRequest : IRequest<object> { }

public class IndexRequestHandler : IRequestHandler<IndexRequest, object>
{
    public Task<object> Handle(IndexRequest request, CancellationToken cancellationToken) =>
        Task.FromResult<object>(new
        {
            ItWorks = true,
            CanYouReadIt = new byte[256].Also(new Random().NextBytes)
                                        .Let(Encoding.UTF8.GetString)
        });
}