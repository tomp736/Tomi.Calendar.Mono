using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.State;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Client.Components
{

    public class CalendarWeekViewBase : ComponentBase
    {
        [Inject]
        public CalendarItemState CalendarState { get; set; }

        [Inject]
        public IModalService Modal { get; set; }


        [Parameter]
        public DateTime Date { get; set; }

        [Parameter]
        public Action StateChangedCallback { get; set; }


        protected DateTime StartDate => CalendarHelpers.GetStartDateOfWeek(new DateTime(Date.Year, Date.Month, Date.Day), CalendarState.StartDayOfWeek);
        protected DateTime EndDate => StartDate.AddDays(6);


        public DateTime CenterDate => CalendarHelpers.CalendarDaysInView(StartDate, EndDate).Skip(17).FirstOrDefault();
        public string Heading => $"{CalendarHelpers.GetMonthName(CenterDate)} - {CenterDate.Year}";

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
            int weeks = wheelEventArgs.DeltaY > 0 ? 1 : -1;
            AddWeeks(weeks);
            StateChanged();
        }

        protected async Task AddNewItem()
        {
            var modal = Modal.Show<CalendarItemView>("Add Calendar Item");
            var result = await modal.Result;

            StateChanged();
        }

        protected void StateChanged()
        {
            if (StateChangedCallback != null)
            {
                StateChangedCallback.Invoke();
            }
            else
            {
                StateHasChanged();
            }
        }
    }
}
