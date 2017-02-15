using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppSincoProject.WsProjectERPref;
using SincoProject.Classes;
using AppSincoProject.Libraries;
using SincoOfficeLibrerias;
using AppExternas;

namespace SincoProject.Configuracion
{
    public partial class FrmConfigWeekExcepts : Form
    {
        ConexionesProject conexiones = Globals.ThisAddIn.DatosConexion;
        Conexiones conexionWSProject = Globals.ThisAddIn.ConexWSProject;
        Conexiones conexionWSFestivos = Globals.ThisAddIn.ConexWSFestivos;
        
        public FrmConfigWeekExcepts()
        {
            InitializeComponent();
        }

        private void BtnSaveExcept_Click(object sender, EventArgs e)
        {
            var tales = this.Controls;
            var listaChks = tales.OfType<CheckBox>();
            //GanttBuilder gb = new GanttBuilder();
            ClienteWSProject wsClient = new ClienteWSProject(conexionWSProject);
            List<ConfigDays> listWeekExcepts = new List<ConfigDays>();

            var unchekeaos = from T in listaChks where T.Checked == false select T;

            foreach (CheckBox chk in unchekeaos)
            {

                string nombre = chk.Text;
                nombre = nombre.ToLower();

                var horaM = tales.Find("Num" + nombre + "HM", true);
                string horaMan = horaM[0].Text;

                var DurM = tales.Find("Num" + nombre + "DM", true);
                var DurMan = DurM[0].Text;

                if (horaMan != "0" && DurMan != "0")
                {
                    ConfigDays weekDay = new ConfigDays();
                    weekDay.ProjectId = Globals.ThisAddIn.globalIdProject;
                    weekDay.DayName = nombre;
                    weekDay.AfternoonDuration = "";
                    weekDay.AfternoonStart = "";
                    weekDay.MorningStart = "";
                    weekDay.MorningDuration = "";
                    weekDay.ConfigOperation = "D";
                    listWeekExcepts.Add(weekDay);

                    var Num = tales.Find("Num" + nombre + "HM", true);
                    var NumSalida = Num.OfType<NumericUpDown>().First();
                    NumSalida.Value = 0;

                    Num = tales.Find("Num" + nombre + "MM", true);
                    NumSalida = Num.OfType<NumericUpDown>().First();
                    NumSalida.Value = 0;

                    Num = tales.Find("Num" + nombre + "DM", true);
                    NumSalida = Num.OfType<NumericUpDown>().First();
                    NumSalida.Value = 0;

                    Num = tales.Find("Num" + nombre + "HT", true);
                    NumSalida = Num.OfType<NumericUpDown>().First();
                    NumSalida.Value = 0;

                    Num = tales.Find("Num" + nombre + "MT", true);
                    NumSalida = Num.OfType<NumericUpDown>().First();
                    NumSalida.Value = 0;

                    Num = tales.Find("Num" + nombre + "DM", true);
                    NumSalida = Num.OfType<NumericUpDown>().First();
                    NumSalida.Value = 0;
                }

            }

            var chekeaos = from T in listaChks where T.Checked == true select T;

            foreach (CheckBox chk in chekeaos) 
            {
                ConfigDays weekDay = new ConfigDays();
                string nombre = chk.Text;
                nombre = nombre.ToLower();

                weekDay.ProjectId = Globals.ThisAddIn.globalIdProject;

                weekDay.DayName = nombre;
                var horaM = tales.Find("Num" + nombre + "HM", true);
                string horaMan = horaM[0].Text;

                var minsM = tales.Find("Num" + nombre + "MM", true);
                string minsMan = minsM[0].Text;

                if(minsMan == "0")
                    minsMan = ":00";

                else
                    minsMan = ":30";

                var DurM = tales.Find("Num" + nombre + "DM", true);
                int DurMan = Convert.ToInt32(DurM[0].Text);
                string finMan = (Convert.ToInt32(horaMan) + DurMan).ToString();
                string Shift1Start = horaMan + minsMan;
                string shift1Finish = finMan + minsMan;
                weekDay.MorningStart = Shift1Start;
                weekDay.MorningDuration = DurMan.ToString();
                
                var horaT = tales.Find("Num" + nombre + "HT", true);
                string horaTar = horaT[0].Text;

                if (horaTar != "0")
                {
                    int horaTarde = Convert.ToInt32(horaTar) + 12;
                    horaTar = horaTarde.ToString();
                    var minsT = tales.Find("Num" + nombre + "MT", true);
                    string minsTar = minsT[0].Text;

                    if (minsTar == "0")
                        minsTar = ":00";

                    else
                        minsTar = ":30";

                    var DurT = tales.Find("Num" + nombre + "DT", true);
                    int DurTar = Convert.ToInt32(DurT[0].Text);
                    string duracionTarde = DurTar.ToString();
                    string Shift2Start = horaTar + minsTar;

                    weekDay.AfternoonStart = Shift2Start;
                    weekDay.AfternoonDuration = duracionTarde;
                    weekDay.ConfigOperation = "";
                    listWeekExcepts.Add(weekDay);
                }

                else 
                {
                    weekDay.AfternoonStart = "";
                    weekDay.AfternoonDuration = "";
                    weekDay.ConfigOperation = "";
                    listWeekExcepts.Add(weekDay);
                }
            }

            foreach (ConfigDays dia in listWeekExcepts)
            {
                wsClient.SaveWeekExceptions(dia);
                //gb.SaveWeekExcepts(dia);
            }
            var setWeek = from T in listWeekExcepts where T.ConfigOperation != "D" select T;

            ExceptionHelper.setWeekExcepts(setWeek.ToList());
            this.Hide();
        }

