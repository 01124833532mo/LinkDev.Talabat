﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Exeptions
{
    public class ValidationExeption : BadRequestExeption
    {
        public required IEnumerable<string> Errors { get; set; }
        public ValidationExeption(string message="Bad Request") : base(message)
        {

            
        }
    }
}
