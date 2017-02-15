using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Data;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using ExcelTool = Microsoft.Office.Tools.Excel;

using SincoOfficeLibrerias.wsOfficeSGD;
using SincoOfficeLibrerias;
using System.Drawing;

using AppExternas;

namespace SincoExcel
{
   public partial class ThisAddIn
   {
      public Login DatosUsuario;
      public List<Controles> ControlesFormato;
      public Conexiones Conexion;
      public ConexionesExcel DatosConexion;

      public bool IsUpdating;
      public string ClaveProteccionHoja = "5PassProteccionHoja";
      public string MensajeTitulos = "Sinco ERP";
      public string MensajeError = "Ocurrió un evento no controlado en la aplicación.\n\nCódigo del evento:\n{0}\n\nConserve el código para reportar el evento posteriormente.";
      public string MensajeErrorNoReportado = "Ocurrió un evento no controlado en la aplicación y no fue notificado, si el evento persiste por favor consulte con el administrador del sistema.";

      public string IdPlantillaFormato;
      public string IdSubSeriePlantillaFormato;

      public bool AccesoCrearFormatoISO;
      public bool AccesoRegistrarFormatoISO;

      //llaves para crear / abrir archivos de licencia
      public Byte[] newKeyFile = { 17, 29, 23, 41, 52, 26, 31, 84, 63, 63, 95, 12, 10, 14, 15, 12, 64, 99, 38, 88, 99, 12, 3, 1 };
      public Byte[] newIVFile = { 75, 22, 255, 110, 65, 201, 209, 154 };

      public Color ColorDescriptorObligatorio;
      public Color ColorDescriptorOpcional;

