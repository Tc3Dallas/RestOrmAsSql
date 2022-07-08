using IBM.Data.DB2.iSeries;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AsOrm3
{
    /// <summary>
    /// Classe astratta che definisce i metodi per l'accesso ad un AS400
    /// Le properties ConnectionString e Library devono essere valorizzate dal costruttore della classe derivata tramite le chiavi 
    /// ConfigurationManager.ConnectionStrings["SqlEntities"].ConnectionString e 
    /// ConfigurationManager.AppSettings["SqlLibrary"]
    /// Può essere valorizzata anche la property Logger
    /// </summary>
    public abstract partial class AsContext : IAsContext
    {
        public string ConnectionString { get; set; }
        public string Library { get; set; }
        public string LibraryCsis { get; set; }
        public string LibraryCsisSoc { get; set; }
        public Logger Logger { get; set; }

        /// <summary>
        /// Valorizzando la connessione questa viene utilizzata per accedere all'AS ignorando la ConnectionString.
        /// La connessione deve essere aperta.
        /// </summary>
        public iDB2Connection ConnAs400 { get; set; }

        /// <summary>
        /// Esegue una query di selezione esplicitata come querystring nel parametro di ingresso.
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto da ritornare</typeparam>
        /// <param name="statement">Query string</param>
        /// <returns></returns>
        public List<T> ExecuteQuery<T>(string statement)
        {
            DataTable result = new DataTable();
            if (ConnAs400 != null && ConnAs400.State == ConnectionState.Open)
            {
                result = ExecuteQuery(statement, ConnAs400);
            }
            else
            {
                using (ConnAs400 = new iDB2Connection())
                {
                    ConnAs400.ConnectionString = ConnectionString;
                    ConnAs400.Open();
                    result = ExecuteQuery(statement, ConnAs400);
                }
                ConnAs400 = null;
            }
            if (result.Columns.Contains("ASCONTEXT_ROWNUMBER"))
            {
                result.Columns.Remove("ASCONTEXT_ROWNUMBER");
            }
            return TableToList<T>(result);
        }

        public object ExecuteScalar(string statement)
        {
            try
            {
                if (ConnAs400 != null && ConnAs400.State == ConnectionState.Open)
                {
                    return ExecuteScalar(statement, ConnAs400);
                }
                else
                {
                    using (ConnAs400 = new iDB2Connection())
                    {
                        ConnAs400.ConnectionString = ConnectionString;
                        ConnAs400.Open();
                        return ExecuteScalar(statement, ConnAs400);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Error(ex, "Errore nell'esecuzione della query");
                throw ex;
            }
        }

        private object ExecuteScalar(string statement, iDB2Connection connAs400)
        {
            if (Logger != null) Logger.Trace(statement);
            try
            {
                using (iDB2Command as400Select = new iDB2Command())
                {
                    as400Select.Connection = connAs400;
                    as400Select.CommandText = statement;
                    return as400Select.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Error(ex, "Errore nell'esecuzione della query");
                return null;
            }

        }

        private DataTable ExecuteQuery(string statement, iDB2Connection connAs400)
        {
            if (Logger != null) Logger.Trace(statement);
            try
            {
                DataTable result = new DataTable();
                using (iDB2Command as400Select = new iDB2Command())
                {
                    as400Select.Connection = connAs400;
                    as400Select.CommandType = CommandType.Text;
                    as400Select.CommandText = statement;
                    using (iDB2DataAdapter oda = new iDB2DataAdapter(as400Select))
                    {
                        oda.Fill(result);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Error(ex, "Errore nell'esecuzione della query");
                return null;
            }
        }

        public int ExecuteCount<T>(string filter = null)
        {
            var statement = string.Empty;
            var tableName = string.Empty;
            var library = string.Empty;

            //tableName = typeof(T).Name;
            tableName = GetTableName<T>();
            library = GetLibraryName<T>();
            statement = "SELECT COUNT(*)";
            statement += " FROM " + library + "." + tableName;
            if (!string.IsNullOrEmpty(filter))
            {
                statement += " WHERE " + filter;
            }
            var ret = ExecuteScalar(statement);
            return (int)ret;
        }

        /// <summary>
        /// Compone ed esegue una query di selezione
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto da ritornare</typeparam>
        /// <param name="filter">Imposta la clausola WHERE della query di SELECT</param>
        /// <param name="orderBy">Imposta la direttiva ORDER BY della query (stringa di valori separati da virgola). Specificare DESC se necessario.</param>
        /// <param name="top">Restituisce le prime N righe della query</param> 
        /// <param name="groupBy">Imposta la direttiva GRUOP BY della query (stringa di valori separati da virgola)</param> 
        /// <param name="firstRow">Numero di riga iniziale (per paginazione)</param> 
        /// <param name="lastRow">numero di riga finame (per paginazione)</param> 
        /// <returns>Lista di oggetti di tipo T</returns>
        public List<T> ExecuteReader<T>(string filter = null, string orderBy = null, int? top = null, string groupBy = null, int? firstRow = null, int? lastRow = null)
        {
            var statement = string.Empty;
            var tableName = string.Empty;
            var library = string.Empty;

            //tableName = typeof(T).Name;
            tableName = GetTableName<T>();
            library = GetLibraryName<T>();
            statement = "SELECT ";

            if (groupBy != null)
            {
                statement += groupBy;
            }
            else
            {
                statement += " * ";
            }
            statement += " FROM " + library + "." + tableName;
            if (!string.IsNullOrEmpty(filter))
            {
                statement += " WHERE " + filter;
            }
            if (!string.IsNullOrEmpty(groupBy))
            {
                statement += " GROUP BY " + groupBy;
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                statement += " ORDER BY " + orderBy;
            }
            if (!string.IsNullOrEmpty(top.ToString()))
            {
                statement += " FETCH FIRST " + top.ToString() + " ROW ONLY";
            }

            if (firstRow != null && lastRow != null)
            {
                statement = statement.Replace("*", $"{library}.{tableName}.*, ROW_NUMBER () OVER () AS ASCONTEXT_ROWNUMBER");
                statement = "WITH BASE AS (" + statement + ") SELECT * FROM BASE WHERE ASCONTEXT_ROWNUMBER BETWEEN " + firstRow + " AND " + lastRow;
            }

            return ExecuteQuery<T>(statement);
        }

        /// <summary>
        /// Seleziona un oggetto in base alla chiave, alcuni campi chiave posso essere esclusi dal filtro se vengono inclusi nella lista keysExcluded
        /// Alcuni altri campi posso essere inclusi se inseriti enlla lista othersAdded
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="keysExcluded"></param> 
        /// <param name="othersAdded"></param> 
        /// <param name="firstRow">Numero di riga iniziale (per paginazione)</param> 
        /// <param name="lastRow">Numero di riga finame (per paginazione)</param> 
        /// <returns></returns>
        public List<T> ExecuteReader<T>(T obj, string[] keysExcluded = null, string[] othersAdded = null, int? firstRow = null, int? lastRow = null, string[] caseUnsensitiveFields = null, string customFilters = null)
        {
            var tableName = string.Empty;
            string key;
            string value;
            var filter = string.Empty;
            keysExcluded = keysExcluded ?? new string[0];
            othersAdded = othersAdded ?? new string[0];
            tableName = GetTableName<T>();
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                key = pi.Name.ToString();
                value = pi.GetValue(obj) != null ? pi.GetValue(obj).ToString() : null;

                //Se il campo è una chiave della tabella lo includo nella clausola WHERE
                //A meno che non sia incluso nell'elenco delle chiavi da escludere
                //Includo invece i campi non chiave menzionati nell'elenco othersAdded_
                var attributeKey = Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) as KeyAttribute;
                if ((attributeKey != null && value != null && !keysExcluded.Contains(key)) || (othersAdded.Contains(key) && attributeKey == null && value != null))
                {
                    if (caseUnsensitiveFields != null && caseUnsensitiveFields.Contains(key))
                        filter += "UPPER(" + key + ")" + "= UPPER('" + value.FormatSqlValue() + "') AND ";
                    else
                        filter += key + "= '" + value.FormatSqlValue() + "' AND ";
                }
                key = null;
                value = null;
                attributeKey = null;
            }

            //filter = filter.Substring(0, filter.Length - 5);
            filter = new string(filter.Take(filter.Length - 4).ToArray());
            if (!string.IsNullOrWhiteSpace(customFilters))
                filter = filter + " " + customFilters;
            return ExecuteReader<T>(filter, null, null, null, firstRow, lastRow);
        }

        /// <summary>
        /// Seleziona un oggetto in base alla chiave, 
        /// i valori passati come parametri DEVONO essre nell'ordine corretto delle chiavi ma posso avere valore null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<T> ExecuteReaderByKyes<T>(params string[] values)
        {
            var tableName = string.Empty;
            string key;
            string value;
            var filter = string.Empty;
            int i = 0;
            tableName = GetTableName<T>();
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                key = pi.Name.ToString();
                //Se il campo è una chiave della tabella lo includo nella clausola WHERE
                //I parametri devono essere nell'ordine esatto delle chiavi
                var attributeKey = Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) as KeyAttribute;
                if (attributeKey != null && values[i] != null)
                {
                    filter += key + "= '" + values[i] + "' AND ";
                    i++;
                }
                key = null;
                value = null;
                attributeKey = null;

            }
            filter = filter.Substring(0, filter.Length - 5);
            return ExecuteReader<T>(filter);
        }

        /// <summary>
        /// Esegue la query contenuta nel parametro statement
        /// </summary>
        /// <param name="statement">Query string</param>
        /// <returns>Numero di righe convolte dalla query. Se la query va in errore restituisc eun valore negativo.</returns>
        public int ExecuteNonQuery(string statement, bool withTransaction = false)
        {

            Exception ex;
            var ret = ExecuteNonQuery(statement, out ex, withTransaction);
            if (ex != null)
                throw ex;
            return ret;
        }

        /// <summary>
        /// Esegue la query contenuta nel parametro statement
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="exception"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string statement, out Exception exception, bool withTransaction = false)
        {
            return ExecuteNonQueries(new string[] { statement }, out exception, withTransaction);
        }

        /// <summary>
        /// Esegue le queries contenute nell'array statements
        /// </summary>
        /// <param name="statements">array delle queries da eseguire</param>
        /// <returns>Numero totale delle righe convolte dalla queries. Se una query va in errore restituisce un valore negativo ed eseguie il Rollback.</returns>
        public int ExecuteNonQueries(string[] statements, bool withTransaction = false)
        {
            Exception ex;
            var ret = ExecuteNonQueries(statements, out ex, withTransaction);
            if (ex != null)
                throw ex;
            return ret;
        }

        /// <summary>
        /// Esegue  le queries contenute nell'array statements
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="exception"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int ExecuteNonQueries(string[] statements, out Exception exception, bool withTransaction = false)
        {
            int ret = 0;

            if (ConnAs400 != null && ConnAs400.State == ConnectionState.Open)
            {
                //if (ConnAs400.State == ConnectionState.Open)
                //{
                ret = ExecuteNonQueries(statements, ConnAs400, out exception, withTransaction);
                //}
                //else
                //{
                //    Logger?.Error("Connection not Opened!");
                //    throw new Exception("Connection not Opened!");
                //}
            }
            else
            {
                using (ConnAs400 = new iDB2Connection(ConnectionString))
                {
                    ConnAs400.Open();
                    ret = ExecuteNonQueries(statements, ConnAs400, out exception, withTransaction);
                    ConnAs400.Close();
                }
                ConnAs400 = null;
            }
            return ret;
        }

        private int ExecuteNonQueries(string[] statements, iDB2Connection connAs400, out Exception exception, bool withTransaction = false)
        {

            iDB2Transaction transaction = null;
            int ret = 0;
            using (iDB2Command as400Command = connAs400.CreateCommand())
            {
                if (withTransaction)
                {
                    transaction = connAs400.BeginTransaction();
                    as400Command.Transaction = transaction;
                }
                foreach (string statement in statements)
                {
                    if (Logger != null) Logger.Trace(statement);
                    if (statement.Length > 0)
                    {
                        as400Command.CommandText = statement;
                        try
                        {
                            //Logger?.Trace(statement);
                            as400Command.Prepare();
                            ret += as400Command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            //Logger?.Error(ex);
                            if (withTransaction) transaction.Rollback();
                            if (Logger != null) Logger.Error(ex);
                            exception = ex;
                            return -1;
                        }
                    }
                }
                if (ret > -1 && withTransaction)
                    transaction.Commit();
            }
            exception = null;
            return ret;

        }

        /// <summary>
        /// Esegue update o insert analizzando la chiave, dell'oggetto obj con o senza transazione
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int SaveChange<T>(T obj, bool withTransaction = false)
        {
            var statements = new List<string>();

            var ret = SaveChanges<T>(new T[] { obj }, withTransaction);
            return ret;
        }

        /// <summary>
        /// Esegue update o insert analizzando la chiave, degli oggetti nell'array obj con o senza transazione
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int SaveChanges<T>(T[] objs, bool withTransaction = false)
        {
            Exception ex;
            var ret = SaveChanges<T>(objs, out ex, withTransaction);
            if (ex != null)
                throw ex;
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="exception"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        private int SaveChanges<T>(T[] objs, out Exception exception, bool withTransaction = false)
        {
            int ret = 0;
            var statements = new List<string>();
            var readStatement = string.Empty;
            exception = null;
            foreach (T obj in objs)
            {
                int count = ExecuteReader<T>(obj).Count();
                //TODO: verificare
                switch (count)
                {
                    case 0:
                        statements.Add(InsertStatement<T>(obj));
                        break;
                    case 1:
                        statements.Add(UpdateStatement<T>(obj));
                        break;
                    default:
                        exception = new Exception("La funzione ExecuteReader<T>(obj).Count ha restituito -1.");
                        ret = -1;
                        break;
                }
            }
            if (ret != -1)
            {
                ret = ExecuteNonQueries(statements.ToArray(), out exception, withTransaction);
            }
            return ret;
        }

        /// <summary>
        /// Popola la proprietà con nome propertyName ed attributo "Virtual" dell'oggetto obj 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public dynamic Include<T>(dynamic obj, string propertyName)
        {
            var filter = string.Empty;
            var attributeVirtual = Attribute.GetCustomAttribute(obj.GetType().GetProperty(propertyName), typeof(Virtual)) as Virtual;
            if (attributeVirtual != null)
            {
                foreach (KeyValuePair<string, string> entry in attributeVirtual.foreignKeys)
                {
                    if (!entry.Key.StartsWith("FIX"))
                        filter += " AND " + entry.Value + "='" + ((string)obj.GetType().GetProperty(entry.Key).GetValue(obj)).FormatSqlValue() + "'";
                    else
                        filter += " AND " + entry.Value;
                }
                filter = filter.Substring(4);
            }
            var readStatement = string.Empty;
            var ret = ExecuteReader<T>(filter);
            return ret;
        }

        /// <summary>
        /// Converte un DataTable in una lista di oggetti di tipo T
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto da ritornare</typeparam>
        /// <param name="table">DataTable Da convertire</param>
        /// <returns>Lista di oggetti di tipo T</returns>
        private List<T> TableToList<T>(DataTable table)
        {
            List<T> result = new List<T>();
            foreach (DataRow rw in table.Rows)
            {
                T item = Activator.CreateInstance<T>();
                foreach (DataColumn cl in table.Columns)
                {
                    PropertyInfo pi = typeof(T).GetProperty(cl.ColumnName);
                    if (pi == null)
                    {
                        foreach (var p in typeof(T).GetProperties())
                        {
                            if (GetColumnName(p).Equals(cl.ColumnName))
                            {
                                pi = p;
                                break;
                            }
                        }
                    }

                    //Type t = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;  
                    //object safeValue = (rw[cl] == null) ? null : Convert.ChangeType(rw[cl], t);

                    if (pi != null && rw[cl] != DBNull.Value)
                        pi.SetValue(item, Convert.ChangeType(rw[cl], pi.PropertyType), new object[0]);
                }
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Data una classe stabilisce se tutti i campi chiave sono valorizzati. Per valorizzati si intende diversi da null, anche vuoto o 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <returns>true o false</returns>
        public bool IsKeyOK<T>(T obj)
        {
            string key;
            string value;
            bool ret = true;

            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                key = pi.Name.ToString();
                try
                {
                    value = pi.GetValue(obj).ToString();
                }
                catch (Exception ex)
                {
                    value = null;
                }

                //Se il campo è una chiave della tabella controllo il valore
                var attributeKey = Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) as KeyAttribute;
                if (attributeKey != null)
                {
                    if (value == null)
                        ret = false;
                }
                key = null;
                value = null;
                attributeKey = null;
            }

            return ret;
        }

        public List<PropertyInfo> Keys<T>()
        {
            var ret = new List<PropertyInfo>();
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                var attributeKey = Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) as KeyAttribute;
                if (attributeKey != null)
                {
                    ret.Add(pi);
                }
            }
            return ret;

        }

        public static string FormatSqlValue(string value)
        {
            if (value != null)
                return value.Trim().Replace("'", "''");
            else
                return null;
        }

        /// <summary>
        /// Se la class di tipo T ha un attributo DBTable ed è specificata la tabella del DB restituisco quest'ultima, altrimenti il nome della classe.
        /// Potrebbe contenere il nome della libreria! In tal caso è quella prescelta a prescindere da tutto il resto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>Table Name</returns>
        public string GetTableName<T>()
        {
            var attributeDBTable = typeof(T).GetCustomAttributes(typeof(DBTable), true).FirstOrDefault() as DBTable;
            if (attributeDBTable != null)
            {
                if (!String.IsNullOrWhiteSpace(attributeDBTable.name))
                {
                    if (attributeDBTable.name.Contains("."))
                    {
                        return attributeDBTable.name.Split('.')[1];
                    }
                    else
                    {
                        return attributeDBTable.name;
                    }
                }
            }
            return typeof(T).Name;
        }

        public string GetLibraryName<T>()
        {
            try
            {
                string tableName = GetTableName<T>();

                if (tableName.Contains("."))
                {
                    return tableName.Split('.')[0];
                }
                else
                {
                    var attributeDBLibrary = typeof(T).GetCustomAttributes(typeof(DBLibrary), true).FirstOrDefault() as DBLibrary;
                    if (attributeDBLibrary != null)
                    {
                        switch (attributeDBLibrary.zLibrary)
                        {
                            case ZLibrary.SNS:
                                return Library;
                            case ZLibrary.CSIS:
                                return LibraryCsis;
                            case ZLibrary.CSISSoc:
                                return LibraryCsisSoc;
                            default:
                                return Library;
                        }
                    }
                }

                throw new Exception("No Library specify");
            }
            catch (Exception ex)
            {
                //Logger?.Error(ex);
                throw new Exception("Eccezione passata", ex);
            }
        }

        private string GetColumnName(PropertyInfo pi)
        {
            var attributeDBColum = pi.GetCustomAttribute(typeof(DBColumn), true) as DBColumn;
            if (attributeDBColum != null)
            {
                if (!String.IsNullOrWhiteSpace(attributeDBColum.name))
                {
                    return attributeDBColum.name;
                }
            }
            return pi.Name;
        }

        //Verifica se una property della classe T è virtuale o meno.
        private bool isVirtual<T>(string propertyName)
        {
            var attributeVirtual = typeof(T).GetProperty(propertyName).GetCustomAttributes(typeof(Virtual), true).FirstOrDefault() as Virtual;
            if (attributeVirtual != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsDecimalType(System.Reflection.PropertyInfo pro)
        {
            switch (Type.GetTypeCode(pro.PropertyType))
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
