using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SincoProject.Classes;
using AppExternas;
using SincoProject.Configuracion;
using SincoOfficeLibrerias;
using AppSincoProject.Libraries;
using System.Web.Services;

namespace SincoProject
{
    public partial class FrmSelectProject : Form
    {
        ConexionesProject conexiones = Globals.ThisAddIn.DatosConexion;
        Conexiones conexionWSProject = Globals.ThisAddIn.ConexWSProject;
        Conexiones conexionWSFestivos = Globals.ThisAddIn.ConexWSFestivos;

        public FrmSelectProject()
        {
            InitializeComponent();
        }

        private void FrmSelect_Load(object sender, EventArgs e)
        {
            if (conexionWSProject == null)
            {
                MessageBox.Show("No tiene Conexion con Sinco Comunicaciones");
                this.Close();
                return;
            }

            ClienteWSProject wsClient = new ClienteWSProject(conexionWSProject);
            var listaModulos = wsClient.GetModules();

            List<LoadCombos> lista = new List<LoadCombos>();

            LoadCombos FirstMod = new LoadCombos();
            FirstMod.Identificador = -1;
            FirstMod.Descripcion = "----Seleccione----";
            lista.Add(FirstMod);
//
            for (int i = 0; i < listaModulos.Count(); i++)
            {
                LoadCombos LoadCB = new LoadCombos();
                LoadCB.Identificador = (int)listaModulos[i].Id;
                LoadCB.Descripcion = listaModulos[i].Descripcion;
                lista.Add(LoadCB);
            }
            ComboModulo.ValueMember = "Identificador";
            ComboModulo.DisplayMember = "Descripcion";
            ComboModulo.DataSource = lista;
            Globals.ThisAddIn.ListaModulosGlobal = lista;


            if (Globals.ThisAddIn.globalIdProject != 0 && Globals.ThisAddIn.selectedModule != 0) 
            {
                ComboModulo.SelectedValue = Globals.ThisAddIn.selectedModule;
                ComboProgramacion.SelectedValue = Globals.ThisAddIn.globalIdProject;
            }
        }

        private void SelectedIndexChange_ComboModule(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ComboModulo.SelectedValue) == -1)
                return;

            if (conexionWSProject == null)
            {
                MessageBox.Show("No tiene Conexion con Sinco Comunicaciones");
                this.Close();
                return;
            }

            ClienteWSProject WsClient = new ClienteWSProject(conexionWSProject);
            var listaProyectos = WsClient.GetProjects(Globals.ThisAddIn.DatosUsuario.IdUsuario, ComboModulo.SelectedValue.ToString());
            
            List<LoadCombos> lista = new List<LoadCombos>();

            LoadCombos FirstProy = new LoadCombos();
            FirstProy.Identificador = -1;
            FirstProy.Descripcion = "----Seleccione----";
            lista.Add(FirstProy);

