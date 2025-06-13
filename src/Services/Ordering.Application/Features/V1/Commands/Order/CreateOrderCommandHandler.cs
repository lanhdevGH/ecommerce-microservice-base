using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Shared.SeedWork;
using Common.Logging;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Features.V1.Commands.Order;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(IMapper mapper, IOrderRepository repository, ILogger<CreateOrderCommandHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new order for user: {UserName}", request.UserName);

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

            _logger.LogInformation("Order created successfully with ID: {OrderId}", result);
            return new ApiSuccessResult<long>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order for user: {UserName}", request.UserName);
            return new ApiErrorResult<long>("Failed to create order");
        }
    }
}