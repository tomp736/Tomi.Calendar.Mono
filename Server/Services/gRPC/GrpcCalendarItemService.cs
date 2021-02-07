using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Server.Data;
using Tomi.Calendar.Mono.Server.Models;
using Tomi.Calendar.Mono.Shared.Dtos.CalendarItem;
using Tomi.Calendar.Mono.Shared.Entities;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Server
{
    public class GrpcCalendarItemService : AuthorizedService, ICalendarItemService
    {
        private readonly ILogger<GrpcCalendarItemService> _logger;
        private readonly AppNpgSqlDataContext _dataContext;
        public GrpcCalendarItemService(AppNpgSqlDataContext dataContext, ILogger<GrpcCalendarItemService> logger)
            : base(dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public ValueTask<GetCalendarItemsResponse> GetCalendarItems(GetCalendarItemsRequest request, CallContext context = default)
        {
            GetCalendarItemsResponse getCalendarItemsResponse = new GetCalendarItemsResponse();
            ApplicationUser applicationUser = base.CurrentUser(context.ServerCallContext.GetHttpContext());
            if (applicationUser != null)
            {
                IEnumerable<CalendarItem> userCalendarItems = _dataContext.ApplicationUserCalendarItem
                    .Where(n => n.UserKey == applicationUser.Id)
                    .Include(n => n.CalendarItem)
                    .Select(n => n.CalendarItem);

                getCalendarItemsResponse.CalendarItems = userCalendarItems.Select(n => n.ToDto());
            }
            return ValueTask.FromResult(getCalendarItemsResponse);
        }

        public async ValueTask<SaveCalendarItemsResponse> SaveCalendarItems(SaveCalendarItemsRequest request, CallContext context = default)
        {
            // save items sent to server for the current user
            // return updated items back to client
            SaveCalendarItemsResponse getCalendarItemsResponse = new SaveCalendarItemsResponse();
            ApplicationUser applicationUser = base.CurrentUser(context.ServerCallContext.GetHttpContext());

            if (applicationUser != null)
            {
                IEnumerable<CalendarItem> userCalendarItems = _dataContext.ApplicationUserCalendarItem
                    .Where(n => n.UserKey == applicationUser.Id)
                    .Include(n => n.CalendarItem)
                    .Where(n => request.CalendarItems.Select(ci => ci.Id).Contains(n.CalendarItem.Id))
                    .Select(n => n.CalendarItem);

                foreach (CalendarItemDto calendarItemDto in request.CalendarItems)
                {
                    CalendarItem calendarItem = userCalendarItems.FirstOrDefault(n => n.Id == calendarItemDto.Id);
                    bool isNew = false;
                    if (calendarItem == null)
                    {
                        calendarItem = new CalendarItem();
                        isNew = true;
                    }

                    calendarItem.Id = calendarItemDto.Id;
                    calendarItem.Title = calendarItemDto.Title;
                    calendarItem.Description = calendarItemDto.Description;
                    calendarItem.StartDate = calendarItemDto.StartDate.GetValueOrDefault();
                    calendarItem.EndDate = calendarItemDto.EndDate.GetValueOrDefault();
                    calendarItem.StartTime = calendarItemDto.StartTime.GetValueOrDefault();
                    calendarItem.EndTime = calendarItemDto.EndTime.GetValueOrDefault();

                    if (isNew)
                    {
                        _dataContext.ApplicationUserCalendarItem.Add(new ApplicationUserCalendarItem() { CalendarItem = calendarItem, User = applicationUser });
                        _dataContext.CalendarItems.Add(calendarItem);
                    }
                }

                try
                {
                    int rowsAffected = await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                getCalendarItemsResponse.CalendarItems = _dataContext.ApplicationUserCalendarItem
                        .Where(n => n.UserKey == applicationUser.Id)
                        .Include(n => n.CalendarItem)
                        .Where(n => request.CalendarItems.Select(ci => ci.Id).Contains(n.CalendarItem.Id))
                        .Select(n => n.CalendarItem.ToDto());
            }


            return await ValueTask.FromResult(getCalendarItemsResponse);
        }


        public async ValueTask<DeleteCalendarItemsResponse> DeleteCalendarItems(DeleteCalendarItemsRequest request, CallContext context = default)
        {

            DeleteCalendarItemsResponse result = new DeleteCalendarItemsResponse();
            ApplicationUser applicationUser = base.CurrentUser(context.ServerCallContext.GetHttpContext());

            if (applicationUser != null)
            {
                IEnumerable<CalendarItem> userCalendarItems = _dataContext.ApplicationUserCalendarItem
                    .Where(n => n.UserKey == applicationUser.Id)
                    .Include(n => n.CalendarItem)
                    .Where(n => request.CalendarItemIds.Contains(n.CalendarItem.Id))
                    .Select(n => n.CalendarItem);

                foreach (Guid calendarItemId in request.CalendarItemIds)
                {
                    CalendarItem calendarItem = userCalendarItems.FirstOrDefault(n => n.Id == calendarItemId);
                    if (calendarItem != null)
                    {
                        _dataContext.CalendarItems.Remove(calendarItem);
                    }
                }

                try
                {
                    int rowsAffected = await _dataContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return await ValueTask.FromResult(result);
        }
    }
}
