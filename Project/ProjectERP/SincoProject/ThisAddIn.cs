using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MSProject = Microsoft.Office.Interop.MSProject;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Ribbon;
using AppExternas;
using System.Windows.Forms;
using Microsoft.Office.Interop.MSProject;
using SincoProject.Classes;
using AppSincoProject.WsProjectERPref;
using SincoOfficeLibrerias;

namespace SincoProject
{
    public partial class ThisAddIn
    {

        public string MensajeTitulos = "Sinco ERP";
        public string MensajeError = "Ocurrió un evento no controlado en la aplicación.\n\nCódigo del evento:\n{0}\n\nConserve el código para reportar el evento posteriormente.";
        public string MensajeErrorNoReportado = "Ocurrió un evento no controlado en la aplicación y no fue notificado, si el evento persiste por favor consulte con el administrador del sistema.";
        
        public Login DatosUsuario;
        public Conexiones Conexion;
        public ConexionesProject DatosConexion;
        public Conexiones ConexWSProject;
        public Conexiones ConexWSFestivos;
        public List<LoadCombos> ListaModulosGlobal;
        public List<LoadCombos> ListaProjectosGlobal;
        public int selectedModule;
        public int globalIdProject;
        public List<string> configProj;
        public UpperGanttNode nodoPapa;

        public Byte[] newKeyFile = { 17, 29, 23, 41, 52, 26, 31, 84, 63, 63, 95, 12, 10, 14, 15, 12, 64, 99, 38, 88, 99, 12, 3, 1 };
        public Byte[] newIVFile = { 75, 22, 255, 110, 65, 201, 209, 154 };


        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Conexion = new Conexiones("http://desarrollo/sincook/ERPNet/Comunicaciones/ServiciosWeb/WsAppExternas.svc");
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            //          Globals.ThisAddIn.Application.ActiveWindow.
        }

        #region Código generado por VSTO

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        //void Application_NewProject(MSProject.Project pj)
        //{
        //    MSProject.Task newTask = pj.Tasks.Add("This text was added by using code", missing);
        //    newTask.Start = DateTime.Now;
        //    newTask.Duration = "3";
        //    newTask.ResourceNames = "Rob Caron, Kelly Krout";
        //}

        //void Application_ProjectBeforeTaskNew(Project pj, ref bool Cancel)
        //{
        //    MessageBox.Show("cambio una tarea");
        //}



        #endregion
    }

}
