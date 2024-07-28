using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class GetOrderListQueryHandler(IOrderRepository _orderRepository, IMapper _mapper)
    : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
{
    public async Task<List<OrderResponse>> Handle(
        GetOrderListQuery request,
        CancellationToken cancellationToken
    )
    {
        var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
        return _mapper.Map<List<OrderResponse>>(orderList);
    }
}
