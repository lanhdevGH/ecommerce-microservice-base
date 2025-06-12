using MediatR;
using Ordering.Application.Common.DTOs;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Queries.Order;

public class GetOrdersQuery : IRequest<ApiResult<List<OrderDto>>>
{
    public string UserName { get; set; }

    public GetOrdersQuery(string userName)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
    }
}
