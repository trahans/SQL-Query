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

namespace GrimmTWEACer
{
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
            XmlSerializer deserializer = new XmlSerializer(typeof(ServerSettings));
            TextReader textReader = new StreamReader(@filePath);
            ServerSettings settings;
            settings = (ServerSettings)deserializer.Deserialize(textReader);
            textReader.Close();

            return settings;
        }

    }
}
