using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.Note;

namespace Tomi.Calendar.Mono.Client.Components.Notes
{
    public partial class NoteCardView : FluxorComponent
    {
        [Inject]
        protected IState<CalendarState> CalendarState { get; set; }
        [Parameter]
        public Guid Id { get; set; } = Guid.Empty;

        protected NoteDto Note { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Id != Guid.Empty)
            {
                Note = CalendarState.Value.Notes.FirstOrDefault(n => n.Id == Id);
            }
        }
    }
}
