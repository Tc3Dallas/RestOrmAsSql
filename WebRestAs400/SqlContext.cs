using AsOrm3;
using MsSqlOrm;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebRestAs400
{
    public class SqlContext : MsSqlContext
    {
        public SqlContext()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["MsSqlEntities"].ConnectionString;
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new Exception("Non è stata specificata la ConnectionString");
            Logger = LogManager.GetCurrentClassLogger();
        }
    }
}