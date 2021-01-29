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

namespace Tomi.Calendar.Mono.Client.Components.Tags
{
    public partial class TagEditView : FluxorComponent
    {
        [Inject]
        protected IState<CalendarState> CalendarState { get; set; }
        [Inject]
        protected StateFacade StateFacade { get; set; }
        [Parameter]
        public Guid Id { get; set; } = Guid.Empty;
        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        private CreateOrUpdateTagValidationModel validationModel =
            new CreateOrUpdateTagValidationModel();

        protected override void OnInitialized()
        {
            if (Id != Guid.Empty)
            {
                StateFacade.LoadTagById(Id);
            }

            // Register a state change to assign the validation fields
            CalendarState.StateChanged += (sender, state) =>
            {
                if (state.CurrentCalendarItem is null)
                {
                    return;
                }
                validationModel.Name = state.CurrentTag.Name;
                validationModel.Description = state.CurrentCalendarItem.Title;

                StateHasChanged();
            };

            base.OnInitialized();
        }

        protected async Task DeleteItem()
        {
            StateFacade.DeleteTag(CalendarState.Value.CurrentTag!.Id);
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
            StateFacade.UpdateTag(
                CalendarState.Value.CurrentTag!.Id,
                validationModel.Name!,
                validationModel.Description!);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
