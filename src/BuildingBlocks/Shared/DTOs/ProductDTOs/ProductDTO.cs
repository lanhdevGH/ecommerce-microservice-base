namespace Shared.DTOs.ProductDTOs;

public class ProductDTO
{
    public long Id { get; set; }

    public required string No { get; set; }

    public required string Name { get; set; }

    public string Summary { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
