using System;
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

        public async Task<CalendarItem[]> GetItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<CalendarItem[]>("/api/calendaritem");
        }

        public async Task<CalendarItem> GetItemAsync(Guid calendarItemId)
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
    }
}
