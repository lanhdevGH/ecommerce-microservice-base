using AutoMapper;
using MediatR;
using Ordering.Application.Common.DTOs;
using Ordering.Application.Common.Interfaces;
using Shared.SeedWork;
using Common.Logging;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Features.V1.Queries.Order;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, ApiResult<List<OrderDto>>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ILogger<GetAllOrdersQueryHandler> _logger;

    public GetAllOrdersQueryHandler(IMapper mapper, IOrderRepository repository, ILogger<GetAllOrdersQueryHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<List<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all orders");

        try
        {
            var orderEntities = _repository.FindAll();
            var orderList = _mapper.Map<List<OrderDto>>(orderEntities);

            _logger.LogInformation("Retrieved {Count} orders", orderList.Count);
            return new ApiSuccessResult<List<OrderDto>>(orderList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all orders");
            return new ApiErrorResult<List<OrderDto>>("Failed to retrieve orders");
        }
    }
}