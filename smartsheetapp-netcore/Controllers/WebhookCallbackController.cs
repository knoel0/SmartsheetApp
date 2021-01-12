using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using smartsheetapp_netcore.Services;
using System.IO;
using System.Threading.Tasks;

namespace SmartsheetAppMvc.Controllers
{
    public class WebhookCallbackController : Controller
    {
        private readonly ILogger<WebhookCallbackController> _logger;
        private readonly ICallbackService _callbackService;

        public WebhookCallbackController(ILogger<WebhookCallbackController> logger, ICallbackService callbackService)
        {
            _logger = logger;
            _callbackService = callbackService;
        }

        // POST /WebhookCallback/
        public async Task<IActionResult> Index()
        {
            using var reader = new StreamReader(Request.Body);
            string body = await reader.ReadToEndAsync();

            _logger.LogInformation($"Request Body: {body}");

            string headerValue = _callbackService.HandleCallback(body).Result;

            if (headerValue != null)
            {
                Response.Headers.Add("Smartsheet-Hook-Response", headerValue);
            }
            
            return Ok();
        }
    }
}