using MediatR;
using Ordering.Application.Common.DTOs;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Queries.Order;

public class GetAllOrdersQuery : IRequest<ApiResult<List<OrderDto>>>
{
    public GetAllOrdersQuery()
    {
    }
}