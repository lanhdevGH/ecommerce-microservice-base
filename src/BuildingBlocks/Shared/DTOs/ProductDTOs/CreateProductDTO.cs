namespace Shared.DTOs.ProductDTOs;

public class CreateProductDTO : CreateOrUpdateProductDTO
{
    public required string No { get; set; }
}
