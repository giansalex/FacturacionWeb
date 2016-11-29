using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBillPanel.Models
{
    public class FilterDoc
    {
        public int TipoDocumento { get; set; }
        public string Serie { get; set; }
        public string Correlativo { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
    }
}