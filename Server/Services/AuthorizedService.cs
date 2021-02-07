using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Server.Models;

namespace Tomi.Calendar.Mono.Server
{
    [Authorize]
    public class AuthorizedService
    {
        private readonly AppNpgSqlDataContext _dataContext;

        public AuthorizedService(AppNpgSqlDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private ApplicationUser _applicationUser = null;
        public ApplicationUser CurrentUser(HttpContext httpContext)
        {
            var userId = CurrentUserId(httpContext);

            if (userId != null && _applicationUser == null)
            {
                var applicationUser = _dataContext.Users.Where(n => n.Id == userId)
                    .Include(item => item.UserCalendarItems.Where(n => n.UserKey == userId))
                    .ThenInclude(item => item.CalendarItem).FirstOrDefault();

                return applicationUser;
            }
            return _applicationUser;
        }

        public string CurrentUserId(HttpContext httpContext)
        {
            return httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
