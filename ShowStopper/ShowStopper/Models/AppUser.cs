using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Models
{
    public class AppUser 
    {
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        private string _name;
        public string Name
        {
            get { return _name != null ? _name : FirstName + " " + LastName; }
            set { _name = value; }
        }

        public string Email { get; set; }
        public string ProfileImage { get; set; }

        //public string ConnectionId { get; set; }
    }
}
