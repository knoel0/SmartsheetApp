using Newtonsoft.Json;
using smartsheetapp_netcore.JsonNetHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartsheetapp_netcore.Models
{
    public class EventCallbackModel
    {
        [Key]
        public string nonce { get; set; }
        
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime timestamp { get; set; }
        
        public long webhookId { get; set; }
        public string scope { get; set; }
        public long scopeObjectId { get; set; }
        public List<EventModel> events { get; set; }
    }
    
    public class EventModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? eventId { get; set; }
        
        public string objectType { get; set; }
        public string eventType { get; set; }
        public long? id { get; set; }
        public long userId { get; set; }
        
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime timestamp { get; set; }
        
        public long? rowId { get; set; }
        public long? columnId { get; set; }
        public string cellValue { get; set; }
        public string eventCallbackModelnonce { get; set; }
        public EventCallbackModel eventCallbackModel { get; set; }
    }
}