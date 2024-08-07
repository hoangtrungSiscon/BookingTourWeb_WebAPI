using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BookingTourWeb_WebAPI.Models.InputModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailchimpController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private string _apiKey;
        private string _listId;
        private string _dataCenter;

        public MailchimpController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration["Mailchimp:apikey"];
            _listId = _configuration["Mailchimp:list_id"];
            _dataCenter = _configuration["Mailchimp:data_center"];
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] Subscriber subscriber)
        {
            if (string.IsNullOrEmpty(subscriber.email))
            {
                return BadRequest("Email is required.");
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"anystring:{_apiKey}")));

            var data = new
            {
                email_address = subscriber.email,
                status = "subscribed"
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"https://{_dataCenter}.api.mailchimp.com/3.0/lists/{_listId}/members", content);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

    }
}
