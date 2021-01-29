using System;

namespace Tomi.Calendar.Mono.Shared.Dtos.Tag
{
    public class CreateOrUpdateTagDto
    {
        public CreateOrUpdateTagDto(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; init; }
        public string Description { get; init; }
    }
}
