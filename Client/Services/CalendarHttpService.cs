using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared.Entities;

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


        #region Notes

        internal async Task<Note[]> GetNotesAsync()
        {
            return await _httpClient.GetFromJsonAsync<Note[]>("/api/note");
        }
        public async Task Save(Note note)
        {
            if (note.Id == Guid.Empty)
                note.Id = Guid.NewGuid();

            await _httpClient.PostAsJsonAsync("/api/note", note);
        }
        public async Task Delete(Note note)
        {
            await _httpClient.DeleteAsync($"/api/note/{note.Id}");
        }

        #endregion
    }
}
