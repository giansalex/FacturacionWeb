using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WebBusinessLayer.Querys;
using WebDataModel;
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
                using (var db = DbHelper.GetConection())
                {
                    var config = db.Query<configuracionfacturacionDto>("SELECT * FROM configuracionfacturacion").First();
                    LastResult.Success = true;
                    return config;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErroMessage = ex.Message;
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
                using (var db = DbHelper.GetConection())
                {
                    var config = db.Query<configuracionfacturacionDto>(nameof(GetLite)).First();
                    LastResult.Success = true;
                    return config;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErroMessage = ex.Message;
                return null;
            }
        }
    }
}
