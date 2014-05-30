using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GrimmTWEACer
{
    public partial class GUI : Form
    {
        ServerSettings settings;
        List<string> servers = new List<string>();
        List<string> logins = new List<string>();
        string status;

        public GUI()
        {
            InitializeComponent();

            if (LoadSettings())
            {
                ListServers();
                ListLoginsForServer(servers[0]);
                ResetServerDataConnection();
                ResetLoginDataConnection();
            }
        }

        public void UpdateStatusText(string updateText)
        {
            status = updateText;
            statusText.Text = status;
        }

        private bool LoadSettings()
        {
            if (File.Exists(Constants.SettingsFilePath))
            {
                settings = ServerSettings.Load(Constants.SettingsFilePath);
                return true;
            }
            return false;
        }

        private void ListServers()
        {
            settings.loginsAndPasswords.Sort();

            foreach (LoginInfo login in settings.loginsAndPasswords)
            {
                bool isNew = true;
                foreach (string server in servers)
                {
                    if (server == login.associatedServer)
                        isNew = false;
                }
                if (isNew)
                    servers.Add(login.associatedServer);
            }
        }

        private void ListLoginsForServer(string server)
        {
            logins.Clear();
            logins.TrimExcess();

            foreach (LoginInfo login in settings.loginsAndPasswords)
            {
                if (login.associatedServer == server)
                    logins.Add(login.loginName);
            }
        }

        private void ResetServerDataConnection()
        {
            serverComboBox.DataSource = null;
            serverComboBox.DataSource = servers;
        }

        private void ResetLoginDataConnection()
        {
            loginComboBox.DataSource = null;
            loginComboBox.DataSource = logins;
        }

        private void FindPasswordForLogin()
        {
            if (loginComboBox.DataSource == null)
                return;

            foreach (LoginInfo login in settings.loginsAndPasswords)
            {
                if (login.associatedServer == servers[serverComboBox.SelectedIndex] &&
                    login.loginName == logins[loginComboBox.SelectedIndex])
                {
                    passwordTextBox.Text = login.password;
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            //FormGrabber formGrabber = new FormGrabber();
            //formGrabber.CheckForms();

            UpdateStatusText("Connecting...");

            List<string> testStrings = new List<string>() { "TEST 1", "TEST 2", "TEST 3" };
            List<LoginInfo> testLogins = new List<LoginInfo>();

            testLogins.Add(new LoginInfo("key 1", "value1", "server1", false));
            testLogins.Add(new LoginInfo("key 2", "value2", "server1", false));
            testLogins.Add(new LoginInfo("key 3", "value3", "server1", false));
            testLogins.Add(new LoginInfo("key 4", "value4", "server2", false));
            testLogins.Add(new LoginInfo("key 5", "value5", "server2", true));
            testLogins.Add(new LoginInfo("key 6", "value6", "server2", false));

            ServerSettings settings = new ServerSettings(testStrings, testLogins);
            ServerSettings.Save(settings, Constants.SettingsFilePath);
            
        }

        private void serverComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListLoginsForServer(servers[serverComboBox.SelectedIndex]);
            ResetLoginDataConnection();
        }

        private void loginComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindPasswordForLogin();
        }
    }
}
