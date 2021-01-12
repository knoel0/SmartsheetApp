using Newtonsoft.Json;
using smartsheetapp_netcore.JsonNetHelpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace smartsheetapp_netcore.Models
{
    public class StatusChangeCallbackModel
    {
        [Key]
        public string nonce { get; set; }
        
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime timestamp { get; set; }
        
        public long webhookId { get; set; }
        public string scope { get; set; }
        public long scopeObjectId { get; set; }
        public string newWebhookStatus { get; set; }
    }
}