using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleProject.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Pwd { get; set; }
        public DateTime Creation { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
