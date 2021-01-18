using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.State;

namespace Tomi.Calendar.Mono.Client.Components.Tag
{
    public class TagSelectorBase : ComponentBase
    {
        [Inject]
        public CalendarItemState CalendarState { get; set; }

        [Inject]
        public IModalService Modal { get; set; }

        [Parameter]
        public List<int> SelectedKeys { get; set; }

        public ICollection<Mono.Shared.Tag> Values => CalendarState.Tags;

        [Parameter]
        public Action StateChangedCallback { get; set; }


        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
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
