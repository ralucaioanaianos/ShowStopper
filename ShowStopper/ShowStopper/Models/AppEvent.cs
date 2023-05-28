using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Models
{
    public class Event
    {
        public string Id { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; } 
        public string Type { get; set; }
        public string Date { get; set; }
        public string Organizer { get; set; }
        public string Location { get; set; }
    }
}
