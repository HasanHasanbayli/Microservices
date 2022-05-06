using FreeCourse.Web.Models.FakePayments;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput)
    {
        var response = await _httpClient.PostAsJsonAsync("fakepayments", paymentInfoInput);
        
        return response.IsSuccessStatusCode;
    }
}