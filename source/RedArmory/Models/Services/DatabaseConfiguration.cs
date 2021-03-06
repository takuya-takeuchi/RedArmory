﻿namespace Ouranos.RedArmory.Models.Services
{

    public sealed class DatabaseConfiguration
    {

        #region Constructors

        public DatabaseConfiguration(string mode, string name, string host, string username, string password, string encoding,
            int port)
        {
            this.Mode = mode;
            this.Name = name;
            this.Host = host;
            this.Username = username;
            this.Password = password;
            this.Encoding = encoding;
            this.Port = port;
        }

        #endregion

        #region Properties

        public string Encoding
        {
            get;
            set;
        }

        public string Host
        {
            get;
            set;
        }

        public string Mode
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        #endregion

    }

}