      private void ThisAddIn_Startup(object sender, System.EventArgs e)
      {
         try
         {
            IsUpdating = true;

            ControlesFormato = new List<Controles>();
            ControlesFormato.Clear();

            IdPlantillaFormato = string.Empty;
            IdSubSeriePlantillaFormato = string.Empty;

            AccesoCrearFormatoISO = false;
            AccesoRegistrarFormatoISO = false;

            CargarColores();

            #region Crear TablaDesencadenadores
            //TablaDesencadenadores = new DataTable();
            //TablaDesencadenadores.Columns.Add("NombreTabla", typeof(string));
            //TablaDesencadenadores.Columns.Add("RangoDatos", typeof(string));
            #endregion

            //CargarUsuarioPrueba();

            this.Application.SheetChange += new Excel.AppEvents_SheetChangeEventHandler(Application_SheetChange);
            this.Application.SheetSelectionChange += new Excel.AppEvents_SheetSelectionChangeEventHandler(Application_SheetSelectionChange);
            this.Application.WorkbookOpen += new Excel.AppEvents_WorkbookOpenEventHandler(Application_WorkbookOpen);
            this.Application.WorkbookBeforeSave += new Excel.AppEvents_WorkbookBeforeSaveEventHandler(Application_WorkbookBeforeSave);

            this.Application.CalculationInterruptKey = Excel.XlCalculationInterruptKey.xlEscKey;

            Excel.Range RangoPrueba = (Excel.Range)Globals.ThisAddIn.Application.ActiveCell;

            Excel.Worksheet HojaPRB = Globals.ThisAddIn.Application.ActiveSheet;

            //Excel.Sheets Hoja = Globals.ThisAddIn.Application.ActiveSheet;
            //Excel.Chart Grafico = Globals.ThisAddIn.Application.ActiveChart;

            //int xlPrinter = (int)Excel.XlPictureAppearance.xlPrinter;
            //int xlScreen = (int)Excel.XlPictureAppearance.xlScreen;
            //int xlBitmap = (int)Excel.XlCopyPictureFormat.xlBitmap;
            //int xlPicture = (int)Excel.XlCopyPictureFormat.xlPicture;


            //int xlCategory = (int)Excel.XlAxisType.xlCategory;
            //int xlValue = (int)Excel.XlAxisType.xlValue;


            //int xlPrimary = (int)Excel.XlAxisGroup.xlPrimary;
            //int xlSecondary = (int)Excel.XlAxisGroup.xlSecondary;

            //Excel.Workbook Libro = Globals.ThisAddIn.Application.ActiveWorkbook;

            //Excel.Chart Graf = Libro.Charts.Item["asd"];

            //Graf.Axes(Excel.XlCategoryType.xlCategoryScale).AxisTitle
            //   .Select();
            //Graf.Axes(Excel.XlCategoryType.xlCategoryScale, Excel.XlAxisGroup.xlPrimary ).AxisTitle.Text = "EjeX";

            //Graf.Axes(Excel.XlCategoryType.xlTimeScale).AxisTitle.Select();
            //Graf.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary).AxisTitle.Text = "EjeX";

            //Graf.ChartType = Excel.XlChartType.xl3DAreaM;
            //Graf.ApplyChartTemplate

            //Graf.ChartArea.
            //Hoja.cen
            
            //Grafico.CopyPicture(Excel.XlPictureAppearance.xlPrinter, Excel.XlCopyPictureFormat.xlBitmap,  Excel.XlPictureAppearance.xlPrinter);

            //int Hidden = (int)Excel.XlSheetVisibility.xlSheetHidden;
            //int VeryHidden = (int)Excel.XlSheetVisibility.xlSheetVeryHidden;
            //int Visible  = (int)Excel.XlSheetVisibility.xlSheetVisible;

            IsUpdating = false;
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      void Application_WorkbookBeforeSave(Excel.Workbook Wb, bool SaveAsUI, ref bool Cancel)
      {
         try
         {
            Globals.Ribbons.RibbonExcel.WorkbookBeforeSave(Wb, SaveAsUI, ref Cancel);
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void Application_WorkbookOpen(Excel.Workbook Wb)
      {
         try
         {
            CargarEstilos(Wb);
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void Application_SheetSelectionChange(object Sh, Excel.Range Target)
      {
         try
         {
            if (!IsUpdating)
            {
               Globals.Ribbons.RibbonExcel.OnSheetSelectionChanged(Sh, Target);
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void Application_SheetChange(object Sh, Excel.Range Target)
      {
         try
         {
            //if (!IsUpdating)
            {
               Globals.Ribbons.RibbonExcel.OnSheetChanged(Sh, Target);
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
      {
         try
         {
            Globals.Ribbons.RibbonExcel.CerrarSesion();

            Globals.Ribbons.RibbonExcel.ConfiguracionInicial();
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      public void CargarUsuarioPrueba()
      {
         try
         {
            if (DatosUsuario == null)
            {
               DatosUsuario = new Login();

               DatosUsuario.IdUsuario = "50";
               DatosUsuario.NomUsuario = " Admin Prueba";
               DatosUsuario.SucDesc = "Medellin Laureles";
               DatosUsuario.SucId = "154";
               DatosUsuario.EmpresaNombre = "Sinco Comunicaciones S.A.";
               DatosUsuario.EmpresaId = "1";
               DatosUsuario.CadenaConexion = "Data Source=DESARROLLO;Initial Catalog=Sinco;User ID=desarrollo;Password=de5arrollo2010";

               Globals.ThisAddIn.DatosUsuario = DatosUsuario;

               Globals.Ribbons.RibbonExcel.LbUsuario.Label = DatosUsuario.NomUsuario;
               Globals.Ribbons.RibbonExcel.LbSucursal.Label = DatosUsuario.SucDesc;
               Globals.Ribbons.RibbonExcel.LbEmpresa.Label = DatosUsuario.EmpresaNombre;
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      /// <summary>
      /// Carga estilos el el libro especificado
      /// </summary>
      /// <param name="Wb"></param>
      /// <returns>Estado final de operación</returns>
      public bool CargarEstilos(Excel.Workbook Wb)
      {
         bool Resultado = false;
         try
         {
            Excel.Style EstiloLabel = Wb.Styles.Add("EstiloLabel", missing);

            EstiloLabel.Font.Name = "Verdana";
            EstiloLabel.Font.Size = 10;
            EstiloLabel.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            EstiloLabel.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            EstiloLabel.Interior.Pattern = Excel.XlPattern.xlPatternSolid;
            EstiloLabel.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            EstiloLabel.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            //EstiloLabel.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            //EstiloLabel.Borders.LineStyle = 1;
            //EstiloLabel.Borders.Weight = 2;

            Excel.Style SinEstilo = Wb.Styles.Add("SinEstilo", missing);

            SinEstilo.Font.Name = "Verdana";
            SinEstilo.Font.Size = 10;
            SinEstilo.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            SinEstilo.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            SinEstilo.Interior.Pattern = Excel.XlPattern.xlPatternSolid;

            Resultado = true;
         }
         catch
         {
            Resultado = false;
         }
         return Resultado;
      }

      public void ValidarMenusUsuario(Login Usuario)
      {
         try
         {
            //// Temporal !!!!!!!!
            //AccesoCrearFormatoISO = true;
            //AccesoRegistrarFormatoISO = true;

            string TextoCrearFormato = "CrearFormato";
            string TextoRegistroFormato = "RegistrarFormato";

            DataTable validar = SGCformatos.ValidarAccesoUsuarios(DatosUsuario, Globals.ThisAddIn.DatosConexion, "ValidarAccesoMenus");
            if (validar.Rows.Count > 0)
            {
               #region Validar crear Formato
               string FiltroCrearFormato = " Ambito='" + TextoCrearFormato + "' ";
               DataView CrearFormato = new DataView(validar, FiltroCrearFormato, "", DataViewRowState.CurrentRows);
               if (CrearFormato.Count > 0)
               {
                  AccesoCrearFormatoISO = bool.Parse(CrearFormato.ToTable().Rows[0]["Resultado"].ToString());
               }
               #endregion

               #region Validar Registro Formato
               string FiltroRegistroFormato = " Ambito='" + TextoRegistroFormato + "' ";
               DataView RegistroFormato = new DataView(validar, FiltroRegistroFormato, "", DataViewRowState.CurrentRows);
               if (RegistroFormato.Count > 0)
               {
                  AccesoRegistrarFormatoISO = bool.Parse(RegistroFormato.ToTable().Rows[0]["Resultado"].ToString());
               }
               #endregion
            }
            else
            {
               AccesoCrearFormatoISO = false;
               AccesoRegistrarFormatoISO = false;
            }

            // Si no tiene acceso a creacion y registro de formatos de SGC
            if (!AccesoCrearFormatoISO && !AccesoRegistrarFormatoISO)
            {
               Globals.Ribbons.RibbonExcel.GroupFormatos.Visible = false;
               Globals.Ribbons.RibbonExcel.GroupElementos.Visible = false;
            }
            else
            {
               Globals.Ribbons.RibbonExcel.GroupFormatos.Visible = true;
               Globals.Ribbons.RibbonExcel.GroupElementos.Visible = true;
            }
         }
         catch (Exception EXC)
         {
            Utilidades.ReportarError(EXC);
         }
      }

      public void CargarColores()
      {
         #region Cargar Colores
         string ColorDescriptorObli = RegistroWindows.ConsultarEntradaRegistro("Color", "DescriptorObligatorio");
         string ColorDescriptorOpci = RegistroWindows.ConsultarEntradaRegistro("Color", "DescriptorOpcional");

         if (!string.IsNullOrEmpty(ColorDescriptorObli) && !string.IsNullOrEmpty(ColorDescriptorOpci))
         {
            Globals.ThisAddIn.ColorDescriptorObligatorio = Color.FromArgb(int.Parse(ColorDescriptorObli));
            Globals.ThisAddIn.ColorDescriptorOpcional = Color.FromArgb(int.Parse(ColorDescriptorOpci));
         }
         else
         {
            ColorDescriptorObligatorio = Color.MistyRose;
            ColorDescriptorOpcional = Color.LemonChiffon;
         }
         #endregion
      }

      #region VSTO generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InternalStartup()
      {
         this.Startup += new System.EventHandler(ThisAddIn_Startup);
         this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
      }

      #endregion
   }
}
