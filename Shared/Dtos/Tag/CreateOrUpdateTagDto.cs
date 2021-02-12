using System;

namespace Tomi.Calendar.Mono.Shared.Dtos.Tag
{
    public class CreateOrUpdateTagDto
    {
        public CreateOrUpdateTagDto(string name, string description, string color)
        {
            Name = name;
            Description = description;
            Color = color;
        }

        public string Name { get; init; }
        public string Description { get; init; }
        public string Color { get; init; }
    }
}
