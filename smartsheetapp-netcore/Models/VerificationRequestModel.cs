namespace smartsheetapp_netcore.Models
{
    public class VerificationRequestModel
    {
        public string challenge { get; set; }
        public long? webhookId { get; set; }
    }
}