using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Common.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controller;

public class BasketsController : BaseController
{
    private readonly IBasketRepository _basketRepository;
    private readonly ICustomLogger<BasketsController> _logger;

    public BasketsController(IBasketRepository basketRepository, ICustomLogger<BasketsController> logger)
    {
        _basketRepository = basketRepository;
        _logger = logger;
    }

    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Cart>> GetBasket([Required] string userName)
    {
        try
        {
            _logger.Info($"Starting to retrieve basket for user: {userName}");
            
            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.Warn($"Invalid username provided: {userName}");
                return BadRequest("Username cannot be null or empty");
            }

            var result = await _basketRepository.GetBasketByUserName(userName);
            
            if (result == null)
            {
                _logger.Info($"No basket found for user: {userName}, creating new empty basket");
                result = new Cart(userName);
            }
            else
            {
                _logger.Info($"Successfully retrieved basket for user: {userName} with {result.Items?.Count ?? 0} items");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.Err(ex, $"Error occurred while retrieving basket for user: {userName}");
            return StatusCode(500, "An error occurred while retrieving the basket");
        }
    }

    [HttpPost(Name = "UpdateBasket")]
    [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Cart basket)
    {
        try
        {
            _logger.Info($"Starting to update basket for user: {basket?.UserName}");
            
            if (basket == null)
            {
                _logger.Warn("Basket object is null in update request");
                return BadRequest("Basket cannot be null");
            }
            
            if (string.IsNullOrWhiteSpace(basket.UserName))
            {
                _logger.Warn("Username is null or empty in basket update request");
                return BadRequest("Username cannot be null or empty");
            }

            _logger.Debug($"Setting cache options for basket update - Absolute expiration: 10 minutes, Sliding expiration: 2 minutes");
            var options = new DistributedCacheEntryOptions()
                //set the absolute expiration time.
                .SetAbsoluteExpiration(DateTime.UtcNow.AddMinutes(10))
                //a cached object will be expired if it not being requested for a defined amount of time period.
                //Sliding Expiration should always be set lower than the absolute expiration.
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            var result = await _basketRepository.UpdateBasket(basket, options);
            
            _logger.Info($"Successfully updated basket for user: {basket.UserName} with {basket.Items?.Count ?? 0} items");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.Err(ex, $"Error occurred while updating basket for user: {basket?.UserName}");
            return StatusCode(500, "An error occurred while updating the basket");
        }
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Accepted)]
    public async Task<IActionResult> DeleteBasket([Required] string userName)
    {
        try
        {
            _logger.Info($"Starting to delete basket for user: {userName}");
            
            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.Warn($"Invalid username provided for deletion: {userName}");
                return BadRequest("Username cannot be null or empty");
            }

            await _basketRepository.DeleteBasketFromUserName(userName);
            
            _logger.Info($"Successfully deleted basket for user: {userName}");
            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.Err(ex, $"Error occurred while deleting basket for user: {userName}");
            return StatusCode(500, "An error occurred while deleting the basket");
        }
    }
}
