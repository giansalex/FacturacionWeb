//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2016/07/22 - 16:46:23
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace WebDataModel.Entities
{
    public partial class ventaDto
    {
        
        public String v_IdVenta { get; set; }

        
        public String v_IdFormatoUnicoFacturacion { get; set; }

        
        public String v_Periodo { get; set; }

        
        public String v_Mes { get; set; }

        
        public String v_Correlativo { get; set; }

        
        public Nullable<Int32> i_IdIgv { get; set; }

        
        public Nullable<Int32> i_IdTipoDocumento { get; set; }

        
        public String v_SerieDocumento { get; set; }

        
        public String v_CorrelativoDocumento { get; set; }

        
        public String v_CorrelativoDocumentoFin { get; set; }

        
        public Nullable<Int32> i_IdTipoDocumentoRef { get; set; }

        
        public String v_SerieDocumentoRef { get; set; }

        
        public String v_CorrelativoDocumentoRef { get; set; }

        
        public Nullable<DateTime> t_FechaRef { get; set; }

        
        public String v_IdCliente { get; set; }

        
        public String v_NombreClienteTemporal { get; set; }

        
        public String v_DireccionClienteTemporal { get; set; }

        
        public Nullable<DateTime> t_FechaRegistro { get; set; }

        
        public Nullable<Decimal> d_TipoCambio { get; set; }

        
        public Nullable<Int32> i_NroDias { get; set; }

        
        public Nullable<DateTime> t_FechaVencimiento { get; set; }

        
        public Nullable<Int32> i_IdCondicionPago { get; set; }

        
        public String v_Concepto { get; set; }

        
        public Nullable<Int32> i_IdMoneda { get; set; }

        
        public Nullable<Int32> i_IdEstado { get; set; }

        
        public Nullable<Int32> i_EsAfectoIgv { get; set; }

        
        public Nullable<Int32> i_PreciosIncluyenIgv { get; set; }

        
        public String v_IdVendedor { get; set; }

        
        public String v_IdVendedorRef { get; set; }

        
        public Nullable<Decimal> d_PorcDescuento { get; set; }

        
        public Nullable<Decimal> d_PocComision { get; set; }

        
        public Nullable<Decimal> d_ValorFOB { get; set; }

        
        public Nullable<Int32> i_DeduccionAnticipio { get; set; }

        
        public String v_NroPedido { get; set; }

        
        public String v_NroGuiaRemisionSerie { get; set; }

        
        public String v_NroGuiaRemisionCorrelativo { get; set; }

        
        public String v_OrdenCompra { get; set; }

        
        public Nullable<DateTime> t_FechaOrdenCompra { get; set; }

        
        public Nullable<Int32> i_IdTipoVenta { get; set; }

        
        public Nullable<Int32> i_IdTipoOperacion { get; set; }

        
        public Nullable<Int32> i_IdEstablecimiento { get; set; }

        
        public Nullable<Int32> i_IdPuntoEmbarque { get; set; }

        
        public Nullable<Int32> i_IdPuntoDestino { get; set; }

        
        public Nullable<Int32> i_IdTipoEmbarque { get; set; }

        
        public Nullable<Int32> i_IdMedioPagoVenta { get; set; }

        
        public String v_Marca { get; set; }

        
        public Nullable<Int32> i_DrawBack { get; set; }

        
        public String v_NroBulto { get; set; }

        
        public String v_BultoDimensiones { get; set; }

        
        public Nullable<Decimal> d_PesoBrutoKG { get; set; }

        
        public Nullable<Decimal> d_PesoNetoKG { get; set; }

        
        public Nullable<Decimal> d_Valor { get; set; }

        
        public Nullable<Decimal> d_ValorVenta { get; set; }

        
        public Nullable<Decimal> d_Descuento { get; set; }

        
        public Nullable<Decimal> d_Percepcion { get; set; }

        
        public Nullable<Decimal> d_Anticipio { get; set; }

        
        public Nullable<Decimal> d_IGV { get; set; }

        
        public Nullable<Decimal> d_Total { get; set; }

        
        public Nullable<Decimal> d_total_isc { get; set; }

        
        public Nullable<Decimal> d_total_otrostributos { get; set; }

        
        public String v_PlacaVehiculo { get; set; }

        
        public String v_IdTipoKardex { get; set; }

        
        public Nullable<Int32> i_Impresion { get; set; }

        
        public Nullable<Int32> i_Eliminado { get; set; }

        
        public Nullable<Int32> i_InsertaIdUsuario { get; set; }

        
        public Nullable<DateTime> t_InsertaFecha { get; set; }

        
        public Nullable<Int32> i_ActualizaIdUsuario { get; set; }

        
        public Nullable<DateTime> t_ActualizaFecha { get; set; }

        
        public Nullable<Int16> i_EstadoSunat { get; set; }

        
        public Nullable<Int32> i_IdTipoNota { get; set; }

        public Nullable<Int16> i_EsGratuito { get; set; }

        public List<String> ventadetalle_v_IdVentaDetalle { get; set; }

        
        public List<Int32> ventahomolagacion_i_IdVentaHomologacion { get; set; }

        public ventaDto()
        {
        }
    }
}
