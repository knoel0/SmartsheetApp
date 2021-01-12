using Newtonsoft.Json;
using smartsheetapp_netcore.Data;
using smartsheetapp_netcore.Models;
using System;
using System.Threading.Tasks;

namespace smartsheetapp_netcore.Services
{
    public interface ICallbackService
    {
        Task<string> HandleCallback(string requestBody);
        object CreateModel(string reqBody);
        Task AddToDb(EventCallbackModel eventCallback);
        Task AddToDb(StatusChangeCallbackModel statusChangeCallback);
        bool IsSheetDeletable(StatusChangeCallbackModel statusChangeCallbackModel);
    }

    public class CallbackService : ICallbackService
    {
        public readonly EventsContext _context;
        private readonly ISmartsheetService _smartsheetService;

        public CallbackService(EventsContext context, ISmartsheetService smartsheetService)
        {
            _context = context;
            _smartsheetService = smartsheetService;
        }

        public async Task<string> HandleCallback(string requestBody)
        {
            string headerValue = null;

            object callbackModel = CreateModel(requestBody);

            if (callbackModel is VerificationRequestModel)
            {
                headerValue = ((VerificationRequestModel)callbackModel).challenge;
            }
            else if (callbackModel is EventCallbackModel)
            {
                await AddToDb((EventCallbackModel)callbackModel);
            }
            else if (callbackModel is StatusChangeCallbackModel)
            {
                await AddToDb((StatusChangeCallbackModel)callbackModel);
                
                if (IsSheetDeletable((StatusChangeCallbackModel)callbackModel))
                {
                    _smartsheetService.smartsheet.WebhookResources.DeleteWebhook(((StatusChangeCallbackModel)callbackModel).webhookId);
                }
            }

            return headerValue;
        }

        public object CreateModel(string reqBody)
        {
            return JsonConvert.DeserializeObject(reqBody, GetModel(reqBody));

            Type GetModel(string reqB)
            {
                if (reqB.IndexOf("\"challenge\":", StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    return typeof(VerificationRequestModel);
                }
                else if (reqB.IndexOf("\"events\":", StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    return typeof(EventCallbackModel);
                }
                else
                {
                    return typeof(StatusChangeCallbackModel);
                }
            }
        }

        public async Task AddToDb(EventCallbackModel eventCallback)
        {
            foreach (EventModel em in eventCallback.events)
            {
                if (em.objectType == "cell")
                {
                    em.cellValue = _smartsheetService.GetCellValue(eventCallback.scopeObjectId, em.rowId.Value, em.columnId.Value);
                }
            }

            _context.AddRange(eventCallback);
            await _context.SaveChangesAsync();
        }

        public async Task AddToDb(StatusChangeCallbackModel statusChangeCallback)
        {
            _context.StatusChangeCallbackModels.Add(statusChangeCallback);
            await _context.SaveChangesAsync();
        }

        public bool IsSheetDeletable(StatusChangeCallbackModel statusChangeCallbackModel)
        {
            string[] deletableStatusAttributes = {"DISABLED_ADMINISTRATIVE", "DISABLED_APP_REVOKED", "DISABLED_SCOPE_INACCESSIBLE"};
            
            return Array.IndexOf(deletableStatusAttributes, statusChangeCallbackModel.newWebhookStatus) >= 0;
        }
    }
}