using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;


namespace SQL_Things
{
    class FormGrabber
    { 

        SqlConnection serverConnection;
        string[] stateWhitelist;
        string[] rockNameWhitelist;
        List<Form> forms = new List<Form>();
        Dictionary<string,string> rockNamesAndFormNames = new Dictionary<string,string>();

        public FormGrabber()
        {
            serverConnection = new SqlConnection("server=localhost;" +
                                                 "Trusted_Connection=yes;" +
                                                 "database=TSE_Connector2; " +
                                                 "connection timeout=30");
        }

        public void GrabMissmatchingForms()
        {
            if (OpenServerConnection())
            {
                GrabForms();
                GrabFormNames();
                WriteFormsToFile();
                serverConnection.Close();
                Console.WriteLine("Done");
            }
        }

        bool OpenServerConnection()
        {
            try
            {
                serverConnection.Open();
                Console.WriteLine("Connection Established");
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection Failed" + ex.Message);
                Console.ReadLine();
                return false;
            }
        }

        void GrabForms()
        {
            SqlDataReader queryRockNames;
            queryRockNames = GrabSQLQueryFromFile(@"SQL Query For RockNames.txt");

            if (queryRockNames.HasRows)
            {
                GrabStateWhitelist();
                GrabRockNameWhitelist();
                ReadRockNames(queryRockNames);
            }
            else
            {
                Console.WriteLine("No rows found.");
                Console.ReadLine();
            }

            queryRockNames.Close();
        }

        void GrabFormNames()
        {
            SqlDataReader queryFormNames;
            queryFormNames = GrabSQLQueryFromFile(@"SQL Query For FormNames.txt");

            if (queryFormNames.HasRows)
            {
                ReadFormNames(queryFormNames);
            }
            else
            {
                Console.WriteLine("No rows found.");
                Console.ReadLine();
            }

            queryFormNames.Close();

            TranscribeFormNames();
        }

        void TranscribeFormNames()
        {
            foreach (Form form in forms)
            {
                form.FormName = rockNamesAndFormNames[form.RockName];
            }
        }

        SqlDataReader GrabSQLQueryFromFile(string filePath)
        {
            SqlCommand cmd = new SqlCommand();

            string query = FileReader.ReadTextFile(filePath);

            cmd.CommandText = query;
            cmd.Connection = serverConnection;

            return cmd.ExecuteReader();
        }

        void GrabStateWhitelist()
        {
            stateWhitelist = FileReader.ReadCSV(@"State Whitelist.csv");
        }

        void GrabRockNameWhitelist()
        {
            rockNameWhitelist = FileReader.ReadCSV(@"RockName Whitelist.csv");
        }   

        void ReadRockNames(SqlDataReader reader)
        {
            while (reader.Read())
            {
                Form incomingForm = new Form();
                incomingForm.RockName = reader.GetString(0);
                //incomingForm.SelectedRate = reader.GetDecimal(1);
                //incomingForm.ImportedRate = reader.GetDecimal(2);

                if (!CheckRockNameWhitelist(incomingForm.RockName))
                {
                    forms.Add(incomingForm);
                }
            }
        }

        void ReadFormNames(SqlDataReader reader)
        {
            while (reader.Read())
            {
                rockNamesAndFormNames.Add(reader.GetString(0), reader.GetString(1));
            }
        }

        bool CheckStateWhitelist (string state)
        {
            foreach (string excludedState in stateWhitelist)
            {
                if (excludedState == state)
                {
                    return true;
                }
            }
            return false;
        }

        bool CheckRockNameWhitelist (string rock)
        {
            foreach (string excludedRock in rockNameWhitelist)
            {
                if (excludedRock == rock)
                {
                    return true;
                }
            }
            return false;
        }

        void WriteFormsToFile()
        {
            int length = forms.Count;
            string[] outputForms = new string[length];

            for (int i = 0; i < outputForms.Length; i++)
            {
                outputForms[i] = forms[i].RockName + " " + forms[i].SelectedRate.ToString() + " " + forms[i].ImportedRate.ToString();
            }

            System.IO.File.WriteAllLines(@"RockNames.txt", outputForms);

            forms = forms.OrderBy(o => o.FormName).ToList();

            List<String> rockNames = new List<String>();
            List<String> formNames = new List<String>();
            foreach (Form form in forms)
            {
                rockNames.Add(form.RockName);
                formNames.Add(form.FormName);
            }

            SheetWriter sheetWriter = new SheetWriter();
            sheetWriter.WriteSheet();
            sheetWriter.WriteToColumn(SheetWriter.Column.RockName, rockNames);
            sheetWriter.WriteToColumn(SheetWriter.Column.FormName, formNames);
            sheetWriter.SaveSheet();
        }
    }
}
