using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrimmTWEACer
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUI());
            //GrabForms();
        }

        static void GrabForms()
        {
            FormGrabber formGrabber = new FormGrabber();
            formGrabber.CheckForms();
        }
    }
}
