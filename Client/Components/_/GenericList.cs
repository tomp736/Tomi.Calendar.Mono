using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Tomi.Calendar.Mono.Client.Components._
{
    public partial class GenericList<ItemType> : FluxorComponent
    {
        [Parameter]
        public IEnumerable<ItemType> Items { get; set; }

        [Parameter]
        public RenderFragment<ItemType> ItemFormat { get; set; }
    }
}
