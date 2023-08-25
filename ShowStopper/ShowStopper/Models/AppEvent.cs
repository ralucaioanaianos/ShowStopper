using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Models
{
    public class AppEvent
    {
        public string Id { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; } 
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Organizer { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public double Relevance { get; set; } 
    }
}
