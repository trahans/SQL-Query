using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Things
{
    static class Constants
    {
        public static string RockNameQuery { get { return "SELECT DISTINCT RockName FROM dbo.tbl_DS_ConvertedBilling"; } }
        public static string FormNameQuery { get { return "SELECT RockName, FormName FROM dbo.tbl_RM_Form WHERE MonthDateEnd LIKE '900001' order by FormName"; } }
        public static string MissmatchingRatesQuery { get { return "SELECT DISTINCT RockName FROM dbo.tbl_DS_ConvertedBilling WHERE SelectedRate <> ImportedRate and RecordType = 'Tax'"; } }
        public static string BlendedRatesQuery { get { return "select distinct C.RockName from tbl_DS_ConvertedTaxRate C inner join tbl_DS_ConvertedBilling D on D.RecordType = 'Tax' and C.MonthDateEnd = '900001' where C.RockName not in ( Select Distinct A.RockName from tbl_DS_ConvertedTaxRate A inner join tbl_DS_ConvertedBilling B On A.RockName = B.RockName and a.MCTLineCode = b.LineCode and b.RecordType = 'Tax' and a.MonthDateEnd = '900001' where A.RateTableRate1 <> B.ImportedRate )"; } }
        public static string ConfigInfoFilePath { get { return "config.txt"; } }
        public static string ToolName { get { return "PtS"; } }
        public static string RockNameTitle { get { return @"RockName"; } }
        public static string FormNameTitle { get { return @"Form Name"; } }
        public static string PassFailTitle { get { return @"Pass/Fail/Pass with Condition"; } }
        public static string DevSigTitle { get { return @"Developer"; } }
        public static string QaSigTitle { get { return @"QA"; } }
        public static string NotesTitle { get { return @"Notes"; } }
    }
}
