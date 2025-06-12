using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.DTOs;
using Ordering.Application.Features.V1.Queries.Order;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API.Controllers;

public class OrdersController : BaseController
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    private static class RouteNames
    {
        public const string GetOrder = nameof(GetOrder);
    }

    [HttpGet("{userName}", Name = RouteNames.GetOrder)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required] string userName)
    {
        var query = new GetOrdersQuery(userName);
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }
}
