using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Timers;
using System.Xml;
using System.IO;

using SincoOfficeLibrerias.wsOfficeSGD;
using System.Web.Services.Protocols;
using SincoOfficeLibrerias;
using AppExternas;


namespace SincoOfficeLibrerias
{
    public class Plantillas
    {
        public Plantillas()
        {


        }

        /// <summary>
        /// Devuelve Matriz con tipos de elementos diponibles para creación.
        /// </summary>
        /// <returns></returns>
        public static string[] ConsultarTiposElementos()
        {
            string[] TiposElementos = Controles.TiposElementos.Split(':');
            //TiposElementos[0] = "Ninguno";
            return TiposElementos;
        }
        
        /// <summary>
        /// devuelve informacion de descriptores de SGD
        /// </summary>
        /// <param name="Usuario">Datos del usuario de sesión.</param>
        /// <param name="Conexion">Datos de Conexion del usuario</param>
        /// <param name="operacion">ConsultarDescriptoresCategoria - ConsultarCategorias - ConsultarInformacionDescriptores</param>
        /// <param name="categoria">Categoria del descriptor (ConsultarDescriptoresCategoria - ConsultarCategorias) o Id descriptor (ConsultarInformacionDescriptores)</param>
        /// <returns></returns>
        public static DataTable ConsultaDescriptores(Login Usuario, ConexionesExcel Conexion, string operacion, string categoria)
        {
            DataTable DT = new DataTable();
            if (!string.IsNullOrEmpty(Usuario.IdUsuario) && !string.IsNullOrEmpty(Usuario.CadenaConexion))
            {
                try
                {
                    DataEncryption Enc = new DataEncryption();
                    Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                    wsOfficeSGD.wsOfficeSGD Sw = new wsOfficeSGD.wsOfficeSGD();
                    Sw.Url = Conexion.urlWsOfficeSGD;
                    Sw.Timeout = Conexion.TimeOut;

                    XmlNode XmlNode1 = Sw.ConsultaDescriptoresCategoria(SessionID, operacion, categoria);

                    Sw.Dispose();

                    DataTable Descriptores = XML.XMLtoDataTable(XmlNode1);

                    return Descriptores;
                }
                catch(Exception Exc)
                {
                    DT = new DataTable();
                    DT.TableName = Exc.ToString();
                    return DT;
                }
            }
            else
            {
                DT = new DataTable();
                DT.TableName = "El usuario no es válido";
                return DT;
            }
        }

        /// <summary>
        /// Devuelve información inicial para el complemento
        /// </summary>
        /// <param name="Usuario">Datos de Login de usuario</param>
        /// <param name="Conexion">Datos de conexion de usuario</param>
        /// <param name="operacion">Depende de BD, procedimiento: [dbo].[FGR_DatosPlantillasOffice] </param>
        /// <param name="categoria"></param>
        /// <returns></returns>
        public static DataSet ConsultaInformacionInicial(Login Usuario, ConexionesExcel Conexion, string operacion, string categoria)
        {
            DataSet DT = new DataSet();
            if (!string.IsNullOrEmpty(Usuario.IdUsuario) && !string.IsNullOrEmpty(Usuario.CadenaConexion))
            {
                try
                {
                    DataEncryption Enc = new DataEncryption();
                    Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                    wsOfficeSGD.wsOfficeSGD Sw = new wsOfficeSGD.wsOfficeSGD();
                    
                    Sw.Url = Conexion.urlWsOfficeSGD;
                    Sw.Timeout = Conexion.TimeOut;

                    DataSet Ds = Sw.ConsultaInformacionInicial(SessionID, operacion, categoria);

                    Sw.Dispose();
                    return Ds;
                }
                catch
                {
                    DT = new DataSet();
                    return DT;
                }
            }
            else
            {
                DT = new DataSet();
                return DT;
            }
        }

        /// <summary>
        /// Devuelve la Información asociada por descriptor. (Fuente de datos de combos por descriptor)
        /// </summary>
        /// <param name="Usuario">Datos de usuario de sesion</param>
        /// <param name="Conexion">Datos de conexion de usuario</param>
        /// <param name="Descriptor">Id Descriptor</param>
        /// <param name="Variables">Variables requeridas para consulta de información, tener en cuenta precedencias de descriptores. Formato:  @var1:Valor1,@var2:Valor2.......</param>
        /// <param name="Busqueda">Criterio de búsqueda</param>
        /// <returns></returns>
        public static DataTable FiltroConsultaDescriptor(Login Usuario, ConexionesExcel Conexion, string Descriptor, string Variables, string Busqueda)
        {
            DataTable DT = new DataTable();

            if (!string.IsNullOrEmpty(Usuario.IdUsuario) && !string.IsNullOrEmpty(Usuario.CadenaConexion))
            {
                try
                {
                    DataEncryption Enc = new DataEncryption();
                    Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                    wsOfficeSGD.wsOfficeSGD Sw = new wsOfficeSGD.wsOfficeSGD();
                    Sw.Url = Conexion.urlWsOfficeSGD;
                    Sw.Timeout = Conexion.TimeOut;

                    XmlNode XmlNode = Sw.FuenteDatosDescriptores(SessionID, Descriptor, Variables, Busqueda);
                    
                    Sw.Dispose();


                    DataTable Descriptores = XML.XMLtoDataTable(XmlNode);

                    return Descriptores;
                }
                catch (Exception Exc)
                {
                    DT = new DataTable();
                    DT.TableName = Exc.ToString();
                    return DT;
                }
            }
            else
            {
                DT = new DataTable();
                return DT;
            }
        }

