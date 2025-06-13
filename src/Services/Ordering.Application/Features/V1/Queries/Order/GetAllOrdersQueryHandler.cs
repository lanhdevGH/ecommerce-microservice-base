using AutoMapper;
using MediatR;
using Ordering.Application.Common.DTOs;
using Ordering.Application.Common.Interfaces;
using Shared.SeedWork;
using Common.Logging;

namespace Ordering.Application.Features.V1.Queries.Order;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, ApiResult<List<OrderDto>>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ICustomLogger<GetAllOrdersQueryHandler> _logger;

    public GetAllOrdersQueryHandler(IMapper mapper, IOrderRepository repository, ICustomLogger<GetAllOrdersQueryHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<List<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        _logger.Info("Getting all orders");

        try
        {
            var orderEntities = _repository.FindAll();
            var orderList = _mapper.Map<List<OrderDto>>(orderEntities);

            _logger.Info($"Retrieved {orderList.Count} orders");
            return new ApiSuccessResult<List<OrderDto>>(orderList);
        }
        catch (Exception ex)
        {
            _logger.Err(ex, "Error getting all orders");
            return new ApiErrorResult<List<OrderDto>>("Failed to retrieve orders");
        }
    }
}