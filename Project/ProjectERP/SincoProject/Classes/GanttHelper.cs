using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AppSincoProject.WsProjectERPref;
using Microsoft.Office.Interop.MSProject;
using AppSincoProject.WsFestivosRef;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using SincoOfficeLibrerias;
using AppExternas;
using AppSincoProject.Libraries;

namespace SincoProject.Classes
{
    public static class GanttHelper
    {
        public static void BuildProject(UpperGanttNode SuperNodo)
        {
            Globals.ThisAddIn.globalIdProject = SuperNodo.IdProject;
            if (SuperNodo.LowerGanttNodes.Count() == 0)
                return;

            
            Microsoft.Office.Interop.MSProject.Project app = Globals.ThisAddIn.Application.ActiveProject;
                var calendar = Globals.ThisAddIn.Application.ActiveProject.Calendar;
                calendar.Reset();

                    for (int i = 0; i < SuperNodo.Exceptions.Count(); i++)
                    {
                        calendar.Exceptions.Add(PjExceptionType.pjDayCount, Start: SuperNodo.Exceptions[i].ExceptionStart, Finish: SuperNodo.Exceptions[i].ExceptionFinish, Name: SuperNodo.Exceptions[i].ExceptionName);
                    }

            app.Name = SuperNodo.DescProject;

            if (SuperNodo.DurationUnits.ToString() == "Day")
                app.DefaultDurationUnits = PjUnit.pjDay;

            if (SuperNodo.DurationUnits.ToString() == "Week")
                app.DefaultDurationUnits = PjUnit.pjWeek;

            if (SuperNodo.DurationUnits.ToString() == "Month")
                app.DefaultDurationUnits = PjUnit.pjMonthUnit;

            if (SuperNodo.WorkUnits.ToString() == "Hour")
                app.DefaultWorkUnits = PjUnit.pjHour;

            if (SuperNodo.WorkUnits.ToString() == "Day")
                app.DefaultWorkUnits = PjUnit.pjDay;

            if (SuperNodo.WorkUnits.ToString() == "Week")
                app.DefaultWorkUnits = PjUnit.pjWeek;

            if (SuperNodo.WorkUnits.ToString() == "Month")
                app.DefaultWorkUnits = PjUnit.pjMonthUnit;

            app.HoursPerDay = SuperNodo.HoursPerDay;
            app.HoursPerWeek = SuperNodo.HoursPerWeek;
            app.DefaultStartTime = SuperNodo.ScheduledStart;
            app.DefaultFinishTime = SuperNodo.ScheduledFinish;

            app.StartWeekOn = PjWeekday.pjMonday;

            for (int i = 0; i < SuperNodo.LowerGanttNodes.Count(); i++)
            {
                Microsoft.Office.Interop.MSProject.Task task = app.Tasks.Add(SuperNodo.LowerGanttNodes[i].ItemName, System.Type.Missing);
                task.Start = SuperNodo.LowerGanttNodes[i].BeginDate;
                task.Finish = SuperNodo.LowerGanttNodes[i].EndDate;
                task.Text1 = SuperNodo.LowerGanttNodes[i].Asignaciones;
            }

            for (int i = 0; i < SuperNodo.LowerGanttNodes.Count(); i++)
            {
                if (SuperNodo.LowerGanttNodes[i].ParentGanttNode != "0") 
                {
                    Microsoft.Office.Interop.MSProject.Task tarea = Globals.ThisAddIn.Application.ActiveProject.Tasks[i + 1];
                    int idPapa = Convert.ToInt32(SuperNodo.LowerGanttNodes[i].ParentGanttNode);
                    Microsoft.Office.Interop.MSProject.Task tareaPadre = Globals.ThisAddIn.Application.ActiveProject.Tasks[idPapa];
                    int nivel = tareaPadre.OutlineLevel;
                            for (int j = 0; j < nivel; j++)
                            {
                                tarea.OutlineIndent();
                            }
                }
             }

            for (int i = 0; i < SuperNodo.LowerGanttNodes.Count(); i++)
            {
                if (SuperNodo.LowerGanttNodes[i].PrecendentGanttNode != null)
                {
                    Microsoft.Office.Interop.MSProject.Task task = Globals.ThisAddIn.Application.ActiveProject.Tasks[i + 1];
                    string predecesora = SuperNodo.LowerGanttNodes[i].PrecendentGanttNode;

                    predecesora = predecesora.Replace("i", "í");
                    predecesora = predecesora.Replace(";", ",");

                    task.Predecessors = predecesora;
                }
            }
        }



