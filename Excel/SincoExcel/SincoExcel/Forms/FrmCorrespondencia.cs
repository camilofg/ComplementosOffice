using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SincoOfficeLibrerias.wsOfficeSGD;
using System.Xml;

using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using ExcelTools = Microsoft.Office.Tools.Excel;

namespace SincoExcel.Forms
{
    public partial class FrmCorrespondencia : Form
    {
        string TipoCorrespondencia;
        List<ActividadesCorrespondencia> ListaActividades;

        public FrmCorrespondencia()
        {
            InitializeComponent();

            TipoCorrespondencia = string.Empty;
            ListaActividades = new List<ActividadesCorrespondencia>();
        }

        private void FrmCorrespondencia_Load(object sender, EventArgs e)
        {
            string Filtro = " SFVid = " + Globals.ThisAddIn.IdPlantillaFormato;
            DataView FiltroFormato = new DataView(Globals.Ribbons.RibbonExcel.DatosFormatosVigentes, Filtro, "", DataViewRowState.CurrentRows);
            if (FiltroFormato.Count > 0)
            {
                TipoCorrespondencia = FiltroFormato.ToTable().Rows[0]["CTIcodigo"].ToString();

                DgvActividadResponsables.AutoGenerateColumns = false;

                //Cargar grid con responsables pasos predeterminados
                XmlNode xmlCorrespondencia = SGCformatos.ResponsablesPasosCorrespondencia(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.DatosConexion, TipoCorrespondencia);
                if (xmlCorrespondencia.ChildNodes.Count > 0)
                {
                    ListaActividades = new List<ActividadesCorrespondencia>();

                    foreach (XmlNode Paso in xmlCorrespondencia.ChildNodes)
                    {
                        ActividadesCorrespondencia Actividad = new ActividadesCorrespondencia();
                        Actividad.Codigo = int.Parse(Paso.SelectSingleNode("Codigo").InnerText);
                        Actividad.Actividad = Paso.SelectSingleNode("Actividad").InnerText;
                        Actividad.Orden = int.Parse(Paso.SelectSingleNode("Orden").InnerText);
                        Actividad.Cargo = Paso.SelectSingleNode("Cargo").InnerText;
                        Actividad.Responsables = new List<ResponsablesActividad>();

                        foreach (XmlNode Responsable in Paso.SelectSingleNode("Responsables"))
                        {
                            ResponsablesActividad Res = new ResponsablesActividad();
                            Res.UsuarioCodigo = int.Parse(Responsable.SelectSingleNode("UsuarioCodigo").InnerText);
                            Res.UsuarioNombre = Responsable.SelectSingleNode("UsuarioNombre").InnerText;
                            Res.Prioridad = int.Parse(Responsable.SelectSingleNode("Prioridad").InnerText);
                            Actividad.Responsables.Add(Res);
                        }
                        ListaActividades.Add(Actividad);
                    }

                    if (ListaActividades.Count > 0)
                    {
                        DgvActividadResponsables.DataSource = ListaActividades;

                        foreach (DataGridViewRow Fila in DgvActividadResponsables.Rows)
                        {
                            int CodigoActividad = int.Parse(Fila.Cells["Codigo"].Value.ToString());

                            List<ResponsablesActividad> Resp = ListaActividades.FindAll(delegate(ActividadesCorrespondencia a) { return a.Codigo == CodigoActividad; })[0].Responsables;

                            if (Resp.Count > 0)
                            {
                                //Cargar Lista de responsables
                                DataGridViewComboBoxCell das = (DataGridViewComboBoxCell)Fila.Cells["Responsable"];
                                das.DataSource = Resp;

                                das.ValueMember = "UsuarioCodigo";
                                das.DisplayMember = "UsuarioNombre";

                                das.Value = Resp[0].UsuarioCodigo;
                            }
                        }

                        DgvActividadResponsables.Update();
                    }
                }
            }
        }

        private void BtnEnviar_Click(object sender, EventArgs e)
        {
            #region Construir XML pasos Usuarios
            string XmlpasosUsuarios = string.Empty;

            string PathTemp = string.Empty;

            foreach (DataGridViewRow Fila in DgvActividadResponsables.Rows)
            {
                DataGridViewComboBoxCell combo = (DataGridViewComboBoxCell)Fila.Cells["Responsable"];
                PathTemp = PathTemp + string.Format("<Pasousuario idPaso='{0}' Usuario='{1}' />", Fila.Cells["Codigo"].Value.ToString(), combo.Value.ToString());
            }

            XmlpasosUsuarios = string.Format("<Pasos>{0}</Pasos>", PathTemp);

            #endregion

            bool ResultadoGuardarFormatoDiligenciado = MetodosRibbon.GuardarFormatoDiligenciado(XmlpasosUsuarios);

            if (!ResultadoGuardarFormatoDiligenciado)
            {
                DialogResult Res = MessageBox.Show(Properties.Settings.Default.MsVolvarCargarFormato, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Res == DialogResult.Yes)
                {
                    Excel.Workbook LibroActivo = Globals.ThisAddIn.Application.ActiveWorkbook;
                    LibroActivo.Close(false);

                    MetodosRibbon.AbrirFormatoParaCompletar(Globals.ThisAddIn.IdPlantillaFormato);
                }
            }
            else
            {
                this.Dispose(true);
            }
        }

        private void DgvActividadResponsables_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            
        }

        private void DgvActividadResponsables_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
           
        }

        private void DgvActividadResponsables_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }
    }

    public class ActividadesCorrespondencia
    {
        public int Codigo { get; set; }
        public string Actividad { get; set; }
        public int Orden { get; set; }
        public string Cargo { get; set; }
        public List<ResponsablesActividad> Responsables { get; set; }

    }
    public class ResponsablesActividad
    {
        public int UsuarioCodigo { get; set; }
        public string UsuarioNombre { get; set; }
        public int Prioridad { get; set; }
    }
}
