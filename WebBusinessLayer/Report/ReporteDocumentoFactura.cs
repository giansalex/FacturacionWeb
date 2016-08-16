using System;

namespace WebBusinessLayer.Report
{
    public class ReporteDocumentoFactura
    {
        public DateTime FechaRegistro { get; set; }
        public string NroDocCliente { get; set; }
        public string NombreCliente { get; set; }
        public int TipoDocCliente { get; set; }
        public string Direccion { get; set; }

        public string Documento { get; set; }
        public int TipoDocumento { get; set; }
        public decimal ValorVenta { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public decimal Descuento { get; set; }
        public int Gratuito { get; set; }

        public string CodigoArticulo { get; set; }
        public decimal Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal d_Valor { get; set; }
        public decimal d_ValorVenta { get; set; }
        public decimal d_Descuento { get; set; }
        public string Unidad { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal TipoCambio { get; set; }
        public int Moneda { get; set; }
        public string d_Igv { get; set; }
        public string Observacion { get; set; }
        public decimal Valor { get; set; }
    }
}