        public static List<string> GetGanttNodes()
        {
            ConexionesProject conexiones = Globals.ThisAddIn.DatosConexion;
            Conexiones conexionWSProject = Globals.ThisAddIn.ConexWSProject;
            Conexiones conexionWSFestivos = Globals.ThisAddIn.ConexWSFestivos;

            List<string> listaRes = new List<string>();
            ClienteWSProject wsClient = new ClienteWSProject(conexionWSProject);
            //esta validacion solo se debe dar cuando se haya iniciado sesion con sincoerp si no el mensaje no debe aparecer
            if (Globals.ThisAddIn.globalIdProject == 0)
            {
                MessageBox.Show("No se puede guardar en base de datos pues no tiene seleccionada una configuracion basica");
                return listaRes;
            }

            Microsoft.Office.Interop.MSProject.Project app = Globals.ThisAddIn.Application.ActiveProject;

            XElement NodoPapa = new XElement("Tasks");
            XElement nodoExcepciones = ExceptionHelper.ArmExceptionsNode();

            var nList = app.Tasks;
            for (int i = 1; i <= nList.Count; i++)
            {
                string predecent = string.Empty;
                var texto1 = nList[i].Text1;

                if (nList[i].Predecessors.Count() != 0)
                {
                    string buscar = string.Empty;
                    predecent = nList[i].Predecessors.ToString().Replace(" ", "_");
                    predecent = predecent.Replace("í", "i");
                }

                string pap = string.Empty;

                if (nList[i].OutlineParent.ID != 0)
                    pap = nList[i].OutlineParent.ID.ToString();

                else
                    pap = "0";


                string inicio = nList[i].Start.ToString().Replace(" ", "_");
                string fin = nList[i].Finish.ToString().Replace(" ", "_");

                string nameTask = nList[i].Name.Replace("'", "");
                nameTask = RemoveAccentsWithRegEx(nameTask);
                nameTask = nameTask.Replace(" ", "¿");

                XElement Tasks = new XElement("Task", new XElement("ID", nList[i].ID), new XElement("Name", nameTask),
                                new XElement("BeginDate", inicio), new XElement("EndDate", fin),
                                new XElement("Predesessor", predecent), new XElement("ParentTask", pap),//new XElement("ParentTask", nList[i].OutlineParent.ID.ToString()), 
                                new XElement("PercentAdvanced", Convert.ToDecimal(nList[i].PercentComplete)), //new XElement("Duration", nList[i].Duration.ToString()),
                                new XElement("TaskNotes", nList[i].Notes), new XElement("ConfigSincoERP", nList[i].Text1));

                NodoPapa.Add(Tasks);
            }

            XElement ProjectId = new XElement("IdProject", Globals.ThisAddIn.globalIdProject.ToString());

            //string resultado = NodoPapa.ToString();
            string xmlHead = "<?xml_version=" + '"' + "1.0" + '"' + "_encoding=" + '"' + "iso-8859-1" + '"' + "?>";
            //iso-8859-1
            //resultado = xmlHead + ProjectId + nodoExcepciones + resultado;
            //NOTA: si se va a enviar un xml hacer el stringformat
            //resultado = "<?xml version=" + '"' + "1.0" + '"' + "encoding=" + '"' + "UTF-8" + '"' + "standalone=" + '"' + "yes" + '"' + "?>" + ProjectId + nodoExcepciones + resultado;
            XElement nodoAbuelo = new XElement("NodoPapa");
            nodoAbuelo.Add(ProjectId);
            nodoAbuelo.Add(nodoExcepciones);
            nodoAbuelo.Add(NodoPapa);

            string resultado = xmlHead + nodoAbuelo.ToString();

            resultado = resultado.Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty).Replace("_", " ").Replace("¿", "_");//.Replace("ñ", "n").Replace("í","i");//.Replace("> <","><").Replace(">  <","><").Replace(">   <","><").Replace(">    <","><").Replace(">     <","><").Replace(">      <","><").Replace(">       <","><");//.Replace(" ", string.Empty);
            //string dePRUEBA = "<?xml version="+'"'+"1.0"+'"' + " encoding="+'"'+"UTF-8"+'"'+"?><NodoPapa><IdProject>180</IdProject><Exceptions><Exception><ExcepName>Fiesta</ExcepName><StartException>04/01/2012</StartException><FinishException>04/01/2012</FinishException></Exception></Exceptions><Tasks><Task><ID>1</ID><Name>PRELIMINRES Y CIMENTACION</Name><BeginDate>16/05/2011 09:00:00 a.m.</BeginDate><EndDate>19/08/2011 07:00:00 p.m.</EndDate><Predesessor></Predesessor><ParentTask>0</ParentTask><PercentAdvanced>0</PercentAdvanced><TaskNotes></TaskNotes><ConfigSincoERP></ConfigSincoERP></Task><Task><ID>2</ID><Name>LOGISTICA</Name><BeginDate>16/05/2011 09:00:00 a.m.</BeginDate><EndDate>30/05/2011 07:00:00 p.m.</EndDate><Predesessor></Predesessor><ParentTask>1</ParentTask><PercentAdvanced>0</PercentAdvanced><TaskNotes></TaskNotes><ConfigSincoERP></ConfigSincoERP></Task></Tasks></NodoPapa>";

