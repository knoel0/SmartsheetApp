using smartsheetapp_netcore.Data;
using smartsheetapp_netcore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace smartsheetapp_netcore.Services
{
    public interface IEventsService
    {
        List<EventCallbackViewModel> CreateViewModel();
        void ViewModelEst(ref List<EventCallbackModel> ecms);
        List<EventCallbackViewModel> EcmsToEcvms(List<EventCallbackModel> ecms);
        List<EventViewModel> EmsToEvms(List<EventModel> ems, long scopeObjectId);
    }

    public class EventsService : IEventsService
    {
        public readonly EventsContext _context;
        private readonly ISmartsheetService _smartsheetService;

        public EventsService(EventsContext context, ISmartsheetService smartsheetService)
        {
            _context = context;
            _smartsheetService = smartsheetService;
        }

        public List<EventCallbackViewModel> CreateViewModel()
        {
            var ecmsView = new List<EventCallbackModel>();

            foreach (EventCallbackModel ecm in (from e in _context.EventCallbackModels orderby e.timestamp select e).ToList())
            {
                ecmsView.Add(new EventCallbackModel()
                    {
                        nonce = ecm.nonce,
                        timestamp = ecm.timestamp,
                        webhookId = ecm.webhookId,
                        scope = ecm.scope,
                        scopeObjectId = ecm.scopeObjectId,
                        events = _context.EventModels.Where(em => em.eventCallbackModelnonce == ecm.nonce).ToList()
                    });
            }

            ViewModelEst(ref ecmsView);

            return EcmsToEcvms(ecmsView);
        }

        public void ViewModelEst(ref List<EventCallbackModel> ecms)
        {   
            foreach (EventCallbackModel ecm in ecms)
            {
                ecm.timestamp = UtcToEst(ecm.timestamp);

                foreach (EventModel em in ecm.events)
                {
                    em.timestamp = UtcToEst(em.timestamp);
                }
            }
                
            DateTime UtcToEst(DateTime dateTime)
            {
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, easternZone);
            }
        }

        public List<EventCallbackViewModel> EcmsToEcvms(List<EventCallbackModel> ecms)
        {
            var ecvms = new List<EventCallbackViewModel>();

            foreach (EventCallbackModel ecm in ecms)
            {
                ecvms.Add(new EventCallbackViewModel()
                {
                    nonce = ecm.nonce,
                    timestamp = ecm.timestamp,
                    webhookId = ecm.webhookId,
                    scope = ecm.scope,
                    scopeObjectId = ecm.scopeObjectId,
                    events = EmsToEvms(ecm.events, ecm.scopeObjectId),
                    scopeObjectName = _smartsheetService.GetScopeObjectName(ecm.scopeObjectId)
                });
            }

            return ecvms;
        }    

        public List<EventViewModel> EmsToEvms(List<EventModel> ems, long scopeObjectId)
        {
            var evms = new List<EventViewModel>();

            foreach (EventModel em in ems)
            {
                evms.Add(new EventViewModel()
                {
                    eventId = em.eventId,
                    objectType = em.objectType,
                    eventType = em.eventType,
                    id = em.id,
                    userId = em.userId,
                    timestamp = em.timestamp,
                    rowId = em.rowId,
                    columnId = em.columnId,
                    cellValue = em.cellValue,
                    columnName = _smartsheetService.GetColumnName(scopeObjectId, em.columnId),
                    rowFirstCell = _smartsheetService.GetRowFirstCell(scopeObjectId, em.rowId)
                });
            }

            return evms;
        }
    }
}