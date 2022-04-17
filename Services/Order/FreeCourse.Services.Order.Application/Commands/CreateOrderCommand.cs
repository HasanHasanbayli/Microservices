using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Shared.DTOs;
using MediatR;

namespace FreeCourse.Services.Order.Application.Commands;

public class CreateOrderCommand : IRequest<Response<CreatedOrderDto>>
{
    public string BuyerId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public AddressDto Address { get; set; }
    
    
}