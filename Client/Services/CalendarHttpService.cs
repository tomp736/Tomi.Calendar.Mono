using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Client.Services
{
    public class CalendarHttpService : ICalendarHttpService
    {
        private readonly HttpClient _httpClient;
        public CalendarHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region CalendarItems

        public async Task<CalendarItem[]> GetCalendarItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<CalendarItem[]>("/api/calendaritem");
        }
        public async Task<CalendarItem> GetCalendarItemAsync(Guid calendarItemId)
        {
            return await _httpClient.GetFromJsonAsync<CalendarItem>($"/api/calendaritem/{calendarItemId}");
        }
        public async Task Save(CalendarItem calendarItemModel)
        {
            if (calendarItemModel.Id == Guid.Empty)
                calendarItemModel.Id = Guid.NewGuid();

            await _httpClient.PostAsJsonAsync("/api/calendaritem", calendarItemModel);
        }
        public async Task Delete(CalendarItem calendarItemModel)
        {
            await _httpClient.DeleteAsync($"/api/calendaritem/{calendarItemModel.Id}");
        }

        #endregion

        #region Tags

        internal async Task<Tag[]> GetTagsAsync()
        {
            return await _httpClient.GetFromJsonAsync<Tag[]>("/api/tag");
        }
        public async Task Save(Tag calendarItemModel)
        {
            if (calendarItemModel.Id == Guid.Empty)
                calendarItemModel.Id = Guid.NewGuid();

            await _httpClient.PostAsJsonAsync("/api/tag", calendarItemModel);
        }
        public async Task Delete(Tag calendarItemModel)
        {
            await _httpClient.DeleteAsync($"/api/tag/{calendarItemModel.Id}");
        }

        #endregion
    }
}
