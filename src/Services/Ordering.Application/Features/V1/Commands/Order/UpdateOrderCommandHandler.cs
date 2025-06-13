using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Shared.SeedWork;
using Common.Logging;

namespace Ordering.Application.Features.V1.Commands.Order;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<bool>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;
    private readonly ICustomLogger<UpdateOrderCommandHandler> _logger;

    public UpdateOrderCommandHandler(IMapper mapper, IOrderRepository repository, ICustomLogger<UpdateOrderCommandHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<bool>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Info($"Updating order with ID: {request.Id}");

        try
        {
            var existingOrder = await _repository.GetByIdAsync(request.Id);
            if (existingOrder == null)
            {
                _logger.Info($"Order with ID: {request.Id} not found");
                return new ApiErrorResult<bool>("Order not found");
            }
            existingOrder.FirstName = request.FirstName;
            existingOrder.LastName = request.LastName;
            existingOrder.EmailAddress = request.EmailAddress;
            existingOrder.ShippingAddress = request.ShippingAddress;
            existingOrder.InvoiceAddress = request.InvoiceAddress;

            await _repository.UpdateAsync(existingOrder);
            await _repository.SaveChangesAsync();

            _logger.Info($"Order with ID: {request.Id} updated successfully");
            return new ApiSuccessResult<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.Err(ex, $"Error updating order with ID: {request.Id}");
            return new ApiErrorResult<bool>("Failed to update order");
        }
    }
}