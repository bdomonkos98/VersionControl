﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimExample.Entities
{
    public class DeathProbability
    {
        public Gender Gender { get; set; }
        public byte Age { get; set; }
        public double DeathP { get; set; }
    }
}
