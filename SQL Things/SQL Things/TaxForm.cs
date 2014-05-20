using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Things
{
    class TaxForm
    {

        public string RockName { get; set; }
        public string FormName { get; set; }
        public bool PassFail { get; set; }

        public TaxForm()
        {
            RockName = "";
            FormName = "";
            PassFail = true;
        }
    }
}
