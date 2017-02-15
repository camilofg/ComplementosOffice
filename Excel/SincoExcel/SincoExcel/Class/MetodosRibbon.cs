using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using ExcelTools = Microsoft.Office.Tools.Excel;

using SincoOfficeLibrerias.wsOfficeSGD;
using SincoOfficeLibrerias.wsSGCdocumentos;
using SincoOfficeLibrerias;
using System.Drawing;
using System.Data;
using System.IO;
using System.Xml;
using AppExternas;
using System.Windows.Forms;
using System.Xml.Linq;
using SincoExcel.Forms;

namespace SincoExcel
{
   public class MetodosRibbon
   {
      public enum ModoTrabajo
      {
         EdicionPlantilla,
         CompletarFormato,
         BloquearContenido,
         VerificarPlantilla
      }

      /// <summary>
      /// Consulta información inicial de complemento, información de descriptores
      /// </summary>
      /// <returns>Estado final de la operación</returns>
      public static bool ActualizarFuentesInformacionDescriptores()
      {
         bool resultado = false;

         try
         {
            DataSet ConsultaInformacionInicial = Plantillas.ConsultaInformacionInicial(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultaInformacionInicial", "");

            if (ConsultaInformacionInicial.Tables.Count > 2)
            {
               Globals.Ribbons.RibbonExcel.DatosCategorias = ConsultaInformacionInicial.Tables[0];
               Globals.Ribbons.RibbonExcel.DatosFuentesExternas = ConsultaInformacionInicial.Tables[1];
               Globals.Ribbons.RibbonExcel.DatosDescriptores = ConsultaInformacionInicial.Tables[2];

               resultado = true;
            }
         }
         catch
         {
            resultado = false;
         }

         return resultado;
      }

      /// <summary>
      /// Actualiza información de formatos de SGC
      /// </summary>
      /// <returns>Estado final de la operación de consulta</returns>
      public static bool ActualizarFuentesInformacionFormatos()
      {
         bool resultado = false;
         try
         {
            Globals.Ribbons.RibbonExcel.DatosFormatos = new DataTable();

            //Actualizar Información de formatos en registro
            DataTable ConsultaFormatos = SGCformatos.ConsultarFormatosEnRegistro(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarFormatosEnRegistro");

            if (ConsultaFormatos.Rows.Count > 0 && ConsultaFormatos.Columns.Count > 2)
            {
               Globals.Ribbons.RibbonExcel.DatosFormatos = ConsultaFormatos;
            }

            resultado = true;
         }
         catch
         {
            resultado = false;
         }

         return resultado;
      }

      /// <summary>
      /// Realiza el enlace de datos de un obj. Tipo Controles con informacion de BD o Fuentes Externas
      /// </summary>
      /// <param name="Control">Elemento </param>
      /// <param name="Busqueda"></param>
      public static void DataBindControl(Controles Control, string Busqueda)
      {
         try
         {
            ExcelTools.Worksheet HojaDatos = Control.HojaExcel;
            string GUIDControl = Control.GUID;
            string tipoControl = Control.Tipo;
            string IdControl = Control.Id;
            string NombreControl = Control.Nombre;

            if (!Globals.ThisAddIn.IsUpdating)
            {
               DataTable DatosFuente = new DataTable();

               // Se actualiza información desde BD siempre
               DataTable Descriptor = Plantillas.ConsultaDescriptores(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarInformacionDescriptores", IdControl);

               if (Descriptor.Rows.Count > 0 && Descriptor.Columns.Count > 2)
               {
                  #region Ajuste de Variables de Tabla - Filtra solo variables de cada fila
                  string variables = "@Empresa:" + Globals.ThisAddIn.DatosUsuario.EmpresaId;

                  if (Control.Tipo == Controles.ComboBusqueda)
                  {
                     List<Controles> ControlesCombo = new List<Controles>();

                     ControlesCombo = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c)
                     {
                        return (c.Tipo == Controles.ComboBusqueda
                                 && c.TablaNombre == Control.TablaNombre
                                 && c.TablaFila == Control.TablaFila);
                     });

                     foreach (Controles ControlVar in ControlesCombo)
                     {
                        string ValorCampo = string.Empty;

                        //Combos de dependencia
                        if (Control.Nombre.Contains('@'))
                        {
                           if (Control.Nombre.Split(':')[0] == ControlVar.Nombre.Split(':')[0])
                           {
                              ValorCampo = Controles.LeerValorControl(ControlVar);
                              ValorCampo = ValorCampo.Split('-')[0].Trim();

                              if (!string.IsNullOrEmpty(ValorCampo))
                              {
                                 variables = variables + "," + ControlVar.Nombre.Split(':')[1] + ":" + ValorCampo;
                              }
                           }
                        }
                        else
                        {
                           ValorCampo = Controles.LeerValorControl(ControlVar);
                           ValorCampo = ValorCampo.Split('-')[0].Trim();

                           if (!string.IsNullOrEmpty(ValorCampo))
                           {
                              variables = variables + "," + "@" + ControlVar.Nombre + ":" + ValorCampo;
                           }
                        }
                     }
                  }
                  #endregion

                  #region Actualizar fuente de datos
                  DataTable ResultadoBusqueda = new DataTable();

                  // Variables de Tabla
                  if (!string.IsNullOrEmpty(Descriptor.Rows[0]["DESfuenteExterna"].ToString()))
                  {
                     DatosFuente = Descriptores.ConsultarSubseries(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarElementosFuenteExterna", Descriptor.Rows[0]["DESfuenteExterna"].ToString());
                  }
                  else
                  {
                     DatosFuente = Plantillas.FiltroConsultaDescriptor(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, Descriptor.Rows[0]["DESid"].ToString(), variables, Busqueda);
                  }
                  #endregion

                  DesprotegerHoja(HojaDatos.InnerObject, Globals.ThisAddIn.ClaveProteccionHoja);
                  #region tipo de control
                  switch (tipoControl)
                  {
                     case Controles.ListaDesplegable:
                        List<string> Lista = new List<string>();

                        for (int i = 0; i < DatosFuente.Rows.Count; i++)
                        {
                           if (!string.IsNullOrEmpty(DatosFuente.Rows[i]["Codigo"].ToString()))
                           {
                              Lista.Add(DatosFuente.Rows[i]["Codigo"].ToString() + " - " + DatosFuente.Rows[i]["Descripcion"].ToString());
                           }
                           else
                           {
                              Lista.Add(DatosFuente.Rows[i]["Descripcion"].ToString());
                           }
                        }

                        string values = string.Join(";", Lista);
                        Excel.Range cell = Control.RangoDatos;
                        cell.Validation.Delete();
                        cell.Validation.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertStop, Excel.XlFormatConditionOperator.xlBetween, values, Type.Missing);
                        cell.Validation.IgnoreBlank = true;
                        cell.Validation.InputMessage = "Seleccione";
                        cell.Validation.ErrorMessage = "El valor no es correcto.\nPor favor seleccione un elemento de la lista.";
                        cell.Validation.ShowError = true;
                        cell.Locked = false;
                        break;

                     case Controles.ListBox:
                        Excel.ListBox ListBoxTemp = (Excel.ListBox)HojaDatos.ListBoxes(GUIDControl);
                        for (int i = 0; i < DatosFuente.Rows.Count; i++)
                        {
                           ListBoxTemp.AddItem(DatosFuente.Rows[i]["Codigo"].ToString() + " - " + DatosFuente.Rows[i]["Descripcion"].ToString(), i + 1);
                        }
                        break;

                     case Controles.ComboBusqueda:
                        List<string> ListaCombo = new List<string>();

                        for (int i = 0; i < DatosFuente.Rows.Count; i++)
                        {
                           if (!string.IsNullOrEmpty(DatosFuente.Rows[i]["Codigo"].ToString()))
                           {
                              ListaCombo.Add(DatosFuente.Rows[i]["Codigo"].ToString() + " - " + DatosFuente.Rows[i]["Descripcion"].ToString());
                           }
                           else
                           {
                              ListaCombo.Add(DatosFuente.Rows[i]["Descripcion"].ToString());
                           }
                        }

                        string ValoresCombo = string.Join(";", ListaCombo);
                        Excel.Range CellCombo = Control.RangoDatos.get_Offset(0, 1);
                        CellCombo.Locked = false;
                        CellCombo.Validation.Delete();

                        DesprotegerHoja(Control.HojaExcel.InnerObject, Globals.ThisAddIn.ClaveProteccionHoja);

                        CellCombo.Validation.Add(Excel.XlDVType.xlValidateList, Excel.XlDVAlertStyle.xlValidAlertStop, Excel.XlFormatConditionOperator.xlBetween, ValoresCombo, Type.Missing);
                        CellCombo.Validation.IgnoreBlank = true;
                        CellCombo.Validation.InputMessage = "Seleccione";
                        CellCombo.Validation.ErrorMessage = "El valor no es correcto.\nPor favor seleccione un elemento de la lista.";
                        CellCombo.Validation.ShowError = true;
                        break;
                  }
                  #endregion
                  ProtegerHoja(HojaDatos.InnerObject, Globals.ThisAddIn.ClaveProteccionHoja);
               }
            }
         }
         catch
         {

         }
      }

      /// <summary>
      /// Bloquea el contenido de la hoja, evita modificacionse de forma y contenido.
      /// </summary>
      /// <param name="Hoja">Hoja bloqueada</param>
      /// <param name="Pass">Clave de protección</param>
      public static void ProtegerHoja(Excel.Worksheet Hoja, string Pass)
      {
         // Para bloquear Imagenes ProtegerFormas = true
         bool ProtegerFormas = true;
         bool ProtegerContenido = true;
         bool Escenario = true;
         bool SoloInterfazUsuario = true;
         bool PermitirFormatoCeldas = false;
         bool PermitirFormatoColumna = false;
         bool PermitirFormatoFilas = false;
         bool PermitirInsertarColumnas = false;
         bool PermitirInsertarFilas = false;
         bool PermitirInsertarHipervinculos = false;
         bool PermitirBorrasColumnas = false;
         bool PermitirBorrasFilas = false;
         bool PermitirOrdenar = true;
         bool PermitirFiltros = true;
         bool PermitirTablasDinamicas = true;

         Hoja.Protect(Pass,
             ProtegerFormas,
             ProtegerContenido,
             Escenario,
             SoloInterfazUsuario,
             PermitirFormatoCeldas,
             PermitirFormatoColumna,
             PermitirFormatoFilas,
             PermitirInsertarColumnas,
             PermitirInsertarFilas,
             PermitirInsertarHipervinculos,
             PermitirBorrasColumnas,
             PermitirBorrasFilas,
             PermitirOrdenar,
             PermitirFiltros,
             PermitirTablasDinamicas);
      }

      /// <summary>
      /// Quita la protección de la hoja
      /// </summary>
      /// <param name="Hoja"></param>
      /// <param name="Pass"></param>
      public static void DesprotegerHoja(Excel.Worksheet Hoja, string Pass)
      {
         Hoja.Unprotect(Pass);
      }

      /// <summary>
      /// Realiza las validaciones previas para guardar plantillas
      /// </summary>
      /// <returns> estado final de la validación</returns>
      public static bool ValidarInformacionGuardarPlantilla()
      {
         bool Resultado = false;
         try
         {
            string DescriptoresFaltantes = string.Empty;
            Excel.Worksheet HojaActiva = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;

            foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
            {
               bool ValidacionDatos = false;

               try
               { 
                   //Si no esta bloqueado por WorkFlow En correspondencia SGD
                   if (!Control.Locked)
                   {
                       ValidacionDatos = ValidarInformacion(Control);
                   }
                   else
                   {
                       ValidacionDatos = true;
                   }
               }
               catch
               { ValidacionDatos = false; }

               if (!ValidacionDatos)
               {
                  DescriptoresFaltantes = DescriptoresFaltantes + " - " + Control.Nombre + "  " + ((!string.IsNullOrEmpty(Control.TablaNombre)) ? "Tabla: " + Control.TablaNombre : "")
                      + "  " + ((!string.IsNullOrEmpty(Control.TablaFila)) ? "Fila: " + Control.TablaFila : "") + "\n";
               }
            }

            if (!string.IsNullOrEmpty(DescriptoresFaltantes))
            {
               MessageBox.Show(Properties.Settings.Default.MsErrorDescriptoresFaltantes + " \n" + DescriptoresFaltantes, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               Resultado = false;
            }
            else
            {
               Resultado = true;
            }
         }
         catch
         {
            Resultado = false;
         }

         return Resultado;
      }

      /// <summary>
      /// Devuelve DataTable con el contenido de los descriptores.
      /// </summary>
      /// <param name="HojaTrabajo"></param>
      /// <returns></returns>
      public static DataTable TextoDescriptores(Excel.Worksheet HojaTrabajo)
      {
         try
         {
            DataTable TextoDescriptores = new DataTable();
            TextoDescriptores.Columns.Add("IdDescriptor", typeof(string));
            TextoDescriptores.Columns.Add("ValorDescriptor", typeof(string));
            TextoDescriptores.Columns.Add("ValorMostrarDescriptor", typeof(string));

            foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
            {
               try
               {
                  if (!Control.BloqueadoWorkFlow)
                  {
                     string ValorDescriptor = Controles.LeerValorControl(Control);
                     if (ValorDescriptor.Split('-').Length > 1)
                     {
                        if (!string.IsNullOrEmpty(ValorDescriptor.Split('-')[0].Trim()))
                        {
                           TextoDescriptores.Rows.Add(Control.Id, ValorDescriptor.Split('-')[0].Trim(), ValorDescriptor.Split('-')[1].Trim());
                        }
                     }
                     else
                     {
                        if (!string.IsNullOrEmpty(ValorDescriptor.Trim()))
                        {
                           TextoDescriptores.Rows.Add(Control.Id, ValorDescriptor.Trim(), ValorDescriptor.Trim());
                        }
                     }
                  }
               }
               catch
               {

               }
            }
            return TextoDescriptores;
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
            return new DataTable();
         }

      }

      /// <summary>
      /// Guarda el formato en SGC,  y lo crea como tipología en SGD.
      /// </summary>
      /// <returns>estado final de la operación</returns>
      public static bool GuardarFormatoPlantilla()
      {
         bool Resultado = false;
         try
         {
            #region guardar Plantilla
            string RutaLocal = RegistroWindows.ConsultarEntradaRegistro("Ruta", "Temporales");
            string Mensajevalidacion = string.Empty;

            if (!string.IsNullOrEmpty(RutaLocal))
            {
               string RutaOriginal = RutaLocal;

               string Filtro = " SFVid = " + Globals.ThisAddIn.IdPlantillaFormato;
               DataView FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatos, Filtro, "", DataViewRowState.CurrentRows);

               if (FiltroFormato.Count > 0)
               {
                  Excel.Workbook LibroActual = (Excel.Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;
                  Excel.Worksheet HojaActual = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
                  ExcelTools.Worksheet HojaActualExtended = (ExcelTools.Worksheet)Globals.Factory.GetVstoObject(HojaActual);

                  #region Crear el archivo en una ubicación temporal
                  string NombreArchivoExcel = Globals.ThisAddIn.IdPlantillaFormato + "_FOR_" + FiltroFormato.ToTable().Rows[0]["SPFdescripcion"].ToString();
                  NombreArchivoExcel = ValidacionesDatos.DarFormatoNombreArchivoSGC(NombreArchivoExcel);
                  NombreArchivoExcel = LimpiarFileName(NombreArchivoExcel) + Globals.Ribbons.RibbonExcel.ExtensionExcel;

                  RutaLocal = RutaLocal + NombreArchivoExcel;

                  bool IsFileCreated = false;
                  string GUIDtemporal = Guid.NewGuid().ToString();

                  GUIDtemporal = RutaOriginal + GUIDtemporal;

                  GUIDtemporal = LimpiarFileName(GUIDtemporal);

                  try
                  {
                     LibroActual.SaveCopyAs(GUIDtemporal);
                     IsFileCreated = true;
                  }
                  catch
                  {
                     IsFileCreated = false;
                  }
                  #endregion

                  if (IsFileCreated)
                  {
                     FileStream objfilestream = new FileStream(GUIDtemporal, FileMode.Open, FileAccess.Read);
                     int len = (int)objfilestream.Length;
                     Byte[] ArchivoBin = new Byte[len];
                     objfilestream.Read(ArchivoBin, 0, len);
                     objfilestream.Close();

                     if (ArchivoBin.Length > 0)
                     {
                        bool IsSaveFileServer = false;
                        string RutaServer = string.Empty;

                        #region Guardar Archivo Fisico en SGC
                        DataTable ConsultarConfiguracion = SGCformatos.ConsultarConfiguracionISO(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarConfiguracionISO");
                        string FiltroRutas = " CFGcodigo  = 'RUTA_FORMATOS' ";

                        DataView DvFiltro = new DataView(ConsultarConfiguracion, FiltroRutas, "", DataViewRowState.CurrentRows);

                        if (DvFiltro.Count > 0)
                        {
                           RutaServer = DvFiltro.ToTable().Rows[0]["CFGvalorTexto"].ToString() + NombreArchivoExcel;
                        }

                        if (!string.IsNullOrEmpty(RutaServer))
                        {
                           string ResultadoGuardar = SGCformatos.GuardarArchivosFormatos(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, RutaServer, ArchivoBin);

                           if (ResultadoGuardar.Split(':')[0] == "0")
                           {
                              Mensajevalidacion = Mensajevalidacion + "No se pudo guardar el archivo en el servidor.\n";
                              IsSaveFileServer = false;
                           }
                           else
                           {
                              IsSaveFileServer = true;
                           }
                        }
                        #endregion

                        // Si el archivo se guardó correctamente
                        if (IsSaveFileServer)
                        {
                           #region Guardar Binario en base de Datos
                           string NombreArchivo = NombreArchivoExcel;
                           string IdTipologia = FiltroFormato.ToTable().Rows[0]["SFVtipologiaDocumental"].ToString();

                           if (string.IsNullOrEmpty(IdTipologia))
                           {  IdTipologia = "-1";  }

                           #region Calcular XML con los datos de controles
                           List<string> Addins = new List<string>();
                           XElement InfoDescriptores = new XElement("Descriptores");
                           XElement InfoDescriptor = new XElement("Descriptor");
                           XElement InfoPropiedades = new XElement("Propiedades");

                           int NumeroResultado = 0;

                           foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                           {
                              if (!Addins.Contains(Control.Id))
                              {
                                 List<Controles> Filtrados = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c) { return c.Id == Control.Id; });
                                 if (Filtrados.Count > 0)
                                 {
                                    NumeroResultado = 0;
                                    InfoPropiedades = new XElement("Propiedades");
                                    InfoDescriptor = new XElement("Descriptor");
                                    InfoDescriptor.SetAttributeValue("id", Control.Id);
                                    InfoDescriptor.SetAttributeValue("obligatorio", Control.Obligatorio);
                                    InfoDescriptor.SetAttributeValue("principal", Control.Principal);

                                    if (Filtrados.Count == 1)
                                    {   InfoDescriptor.SetAttributeValue("multiple", 0);    }
                                    else
                                    {   InfoDescriptor.SetAttributeValue("multiple", 1);    }


                                    foreach (Controles CT  in Filtrados)
                                    {
                                       #region Recopilar datos de la instancia
                                       XElement Datos = new XElement("Instancia");

                                       XElement P_GUID = new XElement("dato", new XElement("nombre", "GUID"), new XElement("valor", CT.GUID.ToString()));
                                       XElement P_TipoControl = new XElement("dato", new XElement("nombre", "TipoControl"), new XElement("valor", CT.Tipo.ToString()));
                                       XElement P_NombreElemento = new XElement("dato", new XElement("nombre", "NombreElemento"), new XElement("valor", CT.Nombre.ToString()));
                                       XElement P_RangoDatos = new XElement("dato", new XElement("nombre", "RangoDatos"), new XElement("valor", CT.RangoDatos.Address.ToString()));
                                       XElement P_Orientacion = new XElement("dato", new XElement("nombre", "Orientacion"), new XElement("valor", CT.Orientacion.ToString()));
                                       XElement P_Obligatorio = new XElement("dato", new XElement("nombre", "Obligatorio"), new XElement("valor",(CT.Principal) ? CT.Principal.ToString() : CT.Obligatorio.ToString()));
                                       XElement P_TipoValidacion = new XElement("dato", new XElement("nombre", "TipoValidacion"), new XElement("valor", CT.TipoValidacion.ToString()));
                                       XElement P_Principal = new XElement("dato", new XElement("nombre", "Principal"), new XElement("valor", CT.Principal.ToString()));
                                       XElement P_Propiedades = new XElement("dato", new XElement("nombre", "Propiedades"), new XElement("valor", (CT.Propiedades == null) ? "null" : CT.Propiedades));
                                       XElement P_Width = new XElement("dato", new XElement("nombre", "Width"), new XElement("valor", CT.Width.ToString()));
                                       XElement P_Height = new XElement("dato", new XElement("nombre", "Height"), new XElement("valor", CT.Height.ToString()));
                                       XElement P_Top = new XElement("dato", new XElement("nombre", "Top"), new XElement("valor", CT.Top.ToString()));
                                       XElement P_Left = new XElement("dato", new XElement("nombre", "Left"), new XElement("valor", CT.Left.ToString()));
                                       XElement P_Locked = new XElement("dato", new XElement("nombre", "Locked"), new XElement("valor", CT.Locked.ToString()));
                                       XElement P_IdFormato = new XElement("dato", new XElement("nombre", "IdFormato"), new XElement("valor", CT.IdFormato.ToString()));
                                       XElement P_IdSubSerie = new XElement("dato", new XElement("nombre", "IdSubSerie"), new XElement("valor", CT.IdSubSerie.ToString()));
                                       XElement P_HojaExcel = new XElement("dato", new XElement("nombre", "HojaExcel"), new XElement("valor", (CT.HojaExcel == null) ? "null" : CT.HojaExcel.Name.ToString()));
                                       XElement P_LibroExcel = new XElement("dato", new XElement("nombre", "LibroExcel"), new XElement("valor", (CT.LibroExcel == null ) ? "null" : CT.LibroExcel.Name.ToString()));
                                       XElement P_TablaNombre = new XElement("dato", new XElement("nombre", "TablaNombre"), new XElement("valor", (CT.TablaNombre == null) ? "null" :CT.TablaNombre.ToString()));
                                       XElement P_TablaFila = new XElement("dato", new XElement("nombre", "TablaFila"), new XElement("valor", (CT.TablaFila == null) ? "null" : CT.TablaFila.ToString()));
                                       XElement P_TablaRangoInicial = new XElement("dato", new XElement("nombre", "TablaRangoInicial"), new XElement("valor", (CT.TablaRangoInicial == null) ? "null" : CT.TablaRangoInicial.Address.ToString()));
                                       XElement P_TablaNumeroMaximoRegistros = new XElement("dato", new XElement("nombre", "TablaNumeroMaximoRegistros"), new XElement("valor", (CT.TablaNumeroMaximoRegistros == null) ? "null" : CT.TablaNumeroMaximoRegistros.ToString()));

                                       Datos.Add(P_GUID);
                                       Datos.Add(P_TipoControl);
                                       Datos.Add(P_NombreElemento);
                                       Datos.Add(P_RangoDatos);
                                       Datos.Add(P_Orientacion);
                                       Datos.Add(P_Obligatorio);
                                       Datos.Add(P_TipoValidacion);
                                       Datos.Add(P_Principal);
                                       Datos.Add(P_Propiedades);
                                       Datos.Add(P_Width);
                                       Datos.Add(P_Height);
                                       Datos.Add(P_Top);
                                       Datos.Add(P_Left);
                                       Datos.Add(P_Locked);
                                       Datos.Add(P_IdFormato);
                                       Datos.Add(P_IdSubSerie);
                                       Datos.Add(P_HojaExcel);
                                       Datos.Add(P_LibroExcel);
                                       Datos.Add(P_TablaNombre);
                                       Datos.Add(P_TablaFila);
                                       Datos.Add(P_TablaRangoInicial);
                                       Datos.Add(P_TablaNumeroMaximoRegistros);
                                       #endregion

                                       Datos.SetAttributeValue("codigo", NumeroResultado.ToString());
                                       InfoPropiedades.Add(Datos);

                                       NumeroResultado++;
                                    }
                                 }

                                 InfoDescriptor.Add(InfoPropiedades);
                                 InfoDescriptores.Add(InfoDescriptor);
                                 Addins.Add(Control.Id);
                              }
                           }

                           StringBuilder sb = new StringBuilder();
                           XmlWriterSettings xws = new XmlWriterSettings();
                           xws.OmitXmlDeclaration = true;
                           xws.Indent = false;

                           using (XmlWriter xw = XmlWriter.Create(sb, xws))
                           {
                              InfoDescriptores.WriteTo(xw);
                           }

                           #endregion

                           string XMLelementos = sb.ToString();

                           DataTable ResultadoGuardar = Plantillas.GuardarFormatoPlantilla(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "GuardarDocumentoFormato",
                                                           NombreArchivo, IdTipologia, RutaServer, ArchivoBin, int.Parse(Globals.ThisAddIn.IdPlantillaFormato),
                                                               int.Parse(Globals.ThisAddIn.IdSubSeriePlantillaFormato), XMLelementos);

                           if (ResultadoGuardar.Rows.Count > 0)
                           {
                              string Descripcion = ResultadoGuardar.Rows[0]["Descripcion"].ToString();
                              if (ResultadoGuardar.Rows[0]["Resultado"].ToString() != "1")
                              {
                                 Mensajevalidacion = Mensajevalidacion + "No se guardó el formato:\n\n " + Descripcion;
                              }
                              else
                              {
                                 // Globals.Ribbons.RibbonExcel.BtnGuardarPlantilla.Visible = false;
                                 MessageBox.Show(Properties.Settings.Default.MsGuardarFormatoCorrecto, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                 Resultado = true;
                              }
                           }
                           else
                           {
                              Mensajevalidacion = Mensajevalidacion + "No se guardó el formato. \n";
                           }
                           #endregion
                        }
                     }
                     else
                     {
                        Mensajevalidacion = Mensajevalidacion + "Se produjo un error al guardar el archivo.\n";
                     }
                  }
                  else
                  {
                     Mensajevalidacion = Mensajevalidacion + "No se pudo crear el archivo temporal en la ubicación: .\n" + GUIDtemporal;
                  }
               }
               else
               {
                  Mensajevalidacion = Mensajevalidacion + "La plantilla no es válida.\n";
               }
            }
            else
            {
               Mensajevalidacion = Mensajevalidacion + "Por Favor Configure una ruta temporal para la aplicación.\n";
            }


            if (!string.IsNullOrEmpty(Mensajevalidacion))
            {
               MessageBox.Show(Mensajevalidacion, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               Resultado = false;
            }
            else
            {
               Resultado = true;
            }

            #endregion
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorGuardarFormato, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Resultado = false;
         }
         return Resultado;
      }

      /// <summary>
      /// Guarda el formato diligenciado en SGD
      /// </summary>
      /// <returns>Estado final de la operación</returns>
      public static bool GuardarFormatoDiligenciado(string XmlPasosUsuario)
      {
         bool resultado = false;
         try
         {
            string Mensajevalidacion = string.Empty;

            #region Guardar Formato Diligenciado
            string RutaLocal = RegistroWindows.ConsultarEntradaRegistro("Ruta", "Temporales");

            if (!string.IsNullOrEmpty(RutaLocal) && !string.IsNullOrEmpty(Globals.ThisAddIn.IdPlantillaFormato))
            {
               string Filtro = " SFVid = " + Globals.ThisAddIn.IdPlantillaFormato;
               DataView FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatosVigentes, Filtro, "", DataViewRowState.CurrentRows);

               if (FiltroFormato.Count > 0)
               {
                  Excel.Workbook LibroActual = (Excel.Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;
                  Excel.Worksheet HojaActual = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;

                  ProtegerHoja(HojaActual, Globals.ThisAddIn.ClaveProteccionHoja);

                  List<Controles> Cont = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c) { return c.Principal == true; });

                  if (Cont.Count > 0)
                  {
                     string ValorDescriptorPrincipal = Controles.LeerValorControl(Cont[0]);

                     string NombreArchivoExcel = FiltroFormato.ToTable().Rows[0]["SPFdescripcion"].ToString() + " - " + ValorDescriptorPrincipal + Globals.Ribbons.RibbonExcel.ExtensionExcel;

                     NombreArchivoExcel = LimpiarFileName(NombreArchivoExcel);

                     RutaLocal = RutaLocal + NombreArchivoExcel;

                     bool IsFileCreated = false;

                     string TextoGUID = Guid.NewGuid().ToString().Replace("-", "");

                     #region Crear el archivo en una ubicación temporal
                     try
                     {
                        LibroActual.SaveCopyAs(RutaLocal + TextoGUID);
                        IsFileCreated = true;
                     }
                     catch
                     {
                        IsFileCreated = false;
                     }
                     #endregion

                     if (IsFileCreated)
                     {
                        FileStream objfilestream = new FileStream(RutaLocal + TextoGUID, FileMode.Open, FileAccess.Read);
                        int len = (int)objfilestream.Length;
                        Byte[] ArchivoBin = new Byte[len];
                        objfilestream.Read(ArchivoBin, 0, len);
                        objfilestream.Close();

                        if (ArchivoBin.Length > 0)
                        {
                           bool IsSaveFileServer = false;
                           string RutaServer = string.Empty;

                           #region Definir responsables de correspondencia

                           #endregion

                           #region Guardar Archivo Fisico en SGD

                           DataTable ConsultarConfiguracion = SGCformatos.ConsultarConfiguracionISO(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarConfiguracionSGD");
                           string FiltroRutas = " CFGcodigo  = 'RUTA_IMAGENES_LINEA' ";

                           DataView DvFiltro = new DataView(ConsultarConfiguracion, FiltroRutas, "", DataViewRowState.CurrentRows);

                           string GUIDArchivo = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

                           if (DvFiltro.Count > 0)
                           {
                              RutaServer = DvFiltro.ToTable().Rows[0]["CFGvalorTexto"].ToString() + GUIDArchivo + NombreArchivoExcel;
                           }

                           if (!string.IsNullOrEmpty(RutaServer))
                           {
                              string ResultadoGuardar = SGCformatos.GuardarArchivosFormatosSGD(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, RutaServer, ArchivoBin);
                              if (ResultadoGuardar.Split(':')[0] == "0")
                              {
                                 Mensajevalidacion = Mensajevalidacion + "No se pudo guardar el archivo en el servidor.\n";
                                 IsSaveFileServer = false;
                              }
                              else
                              {
                                 IsSaveFileServer = true;
                              }
                           }
                           #endregion

                           // Si el archivo se guardó correctamente
                           if (IsSaveFileServer)
                           {
                              bool IsInfoFileSave = false;

                              #region Guardar Binario en base de Datos
                              string NombreArchivo = NombreArchivoExcel;
                              //Este campo corresponde al ID de la Tipología
                              string IdTipologia = FiltroFormato.ToTable().Rows[0]["SFVtipologiaDocumental"].ToString();
                              if (string.IsNullOrEmpty(IdTipologia))
                              {
                                 IdTipologia = "-1";
                              }

                              string XMLTextoDescriptores = string.Empty;
                              XMLTextoDescriptores = XML.FormatearDataTable(TextoDescriptores(HojaActual), "Descriptores", "Dato").InnerXml;

                              string IdSubserie = Globals.ThisAddIn.IdSubSeriePlantillaFormato;
                              if (string.IsNullOrEmpty(IdSubserie))
                              {
                                 IdSubserie = "-1";
                              }

                              DataTable ResultadoGuardar = Plantillas.GuardarFormatoPlantilla(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "GuardarFormatoDiligenciado",
                                                              NombreArchivo, IdTipologia, RutaServer, ArchivoBin, int.Parse(Globals.ThisAddIn.IdPlantillaFormato),
                                                              int.Parse(IdSubserie), XMLTextoDescriptores, XmlPasosUsuario);

                              if (ResultadoGuardar.Rows.Count > 0)
                              {
                                 string Resultado = ResultadoGuardar.Rows[0]["Resultado"].ToString();
                                 string Descripcion = ResultadoGuardar.Rows[0]["Descripcion"].ToString();

                                 if (Resultado != "1")
                                 {
                                    Mensajevalidacion = Mensajevalidacion + "registro de Archivo: " + Descripcion;
                                    IsInfoFileSave = false;
                                 }
                                 else
                                 {
                                    IsInfoFileSave = true;
                                 }
                              }
                              #endregion

                              if (IsInfoFileSave)
                              {
                                 MessageBox.Show(Properties.Settings.Default.MsGuardarFormatoCorrecto, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                 Globals.Ribbons.RibbonExcel.ModoTrabajo = ModoTrabajo.BloquearContenido;
                                 Globals.Ribbons.RibbonExcel.BtnGuardarPlantilla.Visible = false;
                              }
                              else
                              {
                                 MessageBox.Show(Properties.Settings.Default.MsErrorGuardarFormato, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                 Globals.Ribbons.RibbonExcel.BtnGuardarPlantilla.Visible = true;
                              }
                           }
                        }
                        else
                        {
                           Mensajevalidacion = Mensajevalidacion + "Se produjo un error al guardar el archivo.\n";
                        }
                     }
                     else
                     {
                        Mensajevalidacion = Mensajevalidacion + "No se pudo crear el archivo temporal.\n";
                     }
                  }
                  else
                  {
                     Mensajevalidacion = Mensajevalidacion + "El formato debe tener asignado un descriptor Principal.\n";
                  }
               }
               else
               {
                  Mensajevalidacion = Mensajevalidacion + "La plantilla no es válida.\n";
               }
            }
            else
            {
               Mensajevalidacion = Mensajevalidacion + "Por Favor Configure una ruta temporal para la aplicación.\n";
            }

            if (!string.IsNullOrEmpty(Mensajevalidacion))
            {
               MessageBox.Show(Mensajevalidacion, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               resultado = false;
            }
            else
            {
               resultado = true;
            }
            #endregion
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
         return resultado;
      }

      /// <summary>
      /// Elimina los controles uicados en una hoja de excel
      /// </summary>
      /// <param name="ConservarResultados">indica si se debe mantener el texto de los controles</param>
      /// <returns>Estado final de la operacion</returns>
      public static bool LimpiarControlesHoja(bool ConservarResultados)
      {
         try
         {
            Globals.ThisAddIn.Application.ScreenUpdating = false;
            bool Resultado = true;

            foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
            {
               bool ResTemp = Controles.EliminarControl(Control, ConservarResultados, false);
               if (!ResTemp)
               { Resultado = false; }
            }
            Globals.ThisAddIn.Application.ScreenUpdating = true;
            return Resultado;
         }
         catch
         {
            Globals.ThisAddIn.Application.ScreenUpdating = true;
            return false;
         }
      }

      /// <summary>
      /// Carga todos los controles registrados a un documento de SGC, asociado a una Tipología de SGD.
      /// La Función quita la protección de la hoja
      /// </summary>
      /// <param name="IdTipologia"></param>
      /// <returns>Estado Final de la operación</returns>
      public static bool CargarControlesDescriptoresPorTipologia(int IdTipologia)
      {
         bool Resultado = false;
         try
         {
            Globals.ThisAddIn.Application.ScreenUpdating = false;
            //Globals.ThisAddIn.IsUpdating = true;

            //Elimina controles registrados previamente
            Globals.ThisAddIn.ControlesFormato.Clear();

            //Buscar Descriptores asociados a la tipologia
            DataTable ConsultaDescriptores = Plantillas.ConsultaDescriptores(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarDescriptoresTipologia", IdTipologia.ToString());

            if (ConsultaDescriptores.Rows.Count > 0 && ConsultaDescriptores.Columns.Count > 0)
            {
               //Crear Controles
               Excel.Worksheet HojaTrabajo = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
               ExcelTools.Worksheet HojaTrabajoExtendida = Globals.Factory.GetVstoObject(HojaTrabajo);

               if (HojaTrabajo.ProtectContents)
               {
                  MetodosRibbon.DesprotegerHoja(HojaTrabajo, Globals.ThisAddIn.ClaveProteccionHoja);
               }

               Excel.Range RangoDatosTotal = (Excel.Range)HojaTrabajo.UsedRange;
               //RangoDatosTotal.MergeCells = false;

               bool IsElementCreated = true;
               int NumroMaxControles = ConsultaDescriptores.Rows.Count;

               if (Globals.Ribbons.RibbonExcel.ModoTrabajo == ModoTrabajo.VerificarPlantilla
                   || Globals.Ribbons.RibbonExcel.ModoTrabajo == ModoTrabajo.CompletarFormato)
               {
                  Globals.ThisAddIn.ColorDescriptorObligatorio = Color.Transparent;
                  Globals.ThisAddIn.ColorDescriptorOpcional = Color.Transparent;
               }
               else
               {
                  Globals.ThisAddIn.CargarColores();
               }

               foreach (DataRow Fila in ConsultaDescriptores.Rows)
               {
                  if (!string.IsNullOrEmpty(Fila["DCOpropiedadesExtendidas"].ToString()))
                  {
                     XmlDocument Documento = new XmlDocument();
                     Documento.LoadXml(Fila["DCOpropiedadesExtendidas"].ToString());

                     XmlNode Nodo = Documento.DocumentElement;
                     foreach (XmlNode item in Nodo.ChildNodes)
                     {
                        try
                        {
                           DataTable DatosPropiedades = XML.XMLtoDataTable(item);
                           if (DatosPropiedades.Rows.Count > 0)
                           {
                              Controles NewCT = new Controles();

                              NewCT.Id = Fila["DESid"].ToString();

                              #region Actualizar  Propiedades del control
                              foreach (DataRow Prop in DatosPropiedades.Rows)
                              {
                                 string ValorPropiedad = Prop["valor"].ToString();
                                 switch (Prop["nombre"].ToString())
                                 {
                                    case "Width": NewCT.Width = double.Parse(ValorPropiedad); break;
                                    case "Height": NewCT.Height = double.Parse(ValorPropiedad); break;
                                    case "Top": NewCT.Top = double.Parse(ValorPropiedad); break;
                                    case "Left": NewCT.Left = double.Parse(ValorPropiedad); break;
                                    case "Locked": NewCT.Locked = bool.Parse(ValorPropiedad); break;
                                    case "IdFormato": NewCT.IdFormato = ValorPropiedad; break;
                                    case "IdSubSerie": NewCT.IdSubSerie = ValorPropiedad; break;

                                    case "TablaNombre": if (!string.IsNullOrEmpty(ValorPropiedad) && ValorPropiedad != "null") { NewCT.TablaNombre = ValorPropiedad; } break;
                                    case "TablaFila": if (!string.IsNullOrEmpty(ValorPropiedad) && ValorPropiedad != "null") { NewCT.TablaFila = ValorPropiedad; } break;
                                    case "TablaRangoInicial": if (!string.IsNullOrEmpty(ValorPropiedad) && ValorPropiedad != "null") { NewCT.TablaRangoInicial = HojaTrabajo.get_Range(ValorPropiedad, System.Type.Missing); } break;
                                    case "TablaNumeroMaximoRegistros": if (!string.IsNullOrEmpty(ValorPropiedad) && ValorPropiedad != "null") { NewCT.TablaNumeroMaximoRegistros = int.Parse(ValorPropiedad); } break;

                                    case "GUID": NewCT.GUID = ValorPropiedad; break;
                                    case "TipoControl": NewCT.Tipo = ValorPropiedad; break;
                                    case "NombreElemento": NewCT.Nombre = ValorPropiedad; break;
                                    case "RangoDatos": NewCT.RangoDatos = HojaTrabajo.get_Range(ValorPropiedad); break;
                                    case "Orientacion": NewCT.Orientacion = bool.Parse(ValorPropiedad); break;
                                    case "Obligatorio": NewCT.Obligatorio = bool.Parse(ValorPropiedad); break;
                                    case "TipoValidacion": NewCT.TipoValidacion = ValorPropiedad; break;
                                    case "Principal": NewCT.Principal = bool.Parse(ValorPropiedad); break;

                                    case "Propiedades": NewCT.Propiedades = ValorPropiedad; break;
                                    case "HojaExcel": NewCT.HojaExcel = Globals.Factory.GetVstoObject(HojaTrabajo); break;
                                    case "LibroExcel": NewCT.LibroExcel = Globals.ThisAddIn.Application.ActiveWorkbook; break;
                                 }
                              }
                              #endregion

                              NewCT.BloqueadoWorkFlow = bool.Parse(Fila["BloquearEdicionWorkFlow"].ToString());

                              if (NewCT.BloqueadoWorkFlow)
                              { NewCT.Locked = true; }
                              else
                              { NewCT.Locked = false; }

                              Controles ControlCreado = Controles.CrearControl(NewCT, false, Globals.ThisAddIn.ColorDescriptorObligatorio, Globals.ThisAddIn.ColorDescriptorOpcional);

                              #region Validar Bloqueo de descriptores por etapas de correspondencia

                              NewCT.BloqueadoWorkFlow = bool.Parse(Fila["BloquearEdicionWorkFlow"].ToString());
                              if (NewCT.BloqueadoWorkFlow &&
                                      (Globals.Ribbons.RibbonExcel.ModoTrabajo == ModoTrabajo.CompletarFormato
                                      || Globals.Ribbons.RibbonExcel.ModoTrabajo == ModoTrabajo.VerificarPlantilla))
                              {
                                 //BloquearControl(NuevoControl);
                                 //BloquearHoja = true;
                              }

                              #endregion

                              Globals.ThisAddIn.ControlesFormato.Add(ControlCreado);

                              if (ControlCreado.Tipo == Controles.ListBox || ControlCreado.Tipo == Controles.ListaDesplegable)
                              {
                                 MetodosRibbon.DataBindControl(ControlCreado, "_");
                              }
                           }
                        }
                        catch
                        {
                           IsElementCreated = false;
                        }

                     }
                  }
               }

               if (IsElementCreated)
               {
                  Resultado = true;
               }
               else
               {
                  Resultado = false;
               }
            }
            //Globals.ThisAddIn.IsUpdating = false;
            Globals.ThisAddIn.Application.ScreenUpdating = true;
            return Resultado;
         }
         catch
         {
            //Globals.ThisAddIn.IsUpdating = false;
            Globals.ThisAddIn.Application.ScreenUpdating = true;
            return false;
         }
      }

      /// <summary>
      /// Abre un formato de SGC, asociado a una tipología de SGD,   para diligenciar por parte del usuario.
      /// </summary>
      /// <param name="IdPlantilla"></param>
      /// <returns>estado final de la operación.</returns>
      public static bool AbrirFormatoParaCompletar(string IdPlantilla)
      {
         bool IsCreated = false;

         string Filtro = " SFVid = " + IdPlantilla;
         DataView FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatosVigentes, Filtro, "", DataViewRowState.CurrentRows);

         //Buscar en formatos en registro, solo para pruebas de verificación de plantila, desactivar el boton guardar !!
         if (FiltroFormato.Count == 0)
         {
            FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatos, Filtro, "", DataViewRowState.CurrentRows);
         }

         if (FiltroFormato.Count > 0)
         {
            DataTable ConsultarConfiguracion = SGCformatos.ConsultarConfiguracionISO(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarConfiguracionISO");
            string FiltroRutas = " CFGcodigo  = 'RUTA_FORMATOS' ";

            DataView DvFiltro = new DataView(ConsultarConfiguracion, FiltroRutas, "", DataViewRowState.CurrentRows);

            if (DvFiltro.Count > 0)
            {
               string RutaArchivo = DvFiltro.ToTable().Rows[0]["CFGvalorTexto"].ToString() + FiltroFormato.ToTable().Rows[0]["DOCnombreArchivo"].ToString();

               Byte[] Archivo = SGCformatos.LeerArchivosFormatos(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, RutaArchivo);

               if (Archivo.Length > 0)
               {
                  try
                  {
                     string rutaLocal = RegistroWindows.ConsultarEntradaRegistro("Ruta", "Temporales") + FiltroFormato.ToTable().Rows[0]["DOCnombreArchivo"].ToString() + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

                     MemoryStream objstreaminput = new MemoryStream();
                     FileStream objfilestream = new FileStream(rutaLocal, FileMode.Create, FileAccess.ReadWrite);
                     objfilestream.Write(Archivo, 0, Archivo.Length);
                     objfilestream.Close();

                     Excel.Workbooks WorkB = (Excel.Workbooks)Globals.ThisAddIn.Application.Workbooks;

                     WorkB.Open(rutaLocal, System.Type.Missing, false, System.Type.Missing, System.Type.Missing, System.Type.Missing,
                                 System.Type.Missing, System.Type.Missing, System.Type.Missing, true, System.Type.Missing, System.Type.Missing,
                                 System.Type.Missing, System.Type.Missing);

                     Excel.Workbook LibroActual = (Excel.Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;
                     LibroActual.Protect(Globals.ThisAddIn.ClaveProteccionHoja, true, false);

                     IsCreated = true;
                  }
                  catch
                  {
                     IsCreated = false;
                  }

                  if (IsCreated)
                  {
                     string IdTipologia = FiltroFormato.ToTable().Rows[0]["SFVtipologiaDocumental"].ToString();
                     if (!string.IsNullOrEmpty(IdTipologia))
                     {
                        Globals.ThisAddIn.IsUpdating = false;
                        CargarControlesDescriptoresPorTipologia(int.Parse(IdTipologia));
                     }

                     Globals.ThisAddIn.IdPlantillaFormato = IdPlantilla;


                     try
                     {
                        Globals.ThisAddIn.IdSubSeriePlantillaFormato = FiltroFormato.ToTable().Rows[0]["SSEid"].ToString();
                     }
                     catch
                     {
                        Globals.ThisAddIn.IdSubSeriePlantillaFormato = string.Empty;
                     }

                     Excel.Worksheet HojaActual = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;

                     #region Bloquear Celdas

                     if (HojaActual.ProtectContents)
                     {
                        MetodosRibbon.DesprotegerHoja(HojaActual, Globals.ThisAddIn.ClaveProteccionHoja);
                     }

                     foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                     {
                        if (Control.Locked || Control.BloqueadoWorkFlow)
                        {
                           BloquearControl(Control);
                        }
                     }

                     if (!HojaActual.ProtectContents)
                     {
                        MetodosRibbon.ProtegerHoja(HojaActual, Globals.ThisAddIn.ClaveProteccionHoja);
                     }
                     #endregion

                     Globals.Ribbons.RibbonExcel.ModoTrabajo = ModoTrabajo.CompletarFormato;
                     Globals.Ribbons.RibbonExcel.GroupElementos.Visible = false;
                     Globals.Ribbons.RibbonExcel.BtnGuardarPlantilla.Visible = true;
                     return true;
                  }
                  else
                  {
                     return false;
                  }
               }
               else
               {
                  MessageBox.Show(Properties.Settings.Default.MsErrorCargarArchivo, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  return false;
               }
            }
            else
            {
               return false;
            }
         }
         else
         {
            return false;
         }
      }

      /// <summary>
      /// Elimina caracteres inválidos en el nombre de un archivo guardado en disco.
      /// </summary>
      /// <param name="FileName"></param>
      /// <returns>cadena de nombre limpia.</returns>
      public static string LimpiarFileName(string FileName)
      {
         try
         {
            char[] TextoInvalido = System.IO.Path.GetInvalidFileNameChars();

            foreach (char CharInvalid in TextoInvalido)
            {
               FileName = FileName.Replace(CharInvalid.ToString(), " ");
            }

            return FileName;
         }
         catch
         {
            return string.Empty;
         }
      }

      /// <summary>
      /// Actualiza información inicial del complemento.
      /// </summary>
      public static void CargarInformacionInicial()
      {
         try
         {
            try
            {
               if (Globals.ThisAddIn.DatosUsuario.IdUsuario != null)
               {
                  Globals.ThisAddIn.MensajeTitulos = "Sinco ERP - " + Globals.ThisAddIn.DatosUsuario.EmpresaNombre;
               }
            }
            catch
            {
               Globals.ThisAddIn.MensajeTitulos = "Sinco ERP ";
            }

            Globals.ThisAddIn.CargarColores();

            bool Resultado = MetodosRibbon.ActualizarFuentesInformacionDescriptores();
            if (!Resultado)
            {
               MessageBox.Show(Properties.Settings.Default.MsErrorCargarInformacionInicial, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      /// <summary>
      /// Bloquea un control para edición por parte del usuario
      /// </summary>
      /// <param name="Control">Control a bloquear</param>
      /// <returns></returns>
      public static bool BloquearControl(Controles Control)
      {
         bool Resultado = true;
         string TextoComentario = string.Empty;

         if (Control.BloqueadoWorkFlow)
         { TextoComentario = "Descr. Bloqueado por flujo de correspondencia."; }
         else if (Control.Locked)
         { TextoComentario = "Descr. Bloqueado"; }

         try
         {
            #region Bloquear por tipo de control
            switch (Control.Tipo)
            {
               case Controles.TextBox:
                  Control.Locked = true;
                  Control.RangoDatos.Locked = true;
                  Control.RangoDatos.AddComment(TextoComentario);
                  break;
               case Controles.ListaDesplegable:
                  Control.Locked = true;
                  Control.RangoDatos.Locked = true;
                  Control.RangoDatos.AddComment(TextoComentario);
                  Control.RangoDatos.Validation.Delete();
                  break;
               case Controles.ListBox:
                  Control.Locked = true;
                  Excel.ListBox LbControl = (Excel.ListBox)Control.HojaExcel.ListBoxes(Control.GUID);
                  LbControl.Enabled = false;
                  LbControl.Locked = true;
                  Control.RangoDatos.Locked = true;
                  Control.RangoDatos.AddComment(TextoComentario);
                  Control.RangoDatos.Validation.Delete();
                  break;
               case Controles.ComboBusqueda:
                  Control.Locked = true;
                  Control.RangoDatos.Locked = true;
                  Control.RangoDatos.get_Offset(0, 1).Locked = true;
                  Control.RangoDatos.get_Offset(0, 1).AddComment(TextoComentario);
                  Control.RangoDatos.get_Offset(0, 1).Validation.Delete();
                  break;
               case Controles.CheckBox:
                  Control.Locked = true;
                  Excel.CheckBox ChbControl = (Excel.CheckBox)Control.HojaExcel.CheckBoxes(Control.GUID);
                  ChbControl.Locked = true;
                  ChbControl.Enabled = false;
                  ChbControl.LockedText = true;
                  Control.RangoDatos.Locked = true;
                  Control.RangoDatos.AddComment(TextoComentario);
                  break;
            }
            #endregion
            return Resultado;
         }
         catch
         { return false; }
      }

      public static bool CargarControlesHoja(Excel.Worksheet hojaActiva)
      {
         try
         {
            bool Resultado = true;
            Globals.ThisAddIn.Application.ScreenUpdating = false;
            ExcelTools.Worksheet HojaExtended = (ExcelTools.Worksheet)Globals.Factory.GetVstoObject(hojaActiva);

            foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
            {
               Controles ControlCreado = Controles.CrearControl(Control, false, Globals.ThisAddIn.ColorDescriptorObligatorio, Globals.ThisAddIn.ColorDescriptorOpcional);

               if (ControlCreado == null)
               {
                  Resultado = false;
               }
            }

            Globals.ThisAddIn.Application.ScreenUpdating = true;
            return Resultado;
         }
         catch
         {
            Globals.ThisAddIn.Application.ScreenUpdating = true;
            return false;
         }
      }

      public static bool ValidarInformacion(Controles Control)
      {
         string TipoControl = Control.Tipo;
         string TipoValidacion = Control.TipoValidacion;
         bool obligatorio = Control.Obligatorio;

         try
         {
            bool Resultado = true;

            string TextoElemento = Controles.LeerValorControl(Control).Split('-')[0];
            Resultado = ValidacionesDatos.ValidarInformacion(TextoElemento, TipoValidacion);

            if (string.IsNullOrEmpty(TextoElemento) && obligatorio)
            { Resultado = false; }

            if (string.IsNullOrEmpty(TextoElemento) && !obligatorio)
            { Resultado = true; }

            return Resultado;
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
            return false;
         }
      }

      public void Pruebas()
      {
         Globals.ThisAddIn.Application.Workbooks.Add();

         Excel.Workbook LibroActual = (Excel.Workbook)Globals.ThisAddIn.Application.Workbooks[0];



         LibroActual.Sheets.Add(System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);
         LibroActual.Sheets.Delete();

         Excel.Chart Grafico = new Excel.Chart();
         Grafico.ChartType = Excel.XlChartType.xlArea;

         Excel.Worksheet HojaActual = LibroActual.Sheets[0];
         HojaActual.get_Range("A1:B3", System.Type.Missing);

         Excel.Range RangoDatos = Globals.ThisAddIn.Application.Selection;

         LibroActual.Charts.Add(System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);

      }
   }
}
