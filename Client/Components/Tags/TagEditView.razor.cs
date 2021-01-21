using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.State;

namespace Tomi.Calendar.Mono.Client.Components.Tags
{
    public partial class TagEditView : ComponentBase
    {
        [Inject]
        public CalendarItemState CalendarState { get; set; }

        [Inject]
        public IModalService Modal { get; set; }

        [Parameter]
        public Guid Id { get; set; } = Guid.Empty;

        [Parameter]
        public Action StateChangedCallback { get; set; }

        [CascadingParameter]
        protected BlazoredModalInstance ModalInstance { get; set; }

        protected Mono.Shared.Entities.Tag Tag { get; set; }

        protected bool IsNew => Tag.Key == 0;

        protected async override Task OnInitializedAsync()
        {
            if (Id != Guid.Empty)
            {
                Tag = CalendarState.GetTag(Id);
            }
            if (Tag == null)
            {
                Tag = new Mono.Shared.Entities.Tag();
                Tag.Id = Id;
            }
            await base.OnInitializedAsync();
        }

        protected async Task DeleteItem()
        {
            await CalendarState.Delete(Tag);
            await ModalInstance.CloseAsync(ModalResult.Ok(this));
        }

        protected async Task SaveItem()
        {
            await CalendarState.Save(Tag);
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
