using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tomi.Calendar.Mono.Shared.Entities
{
    public class CalendarItem
    {
        [Key]
        public int Key { get; set; }
        public Guid Id { get; set; }

        private DateTime? _startDate;
        public DateTime StartDate
        {
            get
            {
                if (_startDate.HasValue)
                    return _startDate.Value;
                return DateTime.Today;
            }
            set
            {
                _startDate = value;
            }
        }

        private DateTime? _endDate;
        public DateTime EndDate
        {
            get
            {
                if (_endDate.HasValue)
                    return _endDate.Value;
                if (_startDate.HasValue)
                    return _startDate.Value;
                return DateTime.MaxValue;
            }
            set
            {
                _endDate = value;
            }
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public List<CalendarItemTag> CalendarItemTags { get; set; }
        public List<CalendarItemNote> CalendarItemNotes { get; set; }
    }
}
