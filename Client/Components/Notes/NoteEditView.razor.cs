using Blazored.Modal;
using Blazored.Modal.Services;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;

namespace Tomi.Calendar.Mono.Client.Components.Notes
{
    public partial class NoteEditView : FluxorComponent
    {
        [Inject]
        protected IState<CalendarState> CalendarState { get; set; }
        [Inject]
        protected StateFacade StateFacade { get; set; }
        [Inject]
        public IModalService Modal { get; set; }
        [Parameter]
        public Guid Id { get; set; } = Guid.Empty;

        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        protected TextEditor NoteTextEditor { get; set; }

        private CreateOrUpdateNoteValidationModel validationModel =
            new CreateOrUpdateNoteValidationModel();

        protected override void OnInitialized()
        {
            if (Id != Guid.Empty)
            {
                StateFacade.LoadNoteById(Id);
            }

            // Register a state change to assign the validation fields
            CalendarState.StateChanged += (sender, state) =>
            {
                if (state.CurrentCalendarItem is null)
                {
                    return;
                }
                validationModel.Title = state.CurrentNote.Title;
                validationModel.Content = state.CurrentNote.Content;

                StateHasChanged();
            };

            base.OnInitialized();
        }

        protected async Task DeleteItem()
        {
            StateFacade.DeleteNote(CalendarState.Value.CurrentNote!.Id);
            await ModalInstance?.CloseAsync(ModalResult.Ok(this));
        }

        public async Task HandleSubmit(EditContext editContext)
        {
            bool formIsValid = editContext.Validate();
            if (formIsValid)
            {
                HandleValidSubmit();
                await ModalInstance.CloseAsync(ModalResult.Ok(this));
            }
        }
        private void HandleValidSubmit()
        {
            // We use the bang operator (!) to tell the compiler we'll know this string field will not be null
            StateFacade.UpdateNote(
                CalendarState.Value.CurrentNote!.Id,
                validationModel.Title!,
                validationModel.Content!);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
