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
        struct Form
        {
            public string state;
            public string rockName;
        }

        SqlConnection serverConnection;
        string[] stateWhitelist;
        string[] rockNameWhitelist;
        List<Form> forms = new List<Form>();

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
            SqlDataReader reader;
            reader = GrabSQLQuery();

            if (reader.HasRows)
            {
                GrabStateWhitelist();
                GrabRockNameWhitelist();
                ReadSQL(reader);
                WriteFormsToFile();
            }
            else
            {
                Console.WriteLine("No rows found.");
                Console.ReadLine();
            }
        }

        SqlDataReader GrabSQLQuery()
        {
            SqlCommand cmd = new SqlCommand();

            string query = FileReader.ReadTextFile(@"SQL Query.csv");

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

        void ReadSQL(SqlDataReader reader)
        {
            while (reader.Read())
            {
                Form incomingForm;
                incomingForm.rockName = reader.GetString(0);
                incomingForm.state = reader.GetString(1);

                if (!CheckStateWhitelist(incomingForm.state) && !CheckRockNameWhitelist(incomingForm.rockName))
                {
                    forms.Add(incomingForm);
                }
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
                outputForms[i] = forms[i].state + " " + forms[i].rockName;
            }

            System.IO.File.WriteAllLines(@"RockNames.txt", outputForms);

            List<String> rockNames = new List<String>();
            foreach (Form form in forms)
            {
                rockNames.Add(form.rockName);
            }

            SheetWriter sheetWriter = new SheetWriter();
            sheetWriter.WriteSheet();
            sheetWriter.WriteRockNames(rockNames);
            sheetWriter.SaveSheet();
        }
    }
}
