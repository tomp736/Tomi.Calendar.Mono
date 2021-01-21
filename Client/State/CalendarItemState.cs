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
        public List<Tag> Tags { get; internal set; } = new List<Tag>();

        public async Task InitializeCalendarItemsAsync()
        {
            CalendarItems.Clear();
            Tags.Clear();

            CalendarItems.AddRange(await _calendarHttpService.GetCalendarItemsAsync());
            Tags.AddRange(await _calendarHttpService.GetTagsAsync());
        }

        public CalendarItem GetCalendarItem(Guid id)
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


        internal Tag GetTag(Guid id)
        {
            return Tags.FirstOrDefault(item => item.Id == id);
        }
        public async Task Delete(Tag tag)
        {
            await _calendarHttpService.Delete(tag).ContinueWith(result =>
            {
                if (result.IsCompletedSuccessfully)
                {
                    CalendarItems.RemoveAll(item => item.Id == tag.Id);
                }
            });
        }
        public async Task Save(Tag tag)
        {
            await _calendarHttpService.Save(tag).ContinueWith(result =>
            {
                if (result.IsCompletedSuccessfully)
                {
                    if (!CalendarItems.Exists(ci => ci.Id == tag.Id))
                    {
                        Tags.Add(tag);
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
