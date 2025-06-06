using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _repository = customerRepository;
        }

        public async Task<IResult> GetCustomerByUsernameAsync(string username) => Results.Ok(await _repository.GetCustomerByUsernameAsync(username));
    }
}
