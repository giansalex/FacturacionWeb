using System;
using System.Linq;
using Dapper;
using WebBusinessLayer.Querys;
using WebBusinessLayer.Security;
using WebDataModel.Entities;

namespace WebBusinessLayer
{
    /// <summary>
    /// Manage Table configuracionfacturacion
    /// </summary>
    public class ConfiguracionFacturacionBl : BaseBl
    {
        /// <summary>
        /// Obtiene la configuracion de la Facturacion.
        /// </summary>
        /// <returns>dto de la configuracion</returns>
        public configuracionfacturacionDto Get()
        {
            try
            {
                using (var db = DbWeb.GetConection())
                {
                    var config = db.Query<configuracionfacturacionDto>("SELECT * FROM configuracionfacturacion").First();
                    LastResult.Success = true;
                    return config;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErrorMessage = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Obtiene informacion necesaria para visualizar en la Web.
        /// </summary>
        /// <returns>dto</returns>
        public configuracionfacturacionDto GetLite()
        {
            try
            {
                using (var db = DbWeb.GetConection())
                {
                    var config = db.Query<configuracionfacturacionDto>(QueryHelper.GetQuery(nameof(GetLite))).First();
                    LastResult.Success = true;
                    return config;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErrorMessage = ex.Message;
                return null;
            }
        }
    }
}
