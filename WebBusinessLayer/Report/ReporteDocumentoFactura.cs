using System;

namespace WebBusinessLayer.Report
{
    /// <summary>
    /// Class ReporteDocumentoFactura.
    /// </summary>
    public class ReporteDocumentoFactura
    {
        /// <summary>
        /// Gets or sets the fecha registro.
        /// </summary>
        /// <value>The fecha registro.</value>
        public DateTime FechaRegistro { get; set; }
        /// <summary>
        /// Gets or sets the nro document cliente.
        /// </summary>
        /// <value>The nro document cliente.</value>
        public string NroDocCliente { get; set; }
        /// <summary>
        /// Gets or sets the nombre cliente.
        /// </summary>
        /// <value>The nombre cliente.</value>
        public string NombreCliente { get; set; }
        /// <summary>
        /// Gets or sets the tipo document cliente.
        /// </summary>
        /// <value>The tipo document cliente.</value>
        public int TipoDocCliente { get; set; }
        /// <summary>
        /// Gets or sets the direccion.
        /// </summary>
        /// <value>The direccion.</value>
        public string Direccion { get; set; }

        /// <summary>
        /// Gets or sets the documento.
        /// </summary>
        /// <value>The documento.</value>
        public string Documento { get; set; }
        /// <summary>
        /// Gets or sets the tipo documento.
        /// </summary>
        /// <value>The tipo documento.</value>
        public int TipoDocumento { get; set; }
        /// <summary>
        /// Gets or sets the valor venta.
        /// </summary>
        /// <value>The valor venta.</value>
        public decimal ValorVenta { get; set; }
        /// <summary>
        /// Gets or sets the igv.
        /// </summary>
        /// <value>The igv.</value>
        public decimal Igv { get; set; }
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public decimal Total { get; set; }
        /// <summary>
        /// Gets or sets the descuento.
        /// </summary>
        /// <value>The descuento.</value>
        public decimal Descuento { get; set; }
        /// <summary>
        /// Gets or sets the gratuito.
        /// </summary>
        /// <value>The gratuito.</value>
        public int Gratuito { get; set; }

        /// <summary>
        /// Gets or sets the codigo articulo.
        /// </summary>
        /// <value>The codigo articulo.</value>
        public string CodigoArticulo { get; set; }
        /// <summary>
        /// Gets or sets the cantidad.
        /// </summary>
        /// <value>The cantidad.</value>
        public decimal Cantidad { get; set; }
        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        /// <value>The descripcion.</value>
        public string Descripcion { get; set; }
        /// <summary>
        /// Gets or sets the precio.
        /// </summary>
        /// <value>The precio.</value>
        public decimal Precio { get; set; }
        /// <summary>
        /// Gets or sets the d_ valor.
        /// </summary>
        /// <value>The d_ valor.</value>
        public decimal d_Valor { get; set; }
        /// <summary>
        /// Gets or sets the d_ valor venta.
        /// </summary>
        /// <value>The d_ valor venta.</value>
        public decimal d_ValorVenta { get; set; }
        /// <summary>
        /// Gets or sets the d_ descuento.
        /// </summary>
        /// <value>The d_ descuento.</value>
        public decimal d_Descuento { get; set; }
        public string Unidad { get; set; }
        /// <summary>
        /// Gets or sets the precio venta.
        /// </summary>
        /// <value>The precio venta.</value>
        public decimal PrecioVenta { get; set; }
        /// <summary>
        /// Gets or sets the tipo cambio.
        /// </summary>
        /// <value>The tipo cambio.</value>
        public decimal TipoCambio { get; set; }
        /// <summary>
        /// Gets or sets the moneda.
        /// </summary>
        /// <value>The moneda.</value>
        public int Moneda { get; set; }
        /// <summary>
        /// Gets or sets the d_ igv.
        /// </summary>
        /// <value>The d_ igv.</value>
        public string d_Igv { get; set; }
        /// <summary>
        /// Gets or sets the observacion.
        /// </summary>
        /// <value>The observacion.</value>
        public string Observacion { get; set; }
        /// <summary>
        /// Gets or sets the valor.
        /// </summary>
        /// <value>The valor.</value>
        public decimal Valor { get; set; }
    }
}
