﻿using NodaTime;
using System;
using System.ComponentModel.DataAnnotations;
using Tomi.Calendar.Proto;

namespace Tomi.Calendar.Mono.Shared.Entities
{
    public class CalendarItem
    {
        [Key]
        public int Key { get; set; }
        public Guid Id { get; set; }

        public LocalDate StartDate { get; set; }
        public LocalDate EndDate { get; set; }

        public LocalTime StartTime { get; set; }
        public LocalTime EndTime { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public CalendarItemDto ToDto()
        {
            return new CalendarItemDto()
            {
                Id = Id,
                Title = Title,
                Description = Description,
                StartDate = StartDate,
                EndDate = EndDate,
                StartTime = StartTime,
                EndTime = EndTime
            };
        }
    }
}
