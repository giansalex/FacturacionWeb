using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FacturacionElectronica.GeneradorXml;
using FacturacionElectronica.GeneradorXml.Entity;
using FacturacionElectronica.GeneradorXml.Entity.Details;
using FacturacionElectronica.GeneradorXml.Entity.Misc;
using FacturacionElectronica.GeneradorXml.Enums;
using Gs.Ubl.v2.Sac;
using WebBusinessLayer;
using WebDataModel.Entities;
using WebDocs.Utils;
using OperationResult2 = FacturacionElectronica.GeneradorXml.Res.OperationResult;
namespace WebDocs
{
    /// <summary>
    /// Class XmlHelper.
    /// </summary>
    public class XmlHelper
    {
        #region Fields
        private X509Certificate2 _cert;
        private configuracionfacturacionDto _config;
        #endregion

        #region Public Methods
        /// <summary>
        /// Generars the specified identifier venta.
        /// </summary>
        /// <param name="idVenta">The identifier venta.</param>
        /// <returns>System.String.</returns>
        public string Generar(string idVenta)
        {
            #region Entidad
            var helper = new HelperScripts();
            var objs = helper.GetVentaCliente(idVenta);
            if (!helper.LastResult.Success)
            {
                return "";
            }
            #endregion

            #region Action
            Func<Tuple<ventaDto, clienteDto, decimal>, string> action = null;
            switch (objs.Item1.i_IdTipoDocumento)
            {
                case 1: // FACTURA
                case 3: // BOLETA
                    action = WorkingInvoice;
                    break;
                case 7: // NOTA CREDITO
                    action = WorkingNotaCredito;
                    break;
                case 8: // NOTA DEBITO
                    action = WorkingNotaDebito;
                    break;
            }
            return action != null ? action(objs) : "";
            #endregion
        }
        #endregion

        #region Private Methods
        private string WorkingInvoice(Tuple<ventaDto, clienteDto, decimal> objs)
        {
            #region Entidad
            var objventa = objs.Item1;
            var client = objs.Item2;
            #endregion

            #region Generando la Entidad Invoice
            Init();
            var tipoComprobante = (TipoDocumentoElectronico)Enum.ToObject(typeof(TipoDocumentoElectronico), objventa.i_IdTipoDocumento ?? 1);
            var tipoDocIdentidadClient = (TipoDocumentoIdentidad)Enum.ToObject(typeof(TipoDocumentoIdentidad), client.i_IdTipoIdentificacion ?? 1);
            var invoiceHeaderEntity = new InvoiceHeader
            {
                TipoDocumento = tipoComprobante,
                SerieDocumento = objventa.v_SerieDocumento,
                CorrelativoDocumento = objventa.v_CorrelativoDocumento,
                FechaEmision = objventa.t_FechaRegistro ?? DateTime.Now,
                NombreRazonSocialCliente = System.Text.RegularExpressions.Regex.Replace(string.Join(" ", client.v_ApePaterno, client.v_ApeMaterno, client.v_PrimerNombre, client.v_SegundoNombre, client.v_RazonSocial), @"[^\w\.-]", " ").Trim(),
                NroDocCliente = client.v_NroDocIdentificacion,
                RucEmisor = _config.v_Ruc,
                NombreRazonSocialEmisor = _config.v_RazonSocial,
                NombreComercialEmisor = _config.v_NombreComercial,
                CodigoMoneda = objventa.i_IdMoneda == 1 ? "PEN" : "USD",
                TipoDocumentoIdentidadCliente = tipoDocIdentidadClient,
                TipoDocumentoIdentidadEmisor = TipoDocumentoIdentidad.RegistroUnicoContribuyentes,
                DetallesDocumento = new List<InvoiceDetail>(),
                TotalVenta = objventa.d_Total ?? 0M,
                InfoAddicional = new List<AdditionalPropertyType>(),
                Impuesto = GetImpuestosGlobal(objventa),
                DireccionEmisor = new DireccionType
                {
                    CodigoPais = "PE",
                    CodigoUbigueo = _config.v_Ubigueo,
                    Direccion = _config.v_Domicilio,
                    Zona = _config.v_Urbanizacion,
                    Departamento = _config.v_Departamento,
                    Provincia = _config.v_Provincia,
                    Distrito = _config.v_Distrito,
                }
            };

            if (!string.IsNullOrWhiteSpace(objventa.v_SerieDocumentoRef))
            {
                invoiceHeaderEntity.DocumentoReferenciaNumero = objventa.v_SerieDocumentoRef + "-" + objventa.v_CorrelativoDocumentoRef;
                invoiceHeaderEntity.DocumentoReferenciaTipoDocumento = (TipoDocumentoElectronico)Enum.ToObject(typeof(TipoDocumentoElectronico), objventa.i_IdTipoDocumentoRef ?? 1);
            }
            if (!string.IsNullOrWhiteSpace(objventa.v_NroGuiaRemisionSerie))
            {
                invoiceHeaderEntity.GuiaRemisionReferencia = new List<GuiaRemisionType>
                {
                    new GuiaRemisionType
                    {
                        IdTipoGuiaRemision = "09", // Tipo Guia Remision
                        NumeroGuiaRemision = objventa.v_NroGuiaRemisionSerie + "-" + objventa.v_NroGuiaRemisionCorrelativo
                    }
                };
            }

            //Descuento Global
            var factDesc = 1M;
            if (IsValueNotNullorZero(objventa.d_PorcDescuento))
            {
                factDesc = 1 - (objventa.d_PorcDescuento ?? 0) / 100;
                var valTotal = objventa.d_Total / factDesc;
                invoiceHeaderEntity.DescuentoGlobal = valTotal - objventa.d_Total ?? 0;
            }

            var sumTotales = LoadDetail(invoiceHeaderEntity.DetallesDocumento, objventa, objs.Item3);
            invoiceHeaderEntity.TotalTributosAdicionales = GetTributosAddicionales(sumTotales, factDesc);
            if (invoiceHeaderEntity.TotalVenta != 0M)
                invoiceHeaderEntity.InfoAddicional.Add(new AdditionalPropertyType
                {
                    ID = "1000", // Monto en Letras
                    Value = Utils.Utils.ConvertirenLetras(objventa.d_Total ?? 0)
                });

            if (objventa.i_EsGratuito == 1)
            {
                invoiceHeaderEntity.InfoAddicional.Add(new AdditionalPropertyType
                {
                    ID = "1002",
                    Value = "TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE"
                });
            }
            if (IsValueNotNullorZero(objventa.d_Descuento)) //Si Tiene Descuentos
            {
                invoiceHeaderEntity.TotalTributosAdicionales.Add(new TotalTributosType
                {
                    Id = OtrosConceptosTributarios.TotalDescuentos,
                    MontoPagable = objventa.d_Descuento ?? 0
                });
            }

            #endregion

            #region Generando XML
            var objOperationResult = new OperationResult2();
            var xmlResultPath = new XmlDocGenerator(_cert).GeneraDocumentoInvoice(ref objOperationResult,
                invoiceHeaderEntity);

            return objOperationResult.Success ? xmlResultPath : "";

            #endregion
        }

