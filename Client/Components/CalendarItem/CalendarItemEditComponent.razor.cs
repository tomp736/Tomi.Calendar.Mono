using Blazored.Modal;
using Blazored.Modal.Services;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        private CreateOrUpdateCalendarItemValidationModel validationModel =
            new CreateOrUpdateCalendarItemValidationModel();

        protected override void OnInitialized()
        {
            // Register a state change to assign the validation fields
            CalendarState.StateChanged += OnCalendarStateChanged;

            // Load the note detail on initial page navigation
            if (Id.HasValue && Id.Value != Guid.Empty)
            {
                StateFacade.LoadCalendarItemById(Id.Value);
            }
            else
            {
                Id = Guid.NewGuid();
                StateFacade.NewCalendarItem(Id.Value);
            }

            base.OnInitialized();
        }

        private void OnCalendarStateChanged(object sender, CalendarState state)
        {
            if (state.CurrentCalendarItem is null)
            {
                return;
            }

            Id = state.CurrentCalendarItem.Id;
            validationModel.Title = state.CurrentCalendarItem.Title;
            validationModel.Description = state.CurrentCalendarItem.Title;
            validationModel.StartDate = state.CurrentCalendarItem.StartDate.GetValueOrDefault(LocalDate.FromDateTime(DateTime.Today));
            validationModel.EndDate = state.CurrentCalendarItem.EndDate.GetValueOrDefault(LocalDate.FromDateTime(DateTime.Today));
            validationModel.StartTime = state.CurrentCalendarItem.StartTime.GetValueOrDefault(LocalTime.MinValue);
            validationModel.EndTime = state.CurrentCalendarItem.EndTime.GetValueOrDefault(LocalTime.MaxValue);
            validationModel.TagIds = state.CurrentCalendarItem.TagIds?.ToList() ?? new List<Guid>();
            validationModel.NoteIds = state.CurrentCalendarItem.NoteIds?.ToList() ?? new List<Guid>();

            StateHasChanged();
        }

        protected async Task DeleteItem()
        {
            StateFacade.DeleteCalendarItem(Id.Value);
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
                validationModel.EndDate,
                validationModel.StartTime,
                validationModel.EndTime,
                validationModel.TagIds,
                validationModel.NoteIds);
        }
    }
}
