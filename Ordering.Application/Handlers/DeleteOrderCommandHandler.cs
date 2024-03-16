using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entitiies;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class DeleteOrderCommandHandler(
    IOrderRepository _orderRepository,
    ILogger<DeleteOrderCommandHandler> _logger
) : IRequestHandler<DeleteOrderCommand>
{
    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
        if (orderToDelete == null)
        {
            throw new OrderNotFoundException(nameof(Order), request.Id);
        }

        await _orderRepository.DeleteAsync(orderToDelete);
        _logger.LogInformation("Order with Id {requestId} is deleted successfully.", request.Id);
    }
}
