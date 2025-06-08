using Common.Logging;
using Customer.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
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
            async ([FromServices] ICustomerRepository customerRepository) =>
            {
                var logger = Log.ForContext("SourceContext", nameof(CustomerEndpoint));

                logger.Info("Starting to fetch all customers from database");

                try
                {
                    var customers = await customerRepository.FindAll().ToListAsync();
                    
                    logger.Info($"Successfully retrieved {customers.Count} customers from database");
                    
                    return Results.Ok(customers);
                }
                catch (Exception ex)
                {
                    logger.Err(ex, "Error occurred while fetching all customers");
                    return Results.Problem("An error occurred while fetching customers");
                }
            })
            .WithName("GetAllCustomers")
            .WithDescription("Get all customers");

            // GET: Get customer by ID
            group.MapGet("/{id:int}", 
            async (int id, [FromServices] ICustomerRepository customerRepository) => {
                var logger = Log.ForContext("SourceContext", nameof(CustomerEndpoint));
                
                logger.Info($"Starting to search for customer with ID: {id}");
                
                try
                {
                    var customer = await customerRepository.GetByIdAsync(id);
                    
                    if (customer == null)
                    {
                        logger.Warn($"Customer not found with ID: {id}");
                        return Results.NotFound($"Customer with ID {id} not found");
                    }
                    
                    logger.Info($"Successfully found customer with ID: {id}, UserName: {customer.UserName}");
                    return Results.Ok(customer);
                }
                catch (Exception ex)
                {
                    logger.Err(ex, $"Error occurred while searching for customer with ID: {id}");
                    return Results.Problem($"An error occurred while searching for customer with ID: {id}");
                }
            })
            .WithName("GetCustomerById")
            .WithDescription("Get customer by ID");

            // GET: Get customer by username
            group.MapGet("/username/{username}", 
            async (string username, [FromServices] ICustomerRepository customerRepository) => {
                var logger = Log.ForContext("SourceContext", nameof(CustomerEndpoint));
                
                logger.Info($"Starting to search for customer with username: {username}");
                
                try
                {
                    var customer = await customerRepository.GetCustomerByUsernameAsync(username);
                    
                    if (customer == null)
                    {
                        logger.Warn($"Customer not found with username: {username}");
                        return Results.NotFound($"Customer with username {username} not found");
                    }
                    
                    logger.Info($"Successfully found customer with username: {username}, ID: {customer.Id}");
                    return Results.Ok(customer);
                }
                catch (Exception ex)
                {
                    logger.Err(ex, $"Error occurred while searching for customer with username: {username}");
                    return Results.Problem($"An error occurred while searching for customer with username: {username}");
                }
            })
            .WithName("GetCustomerByUsername")
            .WithDescription("Get customer by username");

            // POST: Create new customer
            group.MapPost("/", 
            async (
                [FromBody] CustomerCreateDTO customerCreate,
                [FromServices] ICustomerRepository customerRepository
            ) => {
                var logger = Log.ForContext("SourceContext", nameof(CustomerEndpoint));
                
                logger.Info($"Starting to create new customer with username: {customerCreate.UserName}");
                
                try
                {
                    // Check if username already exists
                    var existingCustomer = await customerRepository.GetCustomerByUsernameAsync(customerCreate.UserName);
                    if (existingCustomer != null)
                    {
                        logger.Warn($"Username {customerCreate.UserName} already exists");
                        return Results.BadRequest($"Username {customerCreate.UserName} already exists");
                    }
                    
                    var customer = new Entities.Customer
                    {
                        FirstName = customerCreate.FirstName,
                        LastName = customerCreate.LastName,
                        EmailAddress = customerCreate.EmailAddress,
                        UserName = customerCreate.UserName
                    };
                    
                    logger.Debug($"Creating customer: {customerCreate.UserName}, Email: {customerCreate.EmailAddress}");
                    
                    var customerId = await customerRepository.CreateAsync(customer);
                    await customerRepository.SaveChangesAsync();
                    
                    var createdCustomer = await customerRepository.GetByIdAsync(customerId);
                    
                    logger.Info($"Successfully created customer with ID: {customerId}, Username: {customerCreate.UserName}");
                    
                    return Results.Created($"/api/customer/{customerId}", createdCustomer);
                }
                catch (Exception ex)
                {
                    logger.Err(ex, $"Error occurred while creating customer with username: {customerCreate.UserName}");
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
                var logger = Log.ForContext("SourceContext", nameof(CustomerEndpoint));
                
                logger.Info($"Starting to update customer with ID: {id}");
                
                try
                {
                    var existingCustomer = await customerRepository.GetByIdAsync(id);
                    if (existingCustomer == null)
                    {
                        logger.Warn($"Customer not found with ID: {id} for update");
                        return Results.NotFound($"Customer with ID {id} not found");
                    }

                    logger.Debug($"Updating customer ID: {id} from {existingCustomer.UserName} to {customerUpdate.UserName}");
                    
                    // Check if new username conflicts with another customer
                    if (existingCustomer.UserName != customerUpdate.UserName)
                    {
                        var duplicateCustomer = await customerRepository.GetCustomerByUsernameAsync(customerUpdate.UserName);
                        if (duplicateCustomer != null && duplicateCustomer.Id != id)
                        {
                            logger.Warn($"Username {customerUpdate.UserName} is already taken by another customer");
                            return Results.BadRequest($"Username {customerUpdate.UserName} is already taken by another customer");
                        }
                    }

                    existingCustomer.UserName = customerUpdate.UserName;
                    existingCustomer.FirstName = customerUpdate.FirstName;
                    existingCustomer.LastName = customerUpdate.LastName;

                    await customerRepository.UpdateAsync(existingCustomer);
                    await customerRepository.SaveChangesAsync();

                    logger.Info($"Successfully updated customer with ID: {id}, New username: {customerUpdate.UserName}");
                    
                    return Results.Ok(existingCustomer);
                }
                catch (Exception ex)
                {
                    logger.Err(ex, $"Error occurred while updating customer with ID: {id}");
                    return Results.BadRequest($"Error updating customer: {ex.Message}");
                }
            })
            .WithName("UpdateCustomer")
            .WithDescription("Update an existing customer");

            // DELETE: Delete customer by ID
            group.MapDelete("/{id:int}", 
            async (int id, [FromServices] ICustomerRepository customerRepository) => {
                var logger = Log.ForContext("SourceContext", nameof(CustomerEndpoint));
                
                logger.Info($"Starting to delete customer with ID: {id}");
                
                try
                {
                    var customer = await customerRepository.GetByIdAsync(id);
                    if (customer == null)
                    {
                        logger.Warn($"Customer not found with ID: {id} for deletion");
                        return Results.NotFound($"Customer with ID {id} not found");
                    }

                    logger.Debug($"Deleting customer: ID: {id}, Username: {customer.UserName}");
                    
                    await customerRepository.DeleteAsync(customer);
                    await customerRepository.SaveChangesAsync();

                    logger.Info($"Successfully deleted customer with ID: {id}, Username: {customer.UserName}");
                    
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    logger.Err(ex, $"Error occurred while deleting customer with ID: {id}");
                    return Results.BadRequest($"Error deleting customer: {ex.Message}");
                }
            })
            .WithName("DeleteCustomer")
            .WithDescription("Delete a customer by ID");
        }
    }
}
