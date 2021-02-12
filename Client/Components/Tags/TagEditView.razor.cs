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
        public Guid? Id { get; set; } = Guid.Empty;

        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        private CreateOrUpdateTagValidationModel validationModel =
            new CreateOrUpdateTagValidationModel();

        protected override void OnInitialized()
        {
            // Register a state change to assign the validation fields
            CalendarState.StateChanged += (sender, state) =>
            {
                if (state.CurrentTag is null)
                {
                    return;
                }

                Id = state.CurrentTag.Id;
                validationModel.Name = state.CurrentTag.Name;
                validationModel.Description = state.CurrentTag.Description;
                validationModel.Color = state.CurrentTag.Color;

                StateHasChanged();
            };

            if (Id.HasValue && Id.Value != Guid.Empty)
            {
                StateFacade.LoadTagById(Id.Value);
            }
            else
            {
                StateFacade.NewTag(Guid.NewGuid());
            }

            base.OnInitialized();
        }

        protected async Task DeleteItem()
        {
            StateFacade.DeleteTag(Id.Value);
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
            StateFacade.UpdateTag(Id.Value, validationModel.ToCreateOrUpdateTagDto());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
