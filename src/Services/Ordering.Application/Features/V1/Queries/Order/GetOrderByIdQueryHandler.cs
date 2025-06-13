using AutoMapper;
using MediatR;
using Ordering.Application.Common.DTOs;
using Ordering.Application.Common.Interfaces;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Queries.Order;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResult<OrderDto>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;

    public GetOrderByIdQueryHandler(IMapper mapper, IOrderRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<ApiResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderEntity = await _repository.GetByIdAsync(request.Id);
        if (orderEntity == null)
        {
            return new ApiErrorResult<OrderDto>("Order not found");
        }

        var orderDto = _mapper.Map<OrderDto>(orderEntity);
        return new ApiSuccessResult<OrderDto>(orderDto);
    }
}