        private string WorkingNotaCredito(Tuple<ventaDto, clienteDto, decimal> objs)
        {
            #region Entidades
            var objVenta = objs.Item1;
            var client = objs.Item2;
            #endregion

            #region Gen Entidad Nota Credito
            Init();
            var notaCredit = new CreditNoteHeader(GetBaseNota(objVenta, client, objs.Item3))
            {
                TipoNota = (TipoNotaCreditoElectronica) Enum.ToObject(typeof(TipoNotaCreditoElectronica), objVenta.i_IdTipoNota ?? 1)
            };

            #endregion

            #region Generando XML
            var objOperationResult = new OperationResult2();
            var xmlResultPath = new XmlDocGenerator(_cert).GenerarDocumentoCreditNote(ref objOperationResult, notaCredit);
            return objOperationResult.Success ? xmlResultPath : string.Empty;
            #endregion
        }

        private string WorkingNotaDebito(Tuple<ventaDto, clienteDto, decimal> objs)
        {
            #region Entidades
            var objVenta = objs.Item1;
            var client = objs.Item2;
            #endregion

            #region Gen Entidad Nota Credito
            Init();
            var notaDebit = new DebitNoteHeader(GetBaseNota(objVenta, client, objs.Item3))
            {
                TipoNota = (TipoNotaDebitoElectronica)Enum.ToObject(typeof(TipoNotaDebitoElectronica), objVenta.i_IdTipoNota ?? 1)
            };
            #endregion

            #region Generando XML
            var objOperationResult = new OperationResult2();
            var xmlResultPath = new XmlDocGenerator(_cert).GenerarDocumentoDebitNote(ref objOperationResult, notaDebit);
            return objOperationResult.Success ? xmlResultPath : string.Empty;
            #endregion
        }

