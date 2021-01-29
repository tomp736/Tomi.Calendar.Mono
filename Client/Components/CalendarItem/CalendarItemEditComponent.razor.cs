using Blazored.Modal;
using Blazored.Modal.Services;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Components.Tags;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;

namespace Tomi.Calendar.Mono.Client.Components.CalendarItem
{
    public partial class CalendarItemEditComponent : FluxorComponent
    {
        [Inject]
        public IState<CalendarState> CalendarState { get; set; }
        [Inject]
        protected StateFacade StateFacade { get; set; }

        [Inject]
        public IModalService Modal { get; set; }

        [Parameter]
        public Guid? Id { get; set; }

        public TagSelector TagSelector { get; set; }

        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        private CreateOrUpdateCalendarItemValidationModel validationModel =
            new CreateOrUpdateCalendarItemValidationModel();

        protected override void OnInitialized()
        {
            // Load the note detail on initial page navigation
            if (Id.HasValue)
            {
                StateFacade.LoadCalendarItemById(Id.Value);
            }
            else
            {
                Id = Guid.NewGuid();
                StateFacade.NewCalendarItem(Id.Value);
            }

            // Register a state change to assign the validation fields
            CalendarState.StateChanged += (sender, state) =>
            {
                if (state.CurrentCalendarItem is null)
                {
                    return;
                }

                Id = state.CurrentCalendarItem.Id;
                validationModel.Title = state.CurrentCalendarItem.Title;
                validationModel.Description = state.CurrentCalendarItem.Title;
                validationModel.StartDate = state.CurrentCalendarItem.StartDate.GetValueOrDefault(DateTime.Today);
                validationModel.EndDate = state.CurrentCalendarItem.EndDate.GetValueOrDefault(DateTime.Today);

                StateHasChanged();
            };

            base.OnInitialized();
        }

        protected async Task DeleteItem()
        {
            StateFacade.DeleteCalendarItem(CalendarState.Value.CurrentCalendarItem!.Id);
            await ModalInstance?.CloseAsync(ModalResult.Ok(this));
        }

        public async Task HandleSubmit(EditContext editContext)
        {
            bool formIsValid = editContext.Validate();
            if (formIsValid)
            {
                HandleValidSubmit();
                await ModalInstance?.CloseAsync(ModalResult.Ok(this));
            }
        }
        private void HandleValidSubmit()
        {
            // We use the bang operator (!) to tell the compiler we'll know this string field will not be null
            StateFacade.UpdateCalendarItem(
                Id.Value,
                validationModel.Title!,
                validationModel.Description!,
                validationModel.StartDate,
                validationModel.EndDate);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
