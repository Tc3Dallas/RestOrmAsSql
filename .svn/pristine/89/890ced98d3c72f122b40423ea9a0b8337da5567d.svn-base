using IBM.Data.DB2.iSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsOrm3
{
    /// <summary>
    /// Classe astratta che definisce i metodi per l'accesso ad un AS400
    /// Le properties ConnectionString e Library devono essere valorizzate dal costruttore della classe derivata tramite le chiavi 
    /// ConfigurationManager.ConnectionStrings["SqlEntities"].ConnectionString e 
    /// ConfigurationManager.AppSettings["SqlLibrary"]
    /// Oppure direttamente come parametri del costruttore.
    /// </summary>
    public abstract partial class AsContext : IAsContext
    {
        /// <summary>
        /// Invoca la frase sql CALL <NomePROCEDURA>(<parametri separati da virgila>)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IEnumerable<T> Call<T>(string name, params string[] values)
        {
            Exception ex;
            var ret = Call<T>(name, out ex, values);
            if (ex != null)
                throw ex;
            return ret;
        }

        /// <summary>
        /// Invoca la frase sql CALL <NomePROCEDURA>(<parametri separati da virgila>)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="exception"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IEnumerable<T> Call<T>(string name, out Exception exception, params string[] values)
        {
            //LOG query
            try
            {
                DataTable result = new DataTable();
                using (iDB2Connection connAs400 = new iDB2Connection())
                {
                    connAs400.ConnectionString = ConnectionString;
                    using (iDB2Command as400Select = new iDB2Command())
                    {
                        as400Select.Connection = connAs400;
                        var statement = CallStatement(name, values);
                        //Logger?.Trace(statement);
                        as400Select.CommandText = statement;
                        connAs400.Open();
                        using (iDB2DataAdapter oda = new iDB2DataAdapter(as400Select))
                        {
                            oda.Fill(result);
                            //Se il DataTable restituito ha una sola colonna di nome "Errore" allora vuol dire che la chiamata è andata in eccezione sull'AS
                            if (result.Columns.Count == 1 && result.Columns[0].ColumnName == "ERRORE")
                            {
                                exception = new Exception(result.Rows[0][0].ToString());
                                return null;
                            }
                        }
                    }
                }
                exception = null;
                return TableToList<T>(result);
            }
            catch (iDB2Exception exO)
            {
                //Logger?.Error(exO);
                exception = exO;
                return null;
            }
            catch (Exception ex)
            {
                //Logger?.Error(ex);
                exception = ex;
                return null;
            }
        }

        public IEnumerable<T> Calls<T>(string[] statements, bool withTransaction = false)
        {
            Exception ex;
            var ret = Calls<T>(statements, out ex, withTransaction);
            if (ex != null)
                throw ex;
            return ret;
        }

        public IEnumerable<T> Calls<T>(string[] statements, out Exception ex, bool withTransaction = false)
        {
            IEnumerable<T> ret;

            if (ConnAs400 != null)
            {
                if (ConnAs400.State == ConnectionState.Open)
                {
                    ret = Calls<T>(statements, ConnAs400, out ex, withTransaction);
                }
                else
                {
                    //Logger?.Error("Connection not Opened!");
                    throw new Exception("Connection not Opened!");
                }
            }
            else
            {
                using (ConnAs400 = new iDB2Connection(ConnectionString))
                {
                    ConnAs400.Open();
                    ret = Calls<T>(statements, ConnAs400, out ex, withTransaction);
                    ConnAs400.Close();
                }
                ConnAs400 = null;
            }
            return ret;
        }

        private IEnumerable<T> Calls<T>(string[] statements, iDB2Connection connAs400, out Exception exception, bool withTransaction = false)
        {
            iDB2Transaction transaction = null;
            DataTable result = new DataTable();
            using (iDB2Command as400Command = connAs400.CreateCommand())
            {
                if (withTransaction)
                {
                    transaction = connAs400.BeginTransaction();
                    as400Command.Transaction = transaction;
                }
                foreach (string statement in statements)
                {
                    if (statement.Length > 0)
                    {
                        as400Command.CommandText = statement;
                        try
                        {
                            //as400Command.Prepare();
                            using (iDB2DataAdapter oda = new iDB2DataAdapter(as400Command))
                            {
                                oda.Fill(result);
                                //Se il DataTable restituito ha una sola colonna di nome "Errore" allora vuol dire che la chiamata è andata in eccezione sull'AS
                                if (result.Columns.Count == 1 && result.Columns[0].ColumnName == "ERRORE")
                                {
                                    throw new Exception(result.Rows[0][0].ToString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //Logger?.Error(ex);
                            if (withTransaction) transaction.Rollback();
                            exception = ex;
                            return null;
                        }
                    }
                }
                if (result != null && withTransaction)
                    transaction.Commit();
                exception = null;
                return TableToList<T>(result);
            }
        }

        /// <summary>
        /// Genera la frase SQL per il comando di CALL 
        /// </summary>
        /// <returns>stringa della frase SQL</returns>
        public string CallStatement(string name, params string[] values)
        {
            //TODO rendere configurabile la libreria degli oggetti adesso fissa a SNSSYSOBJ
            var statement = string.Empty;
            if (!name.Contains("."))
                name = "SNSSYSOBJ." + name;
            statement = "CALL " + name + "(";
            foreach (var value in values)
            {
                statement += "'" + value.FormatSqlValue() + "',";
            }

            statement = statement.Substring(0, statement.Length - 1) + ")";

            return statement;
        }


        //Da qui metodi mantenuti solo per retrocompatibilità

        /// <summary>
        /// Necessita di un T WKENTITY
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="language"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteCallSNSTable<T>(string tableName, string family, string language)
        {
            Exception ex;
            var ret = ExecuteCallSNSTable<T>(tableName, family, language, out ex);
            if (ex != null)
                throw ex;
            return ret;
        }

        /// <summary>
        /// Necessita di un T WKENTITY
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="family"></param>
        /// <param name="language"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteCallSNSTable<T>(string tableName, string family, string language, out Exception exception)
        {
            var statement = string.Empty;
            //WX_GET_ELEMENTI_TABELLA_NO_DS_RS
            //wx_get_elementi_tabella_rs
            statement = "CALL bp2sysobj.WX_GET_ELEMENTI_TABELLA_NO_DS_RS( '" + tableName + "', '" + family + "', '" + language + "', '')";
            //LOG query
            try
            {
                DataTable result = new DataTable();
                using (iDB2Connection connAs400 = new iDB2Connection())
                {
                    connAs400.ConnectionString = ConnectionString;
                    using (iDB2Command as400Select = new iDB2Command())
                    {
                        as400Select.Connection = connAs400;
                        as400Select.CommandText = statement;
                        connAs400.Open();
                        using (iDB2DataAdapter oda = new iDB2DataAdapter(as400Select))
                        {
                            oda.Fill(result);
                        }
                    }
                }
                exception = null;
                return TableToList<T>(result);
            }
            catch (iDB2Exception exO)
            {
                //Logger?.Error(exO);
                exception = exO;
                return null;
            }
            catch (Exception ex)
            {
                //Logger?.Error(ex);
                exception = ex;
                return null;
            }
        }
    }
}
