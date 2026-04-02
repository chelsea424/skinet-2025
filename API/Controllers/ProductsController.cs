using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
    {

        var spec = new ProductSpecification(specParams);

        return await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize);
    }

    [HttpGet("{id:int}")] // api/products/2; Get product by id 
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();

        return product;
    }

    [HttpPost] // api/products
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
        
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        return BadRequest("Failed to create product");
    }

    [HttpPut("{id:int}")] // api/products/2
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        repo.Update(product); // Mark the product as modified in the context
        
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Failed to update product");
    }

    [HttpDelete("{id:int}")] // api/products/2
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();

        repo.Remove(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Failed to delete product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await repo.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await repo.ListAsync(spec));
    }
    private bool ProductExists(int id)
    {
        return repo.Exists(id);
    }
}
