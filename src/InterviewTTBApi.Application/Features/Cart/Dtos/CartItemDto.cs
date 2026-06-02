namespace InterviewTTBApi.Application.Features.Cart.Dtos;

public record CartItemDto(Guid Id, string CartId, Guid ProductId, int Quantity);