        private NotasBase<InvoiceDetail> GetBaseNota(ventaDto pobjVenta, clienteDto pobjCliente, decimal igv)
        {
            var baseNote = new NotasBase<InvoiceDetail>
            {
                SerieDocumento = pobjVenta.v_SerieDocumento,
                CorrelativoDocumento = pobjVenta.v_CorrelativoDocumento,
                DocumentoRef = (pobjVenta.v_SerieDocumentoRef + "-" + pobjVenta.v_CorrelativoDocumentoRef).Trim(),
                TipoDocRef = (TipoDocumentoElectronico)Enum.ToObject(typeof(TipoDocumentoElectronico), pobjVenta.i_IdTipoDocumentoRef ?? 1),
                FechaEmision = pobjVenta.t_FechaRegistro ?? DateTime.Now,
                NombreRazonSocialCliente = System.Text.RegularExpressions.Regex.Replace(string.Join(" ", pobjCliente.v_ApePaterno, pobjCliente.v_ApeMaterno, pobjCliente.v_PrimerNombre, pobjCliente.v_SegundoNombre, pobjCliente.v_RazonSocial), @"[^\w\.-]", " ").Trim(),
                TipoDocumentoIdentidadCliente = (TipoDocumentoIdentidad)Enum.ToObject(typeof(TipoDocumentoIdentidad), pobjCliente.i_IdTipoIdentificacion ?? 6),
                NroDocCliente = pobjCliente.v_NroDocIdentificacion,
                RucEmisor = _config.v_Ruc,
                NombreComercialEmisor = _config.v_NombreComercial,
                NombreRazonSocialEmisor = _config.v_RazonSocial,
                TipoDocumentoIdentidadEmisor = TipoDocumentoIdentidad.RegistroUnicoContribuyentes,
                CodigoMoneda = pobjVenta.i_IdMoneda == 1 ? "PEN" : "USD",
                Motivo = pobjVenta.v_Concepto,
                Total = pobjVenta.d_ValorVenta ?? 0M,
                TotalCargos = 0,
                DireccionEmisor = new DireccionType
                {
                    CodigoPais = "PE",
                    CodigoUbigueo = _config.v_Ubigueo,
                    Departamento = _config.v_Departamento,
                    Provincia = _config.v_Provincia,
                    Distrito = _config.v_Distrito,
                    Zona = _config.v_Urbanizacion,
                    Direccion = _config.v_Domicilio
                },
                Impuesto = GetImpuestosGlobal(pobjVenta),
                DetallesDocumento = new List<InvoiceDetail>()
            };
            //if (!string.IsNullOrWhiteSpace(pobjVenta.v_SerieDocumentoRef))
            //{
            //    baseNote.DocumentoReferenciaNumero = pobjVenta.v_SerieDocumentoRef + "-" + pobjVenta.v_CorrelativoDocumentoRef;
            //    baseNote.DocumentoReferenciaTipoDocumento = (TipoDocumentoElectronico)Enum.ToObject(typeof(TipoDocumentoElectronico), pobjVenta.i_IdTipoDocumentoRef);
            //}
            if (!string.IsNullOrWhiteSpace(pobjVenta.v_NroGuiaRemisionSerie))
            {
                baseNote.GuiaRemisionReferencia = new List<GuiaRemisionType>
                {
                    new GuiaRemisionType {
                        IdTipoGuiaRemision = "09", // Tipo Guia Remision
                        NumeroGuiaRemision = pobjVenta.v_NroGuiaRemisionSerie + "-" + pobjVenta.v_NroGuiaRemisionCorrelativo}
                };
            }
            //Descuento Global
            var factDesc = 1M;
            //if (IsValueNotNullorZero(pobjVenta.d_PorcDescuento))
            //{
            //    factDesc = 1 - (pobjVenta.d_PorcDescuento ?? 0) / 100;
            //    var valTotal = pobjVenta.d_Total / factDesc;
            //    baseNote.DescuentoGlobal = valTotal - pobjVenta.d_Total ?? 0;
            //}
            var sumTotals = LoadDetail(baseNote.DetallesDocumento, pobjVenta, igv);
            baseNote.TotalTributosAdicionales = GetTributosAddicionales(sumTotals, factDesc);
            if (IsValueNotNullorZero(pobjVenta.d_Descuento)) //Si Tiene Descuentos
            {
                baseNote.TotalTributosAdicionales.Add(new TotalTributosType
                {
                    Id = OtrosConceptosTributarios.TotalDescuentos,
                    MontoPagable = pobjVenta.d_Descuento ?? 0m
                });
            }
            return baseNote;
        }

