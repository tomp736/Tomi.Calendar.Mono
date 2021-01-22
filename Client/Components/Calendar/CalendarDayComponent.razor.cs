using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.State;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Client.Components.Calendar
{
    public partial class CalendarDayComponent : ComponentBase
    {
        [Inject]
        public CalendarItemState CalendarState { get; set; }

        [Inject]
        public IModalService Modal { get; set; }


        [Parameter]
        public DateTime Date { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

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

        protected string Heading => $"{Date.Month}/{Date.Day}";
        protected string ClassNames
        {
            get
            {
                string classnames = "";
                classnames += Date.AddDays(1).DayOfWeek == CalendarState.StartDayOfWeek ? "last " : "";
                classnames += Date.Date.CompareTo(DateTime.Today) == 0 ? "today" : "";
                return classnames;
            }
        }

        protected async Task ShowEditItem(Guid itemId)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(CalendarItemEditComponent.Id), itemId);

            var modal = Modal.Show<CalendarItemEditComponent>("Edit Calendar Item", parameters);
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
