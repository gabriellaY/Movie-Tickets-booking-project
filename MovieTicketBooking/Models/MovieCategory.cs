﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTicketBooking.Models
{
    public class MovieCategory
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }
    }
}
