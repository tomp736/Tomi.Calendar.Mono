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
using Tomi.Calendar.Mono.Shared.Dtos.Note;
using Tomi.Calendar.Mono.Shared.Entities;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Server
{
    public class GrpcNoteService : AuthorizedService, INoteService
    {
        private readonly ILogger<GrpcNoteService> _logger;
        private readonly AppNpgSqlDataContext _dataContext;
        public GrpcNoteService(AppNpgSqlDataContext dataContext, ILogger<GrpcNoteService> logger)
            : base(dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public ValueTask<GetNotesResponse> GetNotes(GetNotesRequest request, CallContext context = default)
        {
            GetNotesResponse getNotesResponse = new GetNotesResponse();
            ApplicationUser applicationUser = base.CurrentUser(context.ServerCallContext.GetHttpContext());
            if (applicationUser != null)
            {
                IEnumerable<Note> userNotes = _dataContext.ApplicationUserNotes
                    .Where(n => n.UserKey == applicationUser.Id)
                    .Include(n => n.Note)
                    .Select(n => n.Note).ToList();

                getNotesResponse.Notes = userNotes.Select(n => n.ToDto());
                if (request.NoteIds != null && request.NoteIds.Any())
                {
                    getNotesResponse.Notes = getNotesResponse.Notes.Where(n => (request.NoteIds.Contains(n.Id)));
                }
                
            }
            return ValueTask.FromResult(getNotesResponse);
        }

        public async ValueTask<SaveNotesResponse> SaveNotes(SaveNotesRequest request, CallContext context = default)
        {
            // save items sent to server for the current user
            // return updated items back to client
            SaveNotesResponse getNotesResponse = new SaveNotesResponse();
            ApplicationUser applicationUser = base.CurrentUser(context.ServerCallContext.GetHttpContext());

            if (applicationUser != null)
            {
                IEnumerable<Note> userNotes = _dataContext.ApplicationUserNotes
                    .Where(n => n.UserKey == applicationUser.Id)
                    .Include(n => n.Note)
                    .Where(n => request.Notes.Select(ci => ci.Id).Contains(n.Note.Id))
                    .Select(n => n.Note);

                foreach (NoteDto calendarItemDto in request.Notes)
                {
                    Note calendarItem = userNotes.FirstOrDefault(n => n.Id == calendarItemDto.Id);
                    bool isNew = false;
                    if (calendarItem == null)
                    {
                        calendarItem = new Note();
                        isNew = true;
                    }

                    calendarItem.Id = calendarItemDto.Id;
                    calendarItem.Title = calendarItemDto.Title;
                    calendarItem.Content = calendarItemDto.Content;

                    if (isNew)
                    {
                        _dataContext.ApplicationUserNotes.Add(new ApplicationUserNote() { Note = calendarItem, User = applicationUser });
                        _dataContext.Notes.Add(calendarItem);
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


                getNotesResponse.Notes = _dataContext.ApplicationUserNotes
                        .Where(n => n.UserKey == applicationUser.Id)
                        .Include(n => n.Note)
                        .Where(n => request.Notes.Select(ci => ci.Id).Contains(n.Note.Id))
                        .Select(n => n.Note.ToDto());
            }


            return await ValueTask.FromResult(getNotesResponse);
        }


        public async ValueTask<DeleteNotesResponse> DeleteNotes(DeleteNotesRequest request, CallContext context = default)
        {

            DeleteNotesResponse result = new DeleteNotesResponse();
            ApplicationUser applicationUser = base.CurrentUser(context.ServerCallContext.GetHttpContext());

            if (applicationUser != null)
            {
                IEnumerable<Note> userNotes = _dataContext.ApplicationUserNotes
                    .Where(n => n.UserKey == applicationUser.Id)
                    .Include(n => n.Note)
                    .Where(n => request.NoteIds.Contains(n.Note.Id))
                    .Select(n => n.Note);

                foreach (Guid calendarItemId in request.NoteIds)
                {
                    Note calendarItem = userNotes.FirstOrDefault(n => n.Id == calendarItemId);
                    if (calendarItem != null)
                    {
                        _dataContext.Notes.Remove(calendarItem);
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
