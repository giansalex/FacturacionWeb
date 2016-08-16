using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CrystalDecisions.Shared;
using WebBusinessLayer;
using WebBusinessLayer.Report;
using WebDocs.Report;
using Zen.Barcode;

namespace WebDocs
{
    public class RepFacturacionElectronica
    {
        #region Method
        /// <summary>
        /// Genera un Pdf que representa el Comprobante Electronico.
        /// <para>El que podra ser enviado via Correo Electronico</para>
        /// </summary>
        /// <param name="idVenta">Id de la Venta</param>
        public string GenerarPdf(string idVenta)
        {
            var objBl = new HelperScripts();
            var aptitudeCertificate = objBl.GetReporteInvoice(idVenta);
            var objVenta = aptitudeCertificate.First();

            var ds1 = new DataSet();
            var dt = Utils.Utils.ConvertToDatatable(aptitudeCertificate);
            dt.TableName = "Comprobante";
            ds1.Tables.Add(dt);
            var bl = new ConfiguracionFacturacionBl();
            var config = bl.Get();
            if (!bl.LastResult.Success)
            {
                return string.Empty;
            }
            ds1.Tables.Add(GetDataEmpresa("EmpresaInfo",
                new KeyValuePair<string, object>("Logo", config.b_Logo),
                new KeyValuePair<string, object>("CodigoQr", GetQrCodeArray(objVenta, config.v_Ruc))
                ));
            var leyenda = objVenta.Gratuito == 1 
                ? "TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE" 
                : "SON : " + Utils.Utils.ConvertirenLetras(objVenta.Total) + " " + (objVenta.Moneda == 1 ? "SOLES" : "DOLARES AMERICANOS");
            var rp = new crFacturaElectronica();
            rp.SetDataSource(ds1);
            rp.SetParameterValue("TotalLetras", leyenda);
            rp.SetParameterValue("PaginaWeb", config.v_Web ?? "");
            rp.SetParameterValue("Resolucion", config.v_Resolucion ?? "");
            rp.SetParameterValue("RucEmpresa", "RUC: " + config.v_Ruc);
            try
            {
                var path = Path.Combine(Path.GetTempPath(), objVenta.Documento + ".pdf");
                rp.ExportToDisk(ExportFormatType.PortableDocFormat, path);
                return path;
            }
            catch
            {
                return string.Empty;
            }

        }

        private DataTable GetDataEmpresa(string nameTable, params KeyValuePair<string, object>[] bmps)
        {
            var empresa = new DataTable { TableName = nameTable };
            foreach (var item in bmps)
            {
                empresa.Columns.Add(item.Key, typeof(byte[])); 
            }
            empresa.Rows.Add(bmps.Select(i => i.Value).ToArray());
            return empresa;
        }

        private byte[] GetQrCodeArray(ReporteDocumentoFactura obj, string ruc)
        {
            var docs = obj.Documento.Split('-');
            var listParams = new List<string>
            {
                ruc, // RUC Emisor
                obj.TipoDocumento.ToString("00"), // Tipo Comp
                docs[0],
                docs[1],
                obj.Igv.ToString("0.00"),
                obj.Total.ToString("0.00"),
                obj.FechaRegistro.ToShortDateString(),
                obj.TipoDocCliente.ToString("00"),
                obj.NroDocCliente
            };
            var qrGen = new CodeQrBarcodeDraw();
            var metric = (BarcodeMetricsQr)qrGen.GetDefaultMetrics(30);
            metric.ErrorCorrection = QrErrorCorrection.Q;
            var qr = new CodeQrBarcodeDraw().Draw(string.Join("|", listParams) + "|", metric);
            using (var mem = new MemoryStream())
            {
                qr.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
                return mem.ToArray();
            }
        }
        #endregion
    }

}