        /// <summary>
        /// Guarda el formato en el sistema de Gestión de calidad (Nuevo Formato) o el Sistema de gestión documental (Nuevo Registro)
        /// </summary>
        /// <param name="Usuario">Datos de usuario de sesión</param>
        /// <param name="Conexion">Datos de conexion de usuario</param>
        /// <param name="operacion">depende de Procedimiento en BD: [SGD].[FGR_GuardarFormato_SGC_SGD]</param>
        /// <param name="nombreArchivo"></param>
        /// <param name="NombreTipologia"></param>
        /// <param name="RutaArchivo">ruta de ubicación del archivo</param>
        /// <param name="ArchivoBinario">Arreglo de Byte[] con la información del archivo</param>
        /// <param name="SFVid">Sub formato version ID</param>
        /// <param name="SubSerie">Sub serie de SGD</param>
        /// <param name="XMLelementos">XML.innerXML de los elementos (Controles) asociados al archivo</param>
        /// <returns></returns>
        public static DataTable GuardarFormatoPlantilla(Login Usuario, ConexionesExcel Conexion, string operacion, string nombreArchivo,
                string NombreTipologia, string RutaArchivo, Byte[] ArchivoBinario, int SFVid, int SubSerie, string XMLelementos)
        {
            DataTable DT = new DataTable();

            if (!string.IsNullOrEmpty(Usuario.IdUsuario) && !string.IsNullOrEmpty(Usuario.CadenaConexion))
            {
                try
                {
                    DataEncryption Enc = new DataEncryption();
                    Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                    int UsuarioRegistro = int.Parse(Usuario.IdUsuario);
                    int IdEmpresa = int.Parse(Usuario.EmpresaId);

                    wsOfficeSGD.wsOfficeSGD Sw = new wsOfficeSGD.wsOfficeSGD();
                    Sw.Url = Conexion.urlWsOfficeSGD;
                    Sw.Timeout = Conexion.TimeOut;

                    XmlNode XmlNode = Sw.GuardarFormatoPlantilla(SessionID, operacion, nombreArchivo, UsuarioRegistro, NombreTipologia,
                                        IdEmpresa, RutaArchivo, ArchivoBinario, SFVid, SubSerie, XMLelementos);
                    Sw.Dispose();

                    DataTable Descriptores = XML.XMLtoDataTable(XmlNode);
                    return Descriptores;
                }
                catch (SoapException Error)
                {
                    DT = new DataTable();
                    DT.TableName = Error.ToString();
                    return DT;
                }
                catch (Exception Exc)
                {
                    DT = new DataTable();
                    DT.TableName = Exc.ToString();
                    return DT;
                }
            }
            else
            {
                DT = new DataTable();
                return DT;
            }
        }

        /// <summary>
        /// Guarda el formato en el sistema de Gestión de calidad (Nuevo Formato) o el Sistema de gestión documental (Nuevo Registro)
        /// </summary>
        /// <param name="Usuario">Datos de usuario de sesión</param>
        /// <param name="Conexion">Datos de conexion de usuario</param>
        /// <param name="operacion">depende de Procedimiento en BD: [SGD].[FGR_GuardarFormato_SGC_SGD]</param>
        /// <param name="nombreArchivo"></param>
        /// <param name="NombreTipologia"></param>
        /// <param name="RutaArchivo">ruta de ubicación del archivo</param>
        /// <param name="ArchivoBinario">Arreglo de Byte[] con la información del archivo</param>
        /// <param name="SFVid">Sub formato version ID</param>
        /// <param name="SubSerie">Sub serie de SGD</param>
        /// <param name="XMLelementos">XML.innerXML de los elementos (Controles) asociados al archivo</param>
        /// <returns></returns>
        public static DataTable GuardarFormatoPlantilla(Login Usuario, ConexionesExcel Conexion, string operacion, string nombreArchivo,
                string NombreTipologia, string RutaArchivo, Byte[] ArchivoBinario, int SFVid, int SubSerie, string XMLelementos, string XmlPasosUsuario)
        {
            DataTable DT = new DataTable();

            if (!string.IsNullOrEmpty(Usuario.IdUsuario) && !string.IsNullOrEmpty(Usuario.CadenaConexion))
            {
                try
                {
                    DataEncryption Enc = new DataEncryption();
                    Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                    int UsuarioRegistro = int.Parse(Usuario.IdUsuario);
                    int IdEmpresa = int.Parse(Usuario.EmpresaId);

                    wsOfficeSGD.wsOfficeSGD Sw = new wsOfficeSGD.wsOfficeSGD();
                    Sw.Url = Conexion.urlWsOfficeSGD;
                    Sw.Timeout = Conexion.TimeOut;

                    XmlNode XmlNode = Sw.GuardarFormatoPlantillaDiligenciados(SessionID, operacion, nombreArchivo, UsuarioRegistro, NombreTipologia,
                                        IdEmpresa, RutaArchivo, ArchivoBinario, SFVid, SubSerie, XMLelementos, Usuario.SucId, XmlPasosUsuario);
                    Sw.Dispose();

                    DataTable Descriptores = XML.XMLtoDataTable(XmlNode);
                    return Descriptores;
                }
                catch (SoapException Error)
                {
                    DT = new DataTable();
                    DT.TableName = Error.ToString();
                    return DT;
                }
                catch (Exception Exc)
                {
                    DT = new DataTable();
                    DT.TableName = Exc.ToString();
                    return DT;
                }
            }
            else
            {
                DT = new DataTable();
                return DT;
            }
        }
    }
}
