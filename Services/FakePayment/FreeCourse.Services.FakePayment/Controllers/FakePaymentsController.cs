using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FakePaymentsController : CustomBaseController
{
    //PaymentDto paymentDto
    [HttpPost]
    public async Task<IActionResult> ReceivePayment()
    {
        return CreateActionResultInstance(Response<NoContent>.Success(200));
    }
}