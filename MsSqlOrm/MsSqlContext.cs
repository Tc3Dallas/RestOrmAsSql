using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace MsSqlOrm
{
    public abstract partial class MsSqlContext
    {
        public string ConnectionString { get; set; }
        public Logger Logger { get; set; }

        public List<T> ExecuteQuery<T>(string statement, string orderBy, int? pageNumber = null, int? rowsNumber = null)
        {
            if (Logger != null) Logger.Trace(statement);
            try
            {
                StringBuilder sqlStatement = new StringBuilder();
                sqlStatement.Append(statement);

                //Se nello statement c'è ORDER BY ignoro il parametro
                //TODO si potrebbe accodarlo
                if (!string.IsNullOrWhiteSpace(orderBy) && !statement.ToUpperInvariant().Contains("ORDER BY"))
                {
                    sqlStatement.Append($" ORDER BY {orderBy}");
                }

                //per paginare ho bisogno una query ordinata, il numero di pagina e la sua grandezza in righe
                if (pageNumber != null && rowsNumber != null && pageNumber != 0 && rowsNumber != 0 && statement.Contains("ORDER BY"))
                {
                    sqlStatement.Append($" OFFSET({pageNumber} - 1) * {rowsNumber} ROWS FETCH NEXT {rowsNumber} ROWS ONLY");
                }

                List<T> entities;
                using (IDbConnection db = new SqlConnection(ConnectionString))
                {
                    entities = db.Query<T>(sqlStatement.ToString()).ToList();
                }
                return entities;
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Error(ex, "Errore nell'esecuzione della query");
                return null;
            }
        }

        private int ExecuteNonQuery<T>(string statement, T obj)
        {
            if (Logger != null) Logger.Trace(statement);
            try
            {
                using (IDbConnection db = new SqlConnection(ConnectionString))
                {
                    int rowsAffected = db.Execute(statement, obj);
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Error(ex, "Errore nell'esecuzione della query");
                return -1;
            }
        }

        private int ExecuteNonQueries<T>(List<DapperObj> dapperObjs)
        {
            int index = 0;
            using (IDbConnection db = new SqlConnection(ConnectionString))
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    foreach (var dapperObj in dapperObjs)
                    {
                        if (Logger != null) Logger.Trace(dapperObj.Statement);
                        db.Execute(dapperObj.Statement, dapperObj.Obj, transaction);
                        index++;
                    }
                    transaction.Commit();
                    return index;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (Logger != null) Logger.Error(ex, "Errore nell'esecuzione della transazione");
                    return -1;
                }
            }
        }

        public int Insert<T>(T obj)
        {
            var statement = InsertStatement<T>(obj);
            var ret = ExecuteNonQuery<T>(statement, obj);
            return ret;
        }

        public int Update<T>(T obj, string[] keys, string[] fildsToUpdate)
        {
            var statement = UpdateStatement<T>(obj, fildsToUpdate);
            var ret = ExecuteNonQuery<T>(statement, obj);
            return ret;
        }

        public int Delete<T>(T obj, string[] keys)
        {
            var statement = DeleteStatement<T>(obj, tableName, keys);
            var ret = ExecuteNonQuery<T>(statement, obj);
            return ret;
        }

        public string InsertStatement<T>(T obj)
        {
            try
            {
                var tableName = GetTableName<T>();
                var stringBuilder = new StringBuilder();
                var fields = new List<string>();
                var parameters = new List<string>();
                foreach (PropertyInfo pi in typeof(T).GetProperties())
                {
                    fields.Add(pi.Name.ToString());
                    parameters.Add($"@{pi.Name.ToString()}");
                }
                stringBuilder.Append($"INSERT INTO  {tableName} (");
                string fieldsString = string.Join(",", fields);
                stringBuilder.Append(fieldsString);
                stringBuilder.Append(") VALUES (");
                string parametersString = string.Join(",", parameters);
                stringBuilder.Append(parametersString);
                stringBuilder.Append(")");
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Error(ex, "Errore nella creazione dello statement di INSERT");
                return string.Empty;
            }
        }

        /// <summary>
        /// Se non è utilizzato fildsToUpdate aggiorno tutto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="tableName"></param>
        /// <param name="keys"></param>
        /// <param name="fildsToUpdate"></param>
        /// <returns></returns>
        public string UpdateStatement<T>(T obj, string[] fildsToUpdate)
        {
            try
            {
                var tableName = GetTableName<T>();
                var stringBuilder = new StringBuilder();
                var keys = new List<string>();
                stringBuilder.Append($"UPDATE {tableName} SET ");
                int index = 0;

                foreach (PropertyInfo pi in typeof(T).GetProperties())
                {
                    if (fildsToUpdate == null || fildsToUpdate.Length == 0 || fildsToUpdate.Contains(pi.Name.ToString()))
                    {
                        var field = pi.Name.ToString();
                        var parameter = $"@{pi.Name.ToString()}";
                        if (index > 1) stringBuilder.Append(", ");
                        stringBuilder.Append($"{field} = {parameter}");
                        index++;
                    }
                    var attributeKey = Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) as KeyAttribute;
                    if (attributeKey != null)
                    {
                        keys.Add(pi.Name.ToString());
                    }
                }

                index = 0;
                if (keys.Count > 0)
                {
                    stringBuilder.Append(" WHERE ");
                    foreach (var key in keys)
                    {
                        if (index > 1) stringBuilder.Append(", ");
                        var keyValue = obj.GetType().GetProperty(key).GetValue(obj, null);
                        stringBuilder.Append($"{key} = @{keyValue}");
                        index++;
                    }
                }
                else
                {
                    if (Logger != null) Logger.Warn("Mancano le chiavi nella creazione di una query di UPDATE");
                    return string.Empty;
                }

                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Error(ex, "Errore nella creazione dello statement di UPDATE");
                return string.Empty;
            }
        }

        public string DeleteStatement<T>(T obj)
        {
            try
            {
                var tableName = GetTableName<T>();
                var stringBuilder = new StringBuilder();
                var index = 0;
                var keys = new List<string>();
                stringBuilder.Append($"DELETE {tableName}");
                foreach (PropertyInfo pi in typeof(T).GetProperties())
                {
                    var attributeKey = Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) as KeyAttribute;
                    if (attributeKey != null)
                    {
                        keys.Add(pi.Name.ToString());
                    }
                }

                if (keys.Count > 0)
                {
                    stringBuilder.Append(" WHERE ");
                    foreach (var key in keys)
                    {
                        if (index > 1) stringBuilder.Append(", ");
                        var keyValue = obj.GetType().GetProperty(key).GetValue(obj, null);
                        stringBuilder.Append($"{key} = @{keyValue}");
                        index++;
                    }
                }
                else
                {
                    if (Logger != null) Logger.Warn("Mancano le chiavi nella creazione di una query di DELETE");
                    return string.Empty;
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Error(ex, "Errore nella creazione dello statement di DELETE");
                return string.Empty;
            }
        }

        public string GetTableName<T>()
        {
            var attributeDBTable = typeof(T).GetCustomAttributes(typeof(MsSqlTable), true).FirstOrDefault() as MsSqlTable;
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
    }
}
