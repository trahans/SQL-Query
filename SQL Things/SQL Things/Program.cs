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

                cmd.CommandText = "SELECT DISTINCT RockName FROM dbo.tbl_DS_ConvertedBilling WHERE ReportedRate <> ImportedRate";
                cmd.Connection = myConnection;

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rockNames.Add(reader.GetString(0));
                        //Console.WriteLine(reader.GetString(0));
                        //System.IO.File.WriteAllLines(@"C:\Users\Public\TestFolder\WriteLines.txt",  );
                    }

                    string[] nameString = rockNames.ToArray();
                    System.IO.File.WriteAllLines(@"C:\Users\christopher.grimm\Desktop\rocknames.txt", nameString);

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
