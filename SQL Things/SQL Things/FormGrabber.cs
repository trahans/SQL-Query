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
        List<String> rockNames = new List<string>();
        List<String> states = new List<string>();

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
                GrabRockNameWhitelist();
                GrabForms();
                serverConnection.Close();
                Console.WriteLine("Done");
            }
            Console.ReadLine();
        }

        void GrabStateWhitelist()
        {
            stateWhitelist = FileReader.ReadCSV(@"State Whitelist.csv");
        }

        void GrabRockNameWhitelist()
        {
            rockNameWhitelist = FileReader.ReadCSV(@"RockName Whitelist.csv");
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
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            string query = FileReader.ReadTextFile(@"SQL Query.csv");

            cmd.CommandText = query;
            cmd.Connection = serverConnection;

            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                ReadSQL(reader);
                WriteRockNamesToFile();
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
        }

        void ReadSQL(SqlDataReader reader)
        {
            while (reader.Read())
            {
                string state = reader.GetString(1);
                string rock = reader.GetString(0);

                if (!CheckStateWhitelist(state) && !CheckRockNameWhitelist(rock))
                {
                    rockNames.Add(rock);
                    states.Add(state);
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

        void WriteRockNamesToFile()
        {
            string[] rocks = rockNames.ToArray();

            for (int i = 0; i < rocks.Length; i++)
            {
                rocks[i] = states[i] + " " + rocks[i];
            }

            System.IO.File.WriteAllLines(@"RockNames.txt", rocks);
        }
    }
}
