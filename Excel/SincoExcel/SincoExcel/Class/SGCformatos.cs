using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Configuration;

using SincoOfficeLibrerias.wsOfficeSGD;
using SincoOfficeLibrerias.wsSGCdocumentos;
using SincoOfficeLibrerias;

using AppExternas;

namespace SincoExcel
{
    class SGCformatos
    {
        public static DataTable ConsultarFormatosEnRegistro(Login Usuario, ConexionesExcel Conexion, string operacion)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                wsSGCdocumentos Ws = new wsSGCdocumentos();
                Ws.Url = Conexion.urlwsSGCdocumentos;
                Ws.Timeout = Conexion.TimeOut;

                XmlNode Nodo = Ws.CRUDformatosSGC(SessionID, operacion);

                Ws.Dispose();

                DataTable TablaTemporal = XML.XMLtoDataTable(Nodo);

                return TablaTemporal;
            }
            catch (Exception Exc)
            {
                DataTable DT = new DataTable();
                DT.TableName = Exc.ToString();
                return DT;
            }
        }

        /// <summary>
        /// Devuelve el contenido en Byte[] del archivo ubicado en el directorio de SGC.
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="RutaArchivo"></param>
        /// <returns></returns>
        public static Byte[] LeerArchivosFormatos(Login Usuario, ConexionesExcel Conexion, string RutaArchivo)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                wsSGCdocumentos Ws = new wsSGCdocumentos();
                Ws.Url = Conexion.urlwsSGCdocumentos;
                Ws.Timeout = Conexion.TimeOut;

                Byte[] Archivo = Ws.LeerArchivosFormatos(SessionID, RutaArchivo);

                return Archivo;
            }
            catch
            {
                return new Byte[0];
            }
        }

        /// <summary>
        /// Guarda un archivo en el directorio de SGC
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="RutaArchivo"></param>
        /// <param name="Archivo"></param>
        /// <returns></returns>
        public static string GuardarArchivosFormatos(Login Usuario, ConexionesExcel Conexion, string RutaArchivo, Byte[] Archivo)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                wsSGCdocumentos Ws = new wsSGCdocumentos();
                Ws.Url = Conexion.urlwsSGCdocumentos;
                // no aplica objeto conexion por que el proceso suele ser mas demorado
                Ws.Timeout = 29000;

                string ResArchivo = Ws.GuardarArchivoFormato(SessionID, RutaArchivo, Archivo);

                return ResArchivo;
            }
            catch(Exception Exc)
            {
                return "0:Error en el servicio Web " + Exc.ToString();
            }
        }

        /// <summary>
        /// Devuelve el contenido en Byte[] de un archivo ubicado en el directorio de SGD
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="RutaArchivo"></param>
        /// <returns></returns>
        public static Byte[] LeerArchivosFormatosSGD(Login Usuario, ConexionesExcel Conexion, string RutaArchivo)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                wsOfficeSGD Ws = new wsOfficeSGD();
                Ws.Url = Conexion.urlWsOfficeSGD;
                // no aplica objeto conexion por que el proceso suele ser mas demorado
                Ws.Timeout = 29000;

                Byte[] Archivo = Ws.LeerArchivosFormatos(SessionID, RutaArchivo);

                return Archivo;
            }
            catch
            {
                return new Byte[0];
            }
        }

        /// <summary>
        /// Guarda un archivo el el directorio de SGD
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="RutaArchivo"></param>
        /// <param name="Archivo"></param>
        /// <returns></returns>
        public static string GuardarArchivosFormatosSGD(Login Usuario, ConexionesExcel Conexion, string RutaArchivo, Byte[] Archivo)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                wsOfficeSGD Ws = new wsOfficeSGD();
                Ws.Url = Conexion.urlWsOfficeSGD;
                Ws.Timeout = 29000;

                string ResArchivo = Ws.GuardarArchivoFormato(SessionID, RutaArchivo, Archivo);

                return ResArchivo;
            }
            catch (Exception Exc)
            {
                return "0:Error en el servicio Web " + Exc.ToString();
            }
        }

        /// <summary>
        /// Consulta variablers de configuración de ISO SGC
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="operacion"></param>
        /// <returns></returns>
        public static DataTable ConsultarConfiguracionISO(Login Usuario, ConexionesExcel Conexion, string operacion)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                wsSGCdocumentos Ws = new wsSGCdocumentos();
                Ws.Url = Conexion.urlwsSGCdocumentos;
                Ws.Timeout = Conexion.TimeOut;

                XmlNode Nodo = Ws.CRUDformatosSGC(SessionID, operacion);
                Ws.Dispose();

                DataTable TablaTemporal = XML.XMLtoDataTable(Nodo);

                return TablaTemporal;
            }
            catch (Exception Exc)
            {
                DataTable DT = new DataTable();
                DT.TableName = Exc.ToString();
                return DT;
            }
        }

        /// <summary>
        /// Valida el acesso al menu de opciones de la aplicación
        /// </summary>
        /// <param name="Usuario">Datos de sesion del usuario</param>
        /// <param name="operacion">Tipo de validacion</param>
        /// <returns></returns>
        public static DataTable ValidarAccesoUsuarios(Login Usuario, ConexionesExcel Conexion, string operacion)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                string IdUsuario = Usuario.IdUsuario;
                string IdEmpresa = Usuario.EmpresaId;
                string IdSucursal = Usuario.SucId;

                wsOfficeSGD Ws = new wsOfficeSGD();
                Ws.Url = Conexion.urlWsOfficeSGD;
                Ws.Timeout = Conexion.TimeOut;

                XmlNode Nodo = Ws.ValidarAccesoUsuarios(SessionID, operacion, IdUsuario, IdEmpresa, IdSucursal);
                Ws.Dispose();

                DataTable TablaTemporal = XML.XMLtoDataTable(Nodo);

                return TablaTemporal;
            }
            catch (Exception Exc)
            {
                DataTable DT = new DataTable();
                DT.TableName = Exc.ToString();
                return DT;
            }
        }

        public static XmlNode ResponsablesPasosCorrespondencia(Login Usuario, ConexionesExcel Conexion, string tipo)
        {
            try
            {
                DataEncryption Enc = new DataEncryption();
                Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                string IdUsuario = Usuario.IdUsuario;
                string IdEmpresa = Usuario.EmpresaId;
                string IdSucursal = Usuario.SucId;

                wsOfficeSGD Ws = new wsOfficeSGD();
                Ws.Url = Conexion.urlWsOfficeSGD;
                Ws.Timeout = Conexion.TimeOut;

                XmlNode Nodo = Ws.PasosResponsablesConsultaTS(SessionID, tipo, int.Parse( Usuario.SucId ) );
                Ws.Dispose();

                //DataTable TablaTemporal = XML.XMLtoDataTable(Nodo);

                return Nodo;
            }
            catch (Exception Exc)
            {
                //DataTable DT = new DataTable();
                //DT.TableName = Exc.ToString();
                XmlDocument xml = new XmlDocument();
                return xml;
            }
        }
    }
}
