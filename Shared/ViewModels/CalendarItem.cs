using System;

namespace Tomi.Calendar.Mono.Shared
{
    public class CalendarItem : ICalendarItem
    {
        public Guid Id { get; set; }

        private DateTime? _startDate;
        public DateTime StartDate
        {
            get
            {
                if (_startDate.HasValue)
                    return _startDate.Value;
                return DateTime.MinValue;
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
    }
}
