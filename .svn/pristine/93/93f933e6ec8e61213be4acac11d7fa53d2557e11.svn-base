using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// Esegue l'aggiornamento dell'oggetto obj di tipo T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int ExecuteUpdate<T>(T obj, bool withTransaction = false)
        {
            var statement = UpdateStatement<T>(obj);
            return ExecuteNonQuery(statement, withTransaction);
        }

        public int ExecuteUpdate<T>(T obj, out Exception exception, bool withTransaction = false)
        {
            var statement = UpdateStatement<T>(obj);
            return ExecuteNonQuery(statement, out exception, withTransaction);
        }

        /// <summary>
        /// Esegue l'aggiornamento di tutti gli oggetti contenuti nella lista
        /// </summary>
        /// <typeparam name="T">tipo degli oggetti</typeparam>
        /// <param name="objs">oggetti tipizzati da inserire</param>
        /// <param name="withTransaction">se true esegue le queries sotto transaction</param>
        /// <returns></returns>
        public int ExecuteUpdate<T>(T[] objs, bool withTransaction = false)
        {
            var statements = UpdateStatements<T>(objs).ToList();
            return ExecuteNonQueries(statements.ToArray(), withTransaction);
        }

        /// <summary>
        /// Esegue l'aggiornamento di tutti gli oggetti contenuti nella lista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="exception"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int ExecuteUpdate<T>(T[] objs, out Exception exception, bool withTransaction = false)
        {
            var statements = UpdateStatements<T>(objs).ToList();
            return ExecuteNonQueries(statements.ToArray(), out exception, withTransaction);
        }

        /// <summary>
        /// Genera la frase SQL per il comando di UPDATE dell'oggetto tipizzato obj di tipo T 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>stringa della frase SQL</returns>
        public string UpdateStatement<T>(T obj)
        {
            var statement = string.Empty;
            var tableName = string.Empty;
            var library = string.Empty;
            var keysValues = string.Empty;
            string key;
            string value;
            var filter = string.Empty;

            tableName = GetTableName<T>();
            library = GetLibraryName<T>();
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                key = pi.Name.ToString();
                if (!isVirtual<T>(key))
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
                        keysValues += key + "= '" + value + "',";
                    }
                    //Se il campo è una chiave della tabella lo includo nella clausola WHERE
                    var attributeKey = Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) as KeyAttribute;
                    if (attributeKey != null)
                    {
                        filter += key + "= '" + value + "' AND ";
                    }
                    key = null;
                    value = null;
                    attributeKey = null;
                }
            }

            keysValues = keysValues.Substring(0, keysValues.Length - 1);
            filter = filter.Substring(0, filter.Length - 5);
            statement = "UPDATE " + library + "." + tableName + " SET " + keysValues + " WHERE " + filter;

            return statement;
        }

        public string[] UpdateStatements<T>(T[] objs)
        {
            List<string> statements = new List<string>();
            foreach (T obj in objs)
            {
                statements.Add(UpdateStatement<T>(obj));
            }
            return statements.ToArray();
        }

    }
}
