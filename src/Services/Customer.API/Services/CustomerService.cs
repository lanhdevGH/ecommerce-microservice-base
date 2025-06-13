using AutoMapper;
using Common.Logging;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.CustomerDTOs;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICustomLogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, ICustomLogger<CustomerService> logger)
        {
            _repository = customerRepository;
            _mapper = mapper;
            _logger = logger;
            _logger.Info("CustomerService initialized successfully");
        }

        public async Task<IResult> GetAllCustomersAsync()
        {
            try
            {
                _logger.Info("Starting to fetch all customers from database");
                
                var customers = await _repository.FindAll().ToListAsync();
                var result = _mapper.Map<List<CustomerDto>>(customers);
                
                _logger.Info($"Successfully retrieved {customers.Count} customers from database");
                
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Err(ex, "Error occurred while fetching all customers");
                return Results.Problem("An error occurred while fetching customers");
            }
        }

        public async Task<IResult> GetCustomerByIdAsync(int id)
        {
            try
            {
                _logger.Info($"Starting to search for customer with ID: {id}");
                
                var customer = await _repository.GetByIdAsync(id);
                
                if (customer == null)
                {
                    _logger.Warn($"Customer not found with ID: {id}");
                    return Results.NotFound($"Customer with ID {id} not found");
                }
                
                var result = _mapper.Map<CustomerDto>(customer);
                _logger.Info($"Successfully found customer with ID: {id}, UserName: {customer.UserName}");
                
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Err(ex, $"Error occurred while searching for customer with ID: {id}");
                return Results.Problem($"An error occurred while searching for customer with ID: {id}");
            }
        }

        public async Task<IResult> GetCustomerByUsernameAsync(string username)
        {
            try
            {
                _logger.Info($"Starting to search for customer with username: {username}");
                
                if (string.IsNullOrWhiteSpace(username))
                {
                    _logger.Warn($"Invalid username provided: {username}");
                    return Results.BadRequest("Username cannot be null or empty");
                }
                
                var entity = await _repository.GetCustomerByUsernameAsync(username);
                
                if (entity == null)
                {
                    _logger.Warn($"Customer not found with username: {username}");
                    return Results.NotFound($"Customer with username {username} not found");
                }
                
                var result = _mapper.Map<CustomerDto>(entity);
                _logger.Info($"Successfully found customer with username: {username}, ID: {entity.Id}");
                
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Err(ex, $"Error occurred while searching for customer with username: {username}");
                return Results.Problem($"An error occurred while searching for customer with username: {username}");
            }
        }

        public async Task<IResult> CreateCustomerAsync(CustomerCreateDTO customerCreate)
        {
            try
            {
                _logger.Info($"Starting to create new customer with username: {customerCreate.UserName}");
                
                if (customerCreate == null)
                {
                    _logger.Warn("CustomerCreateDTO is null in create request");
                    return Results.BadRequest("Customer data cannot be null");
                }
                
                if (string.IsNullOrWhiteSpace(customerCreate.UserName))
                {
                    _logger.Warn("Username is null or empty in customer create request");
                    return Results.BadRequest("Username cannot be null or empty");
                }
                
                // Check if username already exists
                var existingCustomer = await _repository.GetCustomerByUsernameAsync(customerCreate.UserName);
                if (existingCustomer != null)
                {
                    _logger.Warn($"Username {customerCreate.UserName} already exists");
                    return Results.BadRequest($"Username {customerCreate.UserName} already exists");
                }
                
                var customer = new Entities.Customer
                {
                    FirstName = customerCreate.FirstName,
                    LastName = customerCreate.LastName,
                    EmailAddress = customerCreate.EmailAddress,
                    UserName = customerCreate.UserName
                };
                
                _logger.Debug($"Creating customer: {customerCreate.UserName}, Email: {customerCreate.EmailAddress}");
                
                var customerId = await _repository.CreateAsync(customer);
                await _repository.SaveChangesAsync();
                
                var createdCustomer = await _repository.GetByIdAsync(customerId);
                var result = _mapper.Map<CustomerDto>(createdCustomer);
                
                _logger.Info($"Successfully created customer with ID: {customerId}, Username: {customerCreate.UserName}");
                
                return Results.Created($"/api/customer/{customerId}", result);
            }
            catch (Exception ex)
            {
                _logger.Err(ex, $"Error occurred while creating customer with username: {customerCreate?.UserName}");
                return Results.BadRequest($"Error creating customer: {ex.Message}");
            }
        }

        public async Task<IResult> UpdateCustomerAsync(int id, CustomerUpdateDTO customerUpdate)
        {
            try
            {
                _logger.Info($"Starting to update customer with ID: {id}");
                
                if (customerUpdate == null)
                {
                    _logger.Warn("CustomerUpdateDTO is null in update request");
                    return Results.BadRequest("Customer update data cannot be null");
                }
                
                if (string.IsNullOrWhiteSpace(customerUpdate.UserName))
                {
                    _logger.Warn("Username is null or empty in customer update request");
                    return Results.BadRequest("Username cannot be null or empty");
                }
                
                var existingCustomer = await _repository.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    _logger.Warn($"Customer not found with ID: {id} for update");
                    return Results.NotFound($"Customer with ID {id} not found");
                }

                _logger.Debug($"Updating customer ID: {id} from {existingCustomer.UserName} to {customerUpdate.UserName}");
                
                // Check if new username conflicts with another customer
                if (existingCustomer.UserName != customerUpdate.UserName)
                {
                    var duplicateCustomer = await _repository.GetCustomerByUsernameAsync(customerUpdate.UserName);
                    if (duplicateCustomer != null && duplicateCustomer.Id != id)
                    {
                        _logger.Warn($"Username {customerUpdate.UserName} is already taken by another customer");
                        return Results.BadRequest($"Username {customerUpdate.UserName} is already taken by another customer");
                    }
                }

                existingCustomer.UserName = customerUpdate.UserName;
                existingCustomer.FirstName = customerUpdate.FirstName;
                existingCustomer.LastName = customerUpdate.LastName;

                await _repository.UpdateAsync(existingCustomer);
                await _repository.SaveChangesAsync();

                var result = _mapper.Map<CustomerDto>(existingCustomer);
                _logger.Info($"Successfully updated customer with ID: {id}, New username: {customerUpdate.UserName}");
                
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Err(ex, $"Error occurred while updating customer with ID: {id}");
                return Results.BadRequest($"Error updating customer: {ex.Message}");
            }
        }

        public async Task<IResult> DeleteCustomerAsync(int id)
        {
            try
            {
                _logger.Info($"Starting to delete customer with ID: {id}");
                
                var customer = await _repository.GetByIdAsync(id);
                if (customer == null)
                {
                    _logger.Warn($"Customer not found with ID: {id} for deletion");
                    return Results.NotFound($"Customer with ID {id} not found");
                }

                _logger.Debug($"Deleting customer: ID: {id}, Username: {customer.UserName}");
                
                await _repository.DeleteAsync(customer);
                await _repository.SaveChangesAsync();

                _logger.Info($"Successfully deleted customer with ID: {id}, Username: {customer.UserName}");
                
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                _logger.Err(ex, $"Error occurred while deleting customer with ID: {id}");
                return Results.BadRequest($"Error deleting customer: {ex.Message}");
            }
        }
    }
}
