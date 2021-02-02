using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.TextEditor;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;

namespace Tomi.Calendar.Mono.Client.Components.Notes
{
    public partial class NoteCardEditView : FluxorComponent
    {
        [Inject]
        protected IState<CalendarState> CalendarState { get; set; }
        [Inject]
        protected StateFacade StateFacade { get; set; }

        [Parameter]
        public Guid? Id { get; set; }

        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        protected CreateOrUpdateNoteValidationModel validationModel =
            new CreateOrUpdateNoteValidationModel();

        protected BlazoredTextEditor NoteTextEditor { get; set; }

        protected override void OnInitialized()
        {
            if (Id.HasValue)
            {
                StateFacade.LoadNoteById(Id.Value);
            }
            else
            {
                Id = Guid.NewGuid();
                StateFacade.NewNote(Id.Value);
            }

            // Register a state change to assign the validation fields
            CalendarState.StateChanged += async (sender, state) =>
            {
                if (state.CurrentNote is null)
                {
                    return;
                }
                validationModel.Title = state.CurrentNote.Title;
                validationModel.Content = state.CurrentNote.Content;
                await NoteTextEditor.LoadHTMLContent(state.CurrentNote.Content);                

                StateHasChanged();
            };

            base.OnInitialized();
        }

        protected async Task DeleteItem()
        {
            StateFacade.DeleteNote(Id.Value);
            await ModalInstance?.CloseAsync(ModalResult.Ok(this));
        }

        public async Task HandleSubmit(EditContext editContext)
        {
            validationModel.ContentText = await NoteTextEditor.GetText();
            editContext.IsModified();
            bool formIsValid = editContext.Validate();
            if (formIsValid)
            {
                validationModel.Content = await NoteTextEditor.GetHTML();
                HandleValidSubmit();
                await ModalInstance.CloseAsync(ModalResult.Ok(this));
            }
        }
        private void HandleValidSubmit()
        {
            // We use the bang operator (!) to tell the compiler we'll know this string field will not be null
            StateFacade.UpdateNote(
                Id.Value,
                validationModel.Title!,
                validationModel.Content!);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
