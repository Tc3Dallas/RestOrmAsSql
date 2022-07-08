using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebRestAs400.Models;

//namespace WebAccessoriPrototipo.Controllers
namespace WebRestAs400.Controllers
{

    public class LuaMpRigaListinoController : ControllerBase
    {
        [HttpGet, HttpPost]
        [Route("api/LuaMp/RigaListino/")]
        public IHttpActionResult GetAll(LuaMpTestaListino testata)
        {
            try
            {
                var db = new DbContext();
                if (testata.XRCSTG == null) testata.XRCSTG = "";
                string statement = " XSUDBF='" + testata.XRUDBF + "' AND XSCFOR='" + testata.XRCFOR + "'" +
                   " AND XSCSTG='" + testata.XRCSTG + "'";
                if (!String.IsNullOrEmpty(testata.XRCLUA))
                    statement += " AND XSCLUA ='" + testata.XRCLUA + "'";
                else
                    statement += " AND XSCLUA =''";
                if (!String.IsNullOrEmpty(testata.XRCUDB))
                    statement += " AND XSCUDB ='" + testata.XRCUDB + "'";
                else
                    statement += " AND XSCUDB =''";

                List<LuaMpRigaListino> listinoRighe = db.ExecuteReader<LuaMpRigaListino>(statement);
                return Ok(listinoRighe);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet, HttpPost]
        [Route("api/LuaMp/Incremento/")]
        public IHttpActionResult GetAllIncrementi(LuaMpTestaListino testata)
        {
            try
            {
                var db = new DbContext();
                string statement = "XVUDBF='" + testata.XRUDBF + "' AND XVCFOR='" + testata.XRCFOR + "'" +
                    " AND XVCSTG='" + testata.XRCSTG + "'";
                if (!String.IsNullOrEmpty(testata.XRCLUA))
                    statement += " AND XVCLUA ='" + testata.XRCLUA + "'";
                else
                    statement += " AND XVCLUA =''";
                if (!String.IsNullOrEmpty(testata.XRCUDB))
                    statement += " AND XVCUDB ='" + testata.XRCUDB + "'";
                else
                    statement += " AND XVCUDB =''";

                List<LuaMpIncremento> listinoIncrementi = db.ExecuteReader<LuaMpIncremento>(statement);
                return Ok(listinoIncrementi);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }
    }
}
