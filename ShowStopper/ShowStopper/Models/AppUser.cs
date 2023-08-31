using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Models
{
    public class AppUser 
    {

        public string Id { get; set; }  
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        private string _name;
        public string Name
        {
            get { return _name != null ? _name : FirstName + " " + LastName; }
            set { _name = value; }
        }

        public FileResult Photo { get; set; }

        public string Email { get; set; }
        public string ProfileImage { get; set; }

        public string UserType { get; set; }

        public string PhoneNumber { get; set; }

        public string CompanyName { get; set; } 

        public string PhotoStr { get; set; }
        public List<Ticket> AttendingEvents { get; set; }
    }
}
