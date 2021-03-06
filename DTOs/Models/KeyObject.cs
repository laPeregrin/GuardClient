﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Models
{
    public class KeyObject : DomainObject
    {
        public string AudNum { get; set; }
        public bool IsBooked { get; set; } = false;

        public User? User { get; set; }
        public Guid? UserId { get; set; }
        public Permission? Permission { get; set; } = null;
    }
}
