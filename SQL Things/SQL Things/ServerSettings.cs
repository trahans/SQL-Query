using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SQL_Things
{
    [Serializable]
    public class LoginInfo : ISerializable
    {
        public string loginName;
        public string password;

        public LoginInfo()
        {
            loginName = null;
            password = null;
        }

        public LoginInfo(string newLogin, string newPassword)
        {
            loginName = newLogin;
            password = newPassword;
        }

        protected LoginInfo(SerializationInfo info, StreamingContext context)
        {
            loginName = info.GetString("login");
            password = info.GetString("password");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("login", loginName);
            info.AddValue("password", password);
        }

    }


    [Serializable]
    public class ServerSettings
    {

        [XmlArray]
        public List<string> servers = new List<string>();
        [XmlArray]
        public List<LoginInfo> loginsAndPasswords = new List<LoginInfo>();

        public ServerSettings()
        {

        }

        public ServerSettings(List<string> serv, List<LoginInfo> log)
        {
            servers = serv;
            loginsAndPasswords = log;
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
