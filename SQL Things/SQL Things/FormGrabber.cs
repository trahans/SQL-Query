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
                GrabStateWhitelist();
                GrabForms();
                serverConnection.Close();
                Console.WriteLine("Done");
            }
            Console.ReadLine();
        }

        void GrabStateWhitelist()
        {
            stateWhitelist = CSVReader.ReadCSV(@"State Whitelist.csv");
            foreach (string state in stateWhitelist)
            {
                Console.WriteLine(state);
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

                return false;
            }
        }

        void GrabForms()
        {
            List<String> rockNames = new List<string>();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string[] query = CSVReader.ReadCSV(@"SQL Query.csv");

            cmd.CommandText = query[0];
            cmd.Connection = serverConnection;

            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    rockNames.Add(reader.GetString(0));
                }

                WriteRockNamesToFile(rockNames);

            }
            else
            {
                Console.WriteLine("No rows found.");
            }
        }

        void WriteRockNamesToFile(List<String> rockNames)
        {
            string[] names = rockNames.ToArray();
            System.IO.File.WriteAllLines(@"rocknames.txt", names);
        }
    }
}
