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

using SincoOfficeLibrerias.wsOfficeSGD;
using SincoOfficeLibrerias;
using AppExternas;

namespace SincoExcel
{
   public partial class FrmConstructorPlantillas : Form
   {
      private Excel.Worksheet HojaTrabajo;
      private ExcelTools.Worksheet HojaTrabajoExtendida;
      private List<string> ElementosTabla;
      private Color ColorDescriptor;
      private Color ColorCategoria;

      private string Requerido = "Obligatorio";

      public FrmConstructorPlantillas()
      {
         try
         {
            InitializeComponent();
            ElementosTabla = new List<string>();
            ColorDescriptor = Color.Blue;
            ColorCategoria = Color.Black;

            DgvEditarElementos.DataError += new DataGridViewDataErrorEventHandler(DgvEditarElementos_DataError);
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      void DgvEditarElementos_DataError(object sender, DataGridViewDataErrorEventArgs e)
      {
         try
         {
            DgvEditarElementos.DataSource = null;
            e.Cancel = true;
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void FrmConstructorPlantillas_Load(object sender, EventArgs e)
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

      private void BtnCrearElemento_Click(object sender, EventArgs e)
      {
         try
         {
            bool Validaciones = false;
            string MensajeResultado = string.Empty;

            // Validaciones antes de crear el control
            Validaciones = ValidacionesPreviasCreacionElementos();

            if (Validaciones)
            {
               string NombreDescriptor = string.Empty;
               string IdDescriptor = string.Empty;

               NombreDescriptor = TVCategorias.SelectedNode.Text.Replace(":", "").Replace("@", "");

               IdDescriptor = TVCategorias.SelectedNode.Name;

               Globals.ThisAddIn.IsUpdating = true;
               Globals.ThisAddIn.Application.ScreenUpdating = false;

               #region Crear Control

               HojaTrabajo = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
               HojaTrabajoExtendida = Globals.Factory.GetVstoObject(HojaTrabajo);
               Excel.Range RangoDatos = Globals.ThisAddIn.Application.ActiveCell;
               if (HojaTrabajo.ProtectContents)
               {
                  MetodosRibbon.DesprotegerHoja(HojaTrabajo, Globals.ThisAddIn.ClaveProteccionHoja);
               }

               if (!string.IsNullOrEmpty(TVCategorias.SelectedNode.Name))
               {
                  #region Controles.ControlDependencias
                  if (CbTipoElemento.SelectedValue.ToString() == Controles.ControlDependencias)
                  {
                     Controles NuevoControl = new Controles();
                     NuevoControl.Nombre = NombreDescriptor;
                     NuevoControl.Id = IdDescriptor;
                     NuevoControl.RangoDatos = RangoDatos;
                     NuevoControl.Orientacion = ChbTipoOrientacion.Checked;
                     NuevoControl.Obligatorio = ChbObligatorio.Checked;
                     NuevoControl.TipoValidacion = CbTipoValidacion.Text;
                     NuevoControl.Principal = ChbDescriptorPrincipal.Checked;
                     NuevoControl.HojaExcel = HojaTrabajoExtendida;
                     NuevoControl.IdFormato = Globals.ThisAddIn.IdPlantillaFormato;
                     NuevoControl.IdSubSerie = Globals.ThisAddIn.IdSubSeriePlantillaFormato;

                     List<Controles> ControlesCreados = Controles.CrearControlDependencias(Globals.ThisAddIn.DatosUsuario,Globals.ThisAddIn.DatosConexion, NuevoControl, Globals.ThisAddIn.ColorDescriptorObligatorio, Globals.ThisAddIn.ColorDescriptorOpcional);

                     if (ControlesCreados.Count > 0)
                     {
                        foreach (Controles Control in ControlesCreados)
                        {
                           if (Control.Id != null)
                           {
                              //if (Globals.Ribbons.RibbonExcel.ModoTrabajo != MetodosRibbon.ModoTrabajo.CompletarFormato)
                              //{
                              //   MetodosRibbon.DataBindControl(Control, "_");
                              //}
                              Globals.ThisAddIn.ControlesFormato.Add(Control);
                           }
                        }
                     }
                  }
                  #endregion

                  #region Otros Controles
                  else
                  {
                     Controles NuevoControl = new Controles();
                     NuevoControl.Tipo = CbTipoElemento.SelectedValue.ToString();
                     NuevoControl.Nombre = NombreDescriptor;
                     NuevoControl.Id = IdDescriptor;
                     NuevoControl.RangoDatos = RangoDatos;
                     NuevoControl.Orientacion = ChbTipoOrientacion.Checked;
                     NuevoControl.Obligatorio = ChbObligatorio.Checked;
                     NuevoControl.TipoValidacion = CbTipoValidacion.Text;
                     NuevoControl.Principal = ChbDescriptorPrincipal.Checked;
                     NuevoControl.HojaExcel = HojaTrabajoExtendida;
                     NuevoControl.IdFormato = Globals.ThisAddIn.IdPlantillaFormato;
                     NuevoControl.IdSubSerie = Globals.ThisAddIn.IdSubSeriePlantillaFormato;

                     Controles ControlCreado = Controles.CrearControl(NuevoControl, true, Globals.ThisAddIn.ColorDescriptorObligatorio, Globals.ThisAddIn.ColorDescriptorOpcional);

                     if (ControlCreado.Id != null)
                     {
                        Globals.ThisAddIn.ControlesFormato.Add(ControlCreado);
                     }
                     else
                     {
                        MessageBox.Show(Properties.Settings.Default.MsErrorCreacionControl, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                  }
                  #endregion
               }
               #endregion

               Globals.ThisAddIn.Application.ScreenUpdating = true;
               Globals.ThisAddIn.IsUpdating = false;
            }
         }
         catch (Exception EXC)
         {
            Globals.ThisAddIn.Application.ScreenUpdating = true;

            Utilidades.ReportarError(EXC);
         }
      }

      #region Fuentes externas

      private void CbTablaTemporal_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            if (CbTablaTemporal.SelectedIndex >= 0)
            {
               //Actualizar DatagridView Fuentes Externas
               DataTable Externas = FuentesExternas.CRUDFuentesExternas(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "Consultar",
                                       int.Parse(CbTablaTemporal.SelectedValue.ToString()), "", "", false);
               if (Externas.Rows.Count > 0)
               {
                  DgvDatosTablaTemporal.AutoGenerateColumns = false;
                  DgvDatosTablaTemporal.DataSource = Externas;
               }
               else
               {
                  DgvDatosTablaTemporal.AutoGenerateColumns = false;
                  DgvDatosTablaTemporal.DataSource = new DataTable();
               }
            }
            else
            {
               DgvDatosTablaTemporal.AutoGenerateColumns = false;
               DgvDatosTablaTemporal.DataSource = new DataTable();
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void CbTablaTemporal_KeyDown(object sender, KeyEventArgs e)
      {
         
      }

      private void DgvDatosTablaTemporal_KeyUp(object sender, KeyEventArgs e)
      {
         try
         {
            if (e.KeyCode == Keys.Enter && CbTablaTemporal.SelectedIndex > -1)
            {
               if (!string.IsNullOrEmpty(CbTablaTemporal.Text))
               {
                  foreach (DataGridViewRow Fila in DgvDatosTablaTemporal.Rows)
                  {
                     try
                     {
                        if (Fila.Cells[0].FormattedValue.ToString() != string.Empty)
                        {
                           DataTable ResModificar = FuentesExternas.CRUDFuentesExternas(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "AgregarElementoFuente",
                                                      int.Parse(CbTablaTemporal.SelectedValue.ToString()), "", Fila.Cells[0].FormattedValue.ToString(),
                                                      bool.Parse(Fila.Cells[1].FormattedValue.ToString()));

                           if (ResModificar.Rows.Count > 0)
                           {
                              if (ResModificar.Rows[0]["Resultado"].ToString() == "0")
                              {
                                 ResModificar = FuentesExternas.CRUDFuentesExternas(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ActualizarEstadoElemento",
                                                      int.Parse(CbTablaTemporal.SelectedValue.ToString()), "", Fila.Cells[0].FormattedValue.ToString(),
                                                      bool.Parse(Fila.Cells[1].FormattedValue.ToString()));
                              }
                           }
                        }
                     }
                     catch
                     {

                     }
                  }

                  //Actualizar DatagridView Fuentes Externas

                  DataTable Externas = FuentesExternas.CRUDFuentesExternas(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "Consultar",
                                          int.Parse(CbTablaTemporal.SelectedValue.ToString()), "", "", false);
                  if (Externas.Rows.Count > 0)
                  {
                     DgvDatosTablaTemporal.AutoGenerateColumns = false;
                     DgvDatosTablaTemporal.DataSource = Externas;
                  }
                  else
                  {
                     DgvDatosTablaTemporal.AutoGenerateColumns = false;
                     DgvDatosTablaTemporal.DataSource = new DataTable();
                  }
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void BtnEliminarFuenteExterna_Click(object sender, EventArgs e)
      {
         try
         {
            if (!string.IsNullOrEmpty(CbTablaTemporal.Text))
            {
               DialogResult Respuesta = MessageBox.Show(Properties.Settings.Default.MsPreguntaEliminarFueneExterna, Globals.ThisAddIn.MensajeTitulos,
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);
               if (Respuesta == System.Windows.Forms.DialogResult.Yes)
               {
                  DataTable ResEliminar = FuentesExternas.CRUDFuentesExternas(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "Eliminar", int.Parse(CbTablaTemporal.SelectedValue.ToString()),
                                                  "", "", false);

                  if (ResEliminar.Rows.Count > 0)
                  {
                     if (ResEliminar.Rows[0]["Resultado"].ToString() == "1")
                     {
                        ConfiguracionInicial();
                     }
                     else
                     {
                        //MessageBox.Show(Properties.Settings.Default.MsErrorEliminarFuenteExterna + "\n\n" + ResEliminar.Rows[0]["Descripcion"].ToString(), Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show(Properties.Settings.Default.MsErrorEliminarFuenteExterna, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                  }
                  else
                  {
                     MessageBox.Show(Properties.Settings.Default.MsErrorEliminarFuenteExterna, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
               }
            }
            else
            {
               MessageBox.Show(Properties.Settings.Default.MsSeleccionarFuenteExterna, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      #endregion

      #region Descriptores

      private void BtnCrearDescriptor_Click(object sender, EventArgs e)
      {
         try
         {
            if (!string.IsNullOrEmpty(TbNuevoDescriptorNombre.Text) && !string.IsNullOrEmpty(CbValidacionNuevoDescriptor.Text) && TvEdicionDescriptores.SelectedNode != null)
            {
               int FuenteDatos = -1;

               if (CbFuenteDatosNuevoDescriptor.SelectedIndex > -1 && int.TryParse(CbFuenteDatosNuevoDescriptor.SelectedValue.ToString(), out FuenteDatos))
               {
                  FuenteDatos = int.Parse(CbFuenteDatosNuevoDescriptor.SelectedValue.ToString());
               }

               DataTable resGuardarDescriptor = Descriptores.GuardarDescriptorCategoria(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "Agregar", "-1", TbNuevoDescriptorNombre.Text,
                                                   TbNuevoDescriptorObservaciones.Text, CbValidacionNuevoDescriptor.SelectedValue.ToString(), TvEdicionDescriptores.SelectedNode.Name,
                                                   FuenteDatos.ToString());

               if (resGuardarDescriptor.Rows.Count > 0)
               {
                  if (resGuardarDescriptor.Rows[0]["Resultado"].ToString() == "1")
                  {
                     TbNuevoDescriptorNombre.Text = string.Empty;
                     TbNuevoDescriptorNombre.Enabled = true;
                     TbNuevoDescriptorObservaciones.Text = string.Empty;
                     CbFuenteDatosNuevoDescriptor.SelectedIndex = -1;
                     CbValidacionNuevoDescriptor.SelectedIndex = -1;

                     MessageBox.Show(resGuardarDescriptor.Rows[0]["Descripcion"].ToString(), Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     ConfiguracionInicial();
                  }
                  else
                  {
                     MessageBox.Show(Properties.Settings.Default.MsErrorAgregarDescriptor + "\n\n" + resGuardarDescriptor.Rows[0]["Descripcion"].ToString(), Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
               }
               else
               {
                  MessageBox.Show(Properties.Settings.Default.MsErrorAgregarDescriptor, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
            }
            else
            {
               MessageBox.Show(Properties.Settings.Default.MsCompletarDatosRequeridosDescriptor, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorAgregarDescriptor, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      #endregion

      private void listBox1_DoubleClick(object sender, EventArgs e)
      {
         try
         {
            string itemSeleccionado = LstBEditarElementos.SelectedItem.ToString();
            string Mensajeresultado = string.Empty;

            Excel.Worksheet HojaActiva = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;

            if (HojaActiva.ProtectContents)
            {
               MetodosRibbon.DesprotegerHoja(HojaActiva, Globals.ThisAddIn.ClaveProteccionHoja);
            }

            #region Opciones de diseño

            switch (itemSeleccionado)
            {
               case "Bloquear Celdas":
                  Excel.Range RangoBlo = (Excel.Range)Globals.ThisAddIn.Application.Selection;
                  RangoBlo.Locked = true;
                  Mensajeresultado = "Celdas Bloqueadas correctamente";
                  break;
               case "Desbloquear Celdas":
                  Excel.Range RangoDesBlo = (Excel.Range)Globals.ThisAddIn.Application.Selection;
                  RangoDesBlo.Locked = false;
                  Mensajeresultado = "Celdas desbloqueadas correctamente";
                  break;
               case "Eliminar Fila":
                  #region Eliminar Fila
                  Excel.Range CeldaActivaFila = (Excel.Range)Globals.ThisAddIn.Application.ActiveCell;
                  Excel.Range CeldaRealFila = CeldaActivaFila.get_Offset(1, 0);

                  bool canDeleteRow = true;
                  foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                  {
                     if (Control.RangoDatos.Row == CeldaActivaFila.Row)
                     {
                        canDeleteRow = false;
                     }
                  }
                  if (canDeleteRow)
                  {
                     CeldaActivaFila = CeldaActivaFila.EntireRow;
                     CeldaActivaFila.Delete(Excel.XlInsertShiftDirection.xlShiftDown);
                     Mensajeresultado = "Fila Eliminada correctamente.";
                  }
                  else
                  {
                     MessageBox.Show(Properties.Settings.Default.MsErrorEliminarFilaExcel, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
                  #endregion
                  break;
               case "Eliminar Columna":
                  #region Eliminar Columna
                  Excel.Range CeldaActivaColumna = (Excel.Range)Globals.ThisAddIn.Application.ActiveCell;

                  bool canDeleteColumn = true;
                  foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                  {
                     if (Control.RangoDatos.Column == CeldaActivaColumna.Column)
                     {
                        canDeleteColumn = false;
                     }
                  }

                  if (canDeleteColumn)
                  {
                     Excel.Range Columna = CeldaActivaColumna.EntireColumn;
                     Excel.Range CeldaReal = CeldaActivaColumna;
                     Columna.Delete(Excel.XlInsertShiftDirection.xlShiftToRight);
                     Mensajeresultado = "Columna Eliminada correctamente.";
                  }
                  else
                  {
                     MessageBox.Show(Properties.Settings.Default.MsErrorEliminarColumnaExcel, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
                  #endregion
                  break;
               case "Mostrar Celdas Protegidas":
                  foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                  {
                     Excel.Range RangoColor = Control.RangoDatos;
                     if (Control.RangoDatos.Locked)
                     {
                        RangoColor.Interior.Color = System.Drawing.Color.Bisque;
                     }
                  }
                  break;
               case "Ocultar Celdas Protegidas":
                  foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                  {
                     Color ColorFondoDescriptor = System.Drawing.Color.White;
                     if (Control.Obligatorio)
                     { ColorFondoDescriptor = System.Drawing.Color.MistyRose; }
                     else
                     { ColorFondoDescriptor = System.Drawing.Color.LemonChiffon; }

                     Excel.Range RangoColor = Control.RangoDatos;
                     if (Control.RangoDatos.Locked)
                     {
                        RangoColor.Interior.Color = ColorFondoDescriptor;
                     }
                  }
                  break;
            }
            if (!string.IsNullOrEmpty(Mensajeresultado))
            {
                MessageBox.Show(Mensajeresultado, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorOpcionesDiseñoExcel, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      #region Tablas de Informacion

      private void ClbElementosTabla_DoubleClick(object sender, EventArgs e)
      {
         try
         {
            DialogResult Respuesta = MessageBox.Show(Properties.Settings.Default.MsPreguntaEliminarDescriptorTabla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (Respuesta == System.Windows.Forms.DialogResult.Yes)
            {
               ClbElementosTabla.Items.RemoveAt(ClbElementosTabla.SelectedIndex);
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void BtnCrearTablaDatos_Click(object sender, EventArgs e)
      {
         try
         {
            int PruebaDias;
            if (!string.IsNullOrEmpty(TbNombreTabla.Text) && ClbElementosTabla.Items.Count > 0 && !string.IsNullOrEmpty(TbNumeroFilas.Text)
                && int.TryParse(TbNumeroFilas.Text, out PruebaDias))
            {
               #region Buscar Tablas Existentes
               int NumTablasExistentes = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c) { return c.TablaNombre == TbNombreTabla.Text; }).Count;
               #endregion

               if (NumTablasExistentes > 0)
               {
                  MessageBox.Show(string.Format(Properties.Settings.Default.MSCambiarNombreTablaDuplicado, TbNombreTabla.Text), Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               else
               {
                  HojaTrabajo = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
                  HojaTrabajoExtendida = Globals.Factory.GetVstoObject(HojaTrabajo);
                  Excel.Range RangoDatosInicial = Globals.ThisAddIn.Application.ActiveCell;
                  Excel.Range RangoReferencia = RangoDatosInicial.get_Offset(1, 1);

                  if (!string.IsNullOrEmpty(Globals.ThisAddIn.IdPlantillaFormato) && !string.IsNullOrEmpty(Globals.ThisAddIn.IdSubSeriePlantillaFormato))
                  {
                     Globals.ThisAddIn.IsUpdating = true;
                     Globals.ThisAddIn.Application.ScreenUpdating = false;

                     List<Controles> ControlesTabla = new List<Controles>();

                     #region Ajustar descriptores seleccionados para creación de elementos Tabla
                     for (int ciclo = 0; ciclo < ClbElementosTabla.Items.Count; ciclo++)
                     {
                        string IdDescriptor = ClbElementosTabla.Items[ciclo].ToString().Split(':')[0].Trim();
                        if (IdDescriptor != "-1")
                        {
                           //Traer Información del Descriptor
                           string Filtro = " DESid = '" + IdDescriptor + "' ";
                           DataView InfoDescriptor = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, Filtro, "", DataViewRowState.CurrentRows);
                           if (InfoDescriptor.Count > 0)
                           {
                              string TipoControl = Controles.TextBox;

                              if (bool.Parse(InfoDescriptor.ToTable().Rows[0]["DESfuenteDatos"].ToString()))
                              {
                                 TipoControl = Controles.ComboBusqueda;
                              }
                              else if (InfoDescriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarBool)
                              {
                                 TipoControl = Controles.CheckBox;
                              }

                              #region Si tiene fuente de datos temporal

                              if (!string.IsNullOrEmpty(InfoDescriptor.ToTable().Rows[0]["DESfuenteExterna"].ToString()))
                              {
                                 TipoControl = Controles.ListaDesplegable;
                              }
                              #endregion

                              ControlesTabla.Add(new Controles()
                              {
                                 Tipo = TipoControl,
                                 GUID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 28),
                                 Nombre = InfoDescriptor.ToTable().Rows[0]["DESdescripcion"].ToString(),
                                 Id = IdDescriptor,
                                 HojaExcel = HojaTrabajoExtendida,
                                 RangoDatos = RangoReferencia,
                                 TipoValidacion = InfoDescriptor.ToTable().Rows[0]["DTDdescripcion"].ToString(),
                                 IdSubSerie = Globals.ThisAddIn.IdSubSeriePlantillaFormato,
                                 IdFormato = Globals.ThisAddIn.IdPlantillaFormato,

                                 TablaNombre = TbNombreTabla.Text,
                                 TablaNumeroMaximoRegistros = int.Parse(TbNumeroFilas.Text),
                                 TablaFila = "1",
                                 TablaRangoInicial = RangoDatosInicial
                              });

                              if (TipoControl == Controles.ComboBusqueda || TipoControl == Controles.ControlDependencias)
                              { RangoReferencia = RangoReferencia.get_Offset(0, 2); }
                              else
                              { RangoReferencia = RangoReferencia.get_Offset(0, 1); }
                           }
                        }
                        else
                        {
                           ControlesTabla.Add(new Controles()
                           {
                              Tipo = Controles.TextBox,
                              GUID = "-1",
                              Nombre = "ColumnaVacia",
                              Id = "-1",
                              HojaExcel = HojaTrabajoExtendida,
                              RangoDatos = RangoReferencia,
                              TipoValidacion = ValidacionesDatos.ValidarTexto,
                              IdSubSerie = Globals.ThisAddIn.IdSubSeriePlantillaFormato,
                              IdFormato = Globals.ThisAddIn.IdPlantillaFormato,

                              TablaNombre = TbNombreTabla.Text,
                              TablaNumeroMaximoRegistros = int.Parse(TbNumeroFilas.Text),
                              TablaFila = "1",
                              TablaRangoInicial = RangoDatosInicial
                           });

                           RangoReferencia = RangoReferencia.get_Offset(0, 1);
                        }
                     }

                     #endregion

                     #region Crear Tabla

                     List<Controles> ControlesParaRegistrar = ControlesTabla.FindAll(delegate(Controles c) { return (c.Id != "-1" && c.GUID != "-1"); });

                     Globals.ThisAddIn.ControlesFormato.AddRange(ControlesTabla);

                     Excel.Range RangoTemp = RangoDatosInicial;
                     Globals.Ribbons.RibbonExcel.CrearFilaElementoTabla(HojaTrabajoExtendida, RangoDatosInicial, TbNombreTabla.Text, "1", true, false);

                     RangoTemp = RangoTemp.get_Offset(2, 1);

                     for (int ciclo = 2; ciclo <= int.Parse(TbNumeroFilas.Text); ciclo++)
                     {
                        Globals.Ribbons.RibbonExcel.CrearFilaElementoTabla(HojaTrabajoExtendida, RangoTemp, TbNombreTabla.Text, ciclo.ToString(), false, false);
                     }

                     MetodosRibbon.DesprotegerHoja(HojaTrabajo, Globals.ThisAddIn.ClaveProteccionHoja);

                     // Eliminar del registro las columnas vacias
                     Globals.ThisAddIn.ControlesFormato.RemoveAll(delegate(Controles c) { return (c.Id == "-1" && c.GUID == "-1"); });

                     #endregion

                     #region Ajustes Graficos de la tabla
                     //RangoDatosInicial = RangoDatosInicial.get_Offset(1, 0);
                     //int NumFilas = int.Parse(TbNumeroFilas.Text);

                     //// Pintar la tabla completa
                     //if (RangoReferencia.MergeCells)
                     //{
                     //   Excel.Range RangoLineasGrid = RangoReferencia.get_Offset(NumFilas + 1, 1);
                     //   RangoLineasGrid = RangoLineasGrid.get_Offset(0, -1);

                     //   Excel.Worksheet hojaActiva = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
                     //   Excel.Range RangoTabla = ((Excel.Range)hojaActiva.get_Range(RangoReferencia.Address, RangoLineasGrid.Address));

                     //   RangoTabla.Borders.Color = System.Drawing.ColorCategoria;
                     //   RangoTabla.Borders.LineStyle = 1;
                     //   RangoTabla.Borders.Weight = 2;
                     //}
                     #endregion

                     Globals.ThisAddIn.Application.ScreenUpdating = true;
                     Globals.ThisAddIn.IsUpdating = false;
                  }
                  else
                  {
                     MessageBox.Show(Properties.Settings.Default.MsErrorPlantillaNoValida, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
               }
            }
            else
            {
               MessageBox.Show(Properties.Settings.Default.MsCompletarInformacionRequeridaCreacionTabla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void ChbObligatorioTablaDatos_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            if (ClbElementosTabla.SelectedItem != null)
            {
               string InfoDescriptor = ClbElementosTabla.SelectedItem.ToString();

               if (!string.IsNullOrEmpty(InfoDescriptor) && !InfoDescriptor.Contains(this.Requerido))
               {
                  InfoDescriptor = InfoDescriptor + " - " + this.Requerido;
                  ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex] = InfoDescriptor;
               }
               if (!string.IsNullOrEmpty(InfoDescriptor) && InfoDescriptor.Contains(this.Requerido))
               {
                  InfoDescriptor = InfoDescriptor.Replace(" - " + this.Requerido, "");
                  ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex] = InfoDescriptor;
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      #endregion

      #region DgvEditar Elementos
      private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            if (tabControl1.SelectedTab.Name == "TbEditarElementos")
            {
               DgvEditarElementos.AutoGenerateColumns = false;

               BindingSource FuenteDatos = new BindingSource();
               FuenteDatos.DataSource = Globals.ThisAddIn.ControlesFormato;
               DgvEditarElementos.DataSource = FuenteDatos;
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void DgvEditarElementos_CellContentClick(object sender, DataGridViewCellEventArgs e)
      {
         try
         {
            //Principal
            if (e.ColumnIndex == 0 && e.RowIndex > -1)
            {
               // si ya existe un descriptor principal asignado
               if (ValidarDescriptorPrincipalCreado())
               {
                  MessageBox.Show(Properties.Settings.Default.MsInformacionDescriptorPrincipalRegistrado, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  Globals.ThisAddIn.ControlesFormato[e.RowIndex].Principal = false;
               }

               BindingSource FuenteDatos = new BindingSource();
               FuenteDatos.DataSource = Globals.ThisAddIn.ControlesFormato;
               DgvEditarElementos.DataSource = FuenteDatos;
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void DgvEditarElementos_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
      {
         try
         {
            e.Cancel = true;

            DialogResult Respuesta = MessageBox.Show(Properties.Settings.Default.MsEliminarControl, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Respuesta == System.Windows.Forms.DialogResult.Yes)
            {
               DgvEditarElementos.AutoGenerateColumns = false;

               string NombreDeTabla = Globals.ThisAddIn.ControlesFormato[e.Row.Index].TablaNombre;
               if (!string.IsNullOrEmpty(NombreDeTabla))
               {
                  //Elemento hace parte de una tabla
                  DialogResult RespuestaTabla = MessageBox.Show(Properties.Settings.Default.MsEliminarControlTabla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                  if (RespuestaTabla == System.Windows.Forms.DialogResult.Yes)
                  {
                     bool ResultadoEliminarTabla = true;

                     #region Eliminar Elementos de tabla

                     int NumControles = Globals.ThisAddIn.ControlesFormato.Count;

                     List<Controles> ControlesTabla = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c) { return c.TablaNombre == NombreDeTabla; });
                     foreach (Controles Control in ControlesTabla)
                     {
                        bool Resultado = Controles.EliminarControl(Control, true, true);
                        if (Resultado)
                        {
                           Globals.ThisAddIn.ControlesFormato.Remove(Control);
                        }
                        else
                        {
                           MessageBox.Show(string.Format(Properties.Settings.Default.MsErrorEliminarControl, Control.Nombre), Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                           ResultadoEliminarTabla = false;
                        }

                        BindingSource FuenteDatos = new BindingSource();
                        FuenteDatos.DataSource = Globals.ThisAddIn.ControlesFormato;
                        DgvEditarElementos.DataSource = FuenteDatos;
                     }

                     //Globals.ThisAddIn.ControlesFormato.RemoveAll(delegate(Controles c) { return c.TablaNombre == NombreDeTabla; });

                     if (ResultadoEliminarTabla)
                     {
                        MessageBox.Show(Properties.Settings.Default.MsControlesTablaEliminadosCorrectamente, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                     else
                     {
                        MessageBox.Show(Properties.Settings.Default.MsErrorEliminarControlesTabla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                     #endregion
                  }
               }
               else
               {
                  #region Eliminar controles individuales
                  HojaTrabajo = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
                  ExcelTools.Worksheet HojaTrabajoExtendida = Globals.Factory.GetVstoObject(HojaTrabajo);

                  bool Resultado = Controles.EliminarControl(Globals.ThisAddIn.ControlesFormato[e.Row.Index], true, true);

                  if (Resultado)
                  { Globals.ThisAddIn.ControlesFormato.RemoveAt(e.Row.Index); }

                  #endregion

                  BindingSource FuenteDatos = new BindingSource();
                  FuenteDatos.DataSource = Globals.ThisAddIn.ControlesFormato;
                  DgvEditarElementos.DataSource = FuenteDatos;
               }
            }
            else
            {
               e.Cancel = true;
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      #endregion

      #region Menu Contextual
      private void eliminarToolStripMenuItem1_Click(object sender, EventArgs e)
      {
         try
         {
            if (ClbElementosTabla.SelectedItem != null)
            {
               ClbElementosTabla.Items.RemoveAt(ClbElementosTabla.SelectedIndex);
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void subirItemToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try
         {
            if (ClbElementosTabla.SelectedItem != null)
            {
               if (ClbElementosTabla.SelectedIndex > 0)
               {
                  string ItemSuperior = ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex - 1].ToString();
                  ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex - 1] = ClbElementosTabla.SelectedItem;
                  ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex] = ItemSuperior;
                  ClbElementosTabla.SelectedIndex = ClbElementosTabla.SelectedIndex - 1;
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void bajarItemToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try
         {
            if (ClbElementosTabla.SelectedItem != null)
            {
               if (ClbElementosTabla.SelectedIndex < ClbElementosTabla.Items.Count - 1)
               {
                  string ItemInferior = ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex + 1].ToString();
                  ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex + 1] = ClbElementosTabla.SelectedItem;
                  ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex] = ItemInferior;
                  ClbElementosTabla.SelectedIndex = ClbElementosTabla.SelectedIndex + 1;
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void editarToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try
         {
            ////Editar descriptores
            //if (TvEdicionDescriptores.SelectedNode != null)
            //{
            //    if (TvEdicionDescriptores.SelectedNode.Nodes.Count == 0)
            //    {
            //        string FiltroDescriptor = " DESid = '" + TvEdicionDescriptores.SelectedNode.Name + "' ";
            //        DataView Filtro = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDescriptor, "", DataViewRowState.CurrentRows);

            //        if (Filtro.Count > 0)
            //        {
            //            TbNuevoDescriptorNombre.Text = Filtro.ToTable().Rows[0]["DESdescripcion"].ToString();
            //            TbNuevoDescriptorNombre.Enabled = false;
            //            TbNuevoDescriptorObservaciones.Text = Filtro.ToTable().Rows[0]["DESobservacion"].ToString();

            //            if (!string.IsNullOrEmpty(Filtro.ToTable().Rows[0]["DESfuenteExterna"].ToString()))
            //            {
            //                CbFuenteDatosNuevoDescriptor.SelectedValue = Filtro.ToTable().Rows[0]["DESfuenteExterna"].ToString();
            //            }

            //            if (!string.IsNullOrEmpty(Filtro.ToTable().Rows[0]["DTDdescripcion"].ToString()))
            //            {
            //                CbValidacionNuevoDescriptor.SelectedItem = Filtro.ToTable().Rows[0]["DTDdescripcion"].ToString();
            //            }
            //        }
            //    }
            //}
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void cancelarEdiciónToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try
         {
            TbNuevoDescriptorNombre.Text = string.Empty;
            TbNuevoDescriptorNombre.Enabled = true;
            TbNuevoDescriptorObservaciones.Text = string.Empty;
            CbFuenteDatosNuevoDescriptor.SelectedIndex = -1;
            CbValidacionNuevoDescriptor.SelectedIndex = -1;
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try
         {
            if (TvEdicionDescriptores.SelectedNode != null)
            {
               //Desea eliminar el elemento: " + TvEdicionDescriptores.SelectedNode.Text + " ?
               DialogResult Respuesta = MessageBox.Show(string.Format(Properties.Settings.Default.MsConfirmarEliminarDescriptor, TvEdicionDescriptores.SelectedNode.Text), Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
               if (Respuesta == System.Windows.Forms.DialogResult.Yes)
               {
                  try
                  {
                     string Descriptor = TvEdicionDescriptores.SelectedNode.Name;
                     string Categoria = TvEdicionDescriptores.SelectedNode.Parent.Name;

                     DataTable resGuardarDescriptor = Descriptores.GuardarDescriptorCategoria(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "Eliminar", Descriptor, "",
                                                     "", "", Categoria, "-1");

                     if (resGuardarDescriptor.Rows.Count > 0)
                     {
                        MessageBox.Show(resGuardarDescriptor.Rows[0]["Descripcion"].ToString(), Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (resGuardarDescriptor.Rows[0]["Resultado"].ToString() == "1")
                        {
                           ConfiguracionInicial();
                        }
                     }
                     else
                     {
                        MessageBox.Show(Properties.Settings.Default.MsErrorEliminarDescriptorSGD, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                  }
                  catch
                  {
                     MessageBox.Show(Properties.Settings.Default.MsErrorEliminarDescriptorSGD, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void añadirToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try
         {
            if (TvDescriptoresTabla.SelectedNode != null)
            {
               string NombreElemento = string.Empty;
               DataTable Descriptor = Plantillas.ConsultaDescriptores(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarInformacionDescriptores", TvDescriptoresTabla.SelectedNode.Name);

               if (Descriptor.Rows.Count > 0 && Descriptor.Columns.Count > 2)
               {
                  wsOfficeSGD wsServicio = new wsOfficeSGD();
                  wsServicio.Url = Globals.ThisAddIn.DatosConexion.urlWsOfficeSGD;
                  wsServicio.Timeout = Globals.ThisAddIn.DatosConexion.TimeOut;

                  DataEncryption Enc = new DataEncryption();
                  Login Usuario = Globals.ThisAddIn.DatosUsuario;
                  Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                  XmlNode XmlNode = wsServicio.ConsultarPrecedenciasDescriptor(SessionID, TvDescriptoresTabla.SelectedNode.Name);
                  
                  DataTable PrecedenciasDescriptor = XML.XMLtoDataTable(XmlNode);

                  if (PrecedenciasDescriptor.Rows.Count > 0 && PrecedenciasDescriptor.Columns.Count > 2)
                  {
                     foreach (DataRow Fila in PrecedenciasDescriptor.Rows)
                     {
                        string Precedencia = Fila["DESfuenteDatosVariable"].ToString();
                        string IdPrecedencia = Fila["codigo"].ToString();
                        if (Precedencia.Contains("@"))
                        {
                           NombreElemento = IdPrecedencia + " : " + Fila["DESdescripcion"].ToString().Replace(":", "") + " : " + Precedencia;
                        }
                        else
                        {
                           NombreElemento = IdPrecedencia + " : " + Fila["DESdescripcion"].ToString().Replace(":", "");
                        }

                        ClbElementosTabla.Items.Add(NombreElemento, true);
                     }
                  }
                  else
                  {
                     NombreElemento = TvDescriptoresTabla.SelectedNode.Name + " - " + TvDescriptoresTabla.SelectedNode.Text.Replace(":", "");
                     ClbElementosTabla.Items.Add(NombreElemento, true);
                  }
               }
            }
            else
            {
               MessageBox.Show(Properties.Settings.Default.MsRequeridoAgregarDescriptor, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         catch(Exception exc)
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorAgregarDescriptor, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Utilidades.ReportarError(exc);
         }
      }

      private void obligatorioToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try
         {
            if (ClbElementosTabla.SelectedItem != null)
            {
               if (ClbElementosTabla.SelectedItem.ToString().Contains(" - " + this.Requerido))
               {
                  ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex] = ClbElementosTabla.SelectedItem.ToString().Replace(" - " + this.Requerido, "");
               }
               else
               {
                  ClbElementosTabla.Items[ClbElementosTabla.SelectedIndex] = ClbElementosTabla.SelectedItem.ToString() + " - " + this.Requerido;
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void crearColumnaVaciaToolStripMenuItem_Click(object sender, EventArgs e)
      {
         try
         {
            ClbElementosTabla.Items.Add("-1 : Columna vacia");
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }
      #endregion

      #region TreeViewCategorias

      private void TVCategorias_AfterCollapse(object sender, TreeViewEventArgs e)
      {
         try
         {
            e.Node.Nodes.Clear();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void TvEdicionDescriptores_AfterCollapse(object sender, TreeViewEventArgs e)
      {
         try
         {
            e.Node.Nodes.Clear();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void TvDescriptoresTabla_AfterCollapse(object sender, TreeViewEventArgs e)
      {
         try
         {
            e.Node.Nodes.Clear();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void TvDescriptoresTabla_KeyPress(object sender, KeyPressEventArgs e)
      {
         try
         {
            if (e.KeyChar == (char)Keys.Enter)
            {
               try
               {
                  if (TvDescriptoresTabla.SelectedNode != null)
                  {
                     if (!ElementosTabla.Contains(TvDescriptoresTabla.SelectedNode.Name))
                     {
                        string NombreElemento = string.Empty;
                        DataTable Descriptor = Plantillas.ConsultaDescriptores(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarInformacionDescriptores", TvDescriptoresTabla.SelectedNode.Name);

                        if (Descriptor.Rows.Count > 0 && Descriptor.Columns.Count > 2)
                        {
                           wsOfficeSGD wsServicio = new wsOfficeSGD();
                           wsServicio.Url = Globals.ThisAddIn.DatosConexion.urlWsOfficeSGD;
                           wsServicio.Timeout = Globals.ThisAddIn.DatosConexion.TimeOut;

                           DataEncryption Enc = new DataEncryption();
                           Login Usuario = Globals.ThisAddIn.DatosUsuario;
                           Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

                           XmlNode XmlNode = wsServicio.ConsultarPrecedenciasDescriptor(SessionID, TvDescriptoresTabla.SelectedNode.Name);

                           DataTable PrecedenciasDescriptor = XML.XMLtoDataTable(XmlNode);

                           if (PrecedenciasDescriptor.Rows.Count > 0 && PrecedenciasDescriptor.Columns.Count > 2)
                           {
                              foreach (DataRow Fila in PrecedenciasDescriptor.Rows)
                              {
                                 //string Precedencia = Fila["DESfuenteDatosVariable"].ToString();

                                 string IdPrecedencia = Fila["codigo"].ToString();
                                 
                                 //if (Precedencia.Contains("@"))
                                 //{
                                 //   NombreElemento = IdPrecedencia + " : " + Fila["DESdescripcion"].ToString() + " : " + Precedencia;
                                 //}
                                 //else
                                 //{
                                 //   NombreElemento = IdPrecedencia + " : " + Fila["DESdescripcion"].ToString();
                                 //}

                                 NombreElemento = IdPrecedencia + " : " + Fila["DESdescripcion"].ToString();

                                 if (!ElementosTabla.Contains(NombreElemento))
                                 {
                                    ClbElementosTabla.Items.Add(NombreElemento, true);
                                    ElementosTabla.Add(NombreElemento);
                                 }
                                 else
                                 {
                                    MessageBox.Show(Properties.Settings.Default.MsErrorAgregarDescriptorTabla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                 }
                              }
                           }
                           else
                           {
                              NombreElemento = TvDescriptoresTabla.SelectedNode.Name + " - " + TvDescriptoresTabla.SelectedNode.Text;
                              if (!ElementosTabla.Contains(NombreElemento))
                              {
                                 ClbElementosTabla.Items.Add(NombreElemento, true);
                                 ElementosTabla.Add(NombreElemento);
                              }
                              else
                              {
                                 MessageBox.Show(Properties.Settings.Default.MsErrorAgregarDescriptorTabla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                              }
                           }
                        }

                        ElementosTabla.Add(TvDescriptoresTabla.SelectedNode.Name);
                     }
                     else
                     {
                        MessageBox.Show(Properties.Settings.Default.MsErrorAgregarDescriptorTabla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                  }
                  else
                  {
                     MessageBox.Show(Properties.Settings.Default.MsRequeridoAgregarDescriptor, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
               }
               catch
               {
                  MessageBox.Show(Properties.Settings.Default.MsErrorAgregarDescriptor, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void TVCategorias_Click(object sender, EventArgs e)
      {
         try
         {
            #region Evento AfterSelect
            if (TVCategorias.SelectedNode != null )
            {
               TreeNode Nodo = TVCategorias.SelectedNode;
               Nodo.Nodes.Clear();
               int nivel = Nodo.Level;

               string CategoriaInicial = Nodo.Name;
               bool TieneCategorias = false;
               bool TieneDescriptores = false;

               string Filtro = " Len([DCAcodigo]) = Len('" + CategoriaInicial + "')+2 And [DCAcodigo] Like '" + CategoriaInicial + "'+'%' ";
               DataView FiltroCategorias = new DataView(Globals.Ribbons.RibbonExcel.DatosCategorias, Filtro, "", DataViewRowState.CurrentRows);

               //Busqueda de  categorias
               if (FiltroCategorias.Count > 0)
               {
                  foreach (DataRow Fila in FiltroCategorias.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DCAcodigo"].ToString(), Fila["DCAdescripcion"].ToString());
                  }
                  TieneCategorias = true;
               }

               //Busqueda de Descriptores
               string FiltroDes = " DCAcodigo = '" + CategoriaInicial + "' ";
               DataView FiltroDescriptores = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDes, "", DataViewRowState.CurrentRows);

               if (FiltroDescriptores.Count > 0)
               {
                  foreach (DataRow Fila in FiltroDescriptores.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DESid"].ToString(), Fila["DESdescripcion"].ToString());
                  }
                  TieneDescriptores = true;
               }

               if (!TieneDescriptores && !TieneCategorias)
               {
                  #region Actualizar Informacion de descriptor
                  if (TVCategorias.SelectedNode != null)
                  {
                     string FiltroDato = " DESid = '" + TVCategorias.SelectedNode.Name + "' ";
                     DataView Descriptor = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDato, "", DataViewRowState.CurrentRows);

                     if (Descriptor.Count > 0)
                     {
                        Nodo.ForeColor = ColorCategoria;

                        // Verificar si tiene fuente de datos el descriptor
                        ChbFuenteDatos.Checked = bool.Parse(Descriptor.ToTable().Rows[0]["DESfuenteDatos"].ToString());

                        #region Seleccionar el tipo de  validación del descriptor
                        if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString()))
                        {
                           LbTipoDato.Text = Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString();
                           CbTipoValidacion.Text = Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString();
                           CbTipoValidacion.Enabled = false;
                        }
                        else
                        {
                           CbTipoValidacion.Enabled = true;
                        }
                        #endregion

                        #region Sugerir Caja de texto como predeterminada
                        if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString()))
                        {
                           if (Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarFecha
                               || Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarTexto
                               || Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarNumero)
                           {
                              if (string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DESfuenteExterna"].ToString()))
                              {
                                 CbTipoElemento.Text = Controles.TextBox;
                              }
                           }
                        }
                        #endregion

                        #region Si tiene fuente de datos, sugerir inicialmente combo búsqueda
                        if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DESfuenteDatos"].ToString()))
                        {
                           if (bool.Parse(Descriptor.ToTable().Rows[0]["DESfuenteDatos"].ToString()))
                           {
                              CbTipoElemento.Text = Controles.ComboBusqueda;
                           }
                        }
                        #endregion

                        #region Limitar  el tipo de control  para descriptores con dependencias
                        if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DPRdescriptorPrecedencia"].ToString()))
                        {
                           CbTipoElemento.Text = Controles.ControlDependencias;
                           CbTipoElemento.Enabled = false;
                        }
                        else
                        {
                           CbTipoElemento.Enabled = true;
                        }
                        #endregion

                        #region Si el tipo de datos es Bool, solo se admite en control CheckBox
                        if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString()))
                        {
                           if (Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarBool)
                           {
                              CbTipoElemento.Text = Controles.CheckBox;
                              CbTipoElemento.Enabled = false;
                           }
                        }
                        #endregion

                        #region Verificar si tiene Fuentes Externas
                        if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DESfuenteExterna"].ToString()))
                        {
                           ChbFuenteDatos.Enabled = false;
                           ChbFuenteDatos.Checked = true;
                           CbTipoElemento.Text = Controles.ListaDesplegable;
                           CbTipoElemento.Enabled = false;
                        }
                        else
                        {
                           ChbFuenteDatos.Enabled = false;
                        }
                        #endregion

                        #region Sugerir caja de texto para descriptores Tipo hora
                        if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString()))
                        {
                           if (Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarHora)
                           {
                              CbTipoElemento.Text = Controles.TextBox;
                           }
                        }

                        #endregion

                        GpbCrearElemento.Visible = true;
                     }
                     else
                     {
                        GpbCrearElemento.Visible = false;
                        Nodo.ForeColor = ColorDescriptor;
                     }
                  }
                  #endregion
               }
               else if (TieneDescriptores || TieneCategorias)
               {
                  GpbCrearElemento.Visible = false;
                  Nodo.ForeColor = ColorDescriptor;
                  LbTipoDato.Text = "Seleccione";
               }
               else
               {
                  GpbCrearElemento.Visible = false;
                  Nodo.ForeColor = ColorDescriptor;
                  LbTipoDato.Text = "Seleccione";
               }
            }
            #endregion
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorActualizarArbolCategorias, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      private void TVCategorias_AfterSelect(object sender, TreeViewEventArgs e)
      {
          try
          {
              #region Evento AfterSelect
              if (TVCategorias.SelectedNode != null)
              {
                  TreeNode Nodo = TVCategorias.SelectedNode;
                  Nodo.Nodes.Clear();
                  int nivel = Nodo.Level;

                  string CategoriaInicial = Nodo.Name;
                  bool TieneCategorias = false;
                  bool TieneDescriptores = false;

                  string Filtro = " Len([DCAcodigo]) = Len('" + CategoriaInicial + "')+2 And [DCAcodigo] Like '" + CategoriaInicial + "'+'%' ";
                  DataView FiltroCategorias = new DataView(Globals.Ribbons.RibbonExcel.DatosCategorias, Filtro, "", DataViewRowState.CurrentRows);

                  //Busqueda de  categorias
                  if (FiltroCategorias.Count > 0)
                  {
                      foreach (DataRow Fila in FiltroCategorias.ToTable().Rows)
                      {
                          Nodo.Nodes.Add(Fila["DCAcodigo"].ToString(), Fila["DCAdescripcion"].ToString());
                      }
                      TieneCategorias = true;
                  }

                  //Busqueda de Descriptores
                  string FiltroDes = " DCAcodigo = '" + CategoriaInicial + "' ";
                  DataView FiltroDescriptores = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDes, "", DataViewRowState.CurrentRows);

                  if (FiltroDescriptores.Count > 0)
                  {
                      foreach (DataRow Fila in FiltroDescriptores.ToTable().Rows)
                      {
                          Nodo.Nodes.Add(Fila["DESid"].ToString(), Fila["DESdescripcion"].ToString());
                      }
                      TieneDescriptores = true;
                  }

                  if (!TieneDescriptores && !TieneCategorias)
                  {
                      #region Actualizar Informacion de descriptor
                      if (TVCategorias.SelectedNode != null)
                      {
                          string FiltroDato = " DESid = '" + TVCategorias.SelectedNode.Name + "' ";
                          DataView Descriptor = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDato, "", DataViewRowState.CurrentRows);

                          if (Descriptor.Count > 0)
                          {
                              Nodo.ForeColor = ColorCategoria;

                              // Verificar si tiene fuente de datos el descriptor
                              ChbFuenteDatos.Checked = bool.Parse(Descriptor.ToTable().Rows[0]["DESfuenteDatos"].ToString());

                              #region Seleccionar el tipo de  validación del descriptor
                              if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString()))
                              {
                                  LbTipoDato.Text = Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString();
                                  CbTipoValidacion.Text = Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString();
                                  CbTipoValidacion.Enabled = false;
                              }
                              else
                              {
                                  CbTipoValidacion.Enabled = true;
                              }
                              #endregion

                              #region Sugerir Caja de texto como predeterminada
                              if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString()))
                              {
                                  if (Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarFecha
                                      || Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarTexto
                                      || Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarNumero)
                                  {
                                      if (string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DESfuenteExterna"].ToString()))
                                      {
                                          CbTipoElemento.Text = Controles.TextBox;
                                      }
                                  }
                              }
                              #endregion

                              #region Si tiene fuente de datos, sugerir inicialmente combo búsqueda
                              if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DESfuenteDatos"].ToString()))
                              {
                                  if (bool.Parse(Descriptor.ToTable().Rows[0]["DESfuenteDatos"].ToString()))
                                  {
                                      CbTipoElemento.Text = Controles.ComboBusqueda;
                                  }
                              }
                              #endregion

                              #region Limitar  el tipo de control  para descriptores con dependencias
                              if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DPRdescriptorPrecedencia"].ToString()))
                              {
                                  CbTipoElemento.Text = Controles.ControlDependencias;
                                  CbTipoElemento.Enabled = false;
                              }
                              else
                              {
                                  CbTipoElemento.Enabled = true;
                              }
                              #endregion

                              #region Si el tipo de datos es Bool, solo se admite en control CheckBox
                              if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString()))
                              {
                                  if (Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarBool)
                                  {
                                      CbTipoElemento.Text = Controles.CheckBox;
                                      CbTipoElemento.Enabled = false;
                                  }
                              }
                              #endregion

                              #region Verificar si tiene Fuentes Externas
                              if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DESfuenteExterna"].ToString()))
                              {
                                  ChbFuenteDatos.Enabled = false;
                                  ChbFuenteDatos.Checked = true;
                                  CbTipoElemento.Text = Controles.ListaDesplegable;
                                  CbTipoElemento.Enabled = false;
                              }
                              else
                              {
                                  ChbFuenteDatos.Enabled = false;
                              }
                              #endregion

                              #region Sugerir caja de texto para descriptores Tipo hora
                              if (!string.IsNullOrEmpty(Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString()))
                              {
                                  if (Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString() == ValidacionesDatos.ValidarHora)
                                  {
                                      CbTipoElemento.Text = Controles.TextBox;
                                  }
                              }

                              #endregion

                              GpbCrearElemento.Visible = true;
                          }
                          else
                          {
                              GpbCrearElemento.Visible = false;
                              Nodo.ForeColor = ColorDescriptor;
                          }
                      }
                      #endregion
                  }
                  else if (TieneDescriptores || TieneCategorias)
                  {
                      GpbCrearElemento.Visible = false;
                      Nodo.ForeColor = ColorDescriptor;
                      LbTipoDato.Text = "Seleccione";
                  }
                  else
                  {
                      GpbCrearElemento.Visible = false;
                      Nodo.ForeColor = ColorDescriptor;
                      LbTipoDato.Text = "Seleccione";
                  }
              }
              #endregion
          }
          catch
          {
              MessageBox.Show(Properties.Settings.Default.MsErrorActualizarArbolCategorias, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
          }
      }

      private void TvDescriptoresTabla_Click(object sender, EventArgs e)
      {
         try
         {
            #region Evento AfterSelect
            if (TvDescriptoresTabla.SelectedNode != null)
            {
               TreeNode Nodo = TvDescriptoresTabla.SelectedNode;
               int nivel = Nodo.Level;
               Nodo.Nodes.Clear();

               bool TieneDescriptores = false;
               bool TieneCategorias = false;

               string CategoriaInicial = Nodo.Name;

               string Filtro = " Len([DCAcodigo]) = Len('" + CategoriaInicial + "')+2 And [DCAcodigo] Like '" + CategoriaInicial + "'+'%' ";
               DataView FiltroCategorias = new DataView(Globals.Ribbons.RibbonExcel.DatosCategorias, Filtro, "", DataViewRowState.CurrentRows);

               //Busqueda de  categorias
               if (FiltroCategorias.Count > 0)
               {
                  foreach (DataRow Fila in FiltroCategorias.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DCAcodigo"].ToString(), Fila["DCAdescripcion"].ToString());
                     Nodo.ForeColor = ColorCategoria;
                  }
                  TieneDescriptores = true;
               }

               //Busqueda de Descriptores
               string FiltroDes = " DCAcodigo = '" + CategoriaInicial + "' ";
               DataView FiltroDescriptores = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDes, "", DataViewRowState.CurrentRows);

               if (FiltroDescriptores.Count > 0)
               {
                  foreach (DataRow Fila in FiltroDescriptores.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DESid"].ToString(), Fila["DESdescripcion"].ToString());
                     Nodo.ForeColor = ColorDescriptor;
                  }
                  TieneCategorias = true;
               }

               if (!TieneCategorias && !TieneDescriptores)
               {
                  if (TVCategorias.SelectedNode != null)
                  {
                     string FiltroDato = " DESid = '" + TvDescriptoresTabla.SelectedNode.Name + "' ";
                     DataView Descriptor = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDato, "", DataViewRowState.CurrentRows);

                     if (Descriptor.Count > 0)
                     {
                        Nodo.ForeColor = ColorCategoria;
                     }
                     else
                     {
                        Nodo.ForeColor = ColorDescriptor;
                     }
                  }
               }
            }
            #endregion
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorActualizarArbolCategorias, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }
     
      private void TvDescriptoresTabla_AfterSelect(object sender, TreeViewEventArgs e)
      {
         try
         {
            #region Evento AfterSelect
            if (TvDescriptoresTabla.SelectedNode != null)
            {
               TreeNode Nodo = TvDescriptoresTabla.SelectedNode;
               int nivel = Nodo.Level;
               Nodo.Nodes.Clear();

               bool TieneDescriptores = false;
               bool TieneCategorias = false;

               string CategoriaInicial = Nodo.Name;

               string Filtro = " Len([DCAcodigo]) = Len('" + CategoriaInicial + "')+2 And [DCAcodigo] Like '" + CategoriaInicial + "'+'%' ";
               DataView FiltroCategorias = new DataView(Globals.Ribbons.RibbonExcel.DatosCategorias, Filtro, "", DataViewRowState.CurrentRows);

               //Busqueda de  categorias
               if (FiltroCategorias.Count > 0)
               {
                  foreach (DataRow Fila in FiltroCategorias.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DCAcodigo"].ToString(), Fila["DCAdescripcion"].ToString());
                     Nodo.ForeColor = ColorCategoria;
                  }
                  TieneDescriptores = true;
               }

               //Busqueda de Descriptores
               string FiltroDes = " DCAcodigo = '" + CategoriaInicial + "' ";
               DataView FiltroDescriptores = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDes, "", DataViewRowState.CurrentRows);

               if (FiltroDescriptores.Count > 0)
               {
                  foreach (DataRow Fila in FiltroDescriptores.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DESid"].ToString(), Fila["DESdescripcion"].ToString());
                     Nodo.ForeColor = ColorDescriptor;
                  }
                  TieneCategorias = true;
               }

               if (!TieneCategorias && !TieneDescriptores)
               {
                  if (TVCategorias.SelectedNode != null)
                  {
                     string FiltroDato = " DESid = '" + TvDescriptoresTabla.SelectedNode.Name + "' ";
                     DataView Descriptor = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDato, "", DataViewRowState.CurrentRows);

                     if (Descriptor.Count > 0)
                     {
                        Nodo.ForeColor = ColorCategoria;
                     }
                     else
                     {
                        Nodo.ForeColor = ColorDescriptor;
                     }
                  }
               }
            }
            #endregion
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorActualizarArbolCategorias, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      private void TvEdicionDescriptores_Click(object sender, EventArgs e)
      {
         try
         {
            #region Evento AfterSelect
            if (TvEdicionDescriptores.SelectedNode != null)
            {
               TreeNode Nodo = TvEdicionDescriptores.SelectedNode;
               Nodo.Nodes.Clear();
               int nivel = Nodo.Level;
               bool TieneCategorias = false;
               bool TieneDescriptores = false;
               string CategoriaInicial = Nodo.Name;

               string Filtro = " Len([DCAcodigo]) = Len('" + CategoriaInicial + "')+2 And [DCAcodigo] Like '" + CategoriaInicial + "'+'%' ";
               DataView FiltroCategorias = new DataView(Globals.Ribbons.RibbonExcel.DatosCategorias, Filtro, "", DataViewRowState.CurrentRows);

               //Busqueda de  categorias
               if (FiltroCategorias.Count > 0)
               {
                  foreach (DataRow Fila in FiltroCategorias.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DCAcodigo"].ToString(), Fila["DCAdescripcion"].ToString());
                     Nodo.ForeColor = ColorCategoria;
                  }
               }

               //Busqueda de Descriptores
               string FiltroDes = " DCAcodigo = '" + CategoriaInicial + "' ";
               DataView FiltroDescriptores = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDes, "", DataViewRowState.CurrentRows);

               if (FiltroDescriptores.Count > 0)
               {
                  foreach (DataRow Fila in FiltroDescriptores.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DESid"].ToString(), Fila["DESdescripcion"].ToString());
                     Nodo.ForeColor = ColorDescriptor;
                  }
               }

               if (!TieneCategorias && !TieneDescriptores)
               {
                  if (TVCategorias.SelectedNode != null)
                  {
                     string FiltroDato = " DESid = '" + TvEdicionDescriptores.SelectedNode.Name + "' ";
                     DataView Descriptor = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDato, "", DataViewRowState.CurrentRows);

                     if (Descriptor.Count > 0)
                     {
                        Nodo.ForeColor = ColorCategoria;
                     }
                     else
                     {
                        Nodo.ForeColor = ColorDescriptor;
                     }
                  }
               }
            }
            #endregion
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorActualizarArbolCategorias, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }

      private void TvEdicionDescriptores_AfterSelect(object sender, TreeViewEventArgs e)
      {
         try
         {
            #region Evento AfterSelect
            if (TvEdicionDescriptores.SelectedNode != null)
            {
               TreeNode Nodo = TvEdicionDescriptores.SelectedNode;
               Nodo.Nodes.Clear();
               int nivel = Nodo.Level;
               bool TieneCategorias = false;
               bool TieneDescriptores = false;
               string CategoriaInicial = Nodo.Name;

               string Filtro = " Len([DCAcodigo]) = Len('" + CategoriaInicial + "')+2 And [DCAcodigo] Like '" + CategoriaInicial + "'+'%' ";
               DataView FiltroCategorias = new DataView(Globals.Ribbons.RibbonExcel.DatosCategorias, Filtro, "", DataViewRowState.CurrentRows);

               //Busqueda de  categorias
               if (FiltroCategorias.Count > 0)
               {
                  foreach (DataRow Fila in FiltroCategorias.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DCAcodigo"].ToString(), Fila["DCAdescripcion"].ToString());
                     Nodo.ForeColor = ColorCategoria;
                  }
               }

               //Busqueda de Descriptores
               string FiltroDes = " DCAcodigo = '" + CategoriaInicial + "' ";
               DataView FiltroDescriptores = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDes, "", DataViewRowState.CurrentRows);

               if (FiltroDescriptores.Count > 0)
               {
                  foreach (DataRow Fila in FiltroDescriptores.ToTable().Rows)
                  {
                     Nodo.Nodes.Add(Fila["DESid"].ToString(), Fila["DESdescripcion"].ToString());
                     Nodo.ForeColor = ColorDescriptor;
                  }
               }

               if (!TieneCategorias && !TieneDescriptores)
               {
                  if (TVCategorias.SelectedNode != null)
                  {
                     string FiltroDato = " DESid = '" + TvEdicionDescriptores.SelectedNode.Name + "' ";
                     DataView Descriptor = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDato, "", DataViewRowState.CurrentRows);

                     if (Descriptor.Count > 0)
                     {
                        Nodo.ForeColor = ColorCategoria;
                     }
                     else
                     {
                        Nodo.ForeColor = ColorDescriptor;
                     }
                  }
               }
            }
            #endregion
         }
         catch
         {
            MessageBox.Show(Properties.Settings.Default.MsErrorActualizarArbolCategorias, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }
      #endregion

      #region Métodos específicos
      private void ConfiguracionInicial()
      {
         //Actualizar Información
         MetodosRibbon.ActualizarFuentesInformacionDescriptores();

         CbTipoElemento.DataSource = Plantillas.ConsultarTiposElementos();

         #region Actualizar Combos de Tab Descriptores

         DataTable DatosTipoDescriptor = Plantillas.ConsultaDescriptores(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "ConsultarTiposDatoDescriptor", "-1");

         if (DatosTipoDescriptor.Rows.Count > 0)
         {
            CbValidacionNuevoDescriptor.ValueMember = "DTDcodigo";
            CbValidacionNuevoDescriptor.DisplayMember = "DTDdescripcion";
            CbValidacionNuevoDescriptor.DataSource = DatosTipoDescriptor;

            CbTipoValidacion.ValueMember = "DTDcodigo";
            CbTipoValidacion.DisplayMember = "DTDdescripcion";
            CbTipoValidacion.DataSource = DatosTipoDescriptor;
         }
         else
         {
            CbValidacionNuevoDescriptor.ValueMember = "DTDcodigo";
            CbValidacionNuevoDescriptor.DisplayMember = "DTDdescripcion";
            CbValidacionNuevoDescriptor.DataSource = new DataTable();

            CbTipoValidacion.ValueMember = "DTDcodigo";
            CbTipoValidacion.DisplayMember = "DTDdescripcion";
            CbTipoValidacion.DataSource = new DataTable();
         }

         #endregion

         #region Actualizar Categorias Descriptores
         if (Globals.Ribbons.RibbonExcel.DatosCategorias.Rows.Count > 0)
         {
            TVCategorias.Nodes.Clear();
            TvEdicionDescriptores.Nodes.Clear();
            TvDescriptoresTabla.Nodes.Clear();

            foreach (DataRow Fila in Globals.Ribbons.RibbonExcel.DatosCategorias.Rows)
            {
               if (Fila["DCAcodigo"].ToString().Length == 2)
               {
                  TVCategorias.Nodes.Add(Fila["DCAcodigo"].ToString(), Fila["DCAdescripcion"].ToString());
                  TvEdicionDescriptores.Nodes.Add(Fila["DCAcodigo"].ToString(), Fila["DCAdescripcion"].ToString());
                  TvDescriptoresTabla.Nodes.Add(Fila["DCAcodigo"].ToString(), Fila["DCAdescripcion"].ToString());

                  TVCategorias.Nodes[TVCategorias.Nodes.Count - 1].ForeColor = ColorDescriptor;
                  TvEdicionDescriptores.Nodes[TVCategorias.Nodes.Count - 1].ForeColor = ColorDescriptor;
                  TvDescriptoresTabla.Nodes[TVCategorias.Nodes.Count - 1].ForeColor = ColorDescriptor;
               }
            }
         }


         #endregion

         #region Actualizar Fuentes externas

         if (Globals.Ribbons.RibbonExcel.DatosFuentesExternas.Rows.Count > 0)
         {
            CbTablaTemporal.DisplayMember = "DFEdescripcion";
            CbTablaTemporal.ValueMember = "DFEid";
            CbTablaTemporal.DataSource = Globals.Ribbons.RibbonExcel.DatosFuentesExternas;
            CbTablaTemporal.SelectedIndex = -1;

            CbFuenteDatosNuevoDescriptor.DisplayMember = "DFEdescripcion";
            CbFuenteDatosNuevoDescriptor.ValueMember = "DFEid";
            CbFuenteDatosNuevoDescriptor.DataSource = Globals.Ribbons.RibbonExcel.DatosFuentesExternas;
            CbFuenteDatosNuevoDescriptor.SelectedIndex = -1;
         }
         else
         {
            CbTablaTemporal.DisplayMember = "DFEdescripcion";
            CbTablaTemporal.ValueMember = "DFEid";
            CbTablaTemporal.DataSource = new DataTable();
         }


         #endregion
      }

      /// <summary>
      /// Revisa todos los prerequisitos que se deben cumplir para crear un elemento
      /// </summary>
      /// <returns>Estado final de la operacion</returns>
      private bool ValidacionesPreviasCreacionElementos()
      {
         bool Validaciones = false;
         try
         {
            #region Validaciones
            string MensajeResultado = string.Empty;

            string FiltroDato = " DESid = '" + TVCategorias.SelectedNode.Name + "' ";
            DataView Descriptor = new DataView(Globals.Ribbons.RibbonExcel.DatosDescriptores, FiltroDato, "", DataViewRowState.CurrentRows);
            if (Descriptor.Count == 0)
            {
               MensajeResultado = " - El elemento seleccionado no es un descriptor válido.\n";
            }

            if (string.IsNullOrEmpty(TVCategorias.SelectedNode.Text) || TVCategorias.SelectedNode.Name == "-1" || string.IsNullOrEmpty(CbTipoElemento.Text))
            {
               MensajeResultado = " - Los Campos Descriptor y Tipo de Elemento son obligatorios.\n";
            }

            if (CbTipoElemento.Text == Controles.ListaDesplegable || CbTipoElemento.Text == Controles.ListBox
                    || CbTipoElemento.Text == Controles.ComboBusqueda || CbTipoElemento.Text == Controles.ControlDependencias)
            {
               if (ChbFuenteDatos.Checked == false)
               {
                  MensajeResultado = MensajeResultado + " - El control necesita una fuente de datos.\n";
               }
            }

            if (CbTipoValidacion.Text == ValidacionesDatos.ValidarBool && CbTipoElemento.Text != Controles.CheckBox)
            {
               MensajeResultado = MensajeResultado + " - Solo se admite el control Caja de Chequeo para este descriptor.\n";
            }

            if (CbTipoElemento.Text == Controles.CheckBox && CbTipoValidacion.Text != ValidacionesDatos.ValidarBool)
            {
               MensajeResultado = MensajeResultado + " - El descriptor no admite el elemento Caja de chequeo.\n";
            }

            if (ChbFuenteDatos.Enabled == false && ChbFuenteDatos.Checked)
            {
               if (CbTipoElemento.Text != Controles.ListaDesplegable && CbTipoElemento.Text != Controles.ListBox &&
                   CbTipoElemento.Text != Controles.ControlDependencias && CbTipoElemento.Text != Controles.ComboBusqueda)
               {
                  MensajeResultado = MensajeResultado + " - El descriptor no admite el elemento seleccionado.\n";
               }
            }

            List<Controles> ControlPrincipal = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles p) { return p.Principal == true; });
            if (ControlPrincipal.Count > 0 && ChbDescriptorPrincipal.Checked)
            {
               MensajeResultado = MensajeResultado + " - Ya existe un descriptor principal en el documento.\n- Descriptor: " + ControlPrincipal[0].Nombre + "\n";
            }

            Excel.Worksheet HojaActiva = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;

            if (string.IsNullOrEmpty(Globals.ThisAddIn.IdPlantillaFormato) || string.IsNullOrEmpty(Globals.ThisAddIn.IdSubSeriePlantillaFormato))
            {
               MensajeResultado = MensajeResultado + " - No se puede agregar el elemento en la hoja seleccionada. \n   Abra una plantilla existente o cree una nueva plantilla.\n";
            }

            Excel.Range RangoActivo = (Excel.Range)Globals.ThisAddIn.Application.ActiveCell;


            List<Controles> ControlRango = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles p) { return p.RangoDatos.Address == RangoActivo.Address; });
            if (ControlRango.Count > 0)
            {
               MensajeResultado = MensajeResultado + " - Ya se ha asignado un descriptor en la celda seleccionada.";
            }


            #endregion

            if (!string.IsNullOrEmpty(MensajeResultado))
            {
               MessageBox.Show(MensajeResultado, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
               Validaciones = true;
            }
            return Validaciones;
         }
         catch
         {
            return false;
         }
      }

      private bool AjustarElementos(Excel.XlInsertShiftDirection direccion, Excel.Range rangoInicial, Excel.Worksheet HojaActual, string operacion)
      {
         bool Resultado = true;

         #region  Se agregó nueva fila
         /*
            if (direccion == Excel.XlInsertShiftDirection.xlShiftDown && operacion == "agregar")
            {
                int NumFila = rangoInicial.Row;
                try
                {
                    foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                    {
                        if (Control.RangoDatos.Row > NumFila)
                        {
                            //Control.RangoDatos = Control.RangoDatos.get_Offset(1, 0);
                            //Resultado = true;
                        }
                    }
                }
                catch
                {   Resultado = false;  }
            }
            */
         #endregion

         #region Se agregó nueva columna
         /*
            if (direccion == Excel.XlInsertShiftDirection.xlShiftToRight && operacion == "agregar")
            {
                try
                {
                    int NumColumna = rangoInicial.Column;

                    foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                    {
                        if (Control.RangoDatos.Column >= NumColumna)
                        {
                            Control.RangoDatos = Control.RangoDatos.get_Offset(0, 1);
                            Resultado = true;
                        }
                    }
                }
                catch
                {
                    Resultado = false;
                }
            }
            */
         #endregion

         #region Se Eliminó fila
         /*
            if (direccion == Excel.XlInsertShiftDirection.xlShiftDown && operacion == "eliminar")
            {
                try
                {
                    int NumFila = rangoInicial.Row;

                    foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                    {
                        if (Control.RangoDatos.Row >= NumFila)
                        {
                            Control.RangoDatos = Control.RangoDatos.get_Offset(-1, 0);
                        }

                    }
                }
                catch
                {
                    Resultado = false;
                }

            }
            */
         #endregion

         #region Se eliminó columna
         /*
            if (direccion == Excel.XlInsertShiftDirection.xlShiftToRight && operacion == "eliminar")
            {
                try
                {
                    int NumColumna = rangoInicial.Column;
                    foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                    {
                        if (Control.RangoDatos.Column >= NumColumna)
                        {
                            Control.RangoDatos = Control.RangoDatos.get_Offset(0, -1);
                            Resultado = true;
                        }
                    }
                }
                catch
                {
                    Resultado = false;
                }
            }
            */
         #endregion

         return Resultado;
      }

      /// <summary>
      /// Revisa si existen controles con  descriptores principales registrados
      /// </summary>
      /// <returns>estado final de la operacion</returns>
      private bool ValidarDescriptorPrincipalCreado()
      {
         bool IsCreated = false;

         try
         {
            int numControlPrincipal = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c) { return c.Principal == true; }).Count;
            if (numControlPrincipal > 0)
            { IsCreated = true; }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }

         return IsCreated;
      }
      #endregion

      private void DgvDatosTablaTemporal_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
      {
         try
         {
            if (e.RowIndex != DgvDatosTablaTemporal.Rows.Count - 1 && e.ColumnIndex == 0)
            {
               MessageBox.Show(Properties.Settings.Default.MsErrorEditarRegistroFuenteExterna, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               DgvDatosTablaTemporal.Rows[DgvDatosTablaTemporal.Rows.Count - 1].Selected = true;
               e.Cancel = true;
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void ChbDescriptorPrincipal_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            if (ChbDescriptorPrincipal.Checked)
            {
               ChbObligatorio.Checked = true;
               ChbObligatorio.Enabled = false;
            }
            else
            {
               ChbObligatorio.Enabled = true;
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void DgvEditarElementos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
      {
         try
         {
            //Locked
            if (e.ColumnIndex == 2 && e.RowIndex > -1)
            {
               Globals.ThisAddIn.ControlesFormato[e.RowIndex].RangoDatos.Locked = Globals.ThisAddIn.ControlesFormato[e.RowIndex].Locked;
               if (Globals.ThisAddIn.ControlesFormato[e.RowIndex].Tipo == Controles.ComboBusqueda)
               {
                  Globals.ThisAddIn.ControlesFormato[e.RowIndex].RangoDatos.get_Offset(0, 1).MergeCells = false;
                  Globals.ThisAddIn.ControlesFormato[e.RowIndex].RangoDatos.get_Offset(0, 1).Locked = Globals.ThisAddIn.ControlesFormato[e.RowIndex].Locked;
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void BtnPruebas_Click(object sender, EventArgs e)
      {
         ExcelTools.Worksheet Hoja = Globals.Factory.GetVstoObject( Globals.ThisAddIn.Application.ActiveSheet );

         WebBrowser WebBr = new WebBrowser();
         WebBr.Url = new Uri("http://www.sinco.com.co");
         Hoja.Controls.AddControl(WebBr, Hoja.InnerObject.get_Range("A1:J19", System.Type.Missing), "WebBr");
         WebBrowser WebGet = (WebBrowser)Hoja.Controls["WebBr"];

         DateTimePicker DTpicker = new DateTimePicker();
         Hoja.Controls.AddControl(DTpicker, Hoja.InnerObject.get_Range("A21:C21", System.Type.Missing), "DTpicker");
         DateTimePicker DTpickerGET = (DateTimePicker)Hoja.Controls["DTpicker"];

         CheckedListBox checkListBox = new CheckedListBox();
         checkListBox.Items.Add("PRB1");
         checkListBox.Items.Add("PRB2");
         checkListBox.Items.Add("PRB3");
         Hoja.Controls.AddControl(checkListBox, Hoja.InnerObject.get_Range("A23:C26", System.Type.Missing), "checkListBox");


         ComboBox CMBPRB = new ComboBox();
         CMBPRB.Items.Add("PRB1");
         CMBPRB.Items.Add("PRB2");
         CMBPRB.Items.Add("PRB3");
         Hoja.Controls.AddControl(CMBPRB, Hoja.InnerObject.get_Range("A28:C28", System.Type.Missing), "CMBPRB");

         //GroupBox GroupB = new GroupBox();
         //GroupB.Controls.Add(DTpicker);
         //GroupB.Controls.Add(checkListBox);
         //GroupB.Controls.Add(CMBPRB);
         //Hoja.Controls.AddControl(CMBPRB, Hoja.InnerObject.get_Range("A27:C32", System.Type.Missing), "GroupB");

      }

      private void BtnAgregarFuenteExterna_Click(object sender, EventArgs e)
      {
          try
          {
              if (!string.IsNullOrEmpty(CbTablaTemporal.Text))
              {
                  string textoFiltro = " DFEdescripcion = '" + CbTablaTemporal.Text + "' ";
                  DataView FuenteExt = new DataView(Globals.Ribbons.RibbonExcel.DatosFuentesExternas, textoFiltro, "", DataViewRowState.CurrentRows);

                  if (FuenteExt.Count > 0)
                  {
                      CbTablaTemporal.SelectedValue = FuenteExt.ToTable().Rows[0]["DFEid"].ToString();
                  }
                  else
                  {
                      DialogResult respuesta = MessageBox.Show(Properties.Settings.Default.MsPreguntaAgregarFuenteExterna, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                      if (respuesta == System.Windows.Forms.DialogResult.Yes)
                      {
                          DataTable ResAgregar = FuentesExternas.CRUDFuentesExternas(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, "Agregar", -1, CbTablaTemporal.Text, "", false);

                          if (ResAgregar.Rows.Count > 0)
                          {
                              if (ResAgregar.Rows[0]["Resultado"].ToString() == "1")
                              {
                                  ConfiguracionInicial();
                              }
                              else
                              {
                                  MessageBox.Show(ResAgregar.Rows[0]["Descripcion"].ToString(), Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                              }
                          }
                      }
                  }
              }
              else
              {
                  MessageBox.Show("Por favor, Escriba el nombre de la fuente externa.", Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                  CbTablaTemporal.Focus();
              }
          }
          catch (Exception EXC)
          {
              Utilidades.ReportarError(EXC);
          }
      }

   }
}
