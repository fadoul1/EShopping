using System.Net;
using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

public class BasketController : ApiController
{
    private readonly ISender _sender;

    public BasketController(IMediator mediator)
    {
        _sender = mediator;
    }

    [HttpGet]
    [Route("[action]/{userName}", Name = "GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
    {
        var query = new GetBasketByUserNameQuery(userName);
        var basket = await _sender.Send(query);
        return Ok(basket);
    }

    [HttpPost("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket(
        [FromBody] CreateShoppingCartCommand createShoppingCartCommand
    )
    {
        var basket = await _sender.Send(createShoppingCartCommand);
        return Ok(basket);
    }

    [HttpDelete]
    [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
    {
        var command = new DeleteBasketByUserNameCommand(userName);
        await _sender.Send(command);
        return Ok();
    }
}
