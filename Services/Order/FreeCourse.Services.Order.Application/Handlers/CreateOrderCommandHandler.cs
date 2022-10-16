using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.DTOs;
using MediatR;

namespace FreeCourse.Services.Order.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
{
    private readonly OrderDbContext _dbContext;

    public CreateOrderCommandHandler(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street,
            request.Address.ZipCode, request.Address.Line);

        var newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAddress);

        request.OrderItems.ForEach(x => { newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl); });

        await _dbContext.Orders.AddAsync(newOrder);

        await _dbContext.SaveChangesAsync();

        return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 200);
    }
}