        private void FrmConfigWeekExcepts_Load(object sender, EventArgs e)
        {
            if (Globals.ThisAddIn.nodoPapa.WeekExceptions.Count() != 0) 
            {
                var controles = this.Controls;
                var weekExcepts = Globals.ThisAddIn.nodoPapa.WeekExceptions;

                foreach (ConfigDays dia in weekExcepts)
                {
                    string nombreDia = dia.DayName.ToLower();

                    var listaChks2 = controles.OfType<CheckBox>().ToList();

                    for (int i = 0; i < listaChks2.Count(); i++)
                    {
                        if (listaChks2[i].Text.ToLower() == nombreDia)
                        {
                            var trin = controles.Find("Chk" + nombreDia, true);

                            var checkControl = trin.OfType<CheckBox>().First();
                            checkControl.Checked = true;

                            var Num = controles.Find("Num" + nombreDia + "HM", true);
                            var NumSalida = Num.OfType<NumericUpDown>().First();
                            var manana = dia.MorningStart.Split(':');
                            NumSalida.Value = Convert.ToDecimal(manana[0]);

                            Num = controles.Find("Num" + nombreDia + "MM", true);
                            NumSalida = Num.OfType<NumericUpDown>().First();
                            NumSalida.Value = Convert.ToDecimal(manana[1]);

                            Num = controles.Find("Num" + nombreDia + "DM", true);
                            NumSalida = Num.OfType<NumericUpDown>().First();
                            NumSalida.Value = Convert.ToDecimal(Convert.ToDecimal(dia.MorningDuration));

                            if (dia.AfternoonStart != "")
                            {
                                Num = controles.Find("Num" + nombreDia + "HT", true);
                                NumSalida = Num.OfType<NumericUpDown>().First();
                                var tarde = dia.AfternoonStart.Split(':');
                                NumSalida.Value = Convert.ToDecimal(tarde[0]) - 12;

                                Num = controles.Find("Num" + nombreDia + "MT", true);
                                NumSalida = Num.OfType<NumericUpDown>().First();
                                NumSalida.Value = Convert.ToDecimal(tarde[1]);

                                Num = controles.Find("Num" + nombreDia + "DT", true);
                                NumSalida = Num.OfType<NumericUpDown>().First();
                                NumSalida.Value = Convert.ToDecimal(Convert.ToDecimal(dia.AfternoonDuration));
                            }
                        }
                    }
                }
                ExceptionHelper.setWeekExcepts(Globals.ThisAddIn.nodoPapa.WeekExceptions.ToList());
            }
        }
    }
}
