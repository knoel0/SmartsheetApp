using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using smartsheetapp_netcore.Services;
using System.IO;
using System.Threading.Tasks;

namespace SmartsheetAppMvc.Controllers
{
    public class ManageWebhooksController : Controller
    {
        private readonly ILogger<ManageWebhooksController> _logger;
        private readonly ISmartsheetService _smartsheetService;
        private readonly IManageWebhooksService _manageWebhooksService;

        public ManageWebhooksController(ILogger<ManageWebhooksController> logger, ISmartsheetService smartsheetService, IManageWebhooksService manageWebhooksService)
        {
            _logger = logger;
            _smartsheetService = smartsheetService;
            _manageWebhooksService = manageWebhooksService;
        }

        // GET /ManageWebhooks/
        public IActionResult Index()
        {
            return Json(_manageWebhooksService.CreateViewModel(_smartsheetService.GetDict()));
        }

        // POST /ManageWebhooks/ToggleWebhookEnabled/
        public async Task<IActionResult> ToggleWebhookEnabled()
        {
            using var reader = new StreamReader(Request.Body);
            string body = await reader.ReadToEndAsync();
            
            _logger.LogInformation(body);

            _manageWebhooksService.HandleForm(body);
            
            return Ok();
        }
    }
}