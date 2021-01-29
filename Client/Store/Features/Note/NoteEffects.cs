using Fluxor;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;

namespace Tomi.Calendar.Mono.Client.Store.Features.Note
{
    public class NoteEffects
    {
        private readonly CalendarHttpService _calendarHttpService;
        private readonly ILogger<NoteEffects> _logger;
        private readonly IState<CalendarState> _state;

        public NoteEffects(CalendarHttpService calendarHttpService, ILogger<NoteEffects> logger, IState<CalendarState> state)
        {
            _calendarHttpService = calendarHttpService;
            _logger = logger;
            _state = state;
        }

        [EffectMethod]
        public async Task HandleAsync(LoadNotesAction action, IDispatcher dispatcher)
        {
            try
            {
                var notes = await _calendarHttpService.GetNotesAsync();
                dispatcher.Dispatch(new LoadNotesSuccessAction(notes));
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
                var note = await _calendarHttpService.GetNoteAsync(action.Id);
                dispatcher.Dispatch(new LoadNoteDetailSuccessAction(note));
            }
            catch (Exception e)
            {
                dispatcher.Dispatch(new LoadNoteDetailFailureAction(e.Message));
            }
        }
    }
}
