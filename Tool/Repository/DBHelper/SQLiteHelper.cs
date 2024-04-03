using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Model.Common;
using Common;

namespace Repository.DBHelper
{
    public class SQLiteHelper
    {
        private static string connectionString = string.Empty;
        private static string dbname = "mydatabase.db";

        public SQLiteHelper()
        {
            string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbname);
            connectionString = string.Format("Data Source={0};Version={1}",
                dbFilePath, 3);
        }

        /// <summary>
        /// 根据数据源、密码、版本号设置连接字符串。
        /// </summary>
        /// <param name="datasource">数据源。</param>
        /// <param name="password">密码。</param>
        /// <param name="version">版本号（缺省为3）。</param>
        public static void SetConnectionString(string datasource, string password, int version = 3)
        {
            connectionString = string.Format("Data Source={0};Version={1};password={2}",
                datasource, version, password);
        }

        /// <summary>
        /// 创建一个数据库文件。如果存在同名数据库文件，则会覆盖。
        /// </summary>
        /// <param name="dbName">数据库文件名。为null或空串时不创建。</param>
        /// <param name="password">（可选）数据库密码，默认为空。</param>
        /// <exception cref="Exception"></exception>
        public static void CreateDB()
        {
            if (!string.IsNullOrEmpty(dbname))
            {
                try
                {
                    // 设置数据库文件路径
                    string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbname);
                    // 如果数据库文件不存在，则创建数据库文件
                    if (!File.Exists(dbFilePath))
                    {
                        SQLiteConnection.CreateFile(dbFilePath);
                    }
                }
                catch (Exception ex)
                {
                    TxtHelper.WriteError(ex);
                }
            }
        }

        public static void CreateTable()
        {
            Dictionary<string, string> tables = new Dictionary<string, string>();
            tables.Add("Article", @"Id TEXT PRIMARY KEY,
                            Title TEXT NOT NULL,
                            Content TEXT,
                            Status INTEGER NOT NULL,
                            CreateTime DATETIME NOT NULL,
                            EditTime DATETIME NOT NULL");
            CreateTable(tables,1);
            CreateTable(tables,2);
        }
        public static void CreateTable(Dictionary<string, string> tables,int num)
        {
            try
            {
                if (tables == null || tables.Count == 0)
                {
                    throw new ArgumentException("The tables dictionary cannot be null or empty.");
                }
                // 使用 connectionString 连接数据库
                using (SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};Version={1}",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbname), 3)))
                {
                    connection.Open();

                    // 遍历字典中的每个表
                    foreach (var table in tables)
                    {
                        // 构建创建表的 SQL 语句
                        string createTableSql = $"CREATE TABLE IF NOT EXISTS {table.Key} ({table.Value})";

                        // 使用 SQLiteCommand 执行创建表的 SQL 语句
                        using (SQLiteCommand command = new SQLiteCommand(createTableSql, connection))
                        {

                            command.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if(num>1)
                    TxtHelper.WriteError(ex);
            }
        }

        /// <summary> 
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。 
        /// </summary> 
        /// <param name="sql">要执行的增删改的SQL语句。</param> 
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param> 
        /// <returns></returns> 
        /// <exception cref="Exception"></exception>
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = sql;
                        if (parameters.Length != 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        affectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception) { throw; }
                }
            }
            return affectedRows;
        }

        /// <summary>
        /// 批量处理数据操作语句。
        /// </summary>
        /// <param name="list">SQL语句集合。</param>
        /// <exception cref="Exception"></exception>
        public void ExecuteNonQueryBatch(List<KeyValuePair<string, SQLiteParameter[]>> list)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try { conn.Open(); }
                catch { throw; }
                using (SQLiteTransaction tran = conn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        try
                        {
                            foreach (var item in list)
                            {
                                cmd.CommandText = item.Key;
                                if (item.Value != null)
                                {
                                    cmd.Parameters.AddRange(item.Value);
                                }
                                cmd.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch (Exception) { tran.Rollback(); throw; }
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，并返回第一个结果。
        /// </summary>
        /// <param name="sql">查询语句。</param>
        /// <returns>查询结果。</returns>
        /// <exception cref="Exception"></exception>
        public object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        if (parameters.Length != 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable。 
        /// </summary> 
        /// <param name="sql">要执行的查询语句。</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param> 
        /// <returns></returns> 
        /// <exception cref="Exception"></exception>
        public DataTable ExecuteQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters.Length != 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    try { adapter.Fill(data); }
                    catch (Exception) { throw; }
                    return data;
                }
            }
        }

        /// <summary> 
        /// 执行一个查询语句，返回一个关联的SQLiteDataReader实例。 
        /// </summary> 
        /// <param name="sql">要执行的查询语句。</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param> 
        /// <returns></returns> 
        /// <exception cref="Exception"></exception>
        public SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] parameters)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            try
            {
                if (parameters.Length != 0)
                {
                    command.Parameters.AddRange(parameters);
                }
                connection.Open();
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception) { throw; }
        }

        /// <summary> 
        /// 查询数据库中的所有数据类型信息。
        /// </summary> 
        /// <returns></returns> 
        /// <exception cref="Exception"></exception>
        public DataTable GetSchema()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return connection.GetSchema("TABLES");
                }
                catch (Exception) { throw; }
            }
        }

        #region 实体
        // 新增
        public int Insert<T>(T item)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableName = typeof(T).Name;
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    string columnNames = string.Join(",", properties.Select(p => p.Name));
                    string valueNames = string.Join(",", properties.Select(p => "@" + p.Name));

                    string commandText = $"INSERT INTO {tableName} ({columnNames}) VALUES ({valueNames});";

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        foreach (var property in properties)
                        {
                            var parameter = new SQLiteParameter("@" + property.Name, property.GetValue(item));
                            command.Parameters.Add(parameter);
                        }

                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error occurred while inserting data: {ex.Message}");
                return -1;
                // throw; // Optionally re-throw the exception
            }
        }

        public void InsertList<T>(List<T> items)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableName = typeof(T).Name;
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    string columnNames = string.Join(",", properties.Select(p => p.Name));
                    string valueNames = string.Join(",", properties.Select(p => "@" + p.Name));

                    foreach (var item in items)
                    {
                        string commandText = $"INSERT INTO {tableName} ({columnNames}) VALUES ({valueNames});";

                        using (var command = new SQLiteCommand(commandText, connection))
                        {
                            foreach (var property in properties)
                            {
                                var parameter = new SQLiteParameter("@" + property.Name, property.GetValue(item));
                                command.Parameters.Add(parameter);
                            }

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error occurred while inserting data: {ex.Message}");
                // throw; // Optionally re-throw the exception
            }
        }

        // 修改数据
        public int Update<T>(T item)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableName = typeof(T).Name;
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // 构建 SET 子句，用于更新每个属性
                    string setClause = string.Join(",", properties.Select(p => $"{p.Name} = @{p.Name}"));

                    string commandText = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id;"; // 假设每个表都有一个名为 Id 的主键

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        foreach (var property in properties)
                        {
                            var parameter = new SQLiteParameter("@" + property.Name, property.GetValue(item));
                            command.Parameters.Add(parameter);
                        }

                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error occurred while updating data: {ex.Message}");
                return -1;
                // throw; // Optionally re-throw the exception
            }
        }

        public void UpdateList<T>(List<T> items)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableName = typeof(T).Name;
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // 构建 SET 子句，用于更新每个属性
                    string setClause = string.Join(",", properties.Select(p => $"{p.Name} = @{p.Name}"));

                    foreach (var item in items)
                    {
                        string commandText = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id;"; // 假设每个表都有一个名为 Id 的主键

                        using (var command = new SQLiteCommand(commandText, connection))
                        {
                            foreach (var property in properties)
                            {
                                var parameter = new SQLiteParameter("@" + property.Name, property.GetValue(item));
                                command.Parameters.Add(parameter);
                            }

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error occurred while updating data: {ex.Message}");
                // throw; // Optionally re-throw the exception
            }
        }

        // 删除数据
        public int Delete<T>(string id)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableName = typeof(T).Name;

                    string commandText = $"DELETE FROM {tableName} WHERE Id = @Id;"; // 假设每个表都有一个名为 Id 的主键

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error occurred while deleting data: {ex.Message}");
                return -1;
                // throw; // Optionally re-throw the exception
            }
        }
        // 查询单个实体
        public T GetById<T>(string id)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableName = typeof(T).Name;

                    string commandText = $"SELECT * FROM {tableName} WHERE Id = @Id;"; // Assuming each table has an 'Id' primary key

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            var item = Activator.CreateInstance<T>();
                            var properties = typeof(T).GetProperties();

                            foreach (var property in properties)
                            {
                                var value = reader[property.Name];
                                if (value != DBNull.Value)
                                {
                                    property.SetValue(item, value);
                                }
                            }

                            return item;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error occurred while retrieving data: {ex.Message}");
                // throw; // Optionally re-throw the exception
            }

            return default(T); // Return default value if no data found or error occurred
        }

        // 查询实体列表
        public List<T> GetAll<T>()
        {
            var items = new List<T>();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableName = typeof(T).Name;

                    string commandText = $"SELECT * FROM {tableName};";

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var item = Activator.CreateInstance<T>();
                            var properties = typeof(T).GetProperties();

                            foreach (var property in properties)
                            {
                                var value = reader[property.Name];
                                if (value != DBNull.Value)
                                {
                                    property.SetValue(item, value);
                                }
                            }

                            items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error occurred while retrieving data: {ex.Message}");
                // throw; // Optionally re-throw the exception
            }

            return items;
        }
        #endregion

        #region 分页查询
        // 查询实体列表，带分页和排序
        public List<T> GetAll<T>(ref PageModel pageModel)
        {
            var items = new List<T>();

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    var tableName = typeof(T).Name;

                    // Prepare SQL command to get the total count
                    string countCommandText = $"SELECT COUNT(*) FROM {tableName};";

                    using (var countCommand = new SQLiteCommand(countCommandText, connection))
                    {
                        pageModel.Count = Convert.ToInt32(countCommand.ExecuteScalar());
                    }

                    // Calculate offset based on current page and number of items per page
                    int offset = (pageModel.CurrentPage - 1) * pageModel.NumCount;

                    // Prepare SQL command with pagination and optional sorting
                    string commandText = $"SELECT * FROM {tableName}";

                    if (!string.IsNullOrEmpty(pageModel.Sord) && !string.IsNullOrEmpty(pageModel.Sort))
                    {
                        commandText += $" ORDER BY {pageModel.Sord} {pageModel.Sort}";
                    }

                    commandText += $" LIMIT @NumCount OFFSET @Offset;";

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@NumCount", pageModel.NumCount);
                        command.Parameters.AddWithValue("@Offset", offset);

                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            var item = Activator.CreateInstance<T>();
                            var properties = typeof(T).GetProperties();

                            foreach (var property in properties)
                            {
                                var value = reader[property.Name];
                                if (value != DBNull.Value)
                                {
                                    property.SetValue(item, value);
                                }
                            }

                            items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error occurred while retrieving data: {ex.Message}");
                // throw; // Optionally re-throw the exception
            }

            return items;
        }
        #endregion
    }
}
