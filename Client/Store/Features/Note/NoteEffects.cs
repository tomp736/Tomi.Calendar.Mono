using Fluxor;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.Note;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Client.Store.Features.Note
{
    public class NoteEffects
    {
        private readonly CalendarDataService _calendarDataService;
        private readonly ILogger<NoteEffects> _logger;
        private readonly IState<CalendarState> _state;

        public NoteEffects(CalendarDataService calendarDataService, ILogger<NoteEffects> logger, IState<CalendarState> state)
        {
            _calendarDataService = calendarDataService;
            _logger = logger;
            _state = state;
        }

        [EffectMethod]
        public async Task HandleAsync(LoadNotesAction action, IDispatcher dispatcher)
        {
            try
            {
                var notes = await _calendarDataService.GetNotes(new GetNotesRequest());
                dispatcher.Dispatch(new LoadNotesSuccessAction(notes.Notes));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadNotesFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(LoadNoteDetailAction action, IDispatcher dispatcher)
        {
            try
            {
                GetNotesResponse calendarItemsResponse = await _calendarDataService.GetNotes(new GetNotesRequest()
                {
                    NoteIds = new Guid[] { action.Id }
                });
                NoteDto calendarItemDto = calendarItemsResponse.Notes.FirstOrDefault();
                if (calendarItemDto != null)
                {
                    dispatcher.Dispatch(new LoadNoteDetailSuccessAction(calendarItemDto));
                }
                else
                {
                    dispatcher.Dispatch(new LoadNoteDetailFailureAction("Resource does not exist"));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadNoteDetailFailureAction(e.Message));
            }
        }


        [EffectMethod]
        public async Task HandleAsync(NewNoteAction action, IDispatcher dispatcher)
        {
            try
            {
                GetNotesResponse calendarItemsResponse = await _calendarDataService.GetNotes(new GetNotesRequest()
                {
                    NoteIds = new Guid[] { action.Id }
                });
                if (calendarItemsResponse.Notes != null &&
                    calendarItemsResponse.Notes.Any())
                {
                    dispatcher.Dispatch(new NewNoteFailureAction($"Resource already exists for {action.Id}"));
                }
                else
                {
                    dispatcher.Dispatch(new NewNoteSuccessAction(action.Id));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new NewNoteFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(UpdateNoteAction action, IDispatcher dispatcher)
        {
            try
            {
                NoteDto noteDto = new NoteDto();
                noteDto.Id = action.Id;
                noteDto.Title = action.NoteDto.Title;
                noteDto.Content = action.NoteDto.Content;

                SaveNotesResponse calendarItemsResponse = await _calendarDataService.SaveNotes(new SaveNotesRequest()
                {
                    Notes = new NoteDto[] { noteDto }
                });
                if (calendarItemsResponse.Notes.Any())
                {
                    dispatcher.Dispatch(new UpdateNoteSuccessAction(calendarItemsResponse.Notes.FirstOrDefault()));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new UpdateNoteFailureAction(e.Message));
            }
        }

        [EffectMethod]
        public async Task HandleAsync(CreateNoteAction action, IDispatcher dispatcher)
        {
            try
            {
                NoteDto noteDto = new NoteDto();
                noteDto.Title = action.Note.Title;
                noteDto.Content = action.Note.Content;

                SaveNotesResponse calendarItemsResponse = await _calendarDataService.SaveNotes(new SaveNotesRequest()
                {
                    Notes = new NoteDto[] { noteDto }
                });
                if (calendarItemsResponse.Notes.Any())
                {
                    dispatcher.Dispatch(new UpdateNoteSuccessAction(calendarItemsResponse.Notes.FirstOrDefault()));
                }
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new UpdateNoteFailureAction(e.Message));
            }
        }


        [EffectMethod]
        public async Task HandleAsync(DeleteNoteAction action, IDispatcher dispatcher)
        {
            try
            {
                DeleteNotesResponse calendarItemsResponse = await _calendarDataService.DeleteNotes(new DeleteNotesRequest()
                {
                    NoteIds = new Guid[] { action.Id }
                });
                dispatcher.Dispatch(new DeleteNoteSuccessAction(action.Id));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new DeleteNoteFailureAction(e.Message));
            }
        }
    }
}
