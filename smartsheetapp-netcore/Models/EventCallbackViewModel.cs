using System;
using System.Collections.Generic;

namespace smartsheetapp_netcore.Models
{
    public class EventCallbackViewModel
    {
        public string nonce { get; set; }
        public DateTime timestamp { get; set; }
        public long webhookId { get; set; }
        public string scope { get; set; }
        public long scopeObjectId { get; set; }
        public List<EventViewModel> events { get; set; }
        public string scopeObjectName { get; set; }
    }

    public class EventViewModel
    {
        public int? eventId { get; set; }
        public string objectType { get; set; }
        public string eventType { get; set; }
        public long? id { get; set; }
        public long userId { get; set; }
        public DateTime timestamp { get; set; }
        public long? rowId { get; set; }
        public long? columnId { get; set; }
        public string cellValue { get; set; }
        public string columnName { get; set; }
        public string rowFirstCell { get; set; }
    }
}