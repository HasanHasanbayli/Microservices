using FreeCourse.Web.Models.FakePayments;

namespace FreeCourse.Web.Services.Interfaces;

public interface IPaymentService
{
    Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
}