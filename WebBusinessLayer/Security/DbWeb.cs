using System;
using System.Data.Common;
using WebDataModel;

namespace WebBusinessLayer.Security
{
    /// <summary>
    /// Class DbWeb for Conection.
    /// </summary>
    internal class DbWeb
    {
        /// <summary>
        /// Gets the conection Currente Database.
        /// </summary>
        /// <returns>DbConnection.</returns>
        public static DbConnection GetConection()
        {
            Func<string, DbConnection> conection;
            if(ManagerConfiguration.Bd == DataBases.SqlServer)
                conection = DbHelper.GetSqlConection;
            else
                conection = DbHelper.GetPostgresConection;

            return conection(ManagerConfiguration.ConectionString);
        }
    }

    /// <summary>
    /// Enum Tipo de Base de Datos
    /// </summary>
    public enum DataBases : byte
    {
        /// <summary>
        /// The SQL server
        /// </summary>
        SqlServer,
        /// <summary>
        /// The postgre SQL
        /// </summary>
        PostgreSql
    }
}
