using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Mono.Shared.Dtos.Note;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;

namespace Tomi.Calendar.Mono.Client.Services
{
    public interface ICalendarHttpService
    {
        Task<CalendarItemDto> GetCalendarItemAsync(Guid calendarItemId);
        Task<CalendarItemDto[]> GetCalendarItemsAsync();
        Task Save(CalendarItemDto calendarItemDto);
        Task Delete(Guid id);

        Task<TagDto> GetTagAsync(Guid tagId);
        Task<TagDto[]> GetTagsAsync();
        Task Save(TagDto tag);
        Task Delete(TagDto tag);

        Task<NoteDto> GetNoteAsync(Guid noteId);
        Task<NoteDto[]> GetNotesAsync();
        Task Save(NoteDto note);
        Task Delete(NoteDto note);
    }
}
