using AutoMapper;
using MediatR;
using Ordering.Application.Common.DTOs;
using Ordering.Application.Common.Interfaces;
using Shared.SeedWork;
using Common.Logging;

namespace Ordering.Application.Features.V1.Queries.Order;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ApiResult<List<OrderDto>>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ICustomLogger<GetOrdersQueryHandler> _logger;

    public GetOrdersQueryHandler(IMapper mapper, IOrderRepository repository, ICustomLogger<GetOrdersQueryHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        _logger.Info($"Getting orders for user: {request.UserName}");

        try
        {
            var orderEntities = await _repository.GetOrdersByUserName(request.UserName);
            var orderList = _mapper.Map<List<OrderDto>>(orderEntities);

            _logger.Info($"Retrieved {orderList.Count} orders for user: {request.UserName}");
            return new ApiSuccessResult<List<OrderDto>>(orderList);
        }
        catch (Exception ex)
        {
            _logger.Err(ex, $"Error getting orders for user: {request.UserName}");
            return new ApiErrorResult<List<OrderDto>>("Failed to retrieve orders");
        }
    }
}
