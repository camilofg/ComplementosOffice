using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AppExternas;
using SincoOfficeLibrerias;
using System.Xml;

namespace SincoExcel
{
    public class FuentesExternas
    {
        public  FuentesExternas()
        {

        }

        public static DataTable CRUDFuentesExternas(Login Usuario, ConexionesExcel Conexion, string operacion, int idFuente, string descripcion, string texto, bool activo)
        {
            DataTable table = new DataTable();
            try
            {
                byte[] sessionId = new DataEncryption().Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);
                SincoOfficeLibrerias.wsOfficeSGD.wsOfficeSGD esgd = new SincoOfficeLibrerias.wsOfficeSGD.wsOfficeSGD
                {
                    Url = Conexion.urlWsOfficeSGD,
                    Timeout = Conexion.TimeOut
                };
                XmlNode nodo = esgd.CRUDFuenteExterna(sessionId, operacion, idFuente, descripcion, int.Parse(Usuario.IdUsuario), texto, activo);
                esgd.Dispose();
                return XML.XMLtoDataTable(nodo);
            }
            catch (Exception exception)
            {
                return new DataTable { TableName = exception.ToString() };
            }
        }

    }
}
