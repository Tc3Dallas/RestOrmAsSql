using MsSqlOrm;
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

    public class SqlSampleController : ControllerBase
    {
        [HttpGet, HttpPost]
        [Route("api/MsSql/Customer/")]
        public IHttpActionResult GetAll(Customer customer)
        {
            try
            {
                var sqlDb = new SqlContext();
                List<Customer> customers = sqlDb.ExecuteQuery<Customer>("SELECT1 * FROM CUSTOMER ORDER BY SURNAME, NAME", null, customer.PageNumber,customer.RowsNumber);
                if (customers != null)
                    return Ok(customers);
                else
                    throw new Exception("Errore nell'esecuzione della query, consultare il file di log.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/MsSql/Customer/Add")]
        public IHttpActionResult Add(Customer customer)
        {
            try
            {
                var sqlDb = new SqlContext();
                var res = sqlDb.Insert<Customer>(customer);
                if (res == 1)
                    return Created<Customer>(string.Empty, customer);
                else
                    throw new Exception("Errore nell'inserimento, consultare il file di log.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/MsSql/Customer/Update")]
        public IHttpActionResult Update(Customer customer)
        {
            try
            {
                var sqlDb = new SqlContext();
                var res = sqlDb.Update<Customer>(customer,null);
                if (res == 1)
                    return Ok<Customer>(customer);
                else
                    throw new Exception("Errore nell'aggiornamento, consultare il file di log.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/MsSql/Customer/Delete")]
        public IHttpActionResult Delete(Customer customer)
        {
            try
            {
                var sqlDb = new SqlContext();
                var res = sqlDb.Delete<Customer>(customer);
                if (res == 1)
                    return Ok(customer);
                else
                    throw new Exception("Errore nella cancellazione, consultare il file di log.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/MsSql/Customer/Multi")]
        public IHttpActionResult Multi()
        {
            try
            {
                var sqlDb = new SqlContext();
                var dapperObjs = new List<DapperObj>();
                var dapperObj1 = new DapperObj();
                var dapperObj2 = new DapperObj();
                var customer1 = new Customer() { Name="Pippotto",Surname="Cruscotto"};
                var customer2 = new Customer() { Id=43,NickName="Nick"};
                dapperObj1.Statement = sqlDb.InsertStatement<Customer>(customer1);
                dapperObj1.Obj = customer1;
                var filedsToUpdate = new List<string> { "NickName" };
                dapperObj2.Statement = sqlDb.UpdateStatement<Customer>(customer2, filedsToUpdate.ToArray());
                dapperObj2.Obj = customer2;
                dapperObjs.Add(dapperObj1);
                dapperObjs.Add(dapperObj2);
                var res = sqlDb.ExecuteNonQueries(dapperObjs);
                if (res >= 1)
                    return Ok();
                else
                    throw new Exception("Errore nella transazione, consultare il file di log.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
        }

    }
}
