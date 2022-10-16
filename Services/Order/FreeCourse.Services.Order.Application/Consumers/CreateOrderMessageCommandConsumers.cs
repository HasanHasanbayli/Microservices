using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Messages;
using MassTransit;

namespace FreeCourse.Services.Order.Application.Consumers;

public class CreateOrderMessageCommandConsumers : IConsumer<CreateOrderMessageCommand>
{
    private readonly OrderDbContext _orderDbContext;

    public CreateOrderMessageCommandConsumers(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
    {
        var newAddress = new Address(context.Message.Province, context.Message.District,
            context.Message.Street, context.Message.ZipCode, context.Message.Line);

        var order = new Domain.OrderAggregate.Order(context.Message.BuyerId, newAddress);

        context.Message.OrderItems.ForEach(x =>
        {
            order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
        });

        await _orderDbContext.Orders.AddAsync(order);

        await _orderDbContext.SaveChangesAsync();
    }
}