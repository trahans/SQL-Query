using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SQL_Things
{
    [Serializable]
    public class Login
    {
        public string login;
        public string password;

        public Login(string newLogin, string newPassword)
        {
            login = newLogin;
            password = newPassword;
        }

    }


    [Serializable]
    public class ServerSettings
    {

        [XmlArray]
        public List<string> servers = new List<string>();
        [XmlArray]
        public List<Login> loginsAndPasswords = new List<Login>();

        public ServerSettings()
        {

        }

        public ServerSettings(List<string> serv, List<Login> log)
        //public ServerSettings(List<string> serv)
        {
            servers = serv;
            //loginsAndPasswords = log;
        }

        public static void Save(ServerSettings settings, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ServerSettings));
            TextWriter textWriter = new StreamWriter(@filePath);
            serializer.Serialize(textWriter, settings);
            textWriter.Close();
        }

        public static ServerSettings Load(string filePath)
        {
            ServerSettings settings = new ServerSettings();
            return settings;
        }

    }
}
