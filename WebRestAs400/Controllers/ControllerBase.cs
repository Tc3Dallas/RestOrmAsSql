using NLog;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebRestAs400.Models;

namespace WebRestAs400.Controllers
{
    public class ControllerBase : ApiController
    {
        public Logger Logger => LogManager.GetCurrentClassLogger();

        public ControllerBase()
        {

        }
    }
}
