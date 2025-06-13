using MediatR;
using Ordering.Application.Common.DTOs;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Queries.Order;

public class GetOrderByIdQuery : IRequest<ApiResult<OrderDto>>
{
    public long Id { get; set; }

    public GetOrderByIdQuery(long id)
    {
        Id = id;
    }
}