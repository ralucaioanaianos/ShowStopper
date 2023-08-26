using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Models
{
    public class AppLocation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public int RatingsNumber { get; set; }
        public decimal Rating { get; set; }
        public List<LocationReview> Reviews { get; set; }
    }
}
