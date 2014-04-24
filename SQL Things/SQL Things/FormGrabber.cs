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
            stateWhitelist = FileReader.ReadCSV(@"State Whitelist.csv");
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
            List<String> states = new List<string>();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string query = FileReader.ReadTextFile(@"SQL Query.csv");

            cmd.CommandText = query;
            cmd.Connection = serverConnection;

            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    rockNames.Add(reader.GetString(0));
                    states.Add(reader.GetString(1));
                }

                WriteRockNamesToFile(rockNames, states);

            }
            else
            {
                Console.WriteLine("No rows found.");
            }
        }

        void WriteRockNamesToFile(List<String> rockNames, List<String> states)
        {
            string[] rocks = rockNames.ToArray();

            for (int i = 0; i < rocks.Length; i++)
            {
                rocks[i] = states[i] + " " + rocks[i];
            }

            System.IO.File.WriteAllLines(@"rocknames.txt", rocks);
        }
    }
}
