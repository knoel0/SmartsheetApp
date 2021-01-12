using Microsoft.Extensions.Configuration;
using Smartsheet.Api;
using Smartsheet.Api.Models;
using System.Collections.Generic;

namespace smartsheetapp_netcore.Services
{
    public interface ISmartsheetService
    {
        SmartsheetClient smartsheet { get; }
        string GetCellValue(long sheetId, long rowId, long columnId);
        List<Sheet> GetNewSheets();
        Webhook CreateWebhook(string sheetName, long sheetId);
        Webhook UpdateWebhook(long webhookId, bool enabled);
        long GetWebhookId(Webhook webhook);
        Dictionary<Webhook, Sheet> GetDict();
        void PollNewSheets();
        string GetColumnName(long scopeObjectId, long? columnId);
        string GetScopeObjectName(long sheetId);
        string GetRowFirstCell(long scopeObjectId, long? rowId);
    }

    public class SmartsheetService : ISmartsheetService
    {
        public IConfiguration Configuration { get; }
        
        public SmartsheetService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public SmartsheetClient smartsheet
        {
            get { return new SmartsheetBuilder().SetAccessToken(Configuration["SmartsheetApiKey"]).Build(); }
        }

        public string GetCellValue(long sheetId, long rowId, long columnId)
        {
            Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId, null, null, new List<long>{rowId}, null, new List<long>{columnId}, null, null);

            return sheet.Rows[0].Cells[0].DisplayValue;
        }

        public List<Sheet> GetNewSheets()
        {
            PaginatedResult<Sheet> sheets = smartsheet.SheetResources.ListSheets(null, null, null);
            PaginatedResult<Webhook> webhooks = smartsheet.WebhookResources.ListWebhooks(null);
            var newSheets = new List<Sheet>();
                
            for (int i = 0; i < sheets.Data.Count; i++)
            {
                if (sheetHasWebhook(sheets.Data[i].Id.Value) == false)
                {
                    newSheets.Add(sheets.Data[i]);
                }
            }

            bool sheetHasWebhook(long sheetId)
            {
                for (int i = 0; i < webhooks.Data.Count; i++)
                {
                    if (sheetId == webhooks.Data[i].ScopeObjectId)
                    {
                        return true;
                    }
                }
                return false;
            }
                
            return newSheets;
        }

        public Webhook CreateWebhook(string sheetName, long sheetId)
        {
            return smartsheet.WebhookResources.CreateWebhook(new Webhook()
                {
                    Name = sheetName,
                    CallbackUrl = $"{Configuration["CallbackUrl"]}/WebhookCallback",
                    Scope = "sheet",
                    ScopeObjectId = sheetId,
                    Events = new List<string> { "*.*" },
                    Version = 1
                }); 
        }

        public long GetWebhookId(Webhook webhook)
        {
            return webhook.Id.Value;
        }

        public Webhook UpdateWebhook(long webhookId, bool enabled)
        {
            return smartsheet.WebhookResources.UpdateWebhook(new Webhook()
                {
                    Id = webhookId,
                    Enabled = enabled
                });
        }

        public Dictionary<Webhook, Sheet> GetDict()
        {

            Dictionary<Webhook, Sheet> dict = new Dictionary<Webhook, Sheet>();
            PaginatedResult<Webhook> webhooks = smartsheet.WebhookResources.ListWebhooks(null);

            for (int i = 0; i < webhooks.Data.Count; i++)
            {
                dict.Add(webhooks.Data[i], smartsheet.SheetResources.GetSheet(webhooks.Data[i].ScopeObjectId.Value, null, null, null, null, null, null, null));
            }

            return dict;
        }

        public void PollNewSheets()
        {
            List<Sheet> newSheets = GetNewSheets();

            if (newSheets.Count > 0)
            {
                for (int i = 0; i < newSheets.Count; i++)
                {
                    Webhook updatedWebhook = UpdateWebhook(GetWebhookId(CreateWebhook(newSheets[i].Name, newSheets[i].Id.Value)), true);
                }
            }
        }

        public string GetColumnName(long scopeObjectId, long? columnId)
        {
            if (!columnId.HasValue)
            {
                return "";
            }
            else if (columnId.HasValue)
            {
                Column column = smartsheet.SheetResources.ColumnResources.GetColumn(scopeObjectId, columnId.Value, null);
                return column.Title;
            }
            else
            {
                return "";
            }
        }

        public string GetScopeObjectName(long sheetId)
        {
            Sheet sheet = smartsheet.SheetResources.GetSheet(sheetId, null, null, null, null, null, null, null);
            return sheet.Name;
        }

        public string GetRowFirstCell(long scopeObjectId, long? rowId)
        {
            if (!rowId.HasValue)
            {
                return "";
            }
            else if (rowId.HasValue)
            {
                Row row = smartsheet.SheetResources.RowResources.GetRow(scopeObjectId, rowId.Value, null, null);
                return row.Cells[0].DisplayValue;
            }
            else
            {
                return "";
            }
        }
    }
}