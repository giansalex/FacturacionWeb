using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WebDataModel;
using WebDataModel.Entities;

namespace WebBusinessLayer
{
    using Querys;
    public class ClienteBl : BaseBl
    {
        public string GetIdClient(string pstrDocumento, string pstrClave)
        {
            try
            {
                using (var db = DbHelper.GetConection())
                {
                    var user = db.Query<clienteDto>(QueryHelper.GetQuery(nameof(GetIdClient)), 
                        new {id = pstrDocumento }).FirstOrDefault();
                    LastResult.Success = true;
                    return user?.v_IdCliente;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErroMessage = ex.Message;
                return null;
            }
        }

        public clienteDto GetClient(string pstrId)
        {
            try
            {
                using (var db = DbHelper.GetConection())
                {
                    var user = db.Query<clienteDto>(QueryHelper.GetQuery(nameof(GetClient)), new {id = pstrId}).FirstOrDefault();
                    LastResult.Success = true;
                    return user;
                }
            }
            catch (Exception ex)
            {
                LastResult.ErroMessage = ex.Message;
                return null;
            }
        }
    }
}
