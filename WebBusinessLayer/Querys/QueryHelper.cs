using System.Resources;
using WebBusinessLayer.Security;

namespace WebBusinessLayer.Querys
{
    /// <summary>
    /// Query Helper
    /// </summary>
    internal class QueryHelper
    {
        /// <summary>
        /// Obtiene la consulta sql.
        /// </summary>
        /// <param name="name">key de la consulta</param>
        /// <returns>Sentencia sql</returns>
        public static string GetQuery(string name)
        {
            ResourceManager resx = ManagerConfiguration.Bd == DataBases.SqlServer 
                ? Query_SqlServer.ResourceManager 
                :Query_Postgres.ResourceManager;
            return resx.GetString(name);
        }
    }

}
