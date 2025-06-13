using MediatR;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Commands.Order;

public class DeleteOrderCommand : IRequest<ApiResult<bool>>
{
    public long Id { get; set; }

    public DeleteOrderCommand(long id)
    {
        Id = id;
    }
}