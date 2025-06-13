using Shared.DTOs.CustomerDTOs;

namespace Customer.API.Services.Interfaces;

public interface ICustomerService
{
    Task<IResult> GetAllCustomersAsync();
    Task<IResult> GetCustomerByIdAsync(int id);
    Task<IResult> GetCustomerByUsernameAsync(string username);
    Task<IResult> CreateCustomerAsync(CustomerCreateDTO customerCreate);
    Task<IResult> UpdateCustomerAsync(int id, CustomerUpdateDTO customerUpdate);
    Task<IResult> DeleteCustomerAsync(int id);
}
