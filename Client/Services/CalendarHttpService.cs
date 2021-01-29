using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Mono.Shared.Dtos.Note;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;
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
        public async Task<CalendarItemDto> GetCalendarItemAsync(Guid calendarItemId)
        {
            return await _httpClient.GetFromJsonAsync<CalendarItemDto>($"/api/calendaritem/{calendarItemId}");
        }
        public async Task<CalendarItemDto[]> GetCalendarItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<CalendarItemDto[]>("/api/calendaritem");
        }
        public async Task Save(CalendarItemDto calendarItemDto)
        {
            if (calendarItemDto.Id == Guid.Empty)
                calendarItemDto.Id = Guid.NewGuid();

            await _httpClient.PostAsJsonAsync("/api/calendaritem", calendarItemDto);
        }
        public async Task Delete(Guid id)
        {
            await _httpClient.DeleteAsync($"/api/calendaritem/{id}");
        }
        #endregion

        #region Tags
        public async Task<TagDto> GetTagAsync(Guid tagId)
        {
            return await _httpClient.GetFromJsonAsync<TagDto>($"/api/tag/{tagId}");
        }
        public async Task<TagDto[]> GetTagsAsync()
        {
            return await _httpClient.GetFromJsonAsync<TagDto[]>("/api/tag");
        }
        public async Task Save(TagDto calendarItemModel)
        {
            if (calendarItemModel.Id == Guid.Empty)
                calendarItemModel.Id = Guid.NewGuid();

            await _httpClient.PostAsJsonAsync("/api/tag", calendarItemModel);
        }
        public async Task Delete(TagDto calendarItemModel)
        {
            await _httpClient.DeleteAsync($"/api/tag/{calendarItemModel.Id}");
        }
        #endregion


        #region Notes
        public async Task<NoteDto> GetNoteAsync(Guid noteId)
        {
            return await _httpClient.GetFromJsonAsync<NoteDto>($"/api/note/{noteId}");
        }

        public async Task<NoteDto[]> GetNotesAsync()
        {
            return await _httpClient.GetFromJsonAsync<NoteDto[]>("/api/note");
        }
        public async Task Save(NoteDto note)
        {
            if (note.Id == Guid.Empty)
                note.Id = Guid.NewGuid();

            await _httpClient.PostAsJsonAsync("/api/note", note);
        }
        public async Task Delete(NoteDto note)
        {
            await _httpClient.DeleteAsync($"/api/note/{note.Id}");
        }
        #endregion
    }
}
