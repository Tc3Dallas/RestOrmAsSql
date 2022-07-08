using AsOrm3;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebRestAs400
{
    public class DbContext : AsContext
    {
        public DbContext()
        {
            //ConnectionString = "DataSource = COTEX.ZEGNA.COM; UserID = FAMA; Password = WEBFAMA; DataCompression = True;AllowUnsupportedChar=true";
            ConnectionString = ConfigurationManager.ConnectionStrings["AS400Entities"].ConnectionString;
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new Exception("Non è stata specificata la ConnectionString");
            Library = ConfigurationManager.AppSettings["AsLibrary"];
            if (string.IsNullOrWhiteSpace(Library))
                throw new Exception("Non è stata specificata la libreria SNS");
            Logger = LogManager.GetCurrentClassLogger();
        }
    }
}