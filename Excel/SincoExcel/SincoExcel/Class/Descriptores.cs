using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;

using AppExternas;
using SincoOfficeLibrerias.wsOfficeSGD;
using System.Configuration;
using SincoOfficeLibrerias;

namespace SincoExcel
{
    public class Descriptores
    {
        /// <summary>
        /// Reliza operación de modificacion, creación y eliminación de descriptores
        /// </summary>
        /// <param name="Usuario">Datos de usuario de sesión</param>
        /// <param name="Conexion">Datos de conexion</param>
        /// <param name="operacion"></param>
        /// <param name="DESid"></param>
        /// <param name="DESdescripcion"></param>
        /// <param name="DESobservacion"></param>
        /// <param name="DEStipoDato"></param>
        /// <param name="DCCcategoria"></param>
        /// <param name="DESfuenteExterna"></param>
        /// <returns></returns>
        public static DataTable GuardarDescriptorCategoria(Login Usuario, ConexionesExcel Conexion, string operacion, string DESid, string DESdescripcion,
            string DESobservacion, string DEStipoDato, string DCCcategoria, string DESfuenteExterna)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                DataTable Resultado = new DataTable();

                wsOfficeSGD Ws2 = new wsOfficeSGD();
                Ws2.Url = Conexion.urlWsOfficeSGD;
                Ws2.Timeout = Conexion.TimeOut;

                XmlNode Retorno = Ws2.GuardarDescriptorCategoria(SessionID, operacion, DESid, DESdescripcion, DESobservacion, DEStipoDato, DCCcategoria, DESfuenteExterna);

                Resultado = XML.XMLtoDataTable(Retorno);
                return Resultado;
            }
            catch
            {
                return new DataTable();
            }
        }

        /// <summary>
        /// Busca subseries de SGD
        /// </summary>
        /// <param name="Usuario">Datos de usuario de sesión</param>
        /// <param name="Conexion">Datos de conexion</param>
        /// <param name="operacion"></param>
        /// <param name="SubSerie"></param>
        /// <returns></returns>
        public static DataTable ConsultarSubseries(Login Usuario, ConexionesExcel Conexion, string operacion, string SubSerie)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                DataTable Resultado = new DataTable();

                wsOfficeSGD WsOffice = new wsOfficeSGD();
                WsOffice.Url = Conexion.urlWsOfficeSGD;
                WsOffice.Timeout = Conexion.TimeOut;

                XmlNode Retorno = WsOffice.ConsultaDescriptoresCategoria(SessionID, operacion, SubSerie);

                Resultado = XML.XMLtoDataTable(Retorno);
                return Resultado;
            }
            catch
            {
                return new DataTable();
            }

        }
    }
}