        #region Invoice Require
        private static List<TotalTributosType> GetTributosAddicionales(IDictionary<OtrosConceptosTributarios, decimal> datos, decimal factorDesc)
        {
            var result = new List<TotalTributosType>();
            //foreach (var key in datos.Keys) if (key != OtrosConceptosTributarios.TotalVentaOperacionesGratuitas) datos[key] * factorDesc;

            if (IsValueNotNullorZero(datos[OtrosConceptosTributarios.TotalVentaOperacionesGravadas])) //TODO: Agregarlo cuando sea Gratuita
                result.Add(new TotalTributosType
                {
                    Id = OtrosConceptosTributarios.TotalVentaOperacionesGravadas,
                    MontoPagable = datos[OtrosConceptosTributarios.TotalVentaOperacionesGravadas] * factorDesc
                });
            if (IsValueNotNullorZero(datos[OtrosConceptosTributarios.TotalVentaOperacionesExoneradas]))
                result.Add(new TotalTributosType
                {
                    Id = OtrosConceptosTributarios.TotalVentaOperacionesExoneradas,
                    MontoPagable = datos[OtrosConceptosTributarios.TotalVentaOperacionesExoneradas] * factorDesc
                });
            if (IsValueNotNullorZero(datos[OtrosConceptosTributarios.TotalVentaOperacionesInafectas]))
                result.Add(new TotalTributosType
                {
                    Id = OtrosConceptosTributarios.TotalVentaOperacionesInafectas,
                    MontoPagable = datos[OtrosConceptosTributarios.TotalVentaOperacionesInafectas] * factorDesc
                });
            if (IsValueNotNullorZero(datos[OtrosConceptosTributarios.TotalVentaOperacionesGratuitas]))
                result.Add(new TotalTributosType
                {
                    Id = OtrosConceptosTributarios.TotalVentaOperacionesGratuitas,
                    MontoPagable = datos[OtrosConceptosTributarios.TotalVentaOperacionesGratuitas]
                });
            return result;
        }
        private static List<TotalImpuestosType> GetImpuestosGlobal(ventaDto objVenta)
        {
            var result = new List<TotalImpuestosType>();
            if (IsValueNotNullorZero(objVenta.d_IGV))
            {
                result.Add(new TotalImpuestosType
                {
                    TipoTributo = TipoTributo.IGV_VAT,
                    Monto = objVenta.d_IGV ?? 0M
                });
            }
            if (IsValueNotNullorZero(objVenta.d_total_isc))
                result.Add(new TotalImpuestosType
                {
                    TipoTributo = TipoTributo.ISC_EXC,
                    Monto = objVenta.d_total_isc ?? 0M
                });
            if (IsValueNotNullorZero(objVenta.d_total_otrostributos))
                result.Add(new TotalImpuestosType
                {
                    TipoTributo = TipoTributo.OTROS_OTH,
                    Monto = objVenta.d_total_otrostributos ?? 0M
                });
            return result;
        }
        private static List<TotalImpuestosType> GetImpuestosLine(ventadetalleDto objItem, TipoAfectacionIgv igv)
        {
            var result = new List<TotalImpuestosType>
            {
                new TotalImpuestosType
                {
                    TipoTributo = TipoTributo.IGV_VAT,
                    TipoAfectacion =  igv,
                    Monto = objItem.d_Igv ?? 0
                }
            };
            //if(IsValueNotNullorZero(objItem.d_isc))
            //    result.Add(new TotalImpuestosType
            //    {
            //        TipoIsc = GetSistemaIsc(objItem.v_IdProductoDetalle),
            //        TipoTributo = TipoTributo.ISC_EXC,
            //        Monto = objItem.d_isc ?? 0
            //    });
            if (IsValueNotNullorZero(objItem.d_otrostributos))
                result.Add(new TotalImpuestosType
                {
                    TipoTributo = TipoTributo.OTROS_OTH,
                    Monto = objItem.d_otrostributos ?? 0,
                });
            return result;
        }
        private static void SetSuma(ref Dictionary<OtrosConceptosTributarios, decimal> totales, TipoAfectacionIgv tipo, decimal valor)
        {
            if ((int)tipo % 10 == 0)
            {
                var t = (int)tipo / 10;
                switch (t)
                {
                    case 1:
                        totales[OtrosConceptosTributarios.TotalVentaOperacionesGravadas] += valor;
                        break;
                    case 2:
                        totales[OtrosConceptosTributarios.TotalVentaOperacionesExoneradas] += valor;
                        break;
                    case 3:
                    case 4:
                        totales[OtrosConceptosTributarios.TotalVentaOperacionesInafectas] += valor;
                        break;
                }
            }
            else
            {
                totales[OtrosConceptosTributarios.TotalVentaOperacionesGratuitas] += valor;
            }
        }
        public static bool IsValueNotNullorZero(decimal? value)
        {
            return value.HasValue && value.Value != 0;
        }
        #endregion

