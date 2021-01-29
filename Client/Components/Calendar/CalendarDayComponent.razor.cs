using Blazored.Modal;
using Blazored.Modal.Services;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Client.Components.CalendarItem;
using Tomi.Calendar.Mono.Client.Services;
using Tomi.Calendar.Mono.Client.Store.State;

namespace Tomi.Calendar.Mono.Client.Components.Calendar
{
    public partial class CalendarDayComponent : FluxorComponent
    {
        [Inject]
        protected IState<CalendarState> CalendarState { get; set; }
        protected StateFacade StateFacade { get; set; }
        [Inject]
        public IModalService Modal { get; set; }
        [Parameter]
        public DateTime Date { get; set; }
        [Parameter]
        public bool Enabled { get; set; }

        protected string Heading => $"{Date.Month}/{Date.Day}";
        protected string ClassNames
        {
            get
            {
                string classnames = "";
                classnames += Date.AddDays(1).DayOfWeek == CalendarState.Value.StartDayOfWeek ? "last " : "";
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
        }
    }
}
