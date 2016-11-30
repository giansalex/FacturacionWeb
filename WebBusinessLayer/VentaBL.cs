using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using WebBusinessLayer.Security;
using WebDataModel.Entities;

namespace WebBusinessLayer
{
    using Querys;
    public class VentaBl : BaseBl
    {
        /// <summary>
        /// Obtiene lista de ventas de un Cliente.
        /// </summary>
        /// <param name="idCliente">Id del Cliente</param>
        /// <returns>Lista de Ventas</returns>
        public List<ventaDto> GetListFromClient(string idCliente)
        {
            try
            {
                using (var dbContext = DbWeb.GetConection())
                {
                    var items = dbContext.Query<ventaDto>(QueryHelper.GetQuery(nameof(GetListFromClient)), new {id =idCliente});
                    LastResult.Success = true;
                    return items.ToList();
                }
            }
            catch (Exception ex)
            {
                LastResult.ErrorMessage = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Obteien el Id de la venta segun la informacion proporcionada.
        /// </summary>
        /// <param name="tipoDoc">Tipo de documento</param>
        /// <param name="serie">Serie del Documento</param>
        /// <param name="correlativo">Correlativo del Documento</param>
        /// <param name="fecha">Fecha de Emision</param>
        /// <param name="total">Total de la Venta</param>
        /// <returns></returns>
        public string GetIdVenta(int tipoDoc, string serie, string correlativo, DateTime fecha, decimal total)
        {
            try
            {
                using (var dbContext = DbWeb.GetConection())
                {
                    var venta = dbContext.Query<ventaDto>(QueryHelper.GetQuery(nameof(GetIdVenta)), new {tipoDoc, serie, correlativo, fecha, total}).FirstOrDefault();
                    LastResult.Success = true;
                    return venta?.v_IdVenta;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErrorMessage = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Obtiene la cabecera de una venta
        /// </summary>
        /// <param name="idVenta">Id de la Venta</param>
        /// <returns>Entity</returns>
        public ventaDto GetVenta(string idVenta)
        {
            try
            {
                using (var dbContext = DbWeb.GetConection())
                {
                    var venta = dbContext.Query<ventaDto>(QueryHelper.GetQuery(nameof(GetVenta)), new { idVenta }).FirstOrDefault();
                    LastResult.Success = true;
                    return venta;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErrorMessage = ex.Message;
                return null;
            }
        }

        public IEnumerable<ventaDto> SearchVentas(string idCliente, DateTime init, DateTime end, IDictionary<string, string> filtros)
        {
            try
            {
                #region Filter
                string stringParams = string.Empty;
                if(filtros != null)
                foreach (var item in filtros)
                {
                    var key = "\"" + item.Key + "\"";
                    stringParams += " AND " + key + "=" + item.Value;
                }
                #endregion

                using (var dbContext = DbWeb.GetConection())
                {
                    var res = dbContext.Query<ventaDto>(QueryHelper.GetQuery(nameof(SearchVentas)) + stringParams, new { id = idCliente, init, end});
                    LastResult.Success = true;
                    return res.ToArray();
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
