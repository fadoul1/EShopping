using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entitiies;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class CheckoutOrderCommandHandler(
    IOrderRepository _orderRepository,
    IMapper _mapper,
    ILogger<CheckoutOrderCommandHandler> _logger
) : IRequestHandler<CheckoutOrderCommand, int>
{
    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request);
        var generatedOrder = await _orderRepository.AddAsync(orderEntity);

        _logger.LogInformation("Order {GeneratedOrder} successfully created.", generatedOrder);
        return generatedOrder.Id;
    }
}
