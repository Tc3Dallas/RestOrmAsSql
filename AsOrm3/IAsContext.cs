
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsOrm3
{
    public interface IAsContext
    {
        string ConnectionString { get; set; }
        string Library { get; set; }
        string LibraryCsis { get; set; }
        string LibraryCsisSoc { get; set; }
        Logger Logger { get; set; }
        //ILoggingService Logger { get; set; }
        List<T> ExecuteQuery<T>(string statements);
        int ExecuteCount<T>(string filter = null);
        List<T> ExecuteReader<T>(string filter, string orderBy, int? top, string groupBy, int? firstRow, int? lastRow);
        List<T> ExecuteReader<T>(T obj, string[] keysExcluded = null, string[] othersAdded = null, int? firstRow = null, int? lastRow = null, string[] caseUnsensitiveFields = null, string customFilters = null);
        int ExecuteNonQueries(string[] statements, bool withTransaction);
        int ExecuteNonQuery(string statements, bool withTransaction);
        int ExecuteInsert<T>(T[] objs, bool withTransaction);
        int ExecuteInsert<T>(T obj, bool withTransaction);
        int ExecuteUpdate<T>(T obj, bool withTransaction);
        int ExecuteUpdate<T>(T[] obj, bool withTransaction);
        int ExecuteDelete<T>(T obj, bool withTransaction);
        int ExecuteDelete<T>(T[] objs, bool withTransaction);
        object ExecuteScalar(string statement);
        string DeleteStatement<T>(T obj);
        string[] DeleteStatements<T>(T[] objs);
        IEnumerable<T> Call<T>(string name, params string[] values);
        IEnumerable<T> Calls<T>(string[] statements, bool withTransaction = false);
        string CallStatement(string name, params string[] values);
        int SaveChange<T>(T obj, bool withTransaction);
        int SaveChanges<T>(T[] objs, bool withTransaction);
        string GetTableName<T>();
        string GetLibraryName<T>();
    }
}
