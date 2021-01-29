﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;

namespace Tomi.Calendar.Mono.Client.Components.Notes
{
    public partial class NoteList : FluxorComponent
    {
        [Inject]
        protected IState<CalendarState> CalendarState { get; set; }

        protected StateFacade StateFacade { get; set; }

        [Inject]
        public IModalService Modal { get; set; }

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

        protected async Task ShowEditItem(Guid itemId)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(NoteEditView.Id), itemId);

            var modal = Modal.Show<NoteEditView>("Edit Note", parameters);
            var result = await modal.Result;

            StateChanged();
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
