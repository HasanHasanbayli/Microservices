using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderItem = FreeCourse.Shared.Messages.OrderItem;

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

        CreateOrderMessageCommand createOrderMessageCommand = new()
        {
            BuyerId = paymentDto.Order.BuyerId,
            Province = paymentDto.Order.Address.Province,
            District = paymentDto.Order.Address.District,
            Street = paymentDto.Order.Address.Street,
            Line = paymentDto.Order.Address.Line,
            ZipCode = paymentDto.Order.Address.ZipCode
        };

        paymentDto.Order.OrderItems.ForEach(x =>
        {
            createOrderMessageCommand.OrderItem.Add(new OrderItem
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