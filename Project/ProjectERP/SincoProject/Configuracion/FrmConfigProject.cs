using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SincoProject.Classes;
using AppSincoProject.WsFestivosRef;
using Microsoft.Office.Interop.MSProject;
using AppSincoProject.Libraries;
using SincoOfficeLibrerias;
using AppExternas;

namespace SincoProject.Configuracion
{
    public partial class FrmConfigProject : Form
    {
        ConexionesProject conexiones = Globals.ThisAddIn.DatosConexion;
        Conexiones conexionWSProject = Globals.ThisAddIn.ConexWSProject;
        Conexiones conexionWSFestivos = Globals.ThisAddIn.ConexWSFestivos;
        List<LoadCombos> Dias = new List<LoadCombos>();

        public FrmConfigProject()
        {
            InitializeComponent();
        }

        private void FrmConfigProject_Load(object sender, EventArgs e)
        {
            var duraci = GanttHelper.getDuraciones();
            CBDuracion.DisplayMember = "Mostrar";
            CBDuracion.ValueMember = "EscribirDB";
            CBDuracion.DataSource = duraci;
            CBDuracion.SelectedValue = "Day";

            var duracion = GanttHelper.getDuraciones();
            CBTrabajo.DisplayMember= "Mostrar";
            CBTrabajo.ValueMember = "EscribirDB";
            CBTrabajo.DataSource = duracion;
            CBTrabajo.SelectedValue = "Hour";

            NumHorasxDia.Value = 8;
            NumHorasxSemana.Value = 40;
            numHorasInicio.Value = 7;
            numHorasFin.Value = 6;

            var merid = GanttHelper.getMeridian();
            CBHoraInicio.DisplayMember = "Mostrar";
            CBHoraInicio.ValueMember = "EscribirDB";
            CBHoraInicio.DataSource = merid;

            var merid2 = GanttHelper.getMeridian();
            CBHoraFin.DisplayMember = "Mostrar";
            CBHoraFin.ValueMember = "EscribirDB";
            CBHoraFin.DataSource = merid2;
            CBHoraFin.SelectedValue = "PM";
       

         if (Globals.ThisAddIn.configProj.Count() != 0)
            {
                var configuracion = Globals.ThisAddIn.configProj;
                TxtName.Text = configuracion[0];
                CBDuracion.SelectedValue = configuracion[2];
                CBTrabajo.SelectedValue = configuracion[3];

                NumHorasxDia.Value = Convert.ToDecimal(configuracion[5]);
                NumHorasxSemana.Value = Convert.ToDecimal(configuracion[6]);

                var horaIni = configuracion[7].Split(':');
                numMinsInicio.Value = Convert.ToInt32(horaIni[1]);
                if (Convert.ToInt32(horaIni[0]) > 12)
                {
                    CBHoraInicio.SelectedValue = "PM";
                    numHorasInicio.Value = Convert.ToInt32(horaIni[0]) - 12;
                }

                else
                {
                    CBHoraInicio.SelectedValue = "AM";
                    numHorasInicio.Value = Convert.ToInt32(horaIni[0]);
                }

                var horaFin = configuracion[8].Split(':');
                numMinsFin.Value = Convert.ToInt32(horaFin[1]);
                if (Convert.ToInt32(horaFin[0]) > 12)
                {
                    CBHoraFin.SelectedValue = "PM";
                    numHorasFin.Value = Convert.ToInt32(horaFin[0]) - 12;
                }

                else
                {
                    CBHoraFin.SelectedValue = "AM";
                    numHorasFin.Value = Convert.ToInt32(horaFin[0]);
                }

                return;

            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            ClienteWSProject wsClient = new ClienteWSProject(conexionWSProject);
            ClienteWSFestivo wsFestClient = new ClienteWSFestivo(conexionWSFestivos);
            //GanttBuilder gb = new GanttBuilder();
            string idProg = "-1";
            if (Globals.ThisAddIn.configProj.Count != 0) 
               idProg = Globals.ThisAddIn.configProj[1].ToString();
            
            string SelectedModule = Globals.ThisAddIn.selectedModule.ToString();
            string projectName = TxtName.Text;

            var listas= Globals.ThisAddIn.ListaProjectosGlobal;
            var nomb = from T in listas where T.Descripcion == projectName select T;

            if (nomb.Count() != 0 && idProg == "-1")
            {
                MessageBox.Show("Este nombre de proyecto ya existe. Por favor cambielo");
                return;
            }

            Microsoft.Office.Interop.MSProject.Project app = Globals.ThisAddIn.Application.ActiveProject;
                var calendar = app.Calendar;

                if (ChkFestivosCol.Checked == true)
                {
                    List<Festivo> listaFiestas = wsFestClient.Calculate(Convert.ToInt32(numFestIni.Value), Convert.ToInt32(numFestFin.Value)).ToList();
                    //List<Festivo> listaFiestas = gb.FestivosColombia(Convert.ToInt32(numFestIni.Value), Convert.ToInt32(numFestFin.Value));

                    var currentExceptions = calendar.Exceptions;

                    if (currentExceptions.Count == 0)
                    {
                        int cuentaCoincidencias = 0;
                        for (int i = 1; i <= currentExceptions.Count; i++)
                        {
                            var coincidenciasExc = from T in listaFiestas where T.Descripcion == currentExceptions[i].Name select T;
                            cuentaCoincidencias = coincidenciasExc.Count();
                            if (cuentaCoincidencias > 0)
                                break;
                        }
                        if (cuentaCoincidencias == 0)
                        {
                            foreach (Festivo fiesta in listaFiestas)
                            {
                                currentExceptions.Add(PjExceptionType.pjDayCount, Start: fiesta.Fecha, Finish: fiesta.Fecha, Name: fiesta.Descripcion);
                                //calendar.Exceptions.Add(PjExceptionType.pjDayCount, Start: fiesta.Fecha, Finish: fiesta.Fecha, Name: fiesta.Descripcion);
                            }
                        }

                    }
                }

                else
                {
                    var rta = MessageBox.Show("Si se crearon excepciones manualmente estas tambien seran eliminadas", "Confirm delete", MessageBoxButtons.OKCancel).ToString();
                    if (rta == "OK")
                    {
                        calendar.Reset();
                        ExceptionHelper.setWeekExcepts(Globals.ThisAddIn.nodoPapa.WeekExceptions.ToList());
                    }

                    else
                        ChkFestivosCol.Checked = true;
                }

            string mins = string.Empty;
            
            int horas = 0;
            if (numMinsInicio.Value.ToString() == "0")
                mins = "00";

            else
                mins = "30";

            if (CBHoraInicio.SelectedValue.ToString() == "PM")
            {
                horas = Convert.ToInt32(numHorasInicio.Value) + 12;
            }

            else
            {
                horas = Convert.ToInt32(numHorasInicio.Value);
            }

            string horaInicio = horas.ToString() + ':' + mins;


            string mins2 = string.Empty;
            
            int horas2 = 0;
            if (numMinsFin.Value.ToString() == "0")
                mins2 = "00";

            else
                mins2 = "30";

            if (CBHoraFin.SelectedValue.ToString() == "PM")
            {
                horas2 = Convert.ToInt32(numHorasInicio.Value) + 12;
            }

            else
            {
                horas2 = Convert.ToInt32(numHorasInicio.Value);
            }

            string horaFin = horas2.ToString() + ':' + mins;


            var tales = wsClient.SaveConfigs("IU", idProg, projectName, SelectedModule, "", 1, CBTrabajo.SelectedValue.ToString(), CBDuracion.SelectedValue.ToString(), NumHorasxDia.Value.ToString(),
                                   NumHorasxSemana.Value.ToString(), horaInicio, horaFin, 0, "50", "lunes");
            //var tales = gb.SaveConf("IU", idProg, projectName, SelectedModule, "", 1, CBTrabajo.SelectedValue.ToString(), CBDuracion.SelectedValue.ToString(), NumHorasxDia.Value.ToString(),
            //                        NumHorasxSemana.Value.ToString(), horaInicio, horaFin, 0, "50", "lunes");
            Globals.ThisAddIn.globalIdProject = Convert.ToInt32(tales[2]);
            MessageBox.Show(tales[0].ToString());
            this.Hide();
        }

        private void BtnConfigWeek_Click(object sender, EventArgs e)
        {
            if (Globals.ThisAddIn.globalIdProject == 0)
            {
                MessageBox.Show("Debe primero realizar la configuracion basica, y guardar los cambios");
                return;
            }

            this.Hide();
            FrmConfigWeekExcepts frmConfigWeek = new FrmConfigWeekExcepts();
            frmConfigWeek.Show();
        }
    }
}
