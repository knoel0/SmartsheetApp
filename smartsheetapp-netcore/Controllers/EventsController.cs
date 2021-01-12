using Microsoft.AspNetCore.Mvc;
using smartsheetapp_netcore.Models;
using smartsheetapp_netcore.Services;
using System.Diagnostics;

namespace smartsheetapp_netcore.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        // GET /Events/
        public IActionResult Index()
        {
            return Json(_eventsService.CreateViewModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}