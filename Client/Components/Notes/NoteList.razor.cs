using Blazored.Modal;
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

        [Inject]
        protected StateFacade StateFacade { get; set; }

        [Inject]
        public IModalService Modal { get; set; }


        protected async override Task OnInitializedAsync()
        {
            StateFacade.LoadNotes();
            await base.OnInitializedAsync();
        }

        protected async Task ShowEditItem(Guid itemId)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(NoteCardEditView.Id), itemId);

            var modal = Modal.Show<NoteCardEditView>("Edit Note", parameters);
            var result = await modal.Result;

            StateHasChanged();
        }
    }
}
