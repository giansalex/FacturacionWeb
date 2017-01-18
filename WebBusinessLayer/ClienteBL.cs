using System;
using System.Linq;
using Dapper;
using WebBusinessLayer.Security;
using WebDataModel.Entities;

namespace WebBusinessLayer
{
    using Querys;
    /// <summary>
    /// Class ClienteBl.
    /// </summary>
    /// <seealso cref="WebBusinessLayer.BaseBl" />
    public class ClienteBl : BaseBl
    {
        /// <summary>
        /// Gets the identifier client.
        /// </summary>
        /// <param name="pstrDocumento">The PSTR documento.</param>
        /// <param name="pstrClave">The PSTR clave.</param>
        /// <returns>System.String.</returns>
        public string GetIdClient(string pstrDocumento, string pstrClave)
        {
            try
            {
                using (var db = DbWeb.GetConection())
                {
                    var user = db.Query<clienteDto>(QueryHelper.GetQuery(nameof(GetIdClient)), 
                        new {id = pstrDocumento, pass = pstrClave }).FirstOrDefault();
                    LastResult.Success = true;
                    return user?.v_IdCliente;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErrorMessage = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="pstrId">The PSTR identifier.</param>
        /// <returns>clienteDto.</returns>
        public clienteDto GetClient(string pstrId)
        {
            try
            {
                using (var db = DbWeb.GetConection())
                {
                    var user = db.Query<clienteDto>(QueryHelper.GetQuery(nameof(GetClient)), new {id = pstrId}).FirstOrDefault();
                    LastResult.Success = true;
                    return user;
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
