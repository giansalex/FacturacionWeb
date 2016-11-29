using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBillPanel.Models
{
    public class ClienteCredencial
    {
        [RegularExpression(@"^\d{8,11}$", ErrorMessage = @"No es un Numero Válido")]
        [MaxLength(11, ErrorMessage = @"No puede ser mayor de 11 digitos")]
        [Required]
        [Display(Name = @"RUC o DNI")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = @"Contraseña")]
        public string Password { get; set; }
    }

    public class VentaCredencial
    {
        [Required]
        [Display(Name = @"Documento")]
        public TypeDocument TipoDocumento { get; set; }
        [Required]
        [RegularExpression("^[FB]\\w{3}$", ErrorMessage = @"Serie Inválida.")]
        public string Serie { get; set; }
        [Required]
        [RegularExpression("^\\d{1,8}$", ErrorMessage = @"Correlativo inválido.")]
        public string Correlativo { get; set; }        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = @"Fecha de Emisión")]
        public DateTime FechaEmision { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
    }

    public enum TypeDocument{
        FACTURA = 1,
        BOLETA = 3,
        [Display(Name = "NOTA DE CREDITO")]
        NOTACREDITO = 7,
        [Display(Name ="NOTA DE DEBITO")]
        NOTADEBITO = 8
    }
}