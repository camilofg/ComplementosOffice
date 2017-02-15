using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SincoOfficeLibrerias;
using AppExternas;
using AppSincoWord.Librerias;
using SincoWord.Classes;
using AppSincoWord.WsArbolVariablesRef;

namespace SincoWord.Configuracion
{
    public partial class FrmSelectSP : Form
    {
        ConexionesWord conexiones = Globals.ThisAddIn.DatosConexion;
        Conexiones conexionWsTree = Globals.ThisAddIn.ConexionTree;
        public FrmSelectSP()
        {
            InitializeComponent();
        }

        private void FrmSelectSP_Load(object sender, EventArgs e)
        {
            

            if (conexionWsTree == null)
            {
                MessageBox.Show("No tiene conexion con Sinco Comunicaciones");
                this.Close();
                return;
            }

            ClienteWSTree wsClient = new ClienteWSTree(conexionWsTree);
            List<LoadComboBox> listModules = new List<LoadComboBox>();
            LoadComboBox firstModule = new LoadComboBox();
            firstModule.Id = -1;
            firstModule.Descripcion = "----Seleccione----";
            listModules.Add(firstModule);

            var listaMod = wsClient.GetModules();

            for (int i = 0; i < listaMod.Count(); i++)
            {
                LoadComboBox LoadCB = new LoadComboBox();
                LoadCB.Id = (int)listaMod[i].Id;
                LoadCB.Descripcion = listaMod[i].Descripcion;
                listModules.Add(LoadCB);
            }


            CBModulo.ValueMember = "Id";
            CBModulo.DisplayMember = "Descripcion";
            CBModulo.DataSource = listModules;
            //Globals.ThisAddIn.ListaModulosGlobal = lista;


            //if (Globals.ThisAddIn.globalIdProject != 0 && Globals.ThisAddIn.selectedModule != 0)
            //{
            //    ComboModulo.SelectedValue = Globals.ThisAddIn.selectedModule;
            //    ComboProgramacion.SelectedValue = Globals.ThisAddIn.globalIdProject;
            //}

        }

        private void CBModulo_SelectedValueChanged(object sender, EventArgs e)
        {
            int tal = Convert.ToInt32(CBModulo.SelectedValue);
            if (tal == -1 || tal == 0)
                return;

            ClienteWSTree wsClient = new ClienteWSTree(conexionWsTree);
            List<LoadComboBox> listModules = new List<LoadComboBox>();
            LoadComboBox firstModule = new LoadComboBox();
            firstModule.Id = -1;
            firstModule.Descripcion = "----Seleccione----";
            listModules.Add(firstModule);

            var listaMod = wsClient.GetStoredProcedures(CBModulo.SelectedValue.ToString());

            for (int i = 0; i < listaMod.Count(); i++)
            {
                LoadComboBox LoadCB = new LoadComboBox();
                LoadCB.AuxId = listaMod[i].AuxId;
                LoadCB.Descripcion = listaMod[i].Descripcion;
                listModules.Add(LoadCB);
            }

            CBAplicacion.ValueMember = "AuxId";
            CBAplicacion.DisplayMember = "Descripcion";
            CBAplicacion.DataSource = listModules;

        }

        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            FrmTreeView FrmTree = new FrmTreeView();
            FrmTree.Text = "Arbol de Variables";
            FrmTree.TopMost = true;
            FrmTree.Show();

            this.Close();
        }

        private void CBAplicacion_SelectedValueChanged(object sender, EventArgs e)
        {
            //var tal = Convert.ToInt32(CBAplicacion.SelectedValue);
            if (CBAplicacion.SelectedValue != null)
            {
                var tal = CBAplicacion.SelectedValue.ToString();
                if (tal == "-1" || tal == "0")
                    return;

                Globals.ThisAddIn.TreeviewStoreProc = CBAplicacion.SelectedValue.ToString();
            }
        }
    }
}
