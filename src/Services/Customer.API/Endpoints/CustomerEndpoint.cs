using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.CustomerDTOs;

namespace Customer.API.Endpoints
{
    public static class CustomerEndpoint
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customer").WithTags("Customer");

            // GET: Get all customers
            group.MapGet("/",
            async ([FromServices] ICustomerService customerService) =>
            {
                return await customerService.GetAllCustomersAsync();
            })
            .WithName("GetAllCustomers")
            .WithDescription("Get all customers");

            // GET: Get customer by ID
            group.MapGet("/{id:int}", 
            async (int id, [FromServices] ICustomerService customerService) => {
                return await customerService.GetCustomerByIdAsync(id);
            })
            .WithName("GetCustomerById")
            .WithDescription("Get customer by ID");

            // GET: Get customer by username
            group.MapGet("/username/{username}", 
            async (string username, [FromServices] ICustomerService customerService) => {
                return await customerService.GetCustomerByUsernameAsync(username);
            })
            .WithName("GetCustomerByUsername")
            .WithDescription("Get customer by username");

            // POST: Create new customer
            group.MapPost("/", 
            async (
                [FromBody] CustomerCreateDTO customerCreate,
                [FromServices] ICustomerService customerService
            ) => {
                return await customerService.CreateCustomerAsync(customerCreate);
            })
            .WithName("CreateCustomer")
            .WithDescription("Create a new customer");

            // PUT: Update customer
            group.MapPut("/{id:int}", 
            async (
                int id,
                [FromBody] CustomerUpdateDTO customerUpdate,
                [FromServices] ICustomerService customerService
            ) => {
                return await customerService.UpdateCustomerAsync(id, customerUpdate);
            })
            .WithName("UpdateCustomer")
            .WithDescription("Update an existing customer");

            // DELETE: Delete customer by ID
            group.MapDelete("/{id:int}", 
            async (int id, [FromServices] ICustomerService customerService) => {
                return await customerService.DeleteCustomerAsync(id);
            })
            .WithName("DeleteCustomer")
            .WithDescription("Delete a customer by ID");
        }
    }
}
