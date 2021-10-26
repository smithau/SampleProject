using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleProject.Models
{
    public class ProjectNoteCounts
    {
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int Count { get; set; }
    }
}