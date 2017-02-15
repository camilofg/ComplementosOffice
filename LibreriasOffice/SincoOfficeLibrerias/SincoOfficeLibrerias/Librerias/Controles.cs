using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;

using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using ExcelTools = Microsoft.Office.Tools.Excel;
using SincoOfficeLibrerias;
using AppExternas;

using System.Drawing;

namespace SincoOfficeLibrerias
{
   public class Controles
   {
      #region Tipos de Controles
      public const string TextBox = "Caja de Texto";
      public const string ListaDesplegable = "Lista Desplegable";
      public const string CheckBox = "Caja de Chequeo";
      public const string ListBox = "Cuadro de Lista";
      public const string ComboBusqueda = "Combo de Búsqueda";
      /// <summary>
      /// Elemento para descriptores que tienen precedencias de información.
      /// </summary>
      public const string ControlDependencias = "Control de Dependencias";

      /// <summary>
      /// Lista de controles disponibles.
      /// </summary>
      public const string TiposElementos = TextBox + ":" + ListaDesplegable + ":" + CheckBox + ":" + ListBox + ":" + ComboBusqueda + ":" + ControlDependencias;
      #endregion

      #region Propiedades Controles

      //Propiedades de control
      public string GUID { get; set; }
      public Excel.Range RangoDatos { get; set; }
      public bool Locked { get; set; }

      //Propiedades de descriptor
      public string Id { get; set; }
      public string Nombre { get; set; }
      public string Tipo { get; set; }
      public bool Obligatorio { get; set; }
      public bool Principal { get; set; }
      public string Propiedades { get; set; }
      public string TipoValidacion { get; set; }
      public string IdFormato { get; set; }
      public string IdSubSerie { get; set; }
      public bool BloqueadoWorkFlow { get; set; }

      //Propiedades de ubicacion
      public ExcelTools.Worksheet HojaExcel { get; set; }
      public Excel.Workbook LibroExcel { get; set; }
      public double Width { get; set; }
      public double Height { get; set; }
      public double Top { get; set; }
      public double Left { get; set; }
      public bool Orientacion { get; set; }

      //propiedades de tabla
      public string TablaNombre { get; set; }
      public string TablaFila { get; set; }
      public int TablaNumeroMaximoRegistros { get; set; }
      public Excel.Range TablaRangoInicial { get; set; }

      #endregion

