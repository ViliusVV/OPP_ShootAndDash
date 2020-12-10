using Client.Utilities;
using Microsoft.AspNetCore.SignalR.Client;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Client.Managers.Proxy
{
    public class ConnectionManagerProxy : IConnectionManager
    {
        private ConnectionManager connectionManager;
        public HubConnection Connection 
        { 
            get { return connectionManager.Connection; }
            set { connectionManager.Connection = value; } 
        }
        public Clock ActivityClock 
        { 
            get { return connectionManager.ActivityClock; }
            set { connectionManager.ActivityClock = value; } 
        }
        public ConnectionManagerProxy(ConnectionManager conMan)
        {
            connectionManager = conMan;
        }
        public bool ConnectToHub()
        {
            GameApplication.defaultLogger.LogMessage(20, "Performing access control");
            if(ControlAccess())
            {
                GameApplication.defaultLogger.LogMessage(30, "Access granted");

            }
            else
            {
                GameApplication.defaultLogger.LogMessage(50, "Access denied, check file integrity!");
                return false;
            }

            GameApplication.defaultLogger.LogMessage(20, "Connecting to server");
            if(connectionManager.ConnectToHub())
            {
                GameApplication.defaultLogger.LogMessage(31, "Connected to server");

                return true;
            }
            else
            {
                GameApplication.defaultLogger.LogMessage(50, "Connection failed");

                return false;
            }
        }

        public bool IsConnected()
        {
            return connectionManager.IsConnected();
        }
        private bool ControlAccess()
        {
            if (GetCheckSum().CompareTo("f121cadc785d9ed4c1fd9b2001c71b7a") == 0)
                return true;
            else
                return false;
        }
        private string GetCheckSum()
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead("SFML.System.dll"))
                {

                    //File.WriteAllBytes("hash.txt", md5.ComputeHash(stream));
                    //return File.WriteAllText("hash.txt", BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant());
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
   
}
