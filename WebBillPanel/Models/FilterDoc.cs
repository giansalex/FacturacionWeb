using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBillPanel.Models
{
    /// <summary>
    /// Class FilterDoc for Filter Document.
    /// </summary>
    public class FilterDoc
    {
        /// <summary>
        /// Gets or sets the tipo documento.
        /// </summary>
        /// <value>The tipo documento.</value>
        public int TipoDocumento { get; set; }
        /// <summary>
        /// Gets or sets the serie.
        /// </summary>
        /// <value>The serie.</value>
        public string Serie { get; set; }
        /// <summary>
        /// Gets or sets the correlativo.
        /// </summary>
        /// <value>The correlativo.</value>
        public string Correlativo { get; set; }
        /// <summary>
        /// Gets or sets the fecha inicial.
        /// </summary>
        /// <value>The fecha inicial.</value>
        public DateTime FechaInicial { get; set; }
        /// <summary>
        /// Gets or sets the fecha final.
        /// </summary>
        /// <value>The fecha final.</value>
        public DateTime FechaFinal { get; set; }
    }
}