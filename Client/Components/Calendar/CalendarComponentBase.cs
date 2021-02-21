using Blazored.Modal;
using Blazored.Modal.Services;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Components.CalendarItem;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;

namespace Tomi.Calendar.Mono.Client.Components.Calendar
{
    public class CalendarComponentBase : FluxorComponent
    {
        [Inject]
        protected IState<CalendarState> CalendarState { get; set; }

        [Inject]
        public StateFacade StateFacade { get; set; }

        [Inject]
        public IModalService Modal { get; set; }

        [Parameter]
        public DateTime Date { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        public void AddMonths(int months)
        {
            Date = Date.AddDays(months * 7 * 5);
        }

        public void AddWeeks(int weeks)
        {
            Date = Date.AddDays(weeks * 7);
        }

        public void AddDays(int days)
        {
            Date = Date.AddDays(days);
        }

        public void EditCalendarItem(Guid itemId)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(CalendarItemEditComponent.Id), itemId);
            Modal.Show<CalendarItemEditComponent>("Edit Calendar Item", parameters);
        }

        public void AddCalendarItem()
        {
            Modal.Show<CalendarItemEditComponent>("Add Calendar Item");
        }
    }
}
