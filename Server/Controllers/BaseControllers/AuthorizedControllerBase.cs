using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Server.Models;
using Tomi.Calendar.Mono.Shared.Entities;

namespace Tomi.Calendar.Mono.Server.Controllers
{
    [Authorize]
    public class AuthorizedControllerBase : ControllerBase
    {
        private readonly AppNpgSqlDataContext _dataContext;

        public AuthorizedControllerBase(AppNpgSqlDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private ApplicationUser _applicationUser = null;
        public ApplicationUser? CurrentUser
        {
            get
            {
                if (CurrentUserId != null && _applicationUser == null)
                {
                    var applicationUser = _dataContext.Users.Where(n => n.Id == CurrentUserId);

                    return applicationUser.FirstOrDefault();
                }
                return _applicationUser;
            }
        }

        public string CurrentUserId
        {
            get
            {
                return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }
    }
}
