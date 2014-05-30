using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace GrimmTWEACer
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

        public void CreateWorksheet()
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

        public void WriteToColumn(Column column, List<string> items)
        {
            Excel.Worksheet currentWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            for (int i = 0; i < items.Count; i++)
            {
                currentWorksheet.Cells[i + 2, column] = items[i];
            }
        }

        public void SaveWorkbookAsXLSX(string name)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string fileName = currentDirectory + @"\" + name + ".xlsx";
            int numAppend = 1;

            while (File.Exists(fileName))
            {
                fileName = currentDirectory + @"\" + name + " " + numAppend.ToString() + ".xlsx";
                numAppend++;
            }
            
            excelApp.ActiveWorkbook.SaveAs(fileName);
        }

        void FormatWorksheet(Excel.Worksheet worksheet)
        {
            worksheet.Cells[1, Column.RockName] = Constants.RockNameTitle;
            worksheet.Cells[1, Column.FormName] = Constants.FormNameTitle;
            worksheet.Cells[1, Column.PassFail] = Constants.PassFailTitle;
            worksheet.Cells[1, Column.Developer] = Constants.DevSigTitle;
            worksheet.Cells[1, Column.QA] = Constants.QaSigTitle;
            worksheet.Cells[1, Column.Notes] = Constants.NotesTitle;

        }
    }
}
