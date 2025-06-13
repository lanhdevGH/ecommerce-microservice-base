using MediatR;
using Ordering.Application.Common.Interfaces;
using Shared.SeedWork;
using Common.Logging;

namespace Ordering.Application.Features.V1.Commands.Order;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ApiResult<bool>>
{
    private readonly IOrderRepository _repository;
    private readonly ICustomLogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository repository, ICustomLogger<DeleteOrderCommandHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ApiResult<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Info($"Deleting order with ID: {request.Id}");

        try
        {
            var existingOrder = await _repository.GetByIdAsync(request.Id);
            if (existingOrder == null)
            {
                _logger.Info($"Order with ID: {request.Id} not found");
                return new ApiErrorResult<bool>("Order not found");
            }

            await _repository.DeleteAsync(existingOrder);
            await _repository.SaveChangesAsync();

            _logger.Info($"Order with ID: {request.Id} deleted successfully");
            return new ApiSuccessResult<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.Err(ex, $"Error deleting order with ID: {request.Id}");
            return new ApiErrorResult<bool>("Failed to delete order");
        }
    }
}