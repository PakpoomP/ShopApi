using InterviewTTBApi.Application.Features.Cart.Commands;
using InterviewTTBApi.Application.Features.Cart.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTTBApi.API.Controllers;

[ApiController]
[Route("api/cart")]
public class CartController(ISender sender) : ControllerBase
{
    

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart(AddToCartCommand request)
    {
        var result = await sender.Send(request);
        return result.IsSuccess
            ? Ok()
            : BadRequest(new { error = result.Error });
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetCartItems()
    {
        var query = new GetCartItemsQuery();
        var result = await sender.Send(query);
        return result.IsSuccess
            ? Ok(result)
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("remove")]
    public async Task<IActionResult> RemoveFromCart(RemoveFromCartCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? Ok()
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CheckoutCartCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? Ok()
            : BadRequest(new { error = result.Error });
    }
}
