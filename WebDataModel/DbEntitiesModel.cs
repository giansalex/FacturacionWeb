using System.Data.Common;
using System.Data.SqlClient;
using Npgsql;

namespace WebDataModel
{
    /// <summary>
    /// Conection Helper.
    /// </summary>
    public class DbHelper
    {

        /// <summary>
        /// Gets the SQL conection to DB.
        /// </summary>
        /// <param name="strconection">The strconection.</param>
        /// <returns>DbConnection.</returns>
        public static DbConnection GetSqlConection(string strconection)
        {
            return new SqlConnection(strconection);
        }

        /// <summary>
        /// Gets the postgres conection to DB.
        /// </summary>
        /// <param name="strconection">The strconection.</param>
        /// <returns>DbConnection.</returns>
        public static DbConnection GetPostgresConection(string strconection)
        {
            return new NpgsqlConnection(strconection);
        }

        /// <summary>
        /// Creates the string conection for SQL server.
        /// </summary>
        /// <param name="server">The server address.</param>
        /// <param name="db">The database name.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>StringConnection</returns>
        public static string CreateStringConectionSqlServer(string server, string db, string user, string password)
        {
            var cn = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = db,
                UserID = user,
                Password = password
            };
            return cn.ConnectionString;

        }
        /// <summary>
        /// Creates the string conection postgre SQL.
        /// </summary>
        /// <param name="server">The server address.</param>
        /// <param name="db">The database name.</param>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>StringConnection</returns>
        public static string CreateStringConectionPostgreSql(string server, string db, string user, string password)
        {
            var cn = new NpgsqlConnectionStringBuilder
            {
                Host = server,
                Database = db,
                UserName = user,
                Password = password
            };
            return cn.ConnectionString;
        }
    }

}