      /// <summary>
      /// Crea y dibuja un conjunto de controles en una hoja de excel
      /// Propiedades de Obj (Controles) mínimas requeridas: Nombre, HojaExcel, RangoDatos, Tipo, Orientacion, TipoValidacion
      /// </summary>
      /// <param name="ListaControles">Lista de controles para pintar</param>
      /// <param name="MostrarLabel">Indica si se debe dibujar el nombre del control</param>
      /// <returns>Lista de controles creados correctamente</returns>
      public static List<Controles> CrearControl(List<Controles> ListaControles, bool MostrarLabel)
      {

         List<Controles> ControlesCreados = new List<Controles>();

         Color ColorLineasControl = Color.Black;
         bool CreacionControl = false;

         Controles NuevoControl = new Controles();

         foreach (Controles Control in ListaControles)
         {
            ExcelTools.Worksheet HojaTrabajo = Control.HojaExcel;
            NuevoControl = Control;

            try
            {
               #region Calcular GUID
               if (!string.IsNullOrEmpty(Control.GUID))
               {
                  if (Control.GUID.Length > 30)
                  { NuevoControl.GUID = Control.GUID.Substring(0, 30); }
                  else
                  { NuevoControl.GUID = Control.GUID; }
               }
               else
               { NuevoControl.GUID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 28); }
               #endregion

               #region Calcular Posicion y tamaño  del elemento
               if (Control.Top != 0 && Control.Left != 0 && Control.Width != 0 && Control.Height != 0)
               {
                  NuevoControl.Top = Control.Top;
                  NuevoControl.Left = Control.Left;
                  NuevoControl.Width = Control.Width;
                  NuevoControl.Height = Control.Height;
               }
               else
               {
                  NuevoControl.Top = Control.RangoDatos.Top;
                  NuevoControl.Left = Control.RangoDatos.Left;
                  NuevoControl.Width = Control.RangoDatos.Width;
                  NuevoControl.Height = Control.RangoDatos.Height;
               }
               #endregion

               //no se pueden crear controles el la fila 1 ni la columna 1
               if (Control.RangoDatos.Row > 1 && Control.RangoDatos.Column > 1)
               {
                  #region TextBox
                  if (Control.Tipo == Controles.TextBox)
                  {
                     //Pintar la celda para el control
                     Control.RangoDatos.Borders.Color = ColorLineasControl;
                     Control.RangoDatos.Borders.LineStyle = 1;
                     Control.RangoDatos.Borders.Weight = 2;
                     Control.RangoDatos.Locked = false;

                     #region Creación de label
                     Excel.Range NuevoRango = Control.RangoDatos.get_Offset(0, -1);
                     if (Control.Orientacion)
                     {
                        NuevoRango = Control.RangoDatos.get_Offset(0, -1);
                     }
                     else
                     {
                        NuevoRango = Control.RangoDatos.get_Offset(-1, 0);
                     }

                     if (MostrarLabel)
                     {
                        NuevoRango.Value2 = NuevoControl.Nombre;
                        NuevoRango.Style = "EstiloLabel";
                     }
                     #endregion

                     CreacionControl = true;
                  }
                  #endregion

                  #region ListaDesplegable
                  if (Control.Tipo == Controles.ListaDesplegable)
                  {
                     #region Crear DropDown
                     //Excel.DropDowns xlDropDowns;
                     //Excel.DropDown xlDropDown;

                     //xlDropDowns = ((Excel.DropDowns)(HojaTrabajo.DropDowns(System.Type.Missing)));
                     //xlDropDown = xlDropDowns.Add(NuevoControl.Left, NuevoControl.Top, NuevoControl.Width, NuevoControl.Height, true);
                     //xlDropDown.Name = NuevoControl.GUID;
                     //xlDropDown.Locked = true;
                     #endregion

                     #region Crear label
                     Excel.Range NuevoRango = Control.RangoDatos.get_Offset(0, -1);

                     if (Control.Orientacion)
                     { NuevoRango = Control.RangoDatos.get_Offset(0, -1); }
                     else
                     { NuevoRango = Control.RangoDatos.get_Offset(-1, 0); }

                     if (MostrarLabel)
                     {
                        NuevoRango.Value2 = NuevoControl.Nombre;
                        NuevoRango.Style = "EstiloLabel";
                     }
                     #endregion

                     CreacionControl = true;
                  }
                  #endregion

                  #region CheckBox
                  if (Control.Tipo == Controles.CheckBox)
                  {
                     #region Crear Caja de Chequeo
                     Excel.CheckBoxes CheckBoxes;
                     Excel.CheckBox CheckBox;

                     CheckBoxes = ((Excel.CheckBoxes)(HojaTrabajo.CheckBoxes(System.Type.Missing)));
                     CheckBox = CheckBoxes.Add(NuevoControl.Left, NuevoControl.Top, NuevoControl.Width, NuevoControl.Height);
                     CheckBox.Name = NuevoControl.GUID;
                     CheckBox.Text = string.Empty;
                     CheckBox.Locked = true;
                     #endregion

                     #region Crear Label
                     Excel.Range NuevoRango = Control.RangoDatos.get_Offset(0, -1);

                     if (Control.Orientacion)
                     {
                        NuevoRango = Control.RangoDatos.get_Offset(0, -1);
                     }
                     else
                     {
                        NuevoRango = Control.RangoDatos.get_Offset(-1, 0);
                     }

                     if (MostrarLabel)
                     {
                        NuevoRango.Value2 = NuevoControl.Nombre;
                        NuevoRango.Style = "EstiloLabel";
                     }
                     #endregion

                     CreacionControl = true;
                  }
                  #endregion

                  #region ListBox
                  if (Control.Tipo == Controles.ListBox)
                  {
                     #region Crear ListBox
                     Excel.ListBoxes ListBoxes;
                     Excel.ListBox ListBox;

                     ListBoxes = ((Excel.ListBoxes)(HojaTrabajo.ListBoxes(System.Type.Missing)));
                     ListBox = ListBoxes.Add(NuevoControl.Left, NuevoControl.Top, NuevoControl.Width, NuevoControl.Height);
                     ListBox.Name = NuevoControl.GUID;
                     ListBox.Locked = true;
                     #endregion

                     #region Crear Label
                     Excel.Range NuevoRango = Control.RangoDatos.get_Offset(0, -1);
                     if (Control.Orientacion)
                     {
                        NuevoRango = Control.RangoDatos.get_Offset(0, -1);
                     }
                     else
                     {
                        NuevoRango = Control.RangoDatos.get_Offset(-1, 0);
                     }

                     if (MostrarLabel)
                     {
                        NuevoRango.Value2 = NuevoControl.Nombre;
                        NuevoRango.Style = "EstiloLabel";
                     }
                     #endregion

                     CreacionControl = true;
                  }
                  #endregion

                  #region ComboBusqueda
                  if (Control.Tipo == Controles.ComboBusqueda)
                  {
                     #region Crear elementos asociados a combo(Caja de texto y Lista desplegable)
                     Control.RangoDatos.MergeCells = false;

                     NuevoControl.RangoDatos.Borders.Color = ColorLineasControl;
                     NuevoControl.RangoDatos.Borders.LineStyle = 2;
                     NuevoControl.RangoDatos.Borders.Weight = 2;
                     NuevoControl.RangoDatos.Locked = false;

                     //Excel.DropDowns xlDropDowns;
                     //Excel.DropDown xlDropDown;

                     Excel.Range RangoDatosDrop = Control.RangoDatos.get_Offset(0, 1);
                     Excel.Range RangoLabel = Control.RangoDatos.get_Offset(-1, 0);

                     RangoDatosDrop.Borders.Color = ColorLineasControl;
                     RangoDatosDrop.Borders.LineStyle = 2;
                     RangoDatosDrop.Borders.Weight = 2;

                     if (Control.Orientacion)
                     {
                        RangoDatosDrop = Control.RangoDatos.get_Offset(0, 1);
                        RangoLabel = Control.RangoDatos.get_Offset(0, -1);
                     }
                     else
                     {
                        RangoDatosDrop = Control.RangoDatos.get_Offset(1, 0);
                        RangoLabel = Control.RangoDatos.get_Offset(-1, 0);
                     }

                     //xlDropDowns = ((Excel.DropDowns)(HojaTrabajo.DropDowns(System.Type.Missing)));
                     //xlDropDown = xlDropDowns.Add(NuevoControl.Left, NuevoControl.Top, NuevoControl.Width, NuevoControl.Height, true);
                     //xlDropDown.Name = NuevoControl.GUID;
                     //xlDropDown.Locked = true;

                     string NombreMostrar = string.Empty;

                     if (NuevoControl.Nombre.Contains("@"))
                     {
                        NombreMostrar = NuevoControl.Nombre.Split('@')[1];
                     }
                     else
                     {
                        NombreMostrar = NuevoControl.Nombre;
                     }

                     if (MostrarLabel)
                     {
                        RangoLabel.Value2 = NombreMostrar;
                        RangoLabel.Style = "EstiloLabel";
                     }
                     #endregion

                     CreacionControl = true;
                  }
                  #endregion

                  // Si creó algún control, guardar para posterior registro en aplicacion
                  if (CreacionControl)
                  {
                     #region Ajustar formato de celda

                     if (Control.TipoValidacion == ValidacionesDatos.ValidarFecha)
                     {
                        Control.RangoDatos.NumberFormat = "dd/mm/yyyy";
                     }

                     if (Control.TipoValidacion == ValidacionesDatos.ValidarHora)
                     {
                        Control.RangoDatos.NumberFormat = "hh:mm";
                     }
                     #endregion

                     ControlesCreados.Add(NuevoControl);
                  }
               }
            }
            catch
            {

            }
         }
         return ControlesCreados;
      }

