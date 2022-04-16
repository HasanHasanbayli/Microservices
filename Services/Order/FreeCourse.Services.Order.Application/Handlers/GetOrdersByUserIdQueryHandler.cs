using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Application.Mapping;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FreeCourse.Services.Order.Application.Handlers;

public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
{
    private readonly OrderDbContext _dbContext;

    public GetOrdersByUserIdQueryHandler(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _dbContext.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == request.UserId)
            .ToListAsync(cancellationToken: cancellationToken);

        if (!orders.Any())
            return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);

        var orderDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

        return Response<List<OrderDto>>.Success(orderDto, 200);
    }
}