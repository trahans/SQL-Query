using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security;

namespace SQL_Things
{
    class SQLGrabber
    {
        public static SqlConnection PrepareSQLServerConnection()
        {
            SqlConnection serverConnection;

            string[] configInfo = GrabConfig();
            string server = configInfo[0];
            string database = configInfo[1];
            string userID = configInfo[2];
            string password = configInfo[3];

            SecureString securePassword = new SecureString();
            foreach (char ch in password)
                securePassword.AppendChar(ch);

            securePassword.MakeReadOnly();
            SqlCredential credentials = new SqlCredential(userID, securePassword);

            /*
            serverConnection = new SqlConnection("server=localhost;" +
                                                 "Trusted_Connection=yes;" +
                                                 "database=TSE_Connector2; " +
                                                 "connection timeout=30");
            */

            serverConnection = new SqlConnection("server=" + server + ";" +
                                                 "Trusted_Connection=no;" +
                                                 "database=" + database + "; " +
                                                 "connection timeout=30", credentials);
            return serverConnection;
        }

        public static string[] GrabConfig()
        {
            return FileReader.ReadCSV(Constants.ConfigInfoFilePath);
        }

        public static List<string> ListFromQuery(SqlConnection serverConnection, string query)
        {
            List<Object[]> rawQueryData;
            List<string> itemsFromQuery = new List<string>();

            rawQueryData = ExecuteQuery(serverConnection, query);

            foreach (Object[] item in rawQueryData)
            {
                itemsFromQuery.Add((string)item[0]);
            }

            return itemsFromQuery;
        }

        public static Dictionary<string, string> DictionaryFromQuery(SqlConnection serverConnection, string query)
        {
            List<Object[]> rawQueryData;
            Dictionary<string, string> itemsFromQuery = new Dictionary<string, string>();

            rawQueryData = ExecuteQuery(serverConnection, query);

            foreach (Object[] item in rawQueryData)
            {
                itemsFromQuery.Add((string)item[0], (string)item[1]);
            }

            return itemsFromQuery;
        }

        static List<Object[]> ExecuteQuery(SqlConnection serverConnection, string query)
        {
            List<Object[]> queryItems = new List<Object[]>();
            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            command.CommandText = query;
            command.Connection = serverConnection;

            reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                queryItems = GrabItemsFromQuery(reader);
            }
            else
            {
                //Console.WriteLine("No rows found in query: " + query);
               // Console.ReadLine();
            }

            reader.Close();

            return queryItems;
        }

        static List<Object[]> GrabItemsFromQuery(SqlDataReader reader)
        {
            List<Object[]> queryItems = new List<Object[]>();
            while (reader.Read())
            {
                Object[] values = new Object[reader.FieldCount];
                reader.GetValues(values);
                queryItems.Add(values);
            }
            return queryItems;
        }
    }
}
