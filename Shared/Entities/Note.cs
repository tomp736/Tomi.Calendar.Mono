using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomi.Calendar.Mono.Shared.Dtos.Note;

namespace Tomi.Calendar.Mono.Shared.Entities
{
    public class Note
    {
        [Key]
        public int Key { get; set; }
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Content { get; set; }

        public ICollection<CalendarItemNote> CalendarItemNotes { get; set; }


        public NoteDto ToDto()
        {
            return new NoteDto()
            {
                Id = Id,
                CreateDate = CreateDate,
                Title = Title,
                Content = Content
            };
        }

    }
}
