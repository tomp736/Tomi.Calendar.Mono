using System;
using System.Collections.Generic;

namespace Tomi.Calendar.Mono.Server.DataServices
{
    public class DbContextEvents
    {
        public Action<IEnumerable<Type>> EntitiesChanged { get; set; }
    }
}
