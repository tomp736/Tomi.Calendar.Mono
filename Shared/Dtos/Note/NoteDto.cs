using System;

namespace Tomi.Calendar.Mono.Shared.Dtos.Note
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
