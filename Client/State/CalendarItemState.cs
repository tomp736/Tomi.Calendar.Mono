using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Client.State
{
    public class CalendarItemState
    {
        private CalendarHttpService _calendarHttpService;
        public DayOfWeek StartDayOfWeek { get; set; }

        public CalendarItemState(CalendarHttpService calendarHttpService)
        {
            _calendarHttpService = calendarHttpService;
        }

        public List<CalendarItem> CalendarItems { get; set; } = new List<CalendarItem>();

        public async Task InitializeCalendarItemsAsync()
        {
            CalendarItems.Clear();
            CalendarItems.AddRange(await _calendarHttpService.GetItemsAsync());
        }
        public CalendarItem GetItem(Guid id)
        {
            return CalendarItems.FirstOrDefault(item => item.Id == id);
        }
        public async Task Delete(CalendarItem calendarItem)
        {
            await _calendarHttpService.Delete(calendarItem).ContinueWith(result =>
            {
                if(result.IsCompletedSuccessfully)
                {
                    CalendarItems.RemoveAll(item => item.Id == calendarItem.Id);
                }
            });
        }
        public async Task Save(CalendarItem calendarItem)
        {
            await _calendarHttpService.Save(calendarItem).ContinueWith(result =>
            {
                if (result.IsCompletedSuccessfully)
                {
                    if(!CalendarItems.Exists(ci => ci.Id == calendarItem.Id))
                    {
                        CalendarItems.Add(calendarItem);
                    }
                }
            });
        }

        public event Action OnChange;

        public void SetProperty(string value)
        {
            // Property = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

    }
}
