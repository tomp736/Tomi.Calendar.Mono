using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.State;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Client.Components
{
    public class CalendarItemViewBase : ComponentBase
    {
        [Inject]
        public CalendarItemState CalendarState { get; set; }

        [Inject]
        public IModalService Modal { get; set; }


        [Parameter]
        public Guid Id { get; set; }

        [Parameter]
        public Action StateChangedCallback { get; set; }


        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        protected CalendarItem CalendarItem { get; set; }

        protected async override Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                CalendarItem = CalendarState.GetItem(Id);
            }
            else
            {
                CalendarItem = new CalendarItem();
            }
            await base.OnInitializedAsync();
        }

        protected async Task DeleteItem()
        {
            await CalendarState.Delete(CalendarItem);
            await ModalInstance.CloseAsync(ModalResult.Ok(this));
        }

        protected async Task SaveItem()
        {
            await CalendarState.Save(CalendarItem);
            await ModalInstance.CloseAsync(ModalResult.Ok(this));
        }

        public void StateChanged()
        {
            if (StateChangedCallback != null)
            {
                StateChangedCallback.Invoke();
            }
            else
            {
                StateHasChanged();
            }
        }
    }
}
