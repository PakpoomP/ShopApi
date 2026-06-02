using InterviewTTBApi.Application.Features.Products.Commands;
using InterviewTTBApi.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTTBApi.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var result = await sender.Send(new GetProductsQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? Created($"/api/products/{result.Value}", new { id = result.Value })
            : BadRequest(new { error = result.Error });
    }
}
