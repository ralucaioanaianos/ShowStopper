﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Models
{
    public class Ticket
    {
        public AppUser AppUser { get; set; }
        public AppEvent AppEvent { get; set; } 
    }
}