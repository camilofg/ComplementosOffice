using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using ExcelTools = Microsoft.Office.Tools.Excel;

using SincoOfficeLibrerias;
using AppExternas;

namespace SincoExcel.Forms
{
   public partial class FrmFormatos : Form
   {
      public FrmFormatos()
      {
         try
         {
            InitializeComponent();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void FrmFormatos_Load(object sender, EventArgs e)
      {
         try
         {
            ConfiguracionInicial();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void ConfiguracionInicial()
      {
         MetodosRibbon.ActualizarFuentesInformacionFormatos();

         #region Restringir acceso de usuarios
         if (!Globals.ThisAddIn.AccesoCrearFormatoISO)
         {
            tabControl1.TabPages.RemoveByKey(tabControl1.TabPages["TabCrearFormato"].Name);
         }

         if (!Globals.ThisAddIn.AccesoRegistrarFormatoISO)
         {
            tabControl1.TabPages.RemoveByKey(tabControl1.TabPages["TabCompletarFormato"].Name);
         }
         #endregion

         #region Actualizar Formatos de Consulta
         DataTable ConsultaFormatos = SGCformatos.ConsultarFormatosEnRegistro(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarFormatosVigentes");

         if (ConsultaFormatos.Rows.Count > 0 && ConsultaFormatos.Columns.Count > 2)
         {
            Globals.Ribbons.RibbonExcel.DatosFormatosVigentes = ConsultaFormatos;
         }

         #endregion

         #region Actualizar CbSubproceso Formatos en registro
         if (Globals.Ribbons.RibbonExcel.DatosFormatos.Rows.Count > 0)
         {
            var x = (from r in Globals.Ribbons.RibbonExcel.DatosFormatos.AsEnumerable()
                     select r["NombreSubProceso"]).Distinct().ToList();

            CbSubProcesoCrearFormato.ValueMember = "SPRid";
            CbSubProcesoCrearFormato.DisplayMember = "NombreSubProceso";
            CbSubProcesoCrearFormato.DataSource = x;
         }
         else
         {
            CbSubProcesoCrearFormato.ValueMember = "SPRid";
            CbSubProcesoCrearFormato.DisplayMember = "NombreSubProceso";
            CbSubProcesoCrearFormato.DataSource = new DataTable();
         }
         #endregion

         #region Actualizar Subprocesos Formatos Vigentes
         if (Globals.Ribbons.RibbonExcel.DatosFormatosVigentes.Rows.Count > 0)
         {
            /*
            List<object> x = (from r in Globals.Ribbons.RibbonExcel.DatosFormatosVigentes.AsEnumerable()
                     select r["NombreSubProceso"]).Distinct().ToList();

            var y =(from p in x
                    select p["IdUsuario"] == Globals.ThisAddIn.DatosUsuario.IdUsuario);
            */

            string Filtro = " IdUsuario= '" + Globals.ThisAddIn.DatosUsuario.IdUsuario + "' ";
            DataView Dv = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatosVigentes, Filtro, "", DataViewRowState.CurrentRows);

            var x = (from r in Dv.ToTable().AsEnumerable()
                     select r["NombreSubProceso"]).Distinct().ToList();

            CbSubProcesoCompletar.ValueMember = "SPRid";
            CbSubProcesoCompletar.DisplayMember = "NombreSubProceso";
            CbSubProcesoCompletar.DataSource = x;
         }
         else
         {
            CbSubProcesoCompletar.ValueMember = "SPRid";
            CbSubProcesoCompletar.DisplayMember = "NombreSubProceso";
            CbSubProcesoCompletar.DataSource = new DataTable();
         }

         #endregion

         #region Actualizar SubSeries disponibles para guardar plantillas
         DataTable ResConsultarSubseriesFormatos = Descriptores.ConsultarSubseries(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarSubSeriesValidasParaFormatosSGC", "");

         if (ResConsultarSubseriesFormatos.Rows.Count > 0 && ResConsultarSubseriesFormatos.Columns.Count > 2)
         {
            CbSubserieFormato.ValueMember = "SSEid";
            CbSubserieFormato.DisplayMember = "SSEdescripcion";
            CbSubserieFormato.DataSource = ResConsultarSubseriesFormatos;
         }
         else
         {
            CbSubserieFormato.ValueMember = "SSEid";
            CbSubserieFormato.DisplayMember = "SSEdescripcion";
            CbSubserieFormato.DataSource = new DataTable(); ;
         }

         #endregion

         if (Globals.Ribbons.RibbonExcel.ModoTrabajo == MetodosRibbon.ModoTrabajo.VerificarPlantilla)
         {
            BtnValidarRequisitosPlantillas.Visible = true;
         }
         else
         {
            BtnValidarRequisitosPlantillas.Visible = false;
         }
      }

      private void CbFormato_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            if (!string.IsNullOrEmpty(CbFormato.Text))
            {
               DgvInformacionFormato.Rows.Clear();

               string Filtro = " SFVid = " + CbFormato.SelectedValue.ToString();
               DataView FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatos, Filtro, "", DataViewRowState.CurrentRows);

               if (FiltroFormato.Count > 0)
               {
                  DgvInformacionFormato.Rows.Add("Versión", FiltroFormato.ToTable().Rows[0]["SFVversion"].ToString());
                  DgvInformacionFormato.Rows.Add("Código", FiltroFormato.ToTable().Rows[0]["SPFcodigo"].ToString());
                  DgvInformacionFormato.Rows.Add("Estado", FiltroFormato.ToTable().Rows[0]["VEEdescripcion"].ToString());
                  DgvInformacionFormato.Rows.Add("Resp. Revisión", FiltroFormato.ToTable().Rows[0]["RESdescripcion"].ToString());
                  DgvInformacionFormato.Rows.Add("Documento", FiltroFormato.ToTable().Rows[0]["DOCnombreArchivo"].ToString());

                  if (!string.IsNullOrEmpty(FiltroFormato.ToTable().Rows[0]["DOCnombreArchivo"].ToString()))
                  {
                     BtnCargarFormatoVerificacion.Enabled = true;
                  }
                  else
                  {
                     BtnCargarFormatoVerificacion.Enabled = false;
                  }
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void BtnCrearFormato_Click(object sender, EventArgs e)
      {
         try
         {
            if (string.IsNullOrEmpty(Globals.ThisAddIn.IdPlantillaFormato) && string.IsNullOrEmpty(Globals.ThisAddIn.IdSubSeriePlantillaFormato))
            {
               if (!string.IsNullOrEmpty(CbFormato.Text) && !string.IsNullOrEmpty(CbSubserieFormato.Text))
               {
                  Globals.Ribbons.RibbonExcel.ModoTrabajo = MetodosRibbon.ModoTrabajo.EdicionPlantilla;

                  string Filtro = " SFVid = " + CbFormato.SelectedValue.ToString();
                  DataView FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatos, Filtro, "", DataViewRowState.CurrentRows);

                  bool IsCreated = false;
                  bool LoadContentFile = false;

                  if (FiltroFormato.Count > 0)
                  {
                     if (!string.IsNullOrEmpty(FiltroFormato.ToTable().Rows[0]["DOCnombreArchivo"].ToString()))
                     {
                        DialogResult Respuesta = MessageBox.Show(Properties.Settings.Default.MsPreguntaCargarFormatoExistente, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (Respuesta == System.Windows.Forms.DialogResult.Yes)
                        {
                           #region Cargar Archivo Existente
                           DataTable ConsultarConfiguracion = SGCformatos.ConsultarConfiguracionISO(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarConfiguracionISO");
                           string FiltroRutas = " CFGcodigo  = 'RUTA_FORMATOS' ";

                           DataView DvFiltro = new DataView(ConsultarConfiguracion, FiltroRutas, "", DataViewRowState.CurrentRows);

                           if (DvFiltro.Count > 0)
                           {
                              string RutaArchivo = DvFiltro.ToTable().Rows[0]["CFGvalorTexto"].ToString() + FiltroFormato.ToTable().Rows[0]["DOCnombreArchivo"].ToString();

                              Byte[] Archivo = SGCformatos.LeerArchivosFormatos(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, RutaArchivo);

                              if (Archivo.Length > 0)
                              {
                                 string rutaLocal = RegistroWindows.ConsultarEntradaRegistro("Ruta", "Temporales") + FiltroFormato.ToTable().Rows[0]["DOCnombreArchivo"].ToString();

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
                              else
                              {
                                 MessageBox.Show(Properties.Settings.Default.MsErrorCargarArchivo, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                              }
                           }
                           else
                           {
                              MessageBox.Show(Properties.Settings.Default.MsErrorVerificarConfiguracionISO, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                           }
                           #endregion
                           LoadContentFile = true;
                        }
                        else if (Respuesta == System.Windows.Forms.DialogResult.No)
                        {
                           #region Crear nueva hoja con la plantilla
                           Excel.Workbooks Libros = Globals.ThisAddIn.Application.Workbooks;
                           Libros.Application.SheetsInNewWorkbook = 1;
                           Libros.Add(System.Type.Missing);

                           Excel.Workbook LibroActual = (Excel.Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;
                           Globals.ThisAddIn.CargarEstilos(LibroActual);


                           Excel.Worksheet HojaActual = (Excel.Worksheet)Libros.Application.ActiveSheet;
                           string NombreHoja = "Formato"; //+ FiltroFormato.ToTable().Rows[0]["NombreFormato"].ToString();

                           NombreHoja = NombreHoja.Replace('\n', ' ');
                           HojaActual.Name = NombreHoja;

                           //Globals.Ribbons.RibbonExcel.ProtegerHoja(HojaActual, Globals.ThisAddIn.ClaveProteccionHoja);
                           LibroActual.Protect(Globals.ThisAddIn.ClaveProteccionHoja, true, false);
                           #endregion
                           IsCreated = true;
                           LoadContentFile = false;
                        }
                        else
                        {
                           IsCreated = false;
                           LoadContentFile = false;
                        }
                     }
                     else
                     {
                        #region Crear nueva hoja con la plantilla
                        Excel.Workbooks Libros = Globals.ThisAddIn.Application.Workbooks;
                        Libros.Application.SheetsInNewWorkbook = 1;
                        Libros.Add(System.Type.Missing);

                        Excel.Workbook LibroActual = (Excel.Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;
                        Globals.ThisAddIn.CargarEstilos(LibroActual);


                        Excel.Worksheet HojaActual = (Excel.Worksheet)Libros.Application.ActiveSheet;
                        string NombreHoja = "Formato"; //+ FiltroFormato.ToTable().Rows[0]["NombreFormato"].ToString();
                        NombreHoja = NombreHoja.Replace('\n', ' ');
                        HojaActual.Name = NombreHoja;

                        //Globals.Ribbons.RibbonExcel.ProtegerHoja(HojaActual, Globals.ThisAddIn.ClaveProteccionHoja);
                        LibroActual.Protect(Globals.ThisAddIn.ClaveProteccionHoja, true, false);
                        #endregion

                        IsCreated = true;
                        LoadContentFile = false;
                     }

                     // Si el archivo fue creado correctamente.
                     if (IsCreated)
                     {
                        bool OpenFileOk = true;

                        Globals.ThisAddIn.IsUpdating = true;

                        string IdTipologia = FiltroFormato.ToTable().Rows[0]["SFVtipologiaDocumental"].ToString();

                        if (LoadContentFile && !string.IsNullOrEmpty(IdTipologia))
                        {
                           bool ResCargar = MetodosRibbon.CargarControlesDescriptoresPorTipologia(int.Parse(IdTipologia));

                           if (!ResCargar)
                           {
                              OpenFileOk = false;
                              MessageBox.Show(Properties.Settings.Default.MsErrorCargarControlesTipologia, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                           }
                        }

                        if (OpenFileOk)
                        {
                           Globals.Ribbons.RibbonExcel.GroupElementos.Visible = true;

                           Globals.Ribbons.RibbonExcel.ModoTrabajo = MetodosRibbon.ModoTrabajo.EdicionPlantilla;
                           Globals.ThisAddIn.IdPlantillaFormato = CbFormato.SelectedValue.ToString();
                           Globals.ThisAddIn.IdSubSeriePlantillaFormato = CbSubserieFormato.SelectedValue.ToString();

                           //Desproteger hoja en modo edición plantilla
                           Excel.Worksheet HojaActual = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
                           if (Globals.Ribbons.RibbonExcel.ModoTrabajo == MetodosRibbon.ModoTrabajo.EdicionPlantilla
                                   && HojaActual.ProtectContents)
                           {
                              MetodosRibbon.DesprotegerHoja(HojaActual, Globals.ThisAddIn.ClaveProteccionHoja);
                           }

                           MessageBox.Show(Properties.Settings.Default.MsPlantillaCreadaCorrectamente, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                           Globals.Ribbons.RibbonExcel.BtnGuardarPlantilla.Visible = true;
                           this.Dispose(true);
                        }
                        else
                        {
                           MessageBox.Show(Properties.Settings.Default.MsErrorCargarPlantilla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        Globals.ThisAddIn.IsUpdating = false;
                     }
                  }
               }
            }
            else
            {
               MessageBox.Show(Properties.Settings.Default.MsCerrarFormatoAbierto, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorCargarArchivo, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      private void CbFormatosConsulta_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            if (!string.IsNullOrEmpty(CbFormatosConsulta.Text))
            {
               DgvInfoFormatoConsulta.Rows.Clear();

               string Filtro = " SFVid = " + CbFormatosConsulta.SelectedValue.ToString();
               DataView FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatosVigentes, Filtro, "", DataViewRowState.CurrentRows);

               if (FiltroFormato.Count > 0)
               {
                  DgvInfoFormatoConsulta.Rows.Add("Versión", FiltroFormato.ToTable().Rows[0]["SFVversion"].ToString());
                  DgvInfoFormatoConsulta.Rows.Add("Código", FiltroFormato.ToTable().Rows[0]["SPFcodigo"].ToString());
                  DgvInfoFormatoConsulta.Rows.Add("Estado", FiltroFormato.ToTable().Rows[0]["VEEdescripcion"].ToString());
                  //DgvInfoFormatoConsulta.Rows.Add("Resp. Revisión", FiltroFormato.ToTable().Rows[0]["RESdescripcion"].ToString());
                  DgvInfoFormatoConsulta.Rows.Add("Documento", FiltroFormato.ToTable().Rows[0]["DOCnombreArchivo"].ToString());
                  DgvInfoFormatoConsulta.Rows.Add("Med. Cons.", FiltroFormato.ToTable().Rows[0]["MCOdescripcion"].ToString());
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void BtnConsultarFormato_Click(object sender, EventArgs e)
      {
         try
         {
            if (CbFormatosConsulta.SelectedIndex > -1)
            {
               if (string.IsNullOrEmpty(Globals.ThisAddIn.IdPlantillaFormato) && string.IsNullOrEmpty(Globals.ThisAddIn.IdSubSeriePlantillaFormato))
               {
                  Globals.Ribbons.RibbonExcel.ModoTrabajo = MetodosRibbon.ModoTrabajo.CompletarFormato;
                  bool resultado = MetodosRibbon.AbrirFormatoParaCompletar(CbFormatosConsulta.SelectedValue.ToString());

                  if (resultado)
                  {
                     Globals.Ribbons.RibbonExcel.BtnGuardarPlantilla.Visible = true;
                  }
                  else
                  {
                     Globals.Ribbons.RibbonExcel.BtnGuardarPlantilla.Visible = false;
                  }
               }
               else
               {
                  MessageBox.Show(Properties.Settings.Default.MsCerrarFormatoAbierto, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
            }
            else
            {
               MessageBox.Show(Properties.Settings.Default.MsSeleccionarFormato, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorCargarArchivo, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      private void CbSubProcesoCrearFormato_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            if (Globals.Ribbons.RibbonExcel.DatosFormatos.Rows.Count > 0 && CbSubProcesoCrearFormato.SelectedValue != null)
            {
               string Filtro = " NombreSubProceso = '" + CbSubProcesoCrearFormato.SelectedValue.ToString() + "' ";
               DataView DvSubProceso = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatos, Filtro, "", DataViewRowState.CurrentRows);

               if (DvSubProceso.Count > 0)
               {
                  CbFormato.ValueMember = "SFVid";
                  CbFormato.DisplayMember = "NombreFormato";
                  CbFormato.DataSource = DvSubProceso.ToTable();
               }
               else
               {
                  CbFormato.ValueMember = "SFVid";
                  CbFormato.DisplayMember = "NombreFormato";
                  CbFormato.DataSource = new DataTable();
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void CbSubProcesoCompletar_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            if (Globals.Ribbons.RibbonExcel.DatosFormatos.Rows.Count > 0 && CbSubProcesoCompletar.SelectedValue != null)
            {
               string Filtro = " NombreSubProceso = '" + CbSubProcesoCompletar.SelectedValue.ToString() + "' AND IdUsuario ='" + Globals.ThisAddIn.DatosUsuario.IdUsuario + "' ";
               DataView DvSubProceso = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatosVigentes, Filtro, "", DataViewRowState.CurrentRows);

               if (DvSubProceso.Count > 0)
               {
                  //CbFormato.ValueMember = "SFVid";
                  //CbFormato.DisplayMember = "NombreFormato";
                  //CbFormato.DataSource = DvSubProceso.ToTable();

                  CbFormatosConsulta.ValueMember = "SFVid";
                  CbFormatosConsulta.DisplayMember = "NombreFormato";
                  CbFormatosConsulta.DataSource = DvSubProceso.ToTable();
               }
               else
               {
                  //CbFormato.ValueMember = "SFVid";
                  //CbFormato.DisplayMember = "NombreFormato";
                  //CbFormato.DataSource = new DataTable();

                  CbFormatosConsulta.ValueMember = "SFVid";
                  CbFormatosConsulta.DisplayMember = "NombreFormato";
                  CbFormatosConsulta.DataSource = new DataTable();
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void BtnCargarFormatoVerificacion_Click_1(object sender, EventArgs e)
      {
         try
         {
            if (string.IsNullOrEmpty(Globals.ThisAddIn.IdPlantillaFormato) && string.IsNullOrEmpty(Globals.ThisAddIn.IdSubSeriePlantillaFormato))
            {
               if (CbFormato.SelectedValue != null)
               {
                  Globals.Ribbons.RibbonExcel.ModoTrabajo = MetodosRibbon.ModoTrabajo.VerificarPlantilla;
                  bool resultado = MetodosRibbon.AbrirFormatoParaCompletar(CbFormato.SelectedValue.ToString());

                  if (resultado)
                  {
                     Globals.Ribbons.RibbonExcel.ModoTrabajo = MetodosRibbon.ModoTrabajo.VerificarPlantilla;
                     BtnValidarRequisitosPlantillas.Visible = true;
                  }
                  else
                  {
                     BtnValidarRequisitosPlantillas.Visible = false;
                  }

                  Globals.Ribbons.RibbonExcel.GroupElementos.Visible = false;
                  Globals.Ribbons.RibbonExcel.BtnGuardarPlantilla.Visible = false;
               }
               else
               {
                  MessageBox.Show(Properties.Settings.Default.MsSeleccionarFormato, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
            }
            else
            {
               MessageBox.Show(Properties.Settings.Default.MsCerrarFormatoAbierto, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorCargarArchivo, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      private void BtnValidarRequisitosPlantillas_Click(object sender, EventArgs e)
      {
         try
         {
            string MensajesValidaciones = string.Empty;

            //Validación de contenidos
            if (!string.IsNullOrEmpty(Globals.ThisAddIn.IdPlantillaFormato) || !string.IsNullOrEmpty(Globals.ThisAddIn.IdSubSeriePlantillaFormato))
            {
               bool resultado = MetodosRibbon.ValidarInformacionGuardarPlantilla();

               if (!resultado)
               {
                  MensajesValidaciones = MensajesValidaciones + "Validación de contenidos incompleta.";
               }

               #region Validaciones  complemento
               string RutaLocal = RegistroWindows.ConsultarEntradaRegistro("Ruta", "Temporales");
               if (string.IsNullOrEmpty(RutaLocal))
               {
                  MensajesValidaciones = MensajesValidaciones + "\nConfigure una ruta temporal para la aplicación.";
               }

               string Filtro = " SFVid = " + Globals.ThisAddIn.IdPlantillaFormato;
               DataView FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatosVigentes, Filtro, "", DataViewRowState.CurrentRows);
               if (FiltroFormato.Count == 0)
               {
                  FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatos, Filtro, "", DataViewRowState.CurrentRows);
               }

               if (FiltroFormato.Count == 0)
               {
                  MensajesValidaciones = MensajesValidaciones + "\nLa plantilla no es válida, cargue de nuevo el formato.";
               }

               int NumPrincipal = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c) { return c.Principal == true; }).Count;

               if (NumPrincipal == 0)
               {
                  MensajesValidaciones = MensajesValidaciones + "\nLa plantilla debe tener asignado un descriptor principal.";
               }

               #endregion

               if (!string.IsNullOrEmpty(MensajesValidaciones))
               {
                  MessageBox.Show("Resultados de Validaciones:\n\n" + MensajesValidaciones, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               else
               {
                  MessageBox.Show(Properties.Settings.Default.MsResultadoValidacionesOK, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }
   }
}
