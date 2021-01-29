using System;

namespace Tomi.Calendar.Mono.Shared.Dtos.Note
{
    public class CreateOrUpdateNoteDto
    {
        public CreateOrUpdateNoteDto(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public string Title { get; init; }
        public string Content { get; init; }
    }
}
