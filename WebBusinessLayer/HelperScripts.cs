using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WebDataModel;
using WebDataModel.Entities;

namespace WebBusinessLayer
{
    public class HelperScripts : BaseBl
    {
        /// <summary>
        /// Return Information from Venta.
        /// </summary>
        /// <param name="idVenta">Id de la Venta</param>
        /// <returns>venta, cliente & IGV</returns>
        public Tuple<ventaDto, clienteDto, decimal> GetVentaCliente(string idVenta)
        {
            try
            {
                using (var con = DbHelper.GetConection())
                {
                    var res = con.Query<ventaDto, clienteDto, string, Tuple<ventaDto, clienteDto, decimal>>(
                        @"SELECT v.*, c.i_IdTipoIdentificacion,c.v_NroDocIdentificacion,c.v_ApePaterno,
                        c.v_ApeMaterno,c.v_PrimerNombre,c.v_SegundoNombre, c.v_RazonSocial, d.v_Value1 FROM venta v 
                        LEFT OUTER JOIN cliente c ON v.v_IdCliente = c.v_IdCliente
                        LEFT OUTER JOIN datahierarchy d ON v.i_IdIgv = d.i_ItemId AND i_GroupId = 27
                        WHERE v.v_IdVenta = @idVenta", (a, b, igv) => new Tuple<ventaDto, clienteDto, decimal>(a, b , decimal.Parse(igv ?? "18") / 100),
                        new { idVenta }, splitOn: "i_IdTipoIdentificacion,v_Value1").First();
                    LastResult = true;
                    return res;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErroMessage = ex.Message;
                return null;
            }
        }

        public Tuple<ventadetalleDto, string, short>[] GetVentaDetalles(string id, int grouUnidad)
        {
            try
            {
                using (var con = DbHelper.GetConection())
                {
                    var res = con.Query<ventadetalleDto, string, string, Tuple <ventadetalleDto, string, short>>(
                        @"SELECT v.*, d2.v_Value2, d3.v_Value2 FROM ventadetalle v
                        LEFT OUTER JOIN datahierarchy d on v.i_IdUnidadMedida = d.i_ItemId AND d.i_GroupId = 17
                        LEFT OUTER JOIN datahierarchy d2 on d.i_ParentItemId = d2.i_ItemId AND d2.i_GroupId = @idg
                        LEFT OUTER JOIN datahierarchy d3 ON v.i_IdTipoOperacion = d3.i_ItemId AND d3.i_GroupId = 35
                        WHERE v.v_IdVenta = @id;", 
                        (a, unid, tipoOp) => new Tuple<ventadetalleDto, string, short>(a, unid, short.Parse(tipoOp)), new { idg = grouUnidad, id}, splitOn: "v_Value2").ToArray();
                    LastResult = true;
                    return res;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErroMessage = ex.Message;
                return null;
            }
        }

        public productoiscDto GetIscFromDetail(string id, string periodo)
        {
            try
            {
                using (var con = DbHelper.GetConection())
                {
                    var isc = con.Query<productoiscDto>(
                            @"SELECT i.d_Monto, i.d_Porcentaje, i.i_IdSistemaIsc FROM productodetalle p
                            LEFT OUTER JOIN productoisc i ON p.v_IdProducto = i.v_IdProducto
                            WHERE p.v_IdProductoDetalle = @id  AND  i.v_Periodo = @periodo;",
                            new {id, periodo}).First();
                    LastResult = true;
                    return isc;
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
