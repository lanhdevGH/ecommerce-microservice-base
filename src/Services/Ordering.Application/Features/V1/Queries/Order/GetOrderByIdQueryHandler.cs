using AutoMapper;
using MediatR;
using Ordering.Application.Common.DTOs;
using Ordering.Application.Common.Interfaces;
using Shared.SeedWork;
using Common.Logging;

namespace Ordering.Application.Features.V1.Queries.Order;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResult<OrderDto>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ICustomLogger<GetOrderByIdQueryHandler> _logger;

    public GetOrderByIdQueryHandler(IMapper mapper, IOrderRepository repository, ICustomLogger<GetOrderByIdQueryHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Info($"Getting order by ID: {request.Id}");

        try
        {
            var orderEntity = await _repository.GetByIdAsync(request.Id);
            if (orderEntity == null)
            {
                _logger.Info($"Order with ID: {request.Id} not found");
                return new ApiErrorResult<OrderDto>("Order not found");
            }

            var orderDto = _mapper.Map<OrderDto>(orderEntity);
            _logger.Info($"Order with ID: {request.Id} retrieved successfully");
            return new ApiSuccessResult<OrderDto>(orderDto);
        }
        catch (Exception ex)
        {
            _logger.Err(ex, $"Error getting order with ID: {request.Id}");
            return new ApiErrorResult<OrderDto>("Failed to retrieve order");
        }
    }
}