using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleProject.Models
{
    public class Note
    {
        public int NoteId { get; set; }
        public string NoteText { get; set; }
        public DateTime CreationTimestamp { get; set; }

        public List<int> AttributeIds { get; set; }
        public int? ProjectId { get; set; }
    }
}
