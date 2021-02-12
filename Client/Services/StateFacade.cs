using Fluxor;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Collections.Generic;
using Tomi.Calendar.Mono.Client.Store.Features.CalendarItem;
using Tomi.Calendar.Mono.Client.Store.Features.Note;
using Tomi.Calendar.Mono.Client.Store.Features.Tag;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Mono.Shared.Dtos.Note;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;

namespace Tomi.Calendar.Mono.Client.Services
{
    public class StateFacade
    {
        private readonly ILogger<StateFacade> _logger;
        private readonly IDispatcher _dispatcher;

        public StateFacade(ILogger<StateFacade> logger, IDispatcher dispatcher) =>
            (_logger, _dispatcher) = (logger, dispatcher);




        public void LoadCalendarItems()
        {
            _dispatcher.Dispatch(new LoadCalendarItemsAction());
        }
        public void LoadCalendarItemById(Guid id)
        {
            _dispatcher.Dispatch(new LoadCalendarItemDetailAction(id));
        }
        internal void NewCalendarItem(Guid id)
        {
            _dispatcher.Dispatch(new NewCalendarItemAction(id));
        }
        public void CreateCalendarItem(string title, string description, LocalDate startDate, LocalDate endDate, LocalTime startTime, LocalTime endTime, IEnumerable<Guid> tagIds, IEnumerable<Guid> noteIds)
        {
            var dto = new CreateOrUpdateCalendarItemDto(title, description, startDate, endDate, startTime, endTime, tagIds, noteIds);
            _dispatcher.Dispatch(new CreateCalendarItemAction(dto));
        }
        public void UpdateCalendarItem(Guid id, string title, string description, LocalDate startDate, LocalDate endDate, LocalTime startTime, LocalTime endTime, IEnumerable<Guid> tagIds, IEnumerable<Guid> noteIds)
        {
            var dto = new CreateOrUpdateCalendarItemDto(title, description, startDate, endDate, startTime, endTime, tagIds, noteIds);
            _dispatcher.Dispatch(new UpdateCalendarItemAction(id, dto));
        }
        public void DeleteCalendarItem(Guid id)
        {
            _dispatcher.Dispatch(new DeleteCalendarItemAction(id));
        }




        public void LoadTags()
        {
            _dispatcher.Dispatch(new LoadTagsAction());
        }
        public void LoadTagById(Guid id)
        {
            _dispatcher.Dispatch(new LoadTagDetailAction(id));
        }
        internal void NewTag(Guid id)
        {
            _dispatcher.Dispatch(new NewTagAction(id));
        }
        public void CreateTag(CreateOrUpdateTagDto createOrUpdateTagDto)
        {
            _dispatcher.Dispatch(new CreateTagAction(createOrUpdateTagDto));
        }
        public void UpdateTag(Guid id, CreateOrUpdateTagDto createOrUpdateTagDto)
        {
            _dispatcher.Dispatch(new UpdateTagAction(id, createOrUpdateTagDto));
        }
        public void DeleteTag(Guid id)
        {
            _dispatcher.Dispatch(new DeleteTagAction(id));
        }




        public void LoadNotes()
        {
            _dispatcher.Dispatch(new LoadNotesAction());
        }
        public void LoadNoteById(Guid id)
        {
            _dispatcher.Dispatch(new LoadNoteDetailAction(id));
        }
        internal void NewNote(Guid id)
        {
            _dispatcher.Dispatch(new NewNoteAction(id));
        }
        public void CreateNote(string title, string content)
        {
            var dto = new CreateOrUpdateNoteDto(title, content);
            _dispatcher.Dispatch(new CreateNoteAction(dto));
        }
        public void UpdateNote(Guid id, string title, string content)
        {
            var dto = new CreateOrUpdateNoteDto(title, content);
            _dispatcher.Dispatch(new UpdateNoteAction(id, dto));
        }
        public void DeleteNote(Guid id)
        {
            _dispatcher.Dispatch(new DeleteNoteAction(id));
        }
    }
}
