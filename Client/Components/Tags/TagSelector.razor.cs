using Blazored.Modal.Services;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;

namespace Tomi.Calendar.Mono.Client.Components.Tags
{
    public partial class TagSelector : FluxorComponent
    {
        [Inject]
        protected IState<CalendarState> CalendarState { get; set; }
        [Inject]
        protected StateFacade StateFacade { get; set; }
        [Inject]
        public IModalService Modal { get; set; }
        [Parameter]
        public List<Guid> SelectedValues { get; set; }

        protected override void OnInitialized()
        {
            StateFacade.LoadTags();
            base.OnInitialized();
        }
    }
}
