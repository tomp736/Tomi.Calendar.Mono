using System;
using System.Collections.Generic;
using Tomi.Calendar.Mono.Client.Store.Features.Shared;
using Tomi.Calendar.Mono.Shared.Dtos.Tag;

namespace Tomi.Calendar.Mono.Client.Store.Features.Tag
{
    public record NewTagAction
    {
        public NewTagAction(Guid id) => Id = id;
        public Guid Id { get; }
    }
    public record NewTagSuccessAction
    {
        public NewTagSuccessAction(Guid id) => Id = id;
        public Guid Id { get; }
    }
    public record NewTagFailureAction : FailureAction
    {
        public NewTagFailureAction(string errorMessage) : base(errorMessage) { }
    }

    public record CreateTagAction
    {
        public CreateTagAction(CreateOrUpdateTagDto note) => Tag = note;
        public CreateOrUpdateTagDto Tag { get; }
    }
    public record CreateTagFailureAction : FailureAction
    {
        public CreateTagFailureAction(string errorMessage) : base(errorMessage) { }
    }
    public record CreateTagSuccessAction
    {
        public CreateTagSuccessAction(TagDto note) => Tag = note;
        public TagDto Tag { get; }
    }


    public record LoadTagsAction { }
    public record LoadTagsSuccessAction
    {
        public LoadTagsSuccessAction(IEnumerable<TagDto> tags) => (Tags) = (tags);
        public IEnumerable<TagDto> Tags { get; }
    }
    public record LoadTagsFailureAction : FailureAction
    {
        public LoadTagsFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record LoadTagDetailAction
    {
        public LoadTagDetailAction(Guid id) => Id = id;
        public Guid Id { get; set; }
    }
    public record LoadTagDetailSuccessAction
    {
        public LoadTagDetailSuccessAction(TagDto tag) => (Tag) = (tag);
        public TagDto Tag { get; }
    }
    public record LoadTagDetailFailureAction : FailureAction
    {
        public LoadTagDetailFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record UpdateTagAction
    {
        public UpdateTagAction(Guid id, CreateOrUpdateTagDto tagDto) => (Id, TagDto) = (id, tagDto);
        public Guid Id { get; }
        public CreateOrUpdateTagDto TagDto { get; }
    }
    public record UpdateTagSuccessAction
    {
        public UpdateTagSuccessAction(TagDto tag) => (Tag) = (tag);
        public TagDto Tag { get; }
    }
    public record UpdateTagFailureAction : FailureAction
    {
        public UpdateTagFailureAction(string errorMessage) : base(errorMessage) { }
    }


    public record DeleteTagAction
    {
        public DeleteTagAction(Guid id) => (Id) = (id);
        public Guid Id { get; }
    }
    public record DeleteTagSuccessAction
    {
        public DeleteTagSuccessAction(Guid id) => (Id) = (id);
        public Guid Id { get; }
    }
    public record DeleteTagFailureAction : FailureAction
    {
        public DeleteTagFailureAction(string errorMessage) : base(errorMessage) { }
    }
}
