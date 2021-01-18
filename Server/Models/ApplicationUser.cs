using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tomi.Calendar.Mono.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Extended Properties
        public string Nickname { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
