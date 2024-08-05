using BookingTourWeb_WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly DVMayBayContext _context;

        private readonly IVnPayService _vnPayService;

        public VnPayController(DVMayBayContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public async Task<ActionResult<Object>> CreatePaymentUrl([FromQuery] PaymentInformationModel model, int id)
        {
            var url = _vnPayService.CreatePaymentUrl(model, id, HttpContext);

            dynamic data = new System.Dynamic.ExpandoObject();

            data.url = url;

            return data;
        }

        [HttpGet]
        public async Task<ActionResult<Object>> PaymentResult(PaymentInformationModel model)
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return response;
        }
    }
}
