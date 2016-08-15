using System;
using System.Collections.Generic;
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
        /// Obtiene una conexion a las base de Datos Especificada.
        /// </summary>
        /// <returns>Conection to db</returns>
        public static DbConnection GetConection()
        {
#if SQLSERVER
            return new SqlConnection(Properties.Resources.ConectionString);
#else
            return new NpgsqlConnection(Properties.Resources.ConectionString);
#endif
        }
    }

    /// <summary>
    /// Tipos de Base de Datos permitidas.
    /// </summary>
    public enum TipoDb
    {
        /// <summary>
        /// Microsot SQL Server.
        /// </summary>
        SqlServer,
        /// <summary>
        /// PostgreSQL
        /// </summary>
        PostgreSql
    }
}
