using Blazored.Modal.Services;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Components.CalendarItem;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Client.Components.Calendar
{
    public partial class CalendarWeekComponent : FluxorComponent
    {
        [Inject]
        public IState<CalendarState> CalendarState { get; set; }
        [Inject]
        public StateFacade StateFacade { get; set; }
        [Inject]
        public IModalService Modal { get; set; }
        [Parameter]
        public DateTime Date { get; set; }

        protected DateTime StartDate => CalendarHelpers.GetStartDateOfWeek(new DateTime(Date.Year, Date.Month, Date.Day), CalendarState.Value.StartDayOfWeek);
        protected DateTime EndDate => StartDate.AddDays(6);

        public string Heading => $"{CalendarHelpers.GetMonthName(StartDate)} - {StartDate.Year}";

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

        public void WheelHandler(WheelEventArgs wheelEventArgs)
        {
            int weeks = wheelEventArgs.DeltaY < 0 ? 1 : -1;
            AddWeeks(weeks);
        }

        protected async Task AddNewItem()
        {
            var modal = Modal.Show<CalendarItemEditComponent>("Add Calendar Item");
            var result = await modal.Result;
        }
    }
}
