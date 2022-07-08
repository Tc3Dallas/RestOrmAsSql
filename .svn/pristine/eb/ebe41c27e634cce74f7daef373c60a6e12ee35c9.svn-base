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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int ExecuteDelete<T>(T obj, bool withTransaction = false)
        {
            var statement = DeleteStatement<T>(obj);
            return ExecuteNonQuery(statement, withTransaction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="exception"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int ExecuteDelete<T>(T obj, out Exception exception, bool withTransaction = false)
        {
            var statement = DeleteStatement<T>(obj);
            return ExecuteNonQuery(statement, out exception, withTransaction);
        }

        /// <summary>
        /// Esegue la cancellazione di tutti gli oggetti contenuti nella lista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int ExecuteDelete<T>(T[] objs, bool withTransaction = false)
        {
            var statements = DeleteStatements<T>(objs).ToList();
            return ExecuteNonQueries(statements.ToArray(), withTransaction);
        }

        /// <summary>
        /// Esegue la cancellazione di tutti gli oggetti contenuti nella lista
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="exception"></param>
        /// <param name="withTransaction"></param>
        /// <returns></returns>
        public int ExecuteDelete<T>(T[] objs, out Exception exception, bool withTransaction = false)
        {
            var statements = DeleteStatements<T>(objs).ToList();
            return ExecuteNonQueries(statements.ToArray(), out exception, withTransaction);
        }

        public string DeleteStatement<T>(T obj)
        {
            var statement = string.Empty;
            var tableName = string.Empty;
            var library = string.Empty;
            string key;
            string value;
            var filter = string.Empty;

            //tableName = typeof(T).Name;
            tableName = GetTableName<T>();
            if (tableName.Contains("."))
            {
                library = tableName.Split('.')[0];
                tableName = tableName.Split('.')[1];
            }
            else
                library = Library;
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                key = pi.Name.ToString();
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

            filter = filter.Substring(0, filter.Length - 5);
            statement = "DELETE FROM " + library + "." + tableName + " WHERE " + filter;

            return statement;
        }

        public string[] DeleteStatements<T>(T[] objs)
        {
            List<string> statements = new List<string>();
            foreach (T obj in objs)
            {
                statements.Add(DeleteStatement<T>(obj));
            }
            return statements.ToArray();
        }

    }
}
