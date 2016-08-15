using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

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
#if SQLSERVER
            ResourceManager resx = Query_SqlServer.ResourceManager;
#else
           ResourceManager resx = Query_Postgres.ResourceManager;
#endif
            return resx.GetString(name);
        }
    }

}
