using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebRestAs400.Models;
using WebRestAs400.Services;

namespace WebRestAs400.Controllers
{
    public class WebAccessoriUserController : ControllerBase
    {

        private WebAccessoriUserService _userService;

        protected WebAccessoriUserService UserService
        {
            get
            {
                if (_userService == null)
                {
                    _userService = new WebAccessoriUserService();
                }

                return _userService;
            }
        }

        /// <summary>
        /// Tutti i records della tabella WAUSR02K
        /// </summary>
        [HttpGet, HttpPost]
        [Route("api/WebAccessori/User/")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var Db = new DbContext();

                var ritorno = Db.ExecuteReader<WebAccessoriUser>();
                return Ok(ritorno);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet, HttpPost]
        [Route("api/WebAccessori/UserGen/")]
        public IHttpActionResult GetAllVariant()
        {
            try
            {
                var Db = new DbContext();

                dynamic foo = Db.ExecuteReader<WebAccessoriUser>();

                foo.Bar = "something";
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);

                return Ok(foo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet, HttpPost]
        [Route("api/WebAccessori/Fornitori/")]
        public IHttpActionResult GetFornitori(WebAccessoriFornitori fornitore)
        {
            try
            {
                var Db = new DbContext();
                var statement = $"SELECT DISTINCT GRCUDF, GRCFOR, GRRAGS FROM {Db.Library}.{Db.GetTableName<WebAccessoriFornitori>()}";
                if (fornitore != null && !string.IsNullOrWhiteSpace(fornitore.GRRAGS))
                    statement += $" WHERE UPPER(GRRAGS) LIKE '{fornitore.GRRAGS.ToUpper()}%' OR UPPER(GRCFOR) LIKE '{fornitore.GRRAGS.ToUpper()}%'";
                statement += " ORDER BY GRRAGS";
                var ritorno = Db.ExecuteQuery<WebAccessoriFornitori>(statement);
                return Ok(ritorno);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet, HttpPost]
        [Route("api/WebAccessori/Fornitori/Users")]
        public IHttpActionResult GetFornitoriUsers(WebAccessoriFornitori fornitore)
        {
            try
            {
                var Db = new DbContext();
                var ritorno = Db.ExecuteReader<WebAccessoriUser>($"GRCUDF = '{fornitore.GRCUDF}' AND GRCFOR = '{fornitore.GRCFOR}'", orderBy: "GRNUTE,GRCUTE");
                return Ok(ritorno);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }


        /// <summary>
        /// Aggiorna l'ultimo login
        /// </summary>
        /// <remarks>
        /// Setta i campi Data ultimo ingresso "GRDULI" e Ora ultimo ingresso  "GROULI"
        /// </remarks>
        [HttpPut]
        [Route("api/WebAccessori/User/Accesso")]
        public IHttpActionResult SetAccesso(WebAccessoriUser accessoriUser)
        {
            try
            {
                var now = DateTime.Now;
                var Db = new DbContext();
                var res = Db.ExecuteReader<WebAccessoriUser>(accessoriUser);
                if (res.Count == 1)
                {
                    var updatable = res.FirstOrDefault();
                    updatable.GRDULI = now.ToIntDate();
                    updatable.GROULI = now.ToIntTime();

                    Db.ExecuteUpdate<WebAccessoriUser>(updatable);

                    return Ok();
                }
                else
                {
                    if (res.Count > 1)
                    {
                        Logger.Trace($"Utente {accessoriUser.GRUTAD}. La query di select ha restituito più di una occorrenza. Chiamante: api/AccessoriUser/LastLogin");
                        return BadRequest($"Utente {accessoriUser.GRUTAD}. La query di select ha restituito più di una occorrenza.");
                    }
                    if (res.Count == 0)
                    {
                        Logger.Trace($"Utente {accessoriUser.GRUTAD}. La query di select non ha restituito nessuna occorrenza. Chiamante: api/AccessoriUser/LastLogin");
                        return BadRequest($"Utente {accessoriUser.GRUTAD}. La query di select non ha restituito nessuna occorrenza.");
                    }
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/Log/All")]
        public HttpResponseMessage GetLogAll()
        {
            try
            {
                return GetLog("All");
            }
            catch (Exception ex)
            {
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return result;
            }
        }

        [HttpGet]
        [Route("api/Log/Err")]
        public HttpResponseMessage GetLogErr()
        {
            try
            {
                return GetLog("Err");
            }
            catch (Exception ex)
            {
                var result = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return result;
            }
        }

        private HttpResponseMessage GetLog(string type)
        {
            var fileTarget = (NLog.Targets.FileTarget)LogManager.Configuration.FindTargetByName(type);
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
            string fileName = fileTarget.FileName.Render(logEventInfo);
            if (!File.Exists(fileName))
                throw new Exception("Log file does not exist.");

            var dataBytes = File.ReadAllBytes(fileName);
            var dataStream = new MemoryStream(dataBytes);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(dataStream.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

    }
}