      /// <summary>
      /// Crea y dibuja un control en una hoja de excel
      /// Propiedades de Obj (Controles) mínimas requeridas: Nombre, HojaExcel, RangoDatos, Tipo, Orientacion, TipoValidacion
      /// </summary>
      /// <param name="ListaControles">Control a pintar</param>
      /// <param name="MostrarLabel">Indica si se debe dibujar el nombre del control</param>
      /// <returns>control creado, o new Controles() si ocurre algún error en la creación</returns>
      public static Controles CrearControl(Controles Control, bool MostrarLabel, Color DesObligatorio, Color DesOpcional)
      {
         Color ColorLineasControl = Color.Black;
         bool CreacionControl = false;

         Excel.Worksheet HojaTrabajo = Control.HojaExcel.InnerObject;

         Color ColorFondoDescriptor = System.Drawing.Color.Transparent;
         if (Control.Obligatorio)
         { ColorFondoDescriptor = DesObligatorio; }
         else
         { ColorFondoDescriptor = DesOpcional; }

         Controles NuevoControl = new Controles();
         NuevoControl = Control;

         try
         {
            #region Calcular GUID
            if (!string.IsNullOrEmpty(Control.GUID))
            {
               if (Control.GUID.Length > 30)
               { NuevoControl.GUID = Control.GUID.Substring(0, 30); }
               else
               { NuevoControl.GUID = Control.GUID; }
            }
            else
            { NuevoControl.GUID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 28); }
            #endregion

            #region Calcular Posicion y tamaño  del elemento
            if (Control.Top != 0 && Control.Left != 0 && Control.Width != 0 && Control.Height != 0)
            {
               NuevoControl.Top = Control.Top;
               NuevoControl.Left = Control.Left;
               NuevoControl.Width = Control.Width;
               NuevoControl.Height = Control.Height;
            }
            else
            {
               if (NuevoControl.Tipo == Controles.ComboBusqueda)
               {
                  NuevoControl.Top = Control.RangoDatos.get_Offset(0, 1).Top;
                  NuevoControl.Left = Control.RangoDatos.get_Offset(0, 1).Left;
                  NuevoControl.Width = Control.RangoDatos.get_Offset(0, 1).Width;
                  NuevoControl.Height = Control.RangoDatos.get_Offset(0, 1).Height;
               }
               else
               {
                  NuevoControl.Top = Control.RangoDatos.Top;
                  NuevoControl.Left = Control.RangoDatos.Left;
                  NuevoControl.Width = Control.RangoDatos.Width;
                  NuevoControl.Height = Control.RangoDatos.Height;
               }
            }
            #endregion

            //no se pueden crear controles el la fila 1 ni la columna 1
            if (Control.RangoDatos.Row > 1 && Control.RangoDatos.Column > 1)
            {
               switch (Control.Tipo)
               {
                  case Controles.TextBox:
                     #region TextBox
                     //Pintar la celda para el control
                     Control.RangoDatos.Borders.Color = ColorLineasControl;
                     Control.RangoDatos.Borders.LineStyle = 1;
                     Control.RangoDatos.Borders.Weight = 2;
                     Control.RangoDatos.Locked = false;

                     if (ColorFondoDescriptor != Color.Transparent)
                     { Control.RangoDatos.Interior.Color = ColorFondoDescriptor; }

                     #region Creación de label
                     Excel.Range NuevoRango = Control.RangoDatos.get_Offset(0, -1);
                     if (Control.Orientacion)
                     {
                        NuevoRango = Control.RangoDatos.get_Offset(0, -1);
                     }
                     else
                     {
                        NuevoRango = Control.RangoDatos.get_Offset(-1, 0);
                     }

                     if (MostrarLabel)
                     {
                        NuevoRango.Value2 = NuevoControl.Nombre;
                        NuevoRango.Style = "EstiloLabel";
                     }
                     #endregion
                     #endregion
                     CreacionControl = true;
                     break;
                  case Controles.ListaDesplegable:
                     #region ListaDesplegable
                     #region Crear DropDown
                     //Excel.DropDowns xlDropDowns;
                     //Excel.DropDown xlDropDown;

                     //xlDropDowns = ((Excel.DropDowns)(HojaTrabajo.DropDowns(System.Type.Missing)));
                     //xlDropDown = xlDropDowns.Add(NuevoControl.Left, NuevoControl.Top, NuevoControl.Width, NuevoControl.Height, true);
                     //xlDropDown.Name = NuevoControl.GUID;
                     //xlDropDown.Locked = true;
                     Control.RangoDatos.Borders.Color = ColorLineasControl;
                     Control.RangoDatos.Borders.LineStyle = 1;
                     Control.RangoDatos.Borders.Weight = 2;
                     Control.RangoDatos.Locked = false;
                     if (ColorFondoDescriptor != Color.Transparent)
                     { Control.RangoDatos.Interior.Color = ColorFondoDescriptor; }
                     #endregion

                     #region Crear label
                     Excel.Range RangoLista = Control.RangoDatos.get_Offset(0, -1);

                     if (Control.Orientacion)
                     { RangoLista = Control.RangoDatos.get_Offset(0, -1); }
                     else
                     { RangoLista = Control.RangoDatos.get_Offset(-1, 0); }

                     if (MostrarLabel)
                     {
                        RangoLista.Value2 = NuevoControl.Nombre;
                        RangoLista.Style = "EstiloLabel";
                     }
                     #endregion
                     #endregion
                     CreacionControl = true;
                     break;
                  case Controles.CheckBox:
                     #region CheckBox
                     #region Crear Caja de Chequeo
                     Excel.CheckBoxes CheckBoxes;
                     Excel.CheckBox CheckBox;

                     Control.RangoDatos.Borders.Color = ColorLineasControl;
                     Control.RangoDatos.Borders.LineStyle = 1;
                     Control.RangoDatos.Borders.Weight = 2;

                     CheckBoxes = ((Excel.CheckBoxes)(HojaTrabajo.CheckBoxes(System.Type.Missing)));
                     CheckBox = CheckBoxes.Add(NuevoControl.Left, NuevoControl.Top, NuevoControl.Width, NuevoControl.Height);
                     CheckBox.Name = NuevoControl.GUID;
                     CheckBox.Text = string.Empty;
                     CheckBox.Locked = true;

                     if (ColorFondoDescriptor != Color.Transparent)
                     { Control.RangoDatos.Interior.Color = ColorFondoDescriptor; }
                     #endregion

                     #region Crear Label
                     Excel.Range RangoCheck = Control.RangoDatos.get_Offset(0, -1);

                     if (Control.Orientacion)
                     {
                        RangoCheck = Control.RangoDatos.get_Offset(0, -1);
                     }
                     else
                     {
                        RangoCheck = Control.RangoDatos.get_Offset(-1, 0);
                     }

                     if (MostrarLabel)
                     {
                        RangoCheck.Value2 = NuevoControl.Nombre;
                        RangoCheck.Style = "EstiloLabel";
                     }
                     #endregion
                     #endregion
                     CreacionControl = true;
                     break;
                  case Controles.ListBox:
                     #region ListBox
                     #region Crear ListBox
                     Excel.ListBoxes ListBoxes;
                     Excel.ListBox ListBox;

                     ListBoxes = ((Excel.ListBoxes)(HojaTrabajo.ListBoxes(System.Type.Missing)));
                     ListBox = ListBoxes.Add(NuevoControl.Left, NuevoControl.Top, NuevoControl.Width, NuevoControl.Height);
                     ListBox.Name = NuevoControl.GUID;
                     ListBox.Locked = true;

                     if (ColorFondoDescriptor != Color.Transparent)
                     { Control.RangoDatos.Interior.Color = ColorFondoDescriptor; }
                     #endregion

                     #region Crear Label
                     Excel.Range RangoList = Control.RangoDatos.get_Offset(0, -1);
                     if (Control.Orientacion)
                     {
                        RangoList = Control.RangoDatos.get_Offset(0, -1);
                     }
                     else
                     {
                        RangoList = Control.RangoDatos.get_Offset(-1, 0);
                     }

                     if (MostrarLabel)
                     {
                        RangoList.Value2 = NuevoControl.Nombre;
                        RangoList.Style = "EstiloLabel";
                     }
                     #endregion

                     #endregion
                     CreacionControl = true;
                     break;
                  case Controles.ComboBusqueda:
                     #region Crear elementos asociados a combo(Caja de texto y Lista desplegable)
                     Control.RangoDatos.MergeCells = false;

                     NuevoControl.RangoDatos.Borders.Color = ColorLineasControl;
                     NuevoControl.RangoDatos.Borders.LineStyle = 2;
                     NuevoControl.RangoDatos.Borders.Weight = 2;
                     NuevoControl.RangoDatos.Locked = false;

                     //Excel.DropDowns xlDropDownsCombo;
                     //Excel.DropDown xlDropDownCombo;

                     Excel.Range RangoDatosDrop = Control.RangoDatos.get_Offset(0, 1);
                     Excel.Range RangoLabel = Control.RangoDatos.get_Offset(-1, 0);

                     RangoDatosDrop.MergeCells = false;
                     RangoDatosDrop.Locked = false;
                     RangoDatosDrop.Borders.Color = ColorLineasControl;

                     if (ColorFondoDescriptor != Color.Transparent)
                     {
                        Control.RangoDatos.Interior.Color = ColorFondoDescriptor;
                        RangoDatosDrop.Interior.Color = ColorFondoDescriptor;
                     }
                     //RangoDatosDrop.Borders.LineStyle = 2;
                     //RangoDatosDrop.Borders.Weight = 2;

                     if (Control.Orientacion)
                     {
                        RangoDatosDrop = Control.RangoDatos.get_Offset(0, 1);
                        RangoLabel = Control.RangoDatos.get_Offset(0, -1);
                     }
                     else
                     {
                        RangoDatosDrop = Control.RangoDatos.get_Offset(1, 0);
                        RangoLabel = Control.RangoDatos.get_Offset(-1, 0);
                     }

                     //xlDropDownsCombo = ((Excel.DropDowns)(HojaTrabajo.DropDowns(System.Type.Missing)));
                     //xlDropDownCombo = xlDropDownsCombo.Add(NuevoControl.Left, NuevoControl.Top, NuevoControl.Width, NuevoControl.Height, true);
                     //xlDropDownCombo.Name = NuevoControl.GUID;
                     //xlDropDown.Locked = true;



                     string NombreMostrar = string.Empty;

                     if (NuevoControl.Nombre.Contains("@"))
                     {
                        NombreMostrar = NuevoControl.Nombre.Split('@')[1];
                     }
                     else
                     {
                        NombreMostrar = NuevoControl.Nombre;
                     }

                     if (MostrarLabel)
                     {
                        RangoLabel.Value2 = NombreMostrar;
                        RangoLabel.Style = "EstiloLabel";
                     }
                     #endregion
                     CreacionControl = true;
                     break;
               }

               // Si creó algún control, guardar para posterior registro en aplicacion
               if (CreacionControl)
               {
                  #region Ajustar formato de celda

                  if (Control.TipoValidacion == ValidacionesDatos.ValidarFecha)
                  {
                     Control.RangoDatos.NumberFormat = "dd/mm/yyyy";

                  }

                  else if (Control.TipoValidacion == ValidacionesDatos.ValidarHora)
                  {
                     Control.RangoDatos.NumberFormat = "hh:mm";
                  }
                  #endregion
               }
               else
               { return new Controles(); }
            }
            else
            { return new Controles(); }
         }
         catch
         { return new Controles(); }
         return NuevoControl;
      }

      /// <summary>
      /// Elimina un control de la hoja de excel
      /// </summary>
      /// <param name="Control"></param>
      /// <param name="ConservarResultados">indica si se deben borrar o no los resultados del control (solo aplica para Controles.CheckBox, Controles.ListBox </param>
      /// <param name="BorrarLabel">Indica si se debe borrar el nombre del control dibujado</param>
      /// <returns>resultado de la operación de eliminacion</returns>
      public static bool EliminarControl(Controles Control, bool ConservarResultados, bool BorrarLabel)
      {
         ExcelTools.Worksheet HojaTrabajo = Control.HojaExcel;
         Excel.Range RangoDatos = Control.RangoDatos;
         string GUIDelemento = Control.GUID;
         string TipoControl = Control.Tipo;
         bool orientacion = Control.Orientacion;
         Color ColorFondoEliminado = Color.White;
         bool Resultado = false;

         try
         {
            switch (TipoControl)
            {
               case Controles.TextBox:
                  #region Eliminar TextBox
                  RangoDatos.MergeCells = false;
                  RangoDatos.Interior.Color = ColorFondoEliminado;
                  Excel.Range NuevoRangoTextBox = RangoDatos.get_Offset(0, -1);

                  if (orientacion)
                  { NuevoRangoTextBox = RangoDatos.get_Offset(0, -1); }
                  else
                  { NuevoRangoTextBox = RangoDatos.get_Offset(-1, 0); }

                  if (BorrarLabel)
                  {
                     NuevoRangoTextBox.MergeCells = false;
                     NuevoRangoTextBox.Value2 = "";
                     NuevoRangoTextBox.Style = "SinEstilo";
                     RangoDatos.Style = "SinEstilo";
                  }

                  RangoDatos.Locked = true;
                  #endregion
                  Resultado = true;
                  break;
               case Controles.ListaDesplegable:
                  #region ListaDesplegable
                  RangoDatos.MergeCells = false;
                  RangoDatos.Validation.Delete();
                  RangoDatos.Interior.Color = ColorFondoEliminado;
                  Excel.Range NuevoRangoLD = RangoDatos.get_Offset(0, -1);
                  if (orientacion)
                  { NuevoRangoLD = RangoDatos.get_Offset(0, -1); }
                  else
                  { NuevoRangoLD = RangoDatos.get_Offset(-1, 0); }

                  // Rango de Label
                  if (BorrarLabel)
                  {
                     NuevoRangoLD.MergeCells = false;
                     NuevoRangoLD.Value2 = "";
                     NuevoRangoLD.Style = "SinEstilo";
                     RangoDatos.Style = "SinEstilo";
                  }

                  //NuevoRango.Locked = true;
                  RangoDatos.Locked = true;
                  #endregion
                  Resultado = true;
                  break;
               case Controles.CheckBox:
                  #region CheckBox
                  RangoDatos.MergeCells = false;
                  RangoDatos.Interior.Color = ColorFondoEliminado;
                  Excel.Range NuevoRangoCB = RangoDatos.get_Offset(0, -1);

                  if (orientacion)
                  { NuevoRangoCB = RangoDatos.get_Offset(0, -1); }
                  else
                  { NuevoRangoCB = RangoDatos.get_Offset(-1, 0); }

                  // Rango de Label
                  if (BorrarLabel)
                  {
                     NuevoRangoCB.MergeCells = false;
                     NuevoRangoCB.Value2 = "";
                     NuevoRangoCB.Style = "SinEstilo";
                     RangoDatos.Style = "SinEstilo";
                  }

                  try
                  {
                     Excel.CheckBox CheckBox;
                     CheckBox = (Excel.CheckBox)HojaTrabajo.CheckBoxes(GUIDelemento);

                     if (ConservarResultados)
                     {
                        if (CheckBox.Value == 1)
                        { RangoDatos.Value2 = "SI"; }
                        else
                        { RangoDatos.Value2 = "NO"; }
                     }
                     else
                     { RangoDatos.Value2 = ""; }

                     CheckBox.Delete();
                  }
                  catch
                  {
                     if (!ConservarResultados)
                     {
                        RangoDatos.Value2 = "";
                     }
                  }

                  //NuevoRango.Locked = true;
                  RangoDatos.Locked = true;
                  #endregion
                  Resultado = true;
                  break;
               case Controles.ListBox:
                  #region ListBox
                  RangoDatos.MergeCells = false;
                  RangoDatos.Interior.Color = ColorFondoEliminado;
                  Excel.Range NuevoRangoLB = RangoDatos.get_Offset(0, -1);

                  if (orientacion)
                  { NuevoRangoLB = RangoDatos.get_Offset(0, -1); }
                  else
                  { NuevoRangoLB = RangoDatos.get_Offset(-1, 0); }

                  // Rango de Label
                  if (BorrarLabel)
                  {
                     NuevoRangoLB.MergeCells = false;
                     NuevoRangoLB.Value2 = "";
                     NuevoRangoLB.Style = "SinEstilo";
                     RangoDatos.Style = "SinEstilo";
                  }

                  try
                  {
                     Excel.ListBox List = (Excel.ListBox)HojaTrabajo.ListBoxes(GUIDelemento);

                     if (ConservarResultados)
                     {
                        string dato = string.Empty;
                        for (int ciclo = 1; ciclo < List.ListCount - 1; ciclo++)
                        {
                           if (List.Selected[ciclo])
                           {
                              dato = List.List[ciclo];
                           }
                        }
                        RangoDatos.set_Value(System.Type.Missing, dato.Trim());
                     }
                     else
                     { RangoDatos.Value2 = ""; }

                     List.Delete();
                  }
                  catch
                  {
                     if (!ConservarResultados)
                     {
                        RangoDatos.MergeCells = false; RangoDatos.Value2 = "";
                     }
                  }

                  //NuevoRango.Locked = true; No activar por que presenta problemas con controles cercanos
                  RangoDatos.Locked = true;
                  #endregion
                  Resultado = true;
                  break;
               case Controles.ComboBusqueda:
                  #region ComboBusqueda
                  RangoDatos.MergeCells = false;
                  RangoDatos.Locked = false;

                  Excel.Range RangoDatosDrop = RangoDatos.get_Offset(0, 1);
                  Excel.Range RangoLabel = RangoDatos.get_Offset(-1, 0);
                  RangoDatos.Interior.Color = ColorFondoEliminado;
                  RangoDatosDrop.Interior.Color = ColorFondoEliminado;
                  RangoDatosDrop.Validation.Delete();

                  RangoDatosDrop.MergeCells = false;
                  RangoDatosDrop.Locked = false;

                  if (orientacion)
                  {
                     RangoDatosDrop = RangoDatos.get_Offset(0, 1);
                     RangoLabel = RangoDatos.get_Offset(0, -1);
                  }
                  else
                  {
                     RangoDatosDrop = RangoDatos.get_Offset(1, 0);
                     RangoLabel = RangoDatos.get_Offset(-1, 0);
                  }

                  Excel.Range RangoMezcla = HojaTrabajo.Cells.get_Range(RangoDatos.Address, RangoDatosDrop.Address);

                  if (BorrarLabel)
                  {
                     RangoLabel.MergeCells = false;
                     RangoLabel.Locked = false;

                     RangoLabel.Value2 = "";
                     RangoLabel.Style = "SinEstilo";
                     RangoMezcla.Locked = false;
                     RangoMezcla.Style = "SinEstilo";
                  }
                  #endregion
                  Resultado = true;
                  break;
            }
            return Resultado;
         }
         catch
         {
            return false;
         }
      }

      /// <summary>
      /// Lee el valor del control
      /// </summary>
      /// <param name="Control"></param>
      /// <returns>texto del control</returns>
      public static string LeerValorControl(Controles Control)
      {
         Excel.Worksheet HojaTrabajo = Control.HojaExcel.InnerObject;
         string GUIDelemento = Control.GUID;
         string TipoControl = Control.Tipo;
         string RangoDatosAddress = Control.RangoDatos.Address;

         Excel.Range RangoDatos = HojaTrabajo.get_Range(RangoDatosAddress, System.Type.Missing);

         string Valor = string.Empty;

         try
         {
            switch (TipoControl)
            {
               case Controles.ListaDesplegable:
                  try
                  { Valor = RangoDatos.Text; }
                  catch
                  { Valor = RangoDatos.Value2.ToString(); }
                  break;
               case Controles.ListBox:
                  try
                  {
                     Excel.ListBox List = (Excel.ListBox)HojaTrabajo.ListBoxes(GUIDelemento);
                     string dato = string.Empty;
                     for (int ciclo = 1; ciclo < List.ListCount - 1; ciclo++)
                     {
                        if (List.Selected[ciclo])
                        {
                           dato = List.List[ciclo];
                        }
                     }
                     Valor = dato.Trim();
                  }
                  catch
                  {
                     try
                     { Valor = RangoDatos.Text; }
                     catch
                     { Valor = RangoDatos.Value2.ToString(); }
                  }
                  break;
               case Controles.ComboBusqueda:
                  /*try
                  {
                      Excel.DropDown Ddl2 = (Excel.DropDown)HojaTrabajo.DropDowns(GUIDelemento);
                      Valor = Ddl2.Text;
                  }
                  catch
                  {
                      try
                      { Valor = RangoDatos.Text; }
                      catch
                      { Valor = RangoDatos.Value2.ToString(); }
                  }*/
                  try
                  { Valor = RangoDatos.get_Offset(0, 1).Text; }
                  catch
                  { Valor = RangoDatos.get_Offset(0, 1).Value2.ToString(); }
                  break;
               case Controles.TextBox:
                  if (!string.IsNullOrEmpty(RangoDatos.Address))
                  {
                     if (RangoDatos.Value2 != null)
                     {
                        try
                        { Valor = RangoDatos.Text; }
                        catch
                        { Valor = RangoDatos.Value2.ToString(); }
                     }
                  }
                  break;

               case Controles.CheckBox:
                  try
                  {
                     Excel.CheckBox CheckBox;
                     CheckBox = (Excel.CheckBox)HojaTrabajo.CheckBoxes(GUIDelemento);

                     if (CheckBox.Value == 1)
                     { Valor = "true"; }
                     else
                     { Valor = "false"; }
                  }
                  catch
                  {
                     if (!string.IsNullOrEmpty(RangoDatos.Address))
                     {
                        if (RangoDatos.Value2 != null)
                        {
                           try
                           { Valor = RangoDatos.Text; }
                           catch
                           { Valor = RangoDatos.Value2.ToString(); }

                           if (Valor == "SI")
                           {
                              Valor = "true";
                           }
                           else if (Valor == "NO")
                           {
                              Valor = "false";
                           }
                           else
                           {
                              Valor = "false";
                           }
                        }
                     }
                  }

                  break;
               default:
                  Valor = string.Empty;
                  break;
            }

            return Valor;
         }
         catch
         {
            Valor = string.Empty;
            return Valor;
         }
      }

      /// <summary>
      /// Crea controles tipo Controles.Combobusqueda para descriptores con precedencias configuradas
      /// </summary>
      /// <param name="Usuario">Datos de sesion del usuario (para buscar informacion de precedencias de descriptor)</param>
      /// <param name="Control"></param>
      /// <returns>lista de controles creados</returns>
      public static List<Controles> CrearControlDependencias(Login Usuario, ConexionesExcel Conexion, Controles Control, Color DescObligatorio, Color DescOpcional)
      {
         ExcelTools.Worksheet HojaTrabajo = Control.HojaExcel;
         List<Controles> ControlesCreados = new List<Controles>();
         Excel.Range Rangotemporal = Control.RangoDatos;

         #region Traer información de precedencias del descriptor
         wsOfficeSGD.wsOfficeSGD wsServicio = new wsOfficeSGD.wsOfficeSGD();
         wsServicio.Url = Conexion.urlWsOfficeSGD;
         wsServicio.Timeout = Conexion.TimeOut;

         DataEncryption Enc = new DataEncryption();
         Byte[] SessionID = Enc.Encryption(Usuario.IdUsuario + ">" + Usuario.NomUsuario + ">" + Usuario.CadenaConexion + ">" + Usuario.EmpresaId);

         XmlNode XmlNode = wsServicio.ConsultarPrecedenciasDescriptor(SessionID, Control.Id);
         DataTable PrecedenciasDescriptor = XML.XMLtoDataTable(XmlNode);
         #endregion

         if (PrecedenciasDescriptor.Rows.Count > 0)
         {
            Controles ControlNuevo = new Controles();
            for (int CicloFila = 0; CicloFila < PrecedenciasDescriptor.Rows.Count; CicloFila++)
            {
               ControlNuevo = new Controles();

               string Precedencia = PrecedenciasDescriptor.Rows[CicloFila]["DESfuenteDatosVariable"].ToString();
               int UltimaPosicion = (PrecedenciasDescriptor.Rows.Count - 1);

               ControlNuevo.Nombre = Control.Nombre + ":" + Precedencia; ;
               ControlNuevo.Id = PrecedenciasDescriptor.Rows[CicloFila]["codigo"].ToString();
               ControlNuevo.RangoDatos = Rangotemporal;
               ControlNuevo.Tipo = Controles.ComboBusqueda;
               ControlNuevo.HojaExcel = Control.HojaExcel;
               ControlNuevo.Orientacion = Control.Orientacion;
               ControlNuevo.TipoValidacion = Control.TipoValidacion;
               ControlNuevo.IdFormato = Control.IdFormato;
               ControlNuevo.IdSubSerie = Control.IdSubSerie;

               ControlNuevo.TablaNombre = Control.TablaNombre;
               ControlNuevo.TablaFila = Control.TablaFila;
               ControlNuevo.TablaRangoInicial = Control.TablaRangoInicial;
               ControlNuevo.TablaNumeroMaximoRegistros = Control.TablaNumeroMaximoRegistros;
               ControlNuevo.Propiedades = Control.Propiedades;

               #region El descriptor principal de precedencias es el último nivel de precedencia
               if (CicloFila == UltimaPosicion && Control.Principal)
               { ControlNuevo.Principal = true; }
               else
               { ControlNuevo.Principal = false; }
               #endregion

               Controles ControlCreado = CrearControl(ControlNuevo, true, DescObligatorio, DescOpcional);
               if (ControlCreado != null)
               {
                  ControlesCreados.Add(ControlCreado);

                  if (Control.Orientacion)
                  { 
                     Rangotemporal = Rangotemporal.get_Offset(1, 0); 
                  }
                  else
                  { 
                     //Dos por que el combo Búsqueda ocupa dos posiciones
                     Rangotemporal = Rangotemporal.get_Offset(0, 2); 
                  }
               }
            }
         }

         return ControlesCreados;
      }

      /// <summary>
      /// Crea una fila de la tabla de datos, con columnas representadas por descriptores.
      /// </summary>
      /// <param name="DatosUsuario">datos del usuario de sesion</param>
      /// <param name="Conexion">datos de conexion de la sesion</param>
      /// <param name="HojaTrabajo">hoja de creacion de la tabla</param>
      /// <param name="CrearEncabezado">indica si se debe crear el encabezado de la tabla (Titulo y nombre de columans)</param>
      /// <param name="ControlesAgregar">Lista de controles para agragar</param>
      /// <param name="RangoDatos">Ubicacion de la tabla</param>
      /// <param name="IdFila">número de la fila (para identificar los nuevos controles creados)</param>
      /// <param name="NombreTabla">nombre de la tabla</param>
      /// <returns>lista de controles creados correctamente</returns>
      public static List<Controles> CrearControlTabla(Login DatosUsuario, ConexionesExcel Conexion, ExcelTools.Worksheet HojaTrabajo, bool CrearEncabezado,
                      List<Controles> ControlesAgregar, Excel.Range RangoDatos, string IdFila, string NombreTabla, Color DesObligatorio, Color DesOpcional)
      {
         List<Controles> ControlesCreados = new List<Controles>();

         int Fila = int.Parse(IdFila);

         if (Fila > 1)
         {  Fila = Fila - 1;  }

         //Filtra solo los controles asociados a la tabla específica
         List<Controles> ControlesFiltro = ControlesAgregar.FindAll(delegate(Controles c) { return (c.TablaNombre == NombreTabla && c.TablaFila == Fila.ToString()); });

         if (CrearEncabezado)
         {
            #region Crear Encabezado de Tabla

            Excel.Range RangoInicialNombre = RangoDatos;
            RangoDatos = RangoDatos.get_Offset(1, 0);
            RangoDatos.Value2 = "No.     ";
            RangoDatos.Style = "EstiloLabel";
            RangoDatos.Borders.Color = System.Drawing.Color.Black;
            RangoDatos.Borders.LineStyle = 1;
            RangoDatos.Borders.Weight = 2;
            RangoDatos.Columns.AutoFit();
            RangoDatos = RangoDatos.get_Offset(0, 1);
            RangoDatos.Locked = true;

            Excel.Range RangoTemp = RangoDatos;
            int NumeroColumnas = 0;
            foreach (Controles Control in ControlesFiltro)
            {
               if (Control.Tipo == Controles.ComboBusqueda || Control.Tipo == Controles.ControlDependencias)
               {
                  RangoTemp = HojaTrabajo.Cells.get_Range(RangoTemp.Address, RangoTemp.get_Offset(0, 1).Address);
                  RangoTemp.MergeCells = true;
                  NumeroColumnas = NumeroColumnas + 2;
               }
               else
               {
                  NumeroColumnas++;
               }

               RangoTemp.Value2 = Control.Nombre;
               RangoTemp.Style = "EstiloLabel";
               RangoTemp.Locked = true;
               RangoTemp.Borders.Color = System.Drawing.Color.Black;
               RangoTemp.Borders.LineStyle = 1;
               RangoTemp.Borders.Weight = 2;

               RangoTemp.Columns.AutoFit();

               RangoTemp = RangoTemp.get_Offset(0, 1);
            }

            RangoInicialNombre = HojaTrabajo.Cells.get_Range(RangoInicialNombre.Address, RangoInicialNombre.get_Offset(0, NumeroColumnas).Address);
            RangoInicialNombre.MergeCells = true;
            RangoInicialNombre.Value2 = NombreTabla;
            RangoInicialNombre.Style = "EstiloLabel";
            RangoInicialNombre.Borders.Color = System.Drawing.Color.Black;
            RangoInicialNombre.Borders.LineStyle = 1;
            RangoInicialNombre.Borders.Weight = 2;
            RangoInicialNombre.Locked = true;

            #endregion
         }

         #region Insertar Nuevo Registro (Fila) a la tabla

         Excel.Range Rango = RangoDatos.get_Offset(1, 0);
         Rango.get_Offset(0, -1).MergeCells = false;
         Rango.get_Offset(0, -1).Locked = false;
         //Rango.get_Offset(0, -1).Value2 = IdFila;
         //Rango.get_Offset(0, -1).Style = "EstiloLabel";
         Rango.Borders.Color = System.Drawing.Color.Black;
         Rango.Borders.LineStyle = 1;
         Rango.Borders.Weight = 2;


         DataTable ConsultaDescriptores = Plantillas.ConsultaDescriptores(DatosUsuario, Conexion, "ConsultarDescriptoresCategoria", "");

         //Rango.get_Offset(0, 1);

         if (ConsultaDescriptores.Rows.Count > 0)
         {
            foreach (Controles Control in ControlesFiltro)
            {
               string Filtro = " DESid = '" + Control.Id + "' ";
               DataView Descriptor = new DataView(ConsultaDescriptores, Filtro, "", DataViewRowState.CurrentRows);

               if (Descriptor.ToTable().Rows.Count > 0)
               {
                  Controles NuevoControl = new Controles();

                  #region Propiedades del nuevo control
                  NuevoControl.Nombre = Control.Nombre;
                  NuevoControl.Id = Control.Id;
                  NuevoControl.IdFormato = Control.IdFormato;
                  NuevoControl.IdSubSerie = Control.IdSubSerie;

                  NuevoControl.Tipo = Control.Tipo;
                  NuevoControl.HojaExcel = Control.HojaExcel;
                  NuevoControl.LibroExcel = Control.LibroExcel;

                  NuevoControl.RangoDatos = Control.RangoDatos.get_Offset(1, 0);
                  NuevoControl.Orientacion = true;
                  NuevoControl.Obligatorio = false;
                  NuevoControl.Principal = false;

                  NuevoControl.TipoValidacion = Descriptor.ToTable().Rows[0]["DTDdescripcion"].ToString();

                  NuevoControl.TablaNombre = Control.TablaNombre;
                  NuevoControl.TablaNumeroMaximoRegistros = Control.TablaNumeroMaximoRegistros;
                  NuevoControl.TablaFila = IdFila;
                  NuevoControl.TablaRangoInicial = Control.TablaRangoInicial;
                  #endregion

                  Controles ControlCreado = CrearControl(NuevoControl, false, DesObligatorio, DesOpcional);

                  if (ControlCreado != null)
                  {
                     ControlesCreados.Add(ControlCreado);
                  }
               }
            }
         }
         #endregion

         return ControlesCreados;
      }

      /// <summary>
      /// Devuelve un array con el valor de las propiedades del control
      /// [0] - Top, [1] - Left, [2] - Width, [3] - Height
      /// </summary>
      /// <param name="Control"></param>
      /// <returns></returns>
      public static string[] LeerPropiedadesControl(Controles Control)
      {
         Excel.Worksheet HojaTrabajo = Control.HojaExcel.InnerObject;
         Excel.Range RangoDatos = Control.RangoDatos;
         string GUIDelemento = Control.GUID;
         string TipoControl = Control.Tipo;

         // [0] - Top, [1] - Left, [2] - Width, [3] - Height
         string[] Propiedades = new string[6];
         try
         {
            switch (TipoControl)
            {
               case Controles.ListaDesplegable:
                  Propiedades[0] = RangoDatos.Top.ToString();
                  Propiedades[1] = RangoDatos.Left.ToString();
                  Propiedades[2] = RangoDatos.Width.ToString();
                  Propiedades[3] = RangoDatos.Height.ToString();

                  break;
               case Controles.ListBox:
                  try
                  {
                     Excel.ListBox List = (Excel.ListBox)HojaTrabajo.ListBoxes(GUIDelemento);
                     Propiedades[0] = List.Top.ToString();
                     Propiedades[1] = List.Left.ToString();
                     Propiedades[2] = List.Width.ToString();
                     Propiedades[3] = List.Height.ToString();
                  }
                  catch
                  {
                     Propiedades[0] = RangoDatos.Top.ToString();
                     Propiedades[1] = RangoDatos.Left.ToString();
                     Propiedades[2] = RangoDatos.Width.ToString();
                     Propiedades[3] = RangoDatos.Height.ToString();
                  }
                  break;
               case Controles.ComboBusqueda:
                  Propiedades[0] = RangoDatos.Top.ToString();
                  Propiedades[1] = RangoDatos.Left.ToString();
                  Propiedades[2] = RangoDatos.Width.ToString();
                  Propiedades[3] = RangoDatos.Height.ToString();

                  break;
               case Controles.TextBox:
                  if (!string.IsNullOrEmpty(RangoDatos.Address))
                  {
                     Propiedades[0] = RangoDatos.Top.ToString();
                     Propiedades[1] = RangoDatos.Left.ToString();
                     Propiedades[2] = RangoDatos.Width.ToString();
                     Propiedades[3] = RangoDatos.Height.ToString();
                  }
                  break;

               case Controles.CheckBox:
                  try
                  {
                     Excel.CheckBox CheckBox;
                     CheckBox = (Excel.CheckBox)HojaTrabajo.CheckBoxes(GUIDelemento);

                     Propiedades[0] = CheckBox.Top.ToString();
                     Propiedades[1] = CheckBox.Left.ToString();
                     Propiedades[2] = CheckBox.Width.ToString();
                     Propiedades[3] = CheckBox.Height.ToString();
                  }
                  catch
                  {
                     Propiedades[0] = RangoDatos.Top.ToString();
                     Propiedades[1] = RangoDatos.Left.ToString();
                     Propiedades[2] = RangoDatos.Width.ToString();
                     Propiedades[3] = RangoDatos.Height.ToString();
                  }

                  break;
               default:
                  Propiedades[0] = RangoDatos.Top.ToString();
                  Propiedades[1] = RangoDatos.Left.ToString();
                  Propiedades[2] = RangoDatos.Width.ToString();
                  Propiedades[3] = RangoDatos.Height.ToString();
                  break;
            }

            return Propiedades;
         }
         catch
         {
            Propiedades[0] = RangoDatos.Top.ToString();
            Propiedades[1] = RangoDatos.Left.ToString();
            Propiedades[2] = RangoDatos.Width.ToString();
            Propiedades[3] = RangoDatos.Height.ToString();

            return Propiedades;
         }
      }
   }
}
