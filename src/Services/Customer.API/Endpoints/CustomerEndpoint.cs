using Customer.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.CustomerDTOs;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Endpoints
{
    public static class CustomerEndpoint
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customer").WithTags("Customer");

            // GET: Get all customers
            group.MapGet("/", 
            async ([FromServices] ICustomerRepository customerRepository) => {
                var customers = await customerRepository.FindAll().ToListAsync();
                return Results.Ok(customers);
            })
            .WithName("GetAllCustomers")
            .WithDescription("Get all customers");

            // GET: Get customer by ID
            group.MapGet("/{id:int}", 
            async (int id, [FromServices] ICustomerRepository customerRepository) => {
                var customer = await customerRepository.GetByIdAsync(id);
                if (customer == null)
                    return Results.NotFound($"Customer with ID {id} not found");
                return Results.Ok(customer);
            })
            .WithName("GetCustomerById")
            .WithDescription("Get customer by ID");

            // GET: Get customer by username
            group.MapGet("/username/{username}", 
            async (string username, [FromServices] ICustomerRepository customerRepository) => {
                var customer = await customerRepository.GetCustomerByUsernameAsync(username);
                if (customer == null)
                    return Results.NotFound($"Customer with username {username} not found");
                return Results.Ok(customer);
            })
            .WithName("GetCustomerByUsername")
            .WithDescription("Get customer by username");

            // POST: Create new customer
            group.MapPost("/", 
            async (
                [FromBody] CustomerCreateDTO customerCreate,
                [FromServices] ICustomerRepository customerRepository
            ) => {
                try
                {
                    var customer = new Entities.Customer
                    {
                        FirstName = customerCreate.FirstName,
                        LastName = customerCreate.LastName,
                        EmailAddress = customerCreate.EmailAddress,
                        UserName = customerCreate.UserName
                    };
                    
                    var customerId = await customerRepository.CreateAsync(customer);
                    await customerRepository.SaveChangesAsync();
                    
                    var createdCustomer = await customerRepository.GetByIdAsync(customerId);
                    return Results.Created($"/api/customer/{customerId}", createdCustomer);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error creating customer: {ex.Message}");
                }
            })
            .WithName("CreateCustomer")
            .WithDescription("Create a new customer");

            // PUT: Update customer
            group.MapPut("/{id:int}", 
            async (
                int id,
                [FromBody] CustomerUpdateDTO customerUpdate,
                [FromServices] ICustomerRepository customerRepository
            ) => {
                try
                {
                    var existingCustomer = await customerRepository.GetByIdAsync(id);
                    if (existingCustomer == null)
                        return Results.NotFound($"Customer with ID {id} not found");

                    existingCustomer.UserName = customerUpdate.UserName;
                    existingCustomer.FirstName = customerUpdate.FirstName;
                    existingCustomer.LastName = customerUpdate.LastName;

                    await customerRepository.UpdateAsync(existingCustomer);
                    await customerRepository.SaveChangesAsync();

                    return Results.Ok(existingCustomer);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error updating customer: {ex.Message}");
                }
            })
            .WithName("UpdateCustomer")
            .WithDescription("Update an existing customer");

            // DELETE: Delete customer by ID
            group.MapDelete("/{id:int}", 
            async (int id, [FromServices] ICustomerRepository customerRepository) => {
                try
                {
                    var customer = await customerRepository.GetByIdAsync(id);
                    if (customer == null)
                        return Results.NotFound($"Customer with ID {id} not found");

                    await customerRepository.DeleteAsync(customer);
                    await customerRepository.SaveChangesAsync();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest($"Error deleting customer: {ex.Message}");
                }
            })
            .WithName("DeleteCustomer")
            .WithDescription("Delete a customer by ID");
        }
    }
}
