using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace GrimmTWEACer
{
    class FileReader
    {

        public static string[] ReadCSV (string filePath)
        {
            string[] csvItems;
            string csvInput = ReadTextFile(filePath);
            csvItems = csvInput.Split(',');
            return csvItems;
        }

        public static string ReadTextFile (string filePath)
        {
            StreamReader reader = new StreamReader(File.OpenRead(filePath));
            string textItem;
            textItem = reader.ReadLine();
            return textItem;
        }
    }
}
