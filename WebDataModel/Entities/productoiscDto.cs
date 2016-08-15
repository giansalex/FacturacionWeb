using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDataModel.Entities
{
    public class productoiscDto
    {
        public Int32 i_IdProductoIsc { get; set; }

        
        public String v_IdProducto { get; set; }

        
        public Nullable<Int32> i_IdSistemaIsc { get; set; }

        
        public Nullable<Decimal> d_Porcentaje { get; set; }

        
        public Nullable<Decimal> d_Monto { get; set; }

        
        public String v_Periodo { get; set; }

        
        public Nullable<Int32> i_InsertaIdUsuario { get; set; }

        
        public Nullable<DateTime> t_InsertaFecha { get; set; }

        
        public Nullable<Int32> i_ActualizaIdUsuario { get; set; }

        
        public Nullable<DateTime> t_ActualizaFecha { get; set; }

        public productoiscDto()
        {
        }
    }
}