            for (int i = 0; i < listaProyectos.Count(); i++)
            {
                LoadCombos LoadCB = new LoadCombos();
                LoadCB.Identificador = listaProyectos[i].Id;
                LoadCB.Descripcion = listaProyectos[i].Descripcion;
                lista.Add(LoadCB);
            }
            ComboProgramacion.ValueMember = "Identificador";
            ComboProgramacion.DisplayMember = "Descripcion";
            ComboProgramacion.DataSource = lista;
            Globals.ThisAddIn.ListaProjectosGlobal = lista;
            Globals.ThisAddIn.selectedModule = Convert.ToInt32(ComboModulo.SelectedValue);

        }


        private void BtnCargar_Click(object sender, EventArgs e)
        {
            if (Globals.ThisAddIn.nodoPapa != null)
            {
                MessageBox.Show("Existe un proyecto en sesion, debe volver a iniciar proyecta para cargar otro proyecto");
                return;
            }

            if (Convert.ToInt32(ComboModulo.SelectedValue) == -1 || Convert.ToInt32(ComboProgramacion.SelectedValue) == -1)
                return;

            //GanttBuilder gb = new GanttBuilder();
            //var upperNode = gb.GetProject(ComboProgramacion.SelectedValue.ToString());

            ClienteWSProject wsClient = new ClienteWSProject(conexionWSProject);
            var upperNode = wsClient.LoadProject(ComboProgramacion.SelectedValue.ToString());

            if (Globals.ThisAddIn.Application.ActiveProject.Tasks.Count == 0)
                GanttHelper.BuildProject(upperNode);

            Globals.ThisAddIn.nodoPapa = upperNode;

            //if (Globals.ThisAddIn.Application.ActiveProject.Calendar.Exceptions.Count == 0)
                ExceptionHelper.setWeekExcepts(Globals.ThisAddIn.nodoPapa.WeekExceptions.ToList());
                //GanttHelper.setWeekExcepts(Globals.ThisAddIn.nodoPapa.WeekExceptions.ToList());

                Globals.Ribbons.ProjectRibbon.LblProgramacion.Label = "Modulo: " + ComboModulo.Text;
                Globals.Ribbons.ProjectRibbon.LblProgramacion.Visible = true;
                Globals.Ribbons.ProjectRibbon.LblNameProg.Label = "Programacion: " + ComboProgramacion.Text;
                Globals.Ribbons.ProjectRibbon.LblNameProg.Visible = true;

            this.Hide();
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            Globals.ThisAddIn.configProj = new List<string>();

            if (Convert.ToInt32(ComboModulo.SelectedValue) == -1)
            {
                MessageBox.Show("Debe seleccionar un modulo");
                return;
            }

            if (Convert.ToInt32(ComboProgramacion.SelectedValue) != -1) 
            {
                MessageBox.Show("La programacion seleccionada ya existe, si desea modificarla utilize el boton de Configuración");
                return;
            }

            FrmConfigProject frmConfig = new FrmConfigProject();
            frmConfig.Show();
            Globals.Ribbons.ProjectRibbon.LblProgramacion.Label = "Modulo: " + ComboModulo.Text;
            Globals.Ribbons.ProjectRibbon.LblProgramacion.Visible = true;
            Globals.Ribbons.ProjectRibbon.LblNameProg.Label = "Programacion: " + ComboProgramacion.Text;
            Globals.Ribbons.ProjectRibbon.LblNameProg.Visible = true;
            this.Hide();
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ComboProgramacion.SelectedValue) == -1)
            {
                MessageBox.Show("Debe seleccionar un proyecto para poder editarlo");
                return;
            }

            ClienteWSProject wsClient = new ClienteWSProject(conexionWSProject);
            var upperNode = wsClient.LoadProject(ComboProgramacion.SelectedValue.ToString());

            //GanttBuilder gb = new GanttBuilder();
            //var upperNode = gb.GetProject(ComboProgramacion.SelectedValue.ToString());

            if (Globals.ThisAddIn.Application.ActiveProject.Tasks.Count == 0)
                GanttHelper.BuildProject(upperNode);

            Globals.ThisAddIn.nodoPapa = upperNode;
            this.Hide();
            var configurations = GanttHelper.getConfigs(upperNode);
            Globals.ThisAddIn.configProj = configurations;
            Globals.ThisAddIn.globalIdProject = Convert.ToInt32(configurations[1]);

            //if (Globals.ThisAddIn.Application.ActiveProject.Calendar.Exceptions.Count == 0)
            ExceptionHelper.setWeekExcepts(Globals.ThisAddIn.nodoPapa.WeekExceptions.ToList());
            //GanttHelper.setWeekExcepts(Globals.ThisAddIn.nodoPapa.WeekExceptions.ToList());
                
            FrmConfigProject frmConfig = new FrmConfigProject();
            frmConfig.Show();
            Globals.Ribbons.ProjectRibbon.LblProgramacion.Label = "Modulo: " + ComboModulo.Text;
            Globals.Ribbons.ProjectRibbon.LblProgramacion.Visible = true;
            Globals.Ribbons.ProjectRibbon.LblNameProg.Label = "Programacion: " + ComboProgramacion.Text;
            Globals.Ribbons.ProjectRibbon.LblNameProg.Visible = true;
            this.Hide();
        }
    }
}
