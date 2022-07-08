using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebRestAs400.Models;

namespace WebRestAs400.Controllers
{
    /// <summary>
    /// Spedizioni
    /// </summary>
    public class DestinazioniController : ControllerBase
    {
        [HttpGet, HttpPost]
        [Route("api/WebAccessori/Destinazioni")]
        public IHttpActionResult Get()
        {
            try
            {
                var Db = new DbContext();
                var tableName = Db.GetTableName<WebAccessoriDestinazioni>();

                var statement = $"SELECT * FROM {Db.Library}.{tableName}";

                var res = Db.ExecuteQuery<WebAccessoriDestinazioni>(statement);

                foreach (var item in res)
                {
                    item.GVDDDF = item.GVCDDF + " - " + item.GVDDDF;
                }

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
