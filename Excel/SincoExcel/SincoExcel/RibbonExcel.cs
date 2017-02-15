using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.IO;

using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using ExcelTools = Microsoft.Office.Tools.Excel;

using SincoOfficeLibrerias.wsOfficeSGD;
using SincoOfficeLibrerias.wsSGCdocumentos;
using SincoOfficeLibrerias;
using System.Drawing;

using AppExternas;
using System.Threading;

namespace SincoExcel
{
   public partial class RibbonExcel
   {
      #region Constructor de propiedades
      //Datos temporales de informacion, para reducir consultas a servidor
      public DataTable DatosDescriptores;
      public DataTable DatosCategorias;
      public DataTable DatosFuentesExternas;
      public DataTable DatosFormatos;
      public DataTable DatosFormatosVigentes;

      //Modos disponibles en la manipulacion de formatos de SGC
      public MetodosRibbon.ModoTrabajo ModoTrabajo;



      //public string ModoEdicionPlantilla = "ModoEdicionPlantilla";
      //public string ModoCompletarFormato = "ModoCompletarFormato";
      //public string ModoBloquearContenido = "ModoBloquearContenido";
      //public string ModoVerificarPlantilla = "ModoVerificarPlantilla";

      public char SimboloSeparacion = '&';
      public char SimboloValor = '%';

      // Define el tiempo en milisegundos que dura activa una sesión.
      public int TimeSessionExpired = 1800000;
      public System.Windows.Forms.Timer TimerSessionExpired;

      public string ExtensionExcel = ".xlsx";

      #endregion

