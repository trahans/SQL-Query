using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

        public enum Column
        {
            RockName = 1,
            FormName = 2,
            PassFail = 3,
            Developer = 4,
            QA = 5,
            Notes = 6
        }
        

        public SheetWriter()
        {
            excelApp = new Excel.Application();
            excelApp.Visible = true;
        }

        public void WriteSheet()
        {

            excelWorkbook = excelApp.Application.Workbooks.Add();
            excelWorkbooks = excelApp.Workbooks;

            Excel.Worksheet currentWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;
 
            FormatWorksheet(currentWorksheet);
        }

        public void NameSheet(string name)
        {
            Excel.Worksheet currentWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;
            currentWorksheet.Name = name;
        }

        public void DeleteExtraSheets()
        {
            for (int i = excelWorkbook.Worksheets.Count; i > 1; i--)
            {
                Excel.Worksheet xlSheet;
                xlSheet = (Excel.Worksheet)excelWorkbook.Worksheets.get_Item(i);
                xlSheet.Delete();
            }
        }

        public void WriteToColumn(Column column, List<String> items)
        {
            Excel.Worksheet currentWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            for (int i = 0; i < items.Count; i++)
            {
                currentWorksheet.Cells[i + 2, column] = items[i];
            }
        }

        public void SaveSheet(string name)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            String fileName = currentDirectory + @"\" + name + ".xlsx";
            excelApp.ActiveWorkbook.SaveAs(fileName);
        }

        void FormatWorksheet(Excel.Worksheet worksheet)
        {
            String rockNameTitle = @"RockName";
            String formNameTitle = @"Form Name";
            String passFailTitle = @"Pass/Fail/Pass with Condition";
            String devSigTitle = @"Developer";
            String qaSigTitle = @"QA";
            String notesTitle = @"Notes";

            worksheet.Cells[1, Column.RockName] = rockNameTitle;
            worksheet.Cells[1, Column.FormName] = formNameTitle;
            worksheet.Cells[1, Column.PassFail] = passFailTitle;
            worksheet.Cells[1, Column.Developer] = devSigTitle;
            worksheet.Cells[1, Column.QA] = qaSigTitle;
            worksheet.Cells[1, Column.Notes] = notesTitle;

        }
    }
}
