using System;
using System.Collections.Generic;
using Tomi.Calendar.Mono.Client.Store.Features.Shared;
using Tomi.Calendar.Mono.Shared.Dtos.Note;

namespace Tomi.Calendar.Mono.Client.Store.Features.Note
{

    public record NewNoteAction
    {
        public NewNoteAction(Guid id) => Id = id;
        public Guid Id { get; }
    }
    public record NewNoteSuccessAction
    {
        public NewNoteSuccessAction(Guid id) => Id = id;
        public Guid Id { get; }
    }
    public record NewNoteFailureAction : FailureAction
    {
        public NewNoteFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record CreateNoteAction
    {
        public CreateNoteAction(CreateOrUpdateNoteDto note) => Note = note;
        public CreateOrUpdateNoteDto Note { get; }
    }
    public record CreateNoteFailureAction : FailureAction
    {
        public CreateNoteFailureAction(string errorMessage) : base(errorMessage) { }
    }
    public record CreateNoteSuccessAction
    {
        public CreateNoteSuccessAction(NoteDto note) => Note = note;
        public NoteDto Note { get; }
    }


    public record LoadNotesAction { }
    public record LoadNotesSuccessAction
    {
        public LoadNotesSuccessAction(IEnumerable<NoteDto> notes) => (Notes) = (notes);
        public IEnumerable<NoteDto> Notes { get; }
    }
    public record LoadNotesFailureAction : FailureAction
    {
        public LoadNotesFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record LoadNoteDetailAction
    {
        public LoadNoteDetailAction(Guid id) => Id = id;
        public Guid Id { get; set; }
    }
    public record LoadNoteDetailSuccessAction
    {
        public LoadNoteDetailSuccessAction(NoteDto note) => (Note) = (note);
        public NoteDto Note { get; }
    }
    public record LoadNoteDetailFailureAction : FailureAction
    {
        public LoadNoteDetailFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record UpdateNoteAction
    {
        public UpdateNoteAction(Guid id, CreateOrUpdateNoteDto noteDto) => (Id, NoteDto) = (id, noteDto);
        public Guid Id { get; }
        public CreateOrUpdateNoteDto NoteDto { get; }
    }
    public record UpdateNoteSuccessAction
    {
        public UpdateNoteSuccessAction(NoteDto note) => (Note) = (note);
        public NoteDto Note { get; }
    }
    public record UpdateNoteFailureAction : FailureAction
    {
        public UpdateNoteFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record DeleteNoteAction
    {
        public DeleteNoteAction(Guid id) => (Id) = (id);
        public Guid Id { get; }
    }
    public record DeleteNoteSuccessAction
    {
        public DeleteNoteSuccessAction(Guid id) => (Id) = (id);
        public Guid Id { get; }
    }
    public record DeleteNoteFailureAction : FailureAction
    {
        public DeleteNoteFailureAction(string errorMessage) : base(errorMessage) { }
    }
}
