using Newtonsoft.Json;
using smartsheetapp_netcore.Models;
using Smartsheet.Api.Models;
using System.Collections.Generic;

namespace smartsheetapp_netcore.Services
{
    public interface IManageWebhooksService
    {
        List<ManageWebhooksViewModel> CreateViewModel(Dictionary<Webhook, Sheet> dict);
        void HandleForm(string reqBody);
    }

    public class ManageWebhooksService : IManageWebhooksService
    {
        private readonly ISmartsheetService _smartsheetService;

        public ManageWebhooksService(ISmartsheetService smartsheetService)
        {
            _smartsheetService = smartsheetService;
        }

        public List<ManageWebhooksViewModel> CreateViewModel(Dictionary<Webhook, Sheet> dict)
        {
            var manageWebhooksViewModel = new List<ManageWebhooksViewModel>();

            foreach(KeyValuePair<Webhook, Sheet> entry in dict)
            {
                manageWebhooksViewModel.Add(new ManageWebhooksViewModel()
                    {
                        Id = entry.Key.Id.Value,
                        Enabled = entry.Key.Enabled.Value,
                        sheetName = entry.Value.Name
                    });
            }

            return manageWebhooksViewModel;
        }

        public void HandleForm(string reqBody)
        {
            var mwcms = JsonConvert.DeserializeObject<List<ManageWebhooksViewModel>>(reqBody);

            foreach (ManageWebhooksViewModel mwvm in mwcms)
            {
                _smartsheetService.UpdateWebhook(mwvm.Id, mwvm.Enabled);
            }
        }
    }
}