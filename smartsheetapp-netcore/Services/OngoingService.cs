using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace smartsheetapp_netcore.Services
{
    public class OngoingService : BackgroundService
    {
        private readonly ISmartsheetService _smartsheetService;
        
        public OngoingService(ISmartsheetService smartsheetService)
        {
            _smartsheetService = smartsheetService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(60000); // 1MIN
                _smartsheetService.PollNewSheets();
            }
        }
    }
}