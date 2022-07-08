using System;
using System.Collections.Generic;
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
    /// Oppure direttamente come parametri del costruttore.
    /// </summary>
    public abstract partial class AsContext : IAsContext
    {
        /// <summary>
        /// Esegue una query di insert del paramtri di tipo T
        /// </summary>
        /// <typeparam name="T">Tipo dell'oggetto interessato alla query</typeparam>
        /// <param name="obj">Oggetto di tipo T da inserire</param>
        /// <returns>Numero di righe inressatre dalla query. Se la query va in errore restituisc eun valore negativo.</returns>
        public int ExecuteInsert<T>(T obj, bool withTransaction = false)
        {
            var statement = InsertStatement<T>(obj);
            return ExecuteNonQuery(statement, withTransaction);
        }

        /// <summary>
        /// Esegue una query di insert del paramtri di tipo T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exception"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int ExecuteInsert<T>(T obj, out Exception exception, bool withTransaction = false)
        {
            var statement = InsertStatement<T>(obj);
            return ExecuteNonQuery(statement, out exception, withTransaction);
        }

        /// <summary>
        /// Esegue l'inserimento di tutti gli oggetti contenuti nella lista
        /// </summary>
        /// <typeparam name="T">tipo degli oggetti</typeparam>
        /// <param name="objs">oggetti tipizzati da inserire</param>
        /// <param name="withTransaction">se true esegue le queries sotto transaction</param>
        /// <returns></returns>
        public int ExecuteInsert<T>(T[] objs, bool withTransaction = false)
        {
            var statements = InsertStatements<T>(objs).ToList();
            return ExecuteNonQueries(statements.ToArray(), withTransaction);
        }

        /// <summary>
        /// Esegue l'inserimento di tutti gli oggetti contenuti nella lista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="exception"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int ExecuteInsert<T>(T[] objs, out Exception exception, bool withTransaction = false)
        {
            var statements = InsertStatements<T>(objs).ToList();
            return ExecuteNonQueries(statements.ToArray(), out exception, withTransaction);
        }

        /// <summary>
        /// Genera la frase SQL per il comando di INSERT dell'oggetto tipizzato obj di tipo T 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>stringa della frase SQL</returns>
        public string InsertStatement<T>(T obj)
        {
            var statement = string.Empty;
            var tableName = string.Empty;
            var library = string.Empty;
            string value;
            var keys = string.Empty;
            var values = string.Empty;
            tableName = GetTableName<T>();
            library = GetLibraryName<T>();
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                if (!isVirtual<T>(pi.Name))
                {
                    try
                    {
                        if (IsDecimalType(pi))
                            value = pi.GetValue(obj).ToString().Replace(",", ".");
                        else
                            value = pi.GetValue(obj).ToString().FormatSqlValue();
                    }
                    catch (Exception ex)
                    {
                        value = null;
                    }
                    if (value != null)
                    {
                        keys += pi.Name.ToString() + ",";
                        values += "'" + value + "',";
                    }
                    value = null;
                }
            }
            keys = keys.Substring(0, keys.Length - 1);
            values = values.Substring(0, values.Length - 1);

            statement = "INSERT INTO " + library + "." + tableName + " (";
            statement += keys;
            statement += ")";
            statement += " VALUES (";
            statement += values;
            statement += ")";

            return statement;
        }

        public string[] InsertStatements<T>(T[] objs)
        {
            List<string> statements = new List<string>();
            foreach (T obj in objs)
            {
                statements.Add(InsertStatement<T>(obj));
            }
            return statements.ToArray();
        }


    }
}
