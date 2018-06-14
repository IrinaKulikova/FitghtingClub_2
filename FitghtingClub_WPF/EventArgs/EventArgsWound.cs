﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitghtingClub_WPF
{
    public class EventArgsWound : EventArgs
    {
        public int Wound { get; set; }
        public BodyPart Part { get; set; }

        public EventArgsWound(BodyPart part,int wound)
        {
            Part = part;
            Wound = wound;            
        }
    }
}