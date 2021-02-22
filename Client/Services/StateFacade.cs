using Fluxor;
using Microsoft.Extensions.Logging;
using System;
using Tomi.Calendar.Mono.Client.Store.Features.CalendarItem;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Client.Services
{
    public class StateFacade
    {
        private readonly ILogger<StateFacade> _logger;
        private readonly IDispatcher _dispatcher;

        public StateFacade(ILogger<StateFacade> logger, IDispatcher dispatcher) =>
            (_logger, _dispatcher) = (logger, dispatcher);

        public void LoadCalendarItems()
        {
            _dispatcher.Dispatch(new LoadCalendarItemsAction());
        }
        public void LoadCalendarItemById(Guid id)
        {
            _dispatcher.Dispatch(new LoadCalendarItemDetailAction(id));
        }
        internal void NewCalendarItem(Guid id)
        {
            _dispatcher.Dispatch(new NewCalendarItemAction(id));
        }
        public void CreateCalendarItem(CalendarItemDto calendarItemDto)
        {
            _dispatcher.Dispatch(new CreateCalendarItemAction(calendarItemDto));
        }
        public void UpdateCalendarItem(Guid id, CalendarItemDto calendarItemDto)
        {
            _dispatcher.Dispatch(new UpdateCalendarItemAction(id, calendarItemDto));
        }
        public void DeleteCalendarItem(Guid id)
        {
            _dispatcher.Dispatch(new DeleteCalendarItemAction(id));
        }
    }
}
