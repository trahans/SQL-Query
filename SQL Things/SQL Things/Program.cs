using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SQL_Things
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerConnect();
        }

        static void ServerConnect()
        {
            List<String> rockNames = new List<string>();

            SqlConnection myConnection = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=TSE_Connector2; " +
                                       "connection timeout=30");
            try
            {
                myConnection.Open();
                Console.WriteLine("Connection Established");

                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;

                cmd.CommandText = "SELECT RockName FROM dbo.tbl_DS_ConvertedBilling WHERE ReportedRate <> ImportedRate";
                cmd.Connection = myConnection;

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                       string rockName = reader.GetString(0);
                       bool containsItem = rockNames.Any(name => name == rockName);

                       if (!containsItem)
                       {
                           rockNames.Add(rockName);
                       }
                    }

                    foreach (string name in rockNames)
                    {
                        Console.WriteLine(name);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                myConnection.Close();
                Console.ReadLine();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Connection Failed" + ex.Message);
                Console.ReadLine();
            }

        }
    }
}
