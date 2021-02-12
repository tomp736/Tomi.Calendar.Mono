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
using Tomi.Calendar.Mono.Shared.Dtos.Tag;
using Tomi.Calendar.Mono.Shared.Entities;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Server
{
    public class GrpcTagService : AuthorizedService, ITagService
    {
        private readonly ILogger<GrpcTagService> _logger;
        private readonly AppNpgSqlDataContext _dataContext;
        public GrpcTagService(AppNpgSqlDataContext dataContext, ILogger<GrpcTagService> logger)
            : base(dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public ValueTask<GetTagsResponse> GetTags(GetTagsRequest request, CallContext context = default)
        {
            GetTagsResponse getTagsResponse = new GetTagsResponse();
            ApplicationUser applicationUser = base.CurrentUser(context.ServerCallContext.GetHttpContext());
            if (applicationUser != null)
            {
                IEnumerable<Tag> userTags = _dataContext.ApplicationUserTags
                    .Where(n => n.UserKey == applicationUser.Id)
                    .Include(n => n.Tag)
                    .Select(n => n.Tag).ToList();

                getTagsResponse.Tags = userTags.Select(n => n.ToDto());
                if (request.TagIds != null && request.TagIds.Any())
                {
                    getTagsResponse.Tags = getTagsResponse.Tags.Where(n => (request.TagIds.Contains(n.Id)));
                }
                
            }
            return ValueTask.FromResult(getTagsResponse);
        }

        public async ValueTask<SaveTagsResponse> SaveTags(SaveTagsRequest request, CallContext context = default)
        {
            // save items sent to server for the current user
            // return updated items back to client
            SaveTagsResponse getTagsResponse = new SaveTagsResponse();
            ApplicationUser applicationUser = base.CurrentUser(context.ServerCallContext.GetHttpContext());

            if (applicationUser != null)
            {
                IEnumerable<Tag> userTags = _dataContext.ApplicationUserTags
                    .Where(n => n.UserKey == applicationUser.Id)
                    .Include(n => n.Tag)
                    .Where(n => request.Tags.Select(ci => ci.Id).Contains(n.Tag.Id))
                    .Select(n => n.Tag);

                foreach (TagDto tagDto in request.Tags)
                {
                    Tag tag = userTags.FirstOrDefault(n => n.Id == tagDto.Id);
                    bool isNew = false;
                    if (tag == null)
                    {
                        tag = new Tag();
                        isNew = true;
                    }

                    tag.Id = tagDto.Id;
                    tag.Name = tagDto.Name;
                    tag.Description = tagDto.Description;
                    tag.Color = tagDto.Color;

                    if (isNew)
                    {
                        _dataContext.ApplicationUserTags.Add(new ApplicationUserTag() { Tag = tag, User = applicationUser });
                        _dataContext.Tags.Add(tag);
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


                getTagsResponse.Tags = _dataContext.ApplicationUserTags
                        .Where(n => n.UserKey == applicationUser.Id)
                        .Include(n => n.Tag)
                        .Where(n => request.Tags.Select(ci => ci.Id).Contains(n.Tag.Id))
                        .Select(n => n.Tag.ToDto());
            }


            return await ValueTask.FromResult(getTagsResponse);
        }


        public async ValueTask<DeleteTagsResponse> DeleteTags(DeleteTagsRequest request, CallContext context = default)
        {

            DeleteTagsResponse result = new DeleteTagsResponse();
            ApplicationUser applicationUser = base.CurrentUser(context.ServerCallContext.GetHttpContext());

            if (applicationUser != null)
            {
                IEnumerable<Tag> userTags = _dataContext.ApplicationUserTags
                    .Where(n => n.UserKey == applicationUser.Id)
                    .Include(n => n.Tag)
                    .Where(n => request.TagIds.Contains(n.Tag.Id))
                    .Select(n => n.Tag);

                foreach (Guid calendarItemId in request.TagIds)
                {
                    Tag calendarItem = userTags.FirstOrDefault(n => n.Id == calendarItemId);
                    if (calendarItem != null)
                    {
                        _dataContext.Tags.Remove(calendarItem);
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