      private void RibbonExcel_Load(object sender, RibbonUIEventArgs e)
      {
         try
         {
            // crea Timer de session
            TimerSessionExpired = new System.Windows.Forms.Timer();
            TimerSessionExpired.Tick += new EventHandler(TimerSessionExpired_Tick);
            TimerSessionExpired.Stop();

            ModoTrabajo = MetodosRibbon.ModoTrabajo.BloquearContenido;

            //Solo para depuración, desactiva sessionExpired !!!
            //Globals.ThisAddIn.CargarUsuarioPrueba();
            //wsOfficeSGD WS1 = new wsOfficeSGD();
            //wsSGCdocumentos WS2 = new wsSGCdocumentos();

            ConfiguracionInicial();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      public void ConfiguracionInicial()
      {
         try
         {
            #region Elimina archivos temporales del complemento
            string RutaLocal = RegistroWindows.ConsultarEntradaRegistro("Ruta", "Temporales");
            if (!string.IsNullOrEmpty(RutaLocal))
            {
               if (System.IO.Directory.Exists(RutaLocal))
               {
                  string[] files = System.IO.Directory.GetFiles(RutaLocal);

                  // Copy the files and overwrite destination files if they already exist.
                  foreach (string s in files)
                  {
                     try
                     {
                        System.IO.File.Delete(s);
                     }
                     catch
                     {

                     }
                  }
               }
            }
            else
            {
               #region Crear ruta temporal automaticamente

               string NombreRutaTEmp = System.IO.Path.GetTempPath() + "Archivos Temporales Sinco ERP\\";
               System.IO.Directory.CreateDirectory(NombreRutaTEmp);
               bool ResultadoCrearTemp = RegistroWindows.AgregarEntradaRegistro("Ruta", "Temporales", NombreRutaTEmp);

               if (!ResultadoCrearTemp)
               {
                  MessageBox.Show(Properties.Settings.Default.MsConfigurarRutaTemporal, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               #endregion
            }
            #endregion
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      #region Eventos de Login y Configuración de Complemento

      /// <summary>
      /// Elimina los datos de sesión del usuario y limita los controles visibles
      /// </summary>
      /// <returns>Estado Final de la operacion</returns>
      public bool CerrarSesion()
      {
         try
         {
            Globals.ThisAddIn.IdPlantillaFormato = string.Empty;
            Globals.ThisAddIn.IdSubSeriePlantillaFormato = string.Empty;

            Globals.ThisAddIn.ControlesFormato.Clear();
            //Globals.ThisAddIn.ItemsElementoTabla.Clear();
            //Globals.ThisAddIn.VariablesDescriptores.Clear();

            Globals.Ribbons.RibbonExcel.LbUsuario.Label = string.Empty;
            Globals.Ribbons.RibbonExcel.LbSucursal.Label = string.Empty;
            Globals.Ribbons.RibbonExcel.LbEmpresa.Label = string.Empty;

            //Globals.Ribbons.RibbonExcel.TabSincoERP.Visible = false;
            Globals.Ribbons.RibbonExcel.GroupElementos.Visible = false;
            Globals.Ribbons.RibbonExcel.GroupFormatos.Visible = false;
            Globals.Ribbons.RibbonExcel.GroupInfoUsuario.Visible = false;
            Globals.Ribbons.RibbonExcel.BtnIngreso.Label = "Iniciar Sesión";

            try
            {
               if (Globals.ThisAddIn.Application.ActiveSheet != null && Globals.ThisAddIn.Application.ThisWorkbook.Sheets.Count > 1)
               {
                  Excel.Worksheet HojaActiva = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
                  HojaActiva.Delete();
               }
            }
            catch
            {

            }

            Login DatosUsuario = new Login();

            if (Globals.ThisAddIn.DatosUsuario != null)
            {
               IconoAppExcelSisTray.BalloonTipText = "Sesión cerrada";
               IconoAppExcelSisTray.BalloonTipTitle = Globals.ThisAddIn.MensajeTitulos;
               IconoAppExcelSisTray.ShowBalloonTip(5000);
            }

            Globals.ThisAddIn.DatosUsuario = DatosUsuario;

            //Globals.ThisAddIn.Conexion = new Conexiones("");
            Globals.ThisAddIn.DatosConexion = new ConexionesExcel();

            return true;
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);

            return false;
         }
      }

      private void BtnIngreso_Click(object sender, RibbonControlEventArgs e)
      {
         try
         {
            if (BtnIngreso.Label == "Cerrar Sesión" && !string.IsNullOrEmpty(Globals.ThisAddIn.DatosUsuario.IdUsuario))
            {
               CerrarSesion();
            }
            else
            {
               FrmLogincs Frm = new FrmLogincs();
               Frm.Show();
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void TimerSessionExpired_Tick(object sender, EventArgs e)
      {
         try
         {
            //Cerrar Sesión activa
            TimerSessionExpired.Stop();
            CerrarSesion();

            DialogResult Respuesta = MessageBox.Show(Properties.Settings.Default.MsSesionCerrada, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Respuesta == DialogResult.Yes)
            {
               FrmLogincs Frm = new FrmLogincs();
               Frm.Show();
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      /// <summary>
      /// Reinicia timer de sesión, ocurre cuando el usuario cambia celdas
      /// </summary>
      private void ReiniciarTiempoSesion()
      {
         try
         {
            if (Globals.ThisAddIn.DatosUsuario != null)
            {
               TimerSessionExpired.Stop();
               TimerSessionExpired.Start();
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void GroupLogin_DialogLauncherClick(object sender, RibbonControlEventArgs e)
      {
         try
         {
            FrmConfigurarComplemento Frm = new FrmConfigurarComplemento();
            Frm.Show();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void IconoAppExcelSisTray_MouseDoubleClick(object sender, MouseEventArgs e)
      {
         try
         {
            string InfoSesion = string.Empty;

            InfoSesion = "Información de sesión:\n\n";

            if (Globals.ThisAddIn.DatosUsuario != null)
            {
               InfoSesion = InfoSesion + "Usuario:  " + Globals.ThisAddIn.DatosUsuario.Nombre + "\nEmpresa:  " + Globals.ThisAddIn.DatosUsuario.EmpresaNombre + "\nSucursal:  " + Globals.ThisAddIn.DatosUsuario.SucDesc;
            }
            else
            {
               InfoSesion = InfoSesion + "Sesión cerrada.";
            }

            if (!string.IsNullOrEmpty(InfoSesion))
            {
               MessageBox.Show(InfoSesion, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      #endregion

      #region Eventos de Plantillas

      private void BtnCrearPlantilla_Click(object sender, RibbonControlEventArgs e)
      {
         try
         {
            SincoExcel.Forms.FrmFormatos frm = new Forms.FrmFormatos();
            frm.Show();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void BtnCrearElemento_Click(object sender, RibbonControlEventArgs e)
      {
         try
         {
            FrmConstructorPlantillas Frm = new FrmConstructorPlantillas();
            Frm.Show();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void BtnGuardarPlantilla_Click(object sender, RibbonControlEventArgs e)
      {
         try
         {
            if (Globals.ThisAddIn.Application.ActiveSheet != null)
            {
               Excel.Worksheet HojaActual = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
               ExcelTools.Worksheet HojaExtendida = (ExcelTools.Worksheet)Globals.Factory.GetVstoObject(HojaActual);

               #region ModoEdicionPlantilla
               if (Globals.Ribbons.RibbonExcel.ModoTrabajo == MetodosRibbon.ModoTrabajo.EdicionPlantilla)
               {
                  DialogResult Respuesta = MessageBox.Show(Properties.Settings.Default.MsGuardarPlantilla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                  if (Respuesta == DialogResult.Yes)
                  {
                     string ValidarPlantilla = ValidacionesGuardarPlanilla();

                     if (string.IsNullOrEmpty(ValidarPlantilla))
                     {
                        #region Ajustar Propiedades de elementos
                        foreach (Controles Control in Globals.ThisAddIn.ControlesFormato)
                        {
                           string[] propiedades = Controles.LeerPropiedadesControl(Control);
                           Control.Top = double.Parse(propiedades[0]);
                           Control.Left = double.Parse(propiedades[1]);
                           Control.Width = double.Parse(propiedades[2]);
                           Control.Height = double.Parse(propiedades[3]);
                           Control.Locked = Control.RangoDatos.Locked;
                        }
                        #endregion

                        bool LimpiarElementos = MetodosRibbon.LimpiarControlesHoja(false);
                        if (LimpiarElementos)
                        {
                           bool ResultadoGuardarPlantilla = MetodosRibbon.GuardarFormatoPlantilla();

                           if (!ResultadoGuardarPlantilla)
                           {
                              MessageBox.Show(Properties.Settings.Default.MsErrorGuardarFormato, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                           }
                        }
                        else
                        {
                           MessageBox.Show(Properties.Settings.Default.MsErrorGuardarFormato, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                     }
                     else
                     {
                        MessageBox.Show("Validaciones de plantilla:\n\n" + ValidarPlantilla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                  }

                  // Si no se guardo correctamente el formato.
                  //if (LoadControls)
                  //{
                     #region Volver a cargar los controles

                     bool resultadoCargarControles = MetodosRibbon.CargarControlesHoja(HojaActual);

                     if (!resultadoCargarControles)
                     {
                        MessageBox.Show(Properties.Settings.Default.MsErrorCargarControl, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                     #endregion
                  //}
               }
               #endregion

               #region ModoCompletarFormato
               if (ModoTrabajo == MetodosRibbon.ModoTrabajo.CompletarFormato)
               {
                  DialogResult Respuesta = MessageBox.Show(Properties.Settings.Default.MsPreguntaGuardarFormatoDiligenciado, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                  if (Respuesta == DialogResult.Yes)
                  {
                     bool ValidacionesGuardar = MetodosRibbon.ValidarInformacionGuardarPlantilla();

                     if (ValidacionesGuardar)
                     {
                        Excel.Workbook LibroAbierto = (Excel.Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;

                        bool LimpiarElementos = MetodosRibbon.LimpiarControlesHoja(true);

                        if (LimpiarElementos)
                        {
                            //Configuracion de correspondencia responsables
                            Forms.FrmCorrespondencia frm = new Forms.FrmCorrespondencia();
                            frm.Show();
                        }
                        else
                        {
                           DialogResult Res = MessageBox.Show(Properties.Settings.Default.MsVolvarCargarFormato, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                           if (Res == DialogResult.Yes)
                           {
                              LibroAbierto.Close(false);
                              MetodosRibbon.AbrirFormatoParaCompletar(Globals.ThisAddIn.IdPlantillaFormato);
                           }
                        }
                     }
                  }
               }
               #endregion
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void BtnCerrarFormato_Click(object sender, RibbonControlEventArgs e)
      {
         try
         {
            Excel.Worksheet HojaActiva = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
            Excel.Workbook LibroActual = (Excel.Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;

            if (!string.IsNullOrEmpty(Globals.ThisAddIn.IdPlantillaFormato) || !string.IsNullOrEmpty(Globals.ThisAddIn.IdSubSeriePlantillaFormato))
            // && HojaActiva.ProtectContents)
            {
               DialogResult respuesta = MessageBox.Show(Properties.Settings.Default.MsCerrarFormato, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

               if (respuesta == DialogResult.Yes)
               {
                  Globals.ThisAddIn.IdPlantillaFormato = string.Empty;
                  Globals.ThisAddIn.IdSubSeriePlantillaFormato = string.Empty;
                  Globals.ThisAddIn.ControlesFormato.Clear();
                  //Globals.ThisAddIn.ItemsElementoTabla.Clear();
                  //Globals.ThisAddIn.VariablesDescriptores.Clear();

                  LibroActual.Close(false, System.Type.Missing, System.Type.Missing);
                  Globals.Ribbons.RibbonExcel.GroupElementos.Visible = false;
               }
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      /// <summary>
      /// Realiza validaciones preliminares antes de guardar una plantilla
      /// </summary>
      /// <returns>string.empty Si todas las validaciones son correctas, de lo contrario, devuelve mensaje con los errores encontrados</returns>
      private string ValidacionesGuardarPlanilla()
      {
         try
         {
            string resultado = string.Empty;

            string RutaLocal = RegistroWindows.ConsultarEntradaRegistro("Ruta", "Temporales");
            string Mensajevalidacion = string.Empty;

            if (!string.IsNullOrEmpty(RutaLocal))
            {
               string Filtro = " SFVid = " + Globals.ThisAddIn.IdPlantillaFormato;
               DataView FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatos, Filtro, "", DataViewRowState.CurrentRows);

               if (FiltroFormato.Count > 0)
               {
                  Excel.Workbook LibroActual = (Excel.Workbook)Globals.ThisAddIn.Application.ActiveWorkbook;
                  Excel.Worksheet HojaActual = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;

                  int NumPrincipal = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c) { return c.Principal == true; }).Count;
                  if (NumPrincipal == 0)
                  {
                     resultado = "La plantilla debe tener asignado un descriptor principal.";
                  }
               }
               else
               {
                  resultado = "La plantilla no es válida.";
               }
            }
            else
            {
               resultado = "Por Favor Configure una ruta temporal para la aplicación.\n";
            }

            return resultado;

         }
         catch
         {
            return "La plantilla no cumple con los requisitos previos";
         }
      }

      #endregion

      #region Eventos de Hojas de trabajo

      public void OnSheetChanged(object Sh, Excel.Range Target)
      {
         try
         {
            // Si la hoja no se esta actualizando y ademas no se trabaja en modo de edición de Hoja
            if (!Globals.ThisAddIn.IsUpdating &&
                (Globals.Ribbons.RibbonExcel.ModoTrabajo == MetodosRibbon.ModoTrabajo.CompletarFormato
                    || Globals.Ribbons.RibbonExcel.ModoTrabajo == MetodosRibbon.ModoTrabajo.VerificarPlantilla))
            {
               #region Actualizar Combos de búsqueda

               Controles ControlCombo = Globals.ThisAddIn.ControlesFormato.Find(delegate(Controles c) { return (c.RangoDatos.Address == Target.Address && (c.Tipo == Controles.ComboBusqueda)); });

               if (ControlCombo != null)
               {
                  MetodosRibbon.DataBindControl(ControlCombo, Target.Text);

                  if (ControlCombo.Tipo == Controles.ComboBusqueda)
                  {
                     Globals.ThisAddIn.IsUpdating = true;
                     Target.Value2 = "";
                     Globals.ThisAddIn.IsUpdating = false;
                  }
               }
               #endregion

               #region Validar Información ingresada por  usuario
               List<Controles> ControlesTextBox = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c) { return (c.RangoDatos.Address == Target.Address && c.Tipo == Controles.TextBox); });

               bool Validar = false;

               if (Target.Text != null)
               {
                  string Valor = Target.Text.ToString();

                  if (!string.IsNullOrEmpty(Valor))
                  {
                     foreach (Controles Control in ControlesTextBox)
                     {
                        Validar = ValidacionesDatos.ValidarInformacion(Valor, Control.TipoValidacion);
                        if (!Validar)
                        {
                           MessageBox.Show(string.Format(Properties.Settings.Default.MsErrorFormatoTextoIngresado, Control.TipoValidacion), Globals.ThisAddIn.MensajeTitulos,
                                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                           Target.Value2 = string.Empty;
                           Target.Select();
                        }
                     }
                  }
               }
               #endregion
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      public void OnSheetSelectionChanged(object Sh, Excel.Range Target)
      {
         try
         {
            ReiniciarTiempoSesion();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      public void WorkbookBeforeSave(Excel.Workbook Wb, bool SaveAsUI, ref bool Cancel)
      {
         try
         {
            if (!string.IsNullOrEmpty(Globals.ThisAddIn.IdPlantillaFormato)
               && !string.IsNullOrEmpty(Globals.ThisAddIn.IdSubSeriePlantillaFormato)
               && Globals.ThisAddIn.DatosUsuario.IdUsuario != null)
            {
               //Preguntar si esea guardar el formato abierto.
               MessageBox.Show(Properties.Settings.Default.MsGuardarFormatoOpcionGuardar, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               Cancel = false;
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }
      #endregion

      #region Métodos Elemento Tabla

      /// <summary>
      /// Crea una nueva fila para una tabla
      /// </summary>
      /// <param name="HojaTrabajoExtendida">Objeto VSTO equivalente a la hoja de trabajo</param>
      /// <param name="RangoDatosInicial"></param>
      /// <param name="NombreTabla"></param>
      /// <param name="IdFila">Número de fila</param>
      /// <param name="CrearEncabezado">crear encabezado y nombres de columna de la tabla</param>
      public bool CrearFilaElementoTabla(ExcelTools.Worksheet HojaTrabajoExtendida, Excel.Range RangoDatosInicial, string NombreTabla, string IdFila, bool CrearEncabezado, bool RegistrarControlesCreados)
      {
         bool Resultado = false;

         try
         {
            Excel.Worksheet HojaActiva = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;

            List<Controles> ControlesCreados = Controles.CrearControlTabla(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, HojaTrabajoExtendida,
                        CrearEncabezado, Globals.ThisAddIn.ControlesFormato, RangoDatosInicial, IdFila, NombreTabla, Globals.ThisAddIn.ColorDescriptorObligatorio, Globals.ThisAddIn.ColorDescriptorOpcional);

            #region Registrar SubControles Creados
            if (ControlesCreados.Count > 0)
            {
               foreach (Controles Control in ControlesCreados)
               {
                  if (int.Parse(Control.Id) > 0)
                  {
                     //Permite agregar los controles creados en la fila, solo aplica cuando se crean filas automaticamente
                     if (RegistrarControlesCreados)
                     { Globals.ThisAddIn.ControlesFormato.Add(Control); }
                     else
                     {
                        Globals.ThisAddIn.ControlesFormato.RemoveAll(delegate(Controles c) { return (c.Nombre == Control.Nombre && c.TablaNombre == Control.TablaNombre && c.TablaFila == Control.TablaFila); });
                        Globals.ThisAddIn.ControlesFormato.Add(Control);
                     }
                  }
               }
            }
            else
            {
               MessageBox.Show(Properties.Settings.Default.MsErrorCrearTabla, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
               Resultado = false;
            }
            #endregion

            Resultado = true;
         }
         catch (Exception EXC)
         {
            Resultado = false;
            Utilidades.ReportarError(EXC);
         }

         return Resultado;

      }

      ///// <summary>
      ///// Elimina los controles asociados a una fila de la tabla, y conserva los resultados.
      ///// </summary>
      ///// <param name="HojaTrabajoExtendida">Objeto VSTO equivalente a la hoja de trabajo </param>
      ///// <param name="NombreTabla"></param>
      ///// <param name="IdFila">Número de la fila a eliminar</param>
      //public void EliminarFilaTabla(ExcelTools.Worksheet HojaTrabajoExtendida, string NombreTabla, string IdFila)
      //{
      //   try
      //   {
      //      Excel.Worksheet HojaActiva = (Excel.Worksheet)Globals.ThisAddIn.Application.ActiveSheet;
      //      List<Controles> ControlesTabla = Globals.ThisAddIn.ControlesFormato.FindAll(delegate(Controles c) { return (c.TablaNombre == NombreTabla && c.TablaFila == IdFila); });

      //      foreach (Controles Control in ControlesTabla)
      //      {
      //         bool resEliminar = Controles.EliminarControl(Control, true, false);

      //         if (resEliminar)
      //         {
      //            Globals.ThisAddIn.ControlesFormato.Remove(Control);
      //         }
      //      }
      //   }
      //   catch (Exception EXC)
      //   {
      //      Utilidades.ReportarError(EXC);
      //   }
      //}

      #endregion
   }
}
