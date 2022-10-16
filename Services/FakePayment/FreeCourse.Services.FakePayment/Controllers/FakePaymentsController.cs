using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FakePaymentsController : CustomBaseController
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    [HttpPost]
    public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
    {
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

        var createOrderMessageCommand = new CreateOrderMessageCommand();

        createOrderMessageCommand.BuyerId = paymentDto.Order.BuyerId;
        createOrderMessageCommand.Province = paymentDto.Order.Address.Province;
        createOrderMessageCommand.District = paymentDto.Order.Address.District;
        createOrderMessageCommand.Street = paymentDto.Order.Address.Street;
        createOrderMessageCommand.Line = paymentDto.Order.Address.Line;
        createOrderMessageCommand.ZipCode = paymentDto.Order.Address.ZipCode;

        paymentDto.Order.OrderItems.ForEach(x =>
        {
            createOrderMessageCommand.OrderItems.Add(new OrderItem
            {
                PictureUrl = x.PictureUrl,
                Price = x.Price,
                ProductId = x.ProductId,
                ProductName = x.ProductName
            });
        });

        await sendEndpoint.Send(createOrderMessageCommand);

        return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Success(200));
    }
}