using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_Things
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            //FormGrabber formGrabber = new FormGrabber();
            //formGrabber.CheckForms();

            List<string> testStrings = new List<string>() {"TEST 1", "TEST 2", "TEST 3"};
            List<Login> testLogins = new List<Login>();

            testLogins.Add(new Login("key 1", "value1"));
            testLogins.Add(new Login("key 2", "value2"));
            testLogins.Add(new Login("key 3", "value3"));
            testLogins.Add(new Login("key 4", "value4"));
            testLogins.Add(new Login("key 5", "value5"));
            testLogins.Add(new Login("key 6", "value6"));

            ServerSettings settings = new ServerSettings(testStrings,testLogins);
            ServerSettings.Save(settings, @"testConfigs.xml");
        }
    }
}
