using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;


namespace GrimmTWEACer
{
    class FormGrabber
    { 

        SqlConnection serverConnection;
        List<TaxForm> forms = new List<TaxForm>();
        List<string> rockNames = new List<string>();
        List<string> missmatchingForms = new List<string>();
        List<string> blendedRates = new List<string>();
        Dictionary<string,string> rockNamesAndFormNames = new Dictionary<string,string>();

        public FormGrabber()
        {
            serverConnection = SQLGrabber.PrepareSQLServerConnection();
        }

        public void CheckForms()
        {
            if (OpenServerConnection())
            {
                QueryServer();
                CreateFormsFromRockNames();
                TranscribeFormNamesToForms();
                OrderFormsByFormName();
                FailMissmatchingRates();
                PassBlendedRates();
                WriteFormsToFile();
                CloseServerConnection();
            }
        }

        bool OpenServerConnection()
        {
            try
            {
                serverConnection.Open();
               // Console.WriteLine("Connection Established");
                return true;
            }
            catch (SqlException ex)
            {
               // Console.WriteLine("Connection Failed" + ex.Message);
               // Console.ReadLine();
                return false;
            }
        }

        void QueryServer()
        {
            rockNames = SQLGrabber.ListFromQuery(serverConnection, Constants.RockNameQuery);
            rockNamesAndFormNames = SQLGrabber.DictionaryFromQuery(serverConnection, Constants.FormNameQuery);
            missmatchingForms = SQLGrabber.ListFromQuery(serverConnection, Constants.MissmatchingRatesQuery);
            blendedRates = SQLGrabber.ListFromQuery(serverConnection, Constants.BlendedRatesQuery);
        }

        void CreateFormsFromRockNames()
        {
            foreach (string rockName in rockNames)
            {
                if (rockNamesAndFormNames.ContainsKey(rockName))
                {
                    TaxForm newForm = new TaxForm();
                    newForm.RockName = rockName;
                    forms.Add(newForm);
                }
            }
        }

        void TranscribeFormNamesToForms()
        {
            foreach (TaxForm form in forms)
            {
                form.FormName = rockNamesAndFormNames[form.RockName];
            }
        }

        void OrderFormsByFormName()
        {
            forms = forms.OrderBy(o => o.FormName).ToList();
        }

        void FailMissmatchingRates()
        {
            foreach (TaxForm form in forms)
            {
                foreach (string rockName in missmatchingForms)
                {
                    if (form.RockName == rockName)
                    {
                        form.PassFail = false;
                    }
                }
                
            }
            
        }

        void PassBlendedRates()
        {
            foreach (TaxForm form in forms)
            {
                foreach (string rockName in blendedRates)
                {
                    if (form.RockName == rockName)
                        form.PassFail = true;
                }        
            }
        }

        void WriteFormsToFile()
        {

            List<string> rockNames = new List<string>();
            List<string> formNames = new List<string>();
            List<string> passFail = new List<string>();
            List<string> developer = new List<string>();
            
            string[] configInfo = SQLGrabber.GrabConfig();
            string databaseName = configInfo[1];

            foreach (TaxForm form in forms)
            {
                rockNames.Add(form.RockName);
                formNames.Add(form.FormName);
                passFail.Add((form.PassFail == true) ? "Pass" : "Fail");
                developer.Add((form.PassFail == true) ? Constants.ToolName : "");
            }

            SheetWriter sheetWriter = new SheetWriter();
            sheetWriter.CreateWorksheet();
            sheetWriter.WriteToColumn(Column.RockName, rockNames);
            sheetWriter.WriteToColumn(Column.FormName, formNames);
            sheetWriter.WriteToColumn(Column.PassFail, passFail);
            sheetWriter.WriteToColumn(Column.Developer, developer);
            sheetWriter.NameSheet(databaseName);
            sheetWriter.DeleteExtraSheets();
            sheetWriter.SaveWorkbookAsXLSX(databaseName);
        }

        void CloseServerConnection()
        {
            serverConnection.Close();
           // Console.WriteLine("Connection Closed");
        }
    }
}
