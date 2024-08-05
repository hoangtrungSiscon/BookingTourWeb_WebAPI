using BookingTourWeb_WebAPI.Models;

namespace BookingTourWeb_WebAPI.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, int id, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
