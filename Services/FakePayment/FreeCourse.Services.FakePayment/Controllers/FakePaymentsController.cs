using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FakePaymentsController : CustomBaseController
{
    [HttpPost]
    public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
    {
        return CreateActionResultInstance(Response<NoContent>.Success(200));
    }
}