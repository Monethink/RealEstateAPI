using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using RealEstateAPI.Models;

namespace RealEstateAPI.Controllers
{
    [ApiController]
    [Route("api/")]
    public class RealEstateController : ControllerBase
    {
        static HttpClient client = new HttpClient();
        static RealEstateController() {
            client.BaseAddress = new Uri("https://api.bitso.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public RealEstateController() { }

        [HttpGet("Forex")]
        public async Task<Decimal?> GetForex()
        {
            ForexResponse responseEntity = null;
            HttpResponseMessage response = await client.GetAsync("v3/ticker/?book=tusd_mxn");
            if (response.IsSuccessStatusCode)
            {
                responseEntity = await response.Content.ReadAsAsync<ForexResponse>();
                return responseEntity.payload.last;
            }
            return null;
        }

        // POST: api/Feedback
        [HttpPost("Feedback")]
        public async Task<ActionResult> Feedback([FromBody] Feedback feedback)
        {
            try
            {
                MailSender.SendMail(feedback);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}
