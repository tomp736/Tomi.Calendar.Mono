using Blazored.Modal;
using Blazored.Modal.Services;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;

namespace Tomi.Calendar.Mono.Client.Components.CalendarItem
{
    public partial class CalendarItemListComponent : FluxorComponent
    {
        [Inject]
        public IModalService Modal { get; set; }

        [Parameter]
        public IEnumerable<CalendarItemDto> CalendarItems { get; set; }

        [Parameter]
        public RenderFragment<CalendarItemDto> CalendarItemFormat { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected void EditCalendarItem(Guid itemId)
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(CalendarItemEditComponent.Id), itemId);
            Modal.Show<CalendarItemEditComponent>("Edit Calendar Item", parameters);
        }
    }
}
