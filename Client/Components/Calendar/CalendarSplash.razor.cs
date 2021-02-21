using Blazored.Modal;
using Blazored.Modal.Services;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using System;
using Tomi.Calendar.Mono.Client.Components.CalendarItem;
using Tomi.Calendar.Mono.Client.Store.State;

namespace Tomi.Calendar.Mono.Client.Components.Calendar
{
    public partial class CalendarSplash : FluxorComponent
    {
        [Inject]
        IState<CalendarState> CalendarState { get; set; }


        [Inject]
        public IModalService Modal { get; set; }

        [Parameter]
        public DateTime SelectedDayDate { get; set; }

        [Parameter]
        public DateTime SelectedMonthDate { get; set; }

        private CalendarMonthComponent _calendarMonthComponent { get; set; }

        public void EditCalendarItem(Guid itemId)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(CalendarItemEditComponent.Id), itemId);
            Modal.Show<CalendarItemEditComponent>("Edit Calendar Item", parameters);
        }

        public void AddCalendarItem()
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(CalendarItemEditComponent.StartDate), NodaTime.LocalDate.FromDateTime(SelectedDayDate));
            parameters.Add(nameof(CalendarItemEditComponent.EndDate), NodaTime.LocalDate.FromDateTime(SelectedDayDate));
            Modal.Show<CalendarItemEditComponent>("Add Calendar Item", parameters);
        }

        public void NextMonth()
        {
            DateTime nextMonth = SelectedMonthDate.AddMonths(1);
            SelectedMonthDate = new DateTime(nextMonth.Year, nextMonth.Month, 1);
        }

        public void ThisMonth()
        {
            SelectedMonthDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        }

        public void PrevMonth()
        {
            DateTime nextMonth = SelectedMonthDate.AddMonths(-1);
            SelectedMonthDate = new DateTime(nextMonth.Year, nextMonth.Month, 1);
        }
    }
}
