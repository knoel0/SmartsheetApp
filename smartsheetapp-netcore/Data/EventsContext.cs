using smartsheetapp_netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace smartsheetapp_netcore.Data
{
    public class EventsContext : DbContext
    {
        public EventsContext (DbContextOptions<EventsContext> options) : base(options) {}
        
        public DbSet<EventCallbackModel> EventCallbackModels { get; set; }
        public DbSet<EventModel> EventModels { get; set; }
        public DbSet<StatusChangeCallbackModel> StatusChangeCallbackModels { get; set; }
    }
}