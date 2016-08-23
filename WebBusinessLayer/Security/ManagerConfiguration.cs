using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static DataBases Bd => (DataBases)Enum.Parse(typeof(DataBases), ConfigurationManager.AppSettings["TipoDb"]);
        /// <summary>
        /// Gets the conection string.
        /// </summary>
        /// <value>The conection string.</value>
        public static string ConectionString => ConfigurationManager.AppSettings["StrConection"];
        //postgres Server=localhost;Port=5432;Database=20100070970;User Id=postgres;Password=123456;

        /// <summary>
        /// Save the specified type database.
        /// </summary>
        /// <param name="typeDb">The type database.</param>
        /// <param name="conectionString">The conection string.</param>
        public static void Save(DataBases typeDb, string conectionString)
        {
            var config = ConfigurationManager.AppSettings;

            config["TipoDb"] = typeDb.ToString();
            config["StrConection"] = conectionString;
            //config.Settings["TipoDb"].Value = typeDb.ToString();
            //config.AppSettings.Settings["StrConection"].Value = conectionString;
            //config.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.AppSettings[""] = "Hola";
        }
    }
}
