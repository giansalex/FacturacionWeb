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
    }

}
