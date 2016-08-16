using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using WebBusinessLayer.Report;
using WebDataModel;
using WebDataModel.Entities;

namespace WebBusinessLayer
{
    public class HelperScripts : BaseBl
    {
        #region Methods
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
                        LEFT OUTER JOIN datahierarchy d ON v.i_IdIgv = d.i_ItemId AND d.i_GroupId = 27
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

        /// <summary>
        /// Obtiene los detalles de una venta con su Cod. Unidad Internacional y TipoAfectacion IGV
        /// </summary>
        /// <param name="id">id de la venta</param>
        /// <param name="grouUnidad">grupo de la unidad intenacional</param>
        /// <returns>detalles</returns>
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

        /// <summary>
        /// Devuelve el ISC de un producto.
        /// </summary>
        /// <param name="id">id del productodetalle</param>
        /// <param name="periodo">el periodo</param>
        /// <returns>dto isc</returns>
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

        public List<ReporteDocumentoFactura> GetReporteInvoice(string id)
        {
            try
            {
                using (var con = DbHelper.GetConection())
                {
                    var res = con.Query<ventaDto, ventadetalleDto, clienteDto, string, string, ReporteDocumentoFactura>(
                                @"SELECT v.*, vd.*, 
                                c.v_IdCliente, c.i_IdTipoIdentificacion, c.v_NroDocIdentificacion, c.v_PrimerNombre, c.v_SegundoNombre, c.v_ApePaterno, c.v_ApeMaterno, c.v_RazonSocial, c.v_DirecPrincipal,
                                d1.v_Value1, d2.v_Value1 FROM venta v
                                LEFT OUTER JOIN cliente c ON v.v_IdCliente = c.v_IdCliente
                                LEFT OUTER JOIN ventadetalle vd ON v.v_IdVenta = vd.v_IdVenta
                                LEFT OUTER JOIN datahierarchy d1 ON  vd.i_IdUnidadMedida = d1.i_ItemId AND d1.i_GroupId = 17
                                LEFT OUTER JOIN datahierarchy d2 ON  v.i_IdIgv = d2.i_ItemId AND d2.i_GroupId = 27
                                WHERE v.v_IdVenta = @id", (venta, detalle, c, unidad, igv) => new ReporteDocumentoFactura
                                {
                                    FechaRegistro = venta.t_FechaRegistro ?? DateTime.Now,
                                    NroDocCliente = c == null ? string.Empty : c.v_NroDocIdentificacion,
                                    NombreCliente = c == null ? string.Empty : string.Join(" ", c.v_ApePaterno, c.v_ApeMaterno, c.v_PrimerNombre, c.v_SegundoNombre, c.v_RazonSocial, venta.v_NombreClienteTemporal).Trim(),
                                    TipoDocCliente = c == null ? 6 : c.i_IdTipoIdentificacion ?? 1,
                                    Direccion = c == null ? venta.v_DireccionClienteTemporal : c.v_DirecPrincipal,
                                    Documento = venta.v_SerieDocumento + "-" + venta.v_CorrelativoDocumento,
                                    TipoDocumento = venta.i_IdTipoDocumento ?? 1,
                                    Valor = venta.d_Valor ?? 0,
                                    ValorVenta =  venta.d_ValorVenta ?? 0,
                                    Igv =  venta.d_IGV ?? 0,
                                    Total = venta.d_Total ?? 0M,
                                    Descuento = venta.d_Descuento ?? 0,
                                    d_Igv = igv, // 18%
                                    Moneda = venta.i_IdMoneda ?? 1,
                                    TipoCambio = venta.d_TipoCambio ?? 0,
                                    Gratuito = venta.i_EsGratuito ?? 0,
                                    CodigoArticulo = detalle.v_IdProductoDetalle,
                                    Cantidad = detalle.d_Cantidad ?? 0, 
                                    Descripcion = detalle.v_DescripcionProducto,
                                    Precio = detalle.d_Precio ?? 0,
                                    PrecioVenta = detalle.d_PrecioVenta ?? 0,
                                    d_Valor = detalle.d_Valor ?? 0,
                                    d_ValorVenta = detalle.d_ValorVenta ?? 0,
                                    d_Descuento =  detalle.d_Descuento ?? 0,
                                    Observacion = detalle.v_Observaciones ?? "",
                                    Unidad = unidad
                                }, new { id }, splitOn: "v_IdVentaDetalle,v_IdCliente,v_Value1,v_Value1").ToList();
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
        #endregion
    }
}
