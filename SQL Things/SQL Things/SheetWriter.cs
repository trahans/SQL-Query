using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Things
{
    class SheetWriter
    {

        Excel.Application excelApp;
        Excel.Workbooks excelWorkbooks;
        Excel.Workbook excelWorkbook;
        

        public SheetWriter()
        {
            excelApp = new Excel.Application();
            excelApp.Visible = true;
        }

        public void WriteSheet()
        {

            //object misValue = System.Reflection.Missing.Value;

            excelWorkbook = excelApp.Application.Workbooks.Add();
            excelWorkbooks = excelApp.Workbooks;

            Excel.Worksheet currentWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;
 
            FormatWorksheet(currentWorksheet);
        }

        public void WriteRockNames(List<String> rockNames)
        {
            Excel.Worksheet currentWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            for (int i = 0; i < rockNames.Count; i++)
            {
                string cell = CellNumber("A", i + 2);
                currentWorksheet.get_Range(cell).Formula = rockNames[i];
            }
        }

        string CellNumber(string column, int row)
        {
            string cell;
            cell = column + row.ToString();
            return cell;
        }

        public void SaveSheet()
        {
            String fileName = @"Sheet1.xlsx";
            excelApp.ActiveWorkbook.SaveAs(fileName);
        }

        void FormatWorksheet(Excel.Worksheet worksheet)
        {
            String titleRockName = @"RockName";
            String titleFormName = @"Form Name";
            String titlePassFail = @"Pass/Fail/Pass with Condition";
            String titleDevSig = @"Developer";
            String titleQASig = @"QA";
            String titleNotes = @"Notes";

            worksheet.get_Range("A1").Formula = titleRockName;
            worksheet.get_Range("B1").Formula = titleFormName;
            worksheet.get_Range("C1").Formula = titlePassFail;
            worksheet.get_Range("D1").Formula = titleDevSig;
            worksheet.get_Range("E1").Formula = titleQASig;
            worksheet.get_Range("F1").Formula = titleNotes;

        }
    }
}