            try
            {
                listaRes = wsClient.SaveProject(resultado).ToList();
            }

            catch (System.Exception ex) {
                ErroresAplicaciones2 errores = new ErroresAplicaciones2();
                errores.InnerException = ex.InnerException.ToString();
                errores.Mensaje = ex.Message;
                errores.StackTrace = ex.StackTrace;
                errores.Fuente = ex.Source;
                string errorRegistrado = wsClient.ReportarErrorExterno(errores, string.Empty);

                if (string.IsNullOrEmpty(errorRegistrado))
                    MessageBox.Show("Se produjo un error, pero no se pudo registrar. Por favor revise su conexion");

                else
                    throw new System.Exception("Se produjo un error al intentar guardar en la base de datos. El registro del error es " + errorRegistrado, ex.InnerException);
            }

            return listaRes;
        }

        public static List<Duraciones> getDays()
        {
            List<Duraciones> diasLoad = new List<Duraciones>();
            string[] dias = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };
            for(int i = 0; i< dias.Count(); i++) 
            {
                Duraciones cargaDias = new Duraciones();
                cargaDias.Mostrar = dias[i];
                cargaDias.EscribirDB = dias[i];
                diasLoad.Add(cargaDias);
            }
            return diasLoad;
        }

        public static List<Duraciones> getMeridian()
        {
            List<Duraciones> meridianLoad = new List<Duraciones>();
            string[] dias = { "AM", "PM" };
            for (int i = 0; i < dias.Count(); i++)
            {
                Duraciones cargaMeridian = new Duraciones();
                cargaMeridian.Mostrar = dias[i];
                cargaMeridian.EscribirDB = dias[i];
                meridianLoad.Add(cargaMeridian);
            }
            return meridianLoad;
        }


        public static List<Duraciones> getDuraciones()
        {
            List<Duraciones> Duracion = new List<Duraciones>();
            string[] tiempos = {"Hora", "Dia", "Semana", "Mes" };

            for (int i = 0; i < tiempos.Count(); i++) 
            {
                Duraciones durar = new Duraciones();
                durar.Mostrar = tiempos[i];

                if (tiempos[i] == "Hora")
                    durar.EscribirDB = "Hour";
                
                if (tiempos[i] == "Dia")
                    durar.EscribirDB = "Day";

                if (tiempos[i] == "Semana")
                    durar.EscribirDB = "Week";

                if (tiempos[i] == "Mes")
                    durar.EscribirDB = "Month";

                Duracion.Add(durar);
            }
            return Duracion;
        }


        public static List<string> getConfigs(UpperGanttNode headNode) 
        {
            List<string> listaConfigs = new List<string>();
            listaConfigs.Add(headNode.DescProject);
            listaConfigs.Add(headNode.IdProject.ToString());
            listaConfigs.Add(headNode.DurationUnits);
            listaConfigs.Add(headNode.WorkUnits);
            listaConfigs.Add(headNode.WeekStart);
            listaConfigs.Add(headNode.HoursPerDay.ToString());
            listaConfigs.Add(headNode.HoursPerWeek.ToString());
            listaConfigs.Add(headNode.ScheduledStart);
            listaConfigs.Add(headNode.ScheduledFinish);
            //listaConfigs.Add(headNode.ScheduledDuration.ToString());

            return listaConfigs;
        }


        public static string RemoveAccentsWithRegEx(string inputString)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            inputString = replace_a_Accents.Replace(inputString, "a");
            inputString = replace_e_Accents.Replace(inputString, "e");
            inputString = replace_i_Accents.Replace(inputString, "i");
            inputString = replace_o_Accents.Replace(inputString, "o");
            inputString = replace_u_Accents.Replace(inputString, "u");
            return inputString;
        }


        //public static void setWeekExcepts(List<ConfigDays> weekExcept)
        #region NestedClass
        public class Duraciones 
        {
            public string Mostrar { get; set; }
            public string EscribirDB { get; set; }
        }
        #endregion
    }
}