using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using WebDataModel;
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
                using (var dbContext = DbHelper.GetConection())
                {
                    var items = dbContext.Query<ventaDto>(QueryHelper.GetQuery(nameof(GetListFromClient)), new {id =idCliente});
                    LastResult.Success = true;
                    return items.ToList();
                }
            }
            catch (Exception ex)
            {
                LastResult.ErroMessage = ex.Message;
                return null;
            }
        }

        public string GetIdVenta(int tipoDoc, string serie, string correlativo, DateTime fecha, decimal total)
        {
            try
            {
                using (var dbContext = DbHelper.GetConection())
                {
                    var venta = dbContext.Query<ventaDto>(QueryHelper.GetQuery(nameof(GetIdVenta)), new {tipoDoc, serie, correlativo, fecha, total}).FirstOrDefault();
                    LastResult.Success = true;
                    return venta?.v_IdVenta;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErroMessage = ex.Message;
                return null;
            }
        }

        public ventaDto GetVenta(string idVenta)
        {
            try
            {
                using (var dbContext = DbHelper.GetConection())
                {
                    var venta = dbContext.Query<ventaDto>(QueryHelper.GetQuery(nameof(GetVenta)), new { idVenta }).FirstOrDefault();
                    LastResult.Success = true;
                    return venta;
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
