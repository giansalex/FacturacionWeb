using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using WebBusinessLayer.Querys;
using WebBusinessLayer.Report;
using WebBusinessLayer.Security;
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
                using (var con = DbWeb.GetConection())
                {
                    var res = con.Query<ventaDto, clienteDto, string, Tuple<ventaDto, clienteDto, decimal>>(
                        QueryHelper.GetQuery(nameof(GetVentaCliente)), (a, b, igv) => new Tuple<ventaDto, clienteDto, decimal>(a, b , decimal.Parse(igv ?? "18") / 100),
                        new { idVenta }, splitOn: "i_IdTipoIdentificacion,v_Value1").First();
                    LastResult = true;
                    return res;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErrorMessage = ex.Message;
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
                using (var con = DbWeb.GetConection())
                {
                    var res = con.Query<ventadetalleDto, string, string, Tuple <ventadetalleDto, string, short>>(
                        QueryHelper.GetQuery(nameof(GetVentaDetalles)), 
                        (a, unid, tipoOp) => new Tuple<ventadetalleDto, string, short>(a, unid, short.Parse(tipoOp)), new { idg = grouUnidad, id}, splitOn: "v_Value2").ToArray();
                    LastResult = true;
                    return res;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErrorMessage = ex.Message;
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
                using (var con = DbWeb.GetConection())
                {
                    var isc = con.Query<productoiscDto>(
                            QueryHelper.GetQuery(nameof(GetIscFromDetail)),
                            new {id, periodo}).First();
                    LastResult = true;
                    return isc;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErrorMessage = ex.Message;
                return null;
            }
        }

        public List<ReporteDocumentoFactura> GetReporteInvoice(string id)
        {
            try
            {
                using (var con = DbWeb.GetConection())
                {
                    var res = con.Query<ventaDto, ventadetalleDto, clienteDto, string, string, ReporteDocumentoFactura>(
                                QueryHelper.GetQuery(nameof(GetReporteInvoice)), (venta, detalle, c, unidad, igv) => new ReporteDocumentoFactura
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
                LastResult.ErrorMessage = ex.Message;
                return null;
            }
        }
        #endregion
    }
}