        private void Init()
        {
            _config = new ConfiguracionFacturacionBl().Get();
            _config.v_Clave = Crypto.DecryptStringAes(_config.v_Clave, "TiSolUciOnEs");
            _config.v_ClaveCertificado = Crypto.DecryptStringAes(_config.v_ClaveCertificado, "TiSolUciOnEs");
            _cert = new X509Certificate2(_config.b_FileCertificado, _config.v_ClaveCertificado);
        }

        private Dictionary<OtrosConceptosTributarios, decimal> LoadDetail(List<InvoiceDetail> details, ventaDto obj, decimal igv)
        {
            var helper = new HelperScripts();
            var ventas = helper.GetVentaDetalles(obj.v_IdVenta, _config.i_GroupUndInter ?? 148);
            if (ventas == null) return null;

            var sum = new Dictionary<OtrosConceptosTributarios, decimal>
            {
                {OtrosConceptosTributarios.TotalVentaOperacionesGravadas, 0},
                {OtrosConceptosTributarios.TotalVentaOperacionesExoneradas, 0},
                {OtrosConceptosTributarios.TotalVentaOperacionesInafectas, 0},
                {OtrosConceptosTributarios.TotalVentaOperacionesGratuitas, 0}
            };

            foreach (var detalle in ventas)
            {
                var item = detalle.Item1;
                var tipoAfectacionIgv = (TipoAfectacionIgv)Enum.ToObject(typeof(TipoAfectacionIgv), detalle.Item3);
                var precioUnit = (tipoAfectacionIgv == TipoAfectacionIgv.GravadoOperacionOnerosa && obj.i_PreciosIncluyenIgv == 1
                                 ? item.d_Precio / (1M + igv) : item.d_Precio) ?? 0M;
                var precioItem = precioUnit;
                productoiscDto isc = null;
                if (IsValueNotNullorZero(item.d_isc))
                {
                    isc = helper.GetIscFromDetail(item.v_IdProductoDetalle, obj.v_Periodo);
                    switch (isc.i_IdSistemaIsc)
                    {
                        case 1:
                            precioItem *= 1M + isc.d_Porcentaje ?? 0;
                            break;
                        case 2:
                            precioItem += isc.d_Monto ?? 0;
                            break;
                        case 3:
                            precioItem += isc.d_Porcentaje * isc.d_Monto ?? 0;
                            break;
                    }
                }
                if (tipoAfectacionIgv == TipoAfectacionIgv.GravadoOperacionOnerosa) precioItem *= (1M + igv);
                var isGratuito = (int)tipoAfectacionIgv % 10 != 0;
                var detail = new InvoiceDetail
                {
                    CodigoProducto = item.v_IdProductoDetalle,
                    Cantidad = item.d_Cantidad ?? 0M,
                    DescripcionProducto = item.v_DescripcionProducto,
                    PrecioUnitario = isGratuito ? 0 : precioUnit,
                    PrecioAlternativos = new List<PrecioItemType>
                            {
                                new PrecioItemType
                                {
                                    TipoDePrecio = TipoPrecioVenta.PrecioUnitario,
                                    Monto = isGratuito ? 0 : precioItem
                                } // Precio con ISC e IGV
                            },
                    UnidadMedida = detalle.Item2,
                    ValorVenta = item.d_ValorVenta ?? 0M,
                    Impuesto = GetImpuestosLine(item, tipoAfectacionIgv)
                };
                if (isGratuito)
                    detail.PrecioAlternativos.Add(new PrecioItemType
                    {
                        TipoDePrecio = TipoPrecioVenta.ValorReferencial,
                        Monto = precioItem
                    });

                if (isc != null)
                {
                    detail.Impuesto.Add(new TotalImpuestosType
                    {
                        TipoIsc = (TipoSistemaIsc)Enum.ToObject(typeof(TipoSistemaIsc), isc.i_IdSistemaIsc ?? 1),
                        TipoTributo = TipoTributo.ISC_EXC,
                        Monto = item.d_isc ?? 0
                    });
                }
                SetSuma(ref sum, tipoAfectacionIgv, isGratuito ? item.d_Cantidad * precioUnit ?? 0 : detail.ValorVenta);
                details.Add(detail);
            }
            return sum;
        }
        #endregion
    }
}
