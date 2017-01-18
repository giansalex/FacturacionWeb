using System;
using System.Configuration;
using WebDataModel;

namespace WebBusinessLayer.Security
{
    /// <summary>
    /// Class ManagerConfiguration.
    /// </summary>
    public class ManagerConfiguration
    {
        /// <summary>
        /// Gets the Current bd.
        /// </summary>
        /// <value>The bd.</value>
        public static DataBases Bd => (DataBases)Enum.Parse(typeof(DataBases), ConfigurationManager.ConnectionStrings["xConect"].ProviderName);
        /// <summary>
        /// Gets the conection string.
        /// </summary>
        /// <value>The conection string.</value>
        public static string ConectionString => ConfigurationManager.ConnectionStrings["xConect"].ConnectionString;
        //postgres Server=localhost;Port=5432;Database=20100070970;User Id=postgres;Password=123456;

        /// <summary>
        /// Save the specified type database.
        /// </summary>
        /// <param name="typeDb">The type database.</param>
        public static void Save(DataBases typeDb, string host, string user, string pass, string db)
        {
            //var config = ConfigurationManager.AppSettings;
            var config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            var conect = config.ConnectionStrings.ConnectionStrings["xConect"];
            conect.ConnectionString = typeDb == DataBases.SqlServer 
                ? DbHelper.CreateStringConectionSqlServer(host, db, user, pass) 
                : DbHelper.CreateStringConectionPostgreSql(host, db, user, pass);
            conect.ProviderName = typeDb.ToString();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
