using Newtonsoft.Json.Converters;

namespace smartsheetapp_netcore.JsonNetHelpers
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}