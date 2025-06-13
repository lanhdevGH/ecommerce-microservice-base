using MediatR;
using Ordering.Application.Common.DTOs;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Commands.Order;

public class CreateOrderCommand : IRequest<ApiResult<long>>
{
    public string UserName { get; set; }
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string ShippingAddress { get; set; }
    public string InvoiceAddress { get; set; }

    public CreateOrderCommand(CreateOrderDto dto)
    {
        UserName = dto.UserName;
        TotalPrice = dto.TotalPrice;
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        EmailAddress = dto.EmailAddress;
        ShippingAddress = dto.ShippingAddress;
        InvoiceAddress = dto.InvoiceAddress;
    }
}