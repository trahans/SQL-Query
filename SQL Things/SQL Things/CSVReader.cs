using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQL_Things
{
    class CSVReader
    {

        public static string[] ReadCSV (string filePath)
        {
            StreamReader reader = new StreamReader(File.OpenRead(filePath));
            string[] csvItems;
            
            string csvInput = reader.ReadLine();
            csvItems = csvInput.Split(',');

            return csvItems;
        }
    }
}
