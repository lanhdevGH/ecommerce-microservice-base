using AutoMapper;
using Common.Logging;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Enums;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Commands.Order;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ICustomLogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(IMapper mapper, IOrderRepository repository, ICustomLogger<CreateOrderCommandHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Info($"Creating new order for user: {request.UserName}");

        try
        {
            var orderEntity = new Domain.Entities.Order
            {
                UserName = request.UserName,
                TotalPrice = request.TotalPrice,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailAddress = request.EmailAddress,
                ShippingAddress = request.ShippingAddress,
                InvoiceAddress = request.InvoiceAddress,
                Status = EOrderStatus.New
            };

            var result = await _repository.CreateAsync(orderEntity);
            await _repository.SaveChangesAsync();

            _logger.Info($"Order created successfully with ID: {result}");
            return new ApiSuccessResult<long>(result);
        }
        catch (Exception ex)
        {
            _logger.Err(ex, $"Error creating order for user: {request.UserName}");
            return new ApiErrorResult<long>("Failed to create order");
        }
    }
}