using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WebDataModel;

namespace WebBusinessLayer
{
    public class DatahierarchyBl : BaseBl
    {
        public void GetTypeDocs()
        {
            try
            {
                using (var dbContext = DbHelper.GetConection())
                {
                    dbContext.Query("SELECT * FROM datahierarycy");
                }
            }
            catch (Exception ex)
            {
                LastResult.ErroMessage = ex.Message;
            }
        }
    }
}
