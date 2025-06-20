﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.Entities;
using Product.API.Repositories.Interfaces;
using Shared.DTOs.ProductDTOs;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Controllers;

public class ProductsController(IProductRepository repository, IMapper mapper) : BaseController
{
    private readonly IProductRepository _repository = repository;

    private readonly IMapper _mapper = mapper;

    #region CRUD
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _repository.GetProducts();
        var result = _mapper.Map<IEnumerable<ProductDTO>>(products);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetProduct([Required] long id)
    {
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();
        var result = _mapper.Map<ProductDTO>(product);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO productDto)
    {
        var productEntity = await _repository.GetProductByNo(productDto.No);
        if (productEntity != null) return BadRequest($"Product No: {productDto.No} is existed.");

        var product = _mapper.Map<CatalogProduct>(productDto);
        await _repository.CreateProduct(product);
        await _repository.SaveChangesAsync();
        var result = _mapper.Map<ProductDTO>(product);
        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateProduct([Required] long id, [FromBody] UpdateProductDTO productDto)
    {
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();

        var updateProduct = _mapper.Map(productDto, product);
        await _repository.UpdateProduct(updateProduct);
        await _repository.SaveChangesAsync();
        var result = _mapper.Map<ProductDTO>(product);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteProduct([Required] long id)
    {
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();

        await _repository.DeleteProduct(id);
        await _repository.SaveChangesAsync();
        return NoContent();
    }

    #endregion

    #region Additional Resources

    [HttpGet("get-product-by-no/{productNo}")]
    public async Task<IActionResult> GetProductByNo([Required] string productNo)
    {
        var product = await _repository.GetProductByNo(productNo);
        if (product == null)
            return NotFound();

        var result = _mapper.Map<ProductDTO>(product);
        return Ok(result);
    }

    #endregion
}
