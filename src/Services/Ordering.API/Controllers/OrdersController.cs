using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.DTOs;
using Ordering.Application.Features.V1.Queries.Order;
using Ordering.Application.Features.V1.Commands.Order;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Common.Logging;

namespace Ordering.API.Controllers;

public class OrdersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ICustomLogger<OrdersController> _logger;

    public OrdersController(IMediator mediator, ICustomLogger<OrdersController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private static class RouteNames
    {
        public const string GetOrder = nameof(GetOrder);
        public const string GetOrderById = nameof(GetOrderById);
        public const string GetAllOrders = nameof(GetAllOrders);
        public const string CreateOrder = nameof(CreateOrder);
        public const string UpdateOrder = nameof(UpdateOrder);
        public const string DeleteOrder = nameof(DeleteOrder);
    }

    [HttpGet(Name = RouteNames.GetAllOrders)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetAllOrders()
    {
        _logger.Info("Getting all orders");
        var query = new GetAllOrdersQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:long}", Name = RouteNames.GetOrderById)]
    [ProducesResponseType(typeof(OrderDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> GetOrderById([Required] long id)
    {
        _logger.Info($"Getting order by ID: {id}");
        var query = new GetOrderByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("user/{userName}", Name = RouteNames.GetOrder)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetOrdersByUserName([Required] string userName)
    {
        _logger.Info($"Getting orders for user: {userName}");
        var query = new GetOrdersQuery(userName);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost(Name = RouteNames.CreateOrder)]
    [ProducesResponseType(typeof(long), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        _logger.Info($"Creating new order for user: {createOrderDto.UserName}");
        var command = new CreateOrderCommand(createOrderDto);
        var result = await _mediator.Send(command);
        
        if (result.IsSucceeded)
        {
            return CreatedAtRoute(RouteNames.GetOrderById, new { id = result.Data }, result);
        }
        return BadRequest(result);
    }

    [HttpPut("{id:long}", Name = RouteNames.UpdateOrder)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> UpdateOrder([Required] long id, [FromBody] UpdateOrderDto updateOrderDto)
    {
        _logger.Info($"Updating order with ID: {id}");
        var command = new UpdateOrderCommand(id, updateOrderDto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:long}", Name = RouteNames.DeleteOrder)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> DeleteOrder([Required] long id)
    {
        _logger.Info($"Deleting order with ID: {id}");
        var command = new DeleteOrderCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
