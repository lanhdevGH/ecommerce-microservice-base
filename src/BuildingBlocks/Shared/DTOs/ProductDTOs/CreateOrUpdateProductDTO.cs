﻿using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.ProductDTOs;

public abstract class CreateOrUpdateProductDTO
{
    [Required]
    [MaxLength(250, ErrorMessage = "Maximum length for Product Name is 250 characters.")]
    public string Name { get; set; }

    [MaxLength(255, ErrorMessage = "Maximum length for Product Summary is 255 characters.")]
    public string Summary { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
