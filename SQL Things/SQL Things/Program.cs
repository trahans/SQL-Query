﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Things
{
    class Program
    {
        static void Main(string[] args)
        {
            GrabForms();
        }

        static void GrabForms()
        {
            FormGrabber formGrabber = new FormGrabber();
            formGrabber.CheckForms();
        }
    }
}
