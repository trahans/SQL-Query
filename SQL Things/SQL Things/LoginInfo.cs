using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace GrimmTWEACer
{
    [Serializable]
    public class LoginInfo : ISerializable, IComparable
    {
        public string loginName;
        public string password;
        public string associatedServer;
        public bool wasLastUsed;

        public LoginInfo()
        {
            loginName = null;
            password = null;
            associatedServer = null;
            wasLastUsed = false;
        }

        public LoginInfo(string newLogin, string newPassword, string server, bool lastUsed)
        {
            loginName = newLogin;
            password = newPassword;
            associatedServer = server;
            wasLastUsed = lastUsed;
        }

        protected LoginInfo(SerializationInfo info, StreamingContext context)
        {
            loginName = info.GetString("login");
            password = info.GetString("password");
            associatedServer = info.GetString("associatedServer");
            wasLastUsed = info.GetBoolean("wasLastUsed");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("login", loginName);
            info.AddValue("password", password);
            info.AddValue("associatedServer", associatedServer);
            info.AddValue("wasLastUsed", wasLastUsed);
        }

        // IComparable: sorts last used login to front
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            LoginInfo otherLogin = obj as LoginInfo;
            if (otherLogin != null)
            {
                if (this.wasLastUsed)
                    return -1;
                else if (otherLogin.wasLastUsed)
                    return 1;
                else
                    return 0;
            }
            else
                throw new ArgumentException("Object is not LoginInfo");
        }

    }
}
