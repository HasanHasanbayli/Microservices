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
        var newAddress = new Address(request.AddressDto.Province, request.AddressDto.District,
            request.AddressDto.Street,
            request.AddressDto.ZipCode, request.AddressDto.Line);

        var newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAddress);

        request.OrderItems.ForEach(x =>
        {
            newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
        });

        await _dbContext.Orders.AddAsync(newOrder, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Response<CreatedOrderDto>.Success(new CreatedOrderDto {OrderId = newOrder.Id}, 200);
    }
}