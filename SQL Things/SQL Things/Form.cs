using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Things
{
    class Form
    {

        public string RockName { get; set; }
        public string FormName { get; set; }
        public decimal ImportedRate { get; set; }
        public decimal SelectedRate { get; set; }
        public bool PassFail { get; set; }

        public Form()
        {
            RockName = "";
            FormName = "";
            ImportedRate = 0;
            SelectedRate = 0;
            PassFail = false;
        }
    }
}
