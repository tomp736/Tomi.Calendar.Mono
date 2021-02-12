using System;

namespace Tomi.Calendar.Mono.Shared.Dtos.Tag
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
