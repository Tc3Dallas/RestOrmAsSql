using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebRestAs400;
using WebRestAs400.Models;

namespace WebRestAs400.Controllers
{
    public class LuaMpTestataListinoController : ControllerBase
    {
        [HttpGet, HttpPost]
        [Route("api/LuaMp/TestataListino/")]
        public IHttpActionResult Get()
        {
            try
            {
                var Db = new DbContext();
                var tableName = Db.GetTableName<LuaMpTestaListino>();

                var statement = $"SELECT * FROM {Db.Library}.{tableName}";

                var res = Db.ExecuteQuery<LuaMpTestaListino>(statement);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }
    }
}
