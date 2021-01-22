using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.State;

namespace Tomi.Calendar.Mono.Client.Components.Notes
{
    public partial class NoteCardEditView : ComponentBase
    {
        [Inject]
        public CalendarItemState CalendarState { get; set; }

        [Parameter]
        public Guid Id { get; set; } = Guid.Empty;

        protected Mono.Shared.Entities.Note Note { get; set; }

        protected TextEditor NoteTextEditor { get; set; }

        protected bool IsNew => Note.Key == 0;

        protected async override Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                Note = CalendarState.GetNote(Id);
            }
            if (Note == null)
            {
                Note = new Mono.Shared.Entities.Note();
                Note.Id = Id;
            }
            await base.OnInitializedAsync();
        }
        protected async Task DeleteItem()
        {
            await CalendarState.Delete(Note);
        }

        protected async Task SaveItem()
        {
            Note.Content = await NoteTextEditor.GetHtmlContent();
            await CalendarState.Save(Note);
        }
    }
}
