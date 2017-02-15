using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using AppExternas;

namespace SincoGeneradorLicencias
{
    class Licencias
    {

        #region Propiedades de Licencia
        public string Nombre { get; set; }
        public byte[] Archivo { get; set; }
        public Dictionary<string, string> Propiedades { get; set; }
        public string Key { get; set; }

        // llaves para crear / Consultar archivos de licencia
        private static Byte[] newKeyFile = { 17, 29, 23, 41, 52, 26, 31, 84, 63, 63, 95, 12, 10, 14, 15, 12, 64, 99, 38, 88, 99, 12, 3, 1 };
        private static Byte[] newIVFile = { 75, 22, 255, 110, 65, 201, 209, 154 };

        #endregion

        /// <summary>
        /// Retorna los valores de la licencia entregada
        /// </summary>
        /// <param name="Archivo"></param>
        /// <returns></returns>
        public static Licencias LeerLicencia(Byte[] Archivo)
        {
            try
            {
                Licencias Licencia = new Licencias();

                #region Desencriptar la informacion de la licencia
                XmlDocument XmlLicencia = new XmlDocument();

                string TextoXML = DataEncryption.Decryption(Archivo, newKeyFile, newIVFile);
                XmlLicencia.LoadXml(TextoXML);
                DataTable DatosLicencia = XML.XMLtoDataTable(XmlLicencia.DocumentElement);
                #endregion

                #region Cargar Informacion de Licencia

                Licencia.Propiedades = new Dictionary<string, string>();

                foreach (DataRow Fila in DatosLicencia.Rows)
                {
                    if (Fila["Propiedad"].ToString() == "Nombre")
                    { Licencia.Nombre = Fila["Valor"].ToString(); }
                    else if (Fila["Propiedad"].ToString() == "Key")
                    { Licencia.Key = Fila["Valor"].ToString(); }
                    else
                    { Licencia.Propiedades.Add(Fila["Propiedad"].ToString(), Fila["Valor"].ToString()); }
                }
                Licencia.Archivo = Archivo;
                #endregion

                return Licencia;
            }
            catch
            { return new Licencias(); }
        }


        /// <summary>
        /// Crea un nuevo archivo de licencia
        /// </summary>
        /// <param name="NuevaLicencia"></param>
        /// <returns></returns>
        public static Byte[] CrearArchivoLicencia(Licencias NuevaLicencia)
        {
            try
            {
                #region Crear XML con información de la licencia
                DataTable DatosLicencia = new DataTable();
                DatosLicencia.Columns.Add("Propiedad", typeof(string));
                DatosLicencia.Columns.Add("Valor", typeof(string));

                if (NuevaLicencia.Propiedades != null)
                {
                    foreach (KeyValuePair<string, string> Dict in NuevaLicencia.Propiedades)
                    {
                        if (!string.IsNullOrEmpty(Dict.Key) && !string.IsNullOrEmpty(Dict.Value))
                        {
                            DatosLicencia.Rows.Add(Dict.Key, Dict.Value);
                        }
                    }
                }

                DatosLicencia.Rows.Add("Nombre", NuevaLicencia.Nombre);
                DatosLicencia.Rows.Add("Key", NuevaLicencia.Key);

                XmlDocument Nodo = XML.FormatearDataTable(DatosLicencia, "Propiedades", "Dato");
                #endregion

                #region Encriptar informacion de licencia
                Byte[] archivo = DataEncryption.Encryption(Nodo.InnerXml, newKeyFile, newIVFile);
                #endregion

                return archivo;
            }
            catch
            {
                return new Byte[0];
            }
        }



    }
}
