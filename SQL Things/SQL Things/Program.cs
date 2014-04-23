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
            SqlConnection myConnection = new SqlConnection("server=localhost;" +
                                       "Trusted_Connection=yes;" +
                                       "database=TSE_Connector2; " +
                                       "connection timeout=30");
            try
            {
                myConnection.Open();
                Console.WriteLine("Well done!");
                Console.ReadLine();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("You failed!" + ex.Message);
                Console.ReadLine();
            }

        }
    }
}
