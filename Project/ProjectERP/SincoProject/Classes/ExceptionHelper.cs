using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppSincoProject.WsProjectERPref;
using System.Windows.Forms;
using AppSincoProject.WsFestivosRef;
using System.Xml.Linq;

namespace SincoProject.Classes
{
    class ExceptionHelper
    {
        public static void setWeekExcepts(List<ConfigDays> weekExcept)
        {
            var calendar = Globals.ThisAddIn.Application.ActiveProject.Calendar;
            var diasSemana = calendar.WeekDays;

            for (int i = 0; i < weekExcept.Count(); i++)
            {
                string nombreDia = weekExcept[i].DayName;
                int numDia = 0;
                if (nombreDia == "domingo")
                    numDia = 1;

                if (nombreDia == "lunes")
                    numDia = 2;

                if (nombreDia == "martes")
                    numDia = 3;

                if (nombreDia == "miercoles")
                    numDia = 4;

                if (nombreDia == "jueves")
                    numDia = 5;

                if (nombreDia == "viernes")
                    numDia = 6;

                if (nombreDia == "sabado")
                    numDia = 7;

                diasSemana[numDia].Shift1.Start = weekExcept[i].MorningStart + "a.m";
                var morningSplit = weekExcept[i].MorningStart.Split(':');
                int HoraMana = Convert.ToInt32(morningSplit[0]);
                var shift1Finish = Convert.ToInt32(weekExcept[i].MorningDuration) + HoraMana;
                diasSemana[numDia].Shift1.Finish = shift1Finish.ToString() + ':' + morningSplit[1] + "a.m";

                if (weekExcept[i].AfternoonStart != "")
                {
                    diasSemana[numDia].Shift2.Start = weekExcept[i].AfternoonStart;
                    var afternoonSplit = weekExcept[i].AfternoonStart.Split(':');
                    int HoraTard = Convert.ToInt32(afternoonSplit[0]);
                    var shift2Finish = Convert.ToInt32(weekExcept[i].AfternoonDuration) + HoraTard;
                    diasSemana[numDia].Shift2.Finish = shift2Finish.ToString() + ':' + afternoonSplit[1];
                }

                else
                {
                    diasSemana[numDia].Shift2.Start = weekExcept[i].MorningStart + "a.m";
                    var morningSplit2 = weekExcept[i].MorningStart.Split(':');
                    int HoraMana2 = Convert.ToInt32(morningSplit2[0]);
                    var shift2Finish = Convert.ToInt32(weekExcept[i].MorningDuration) + HoraMana2;
                    diasSemana[numDia].Shift1.Finish = shift2Finish.ToString() + ':' + morningSplit2[1] + "a.m";
                }
            }
        }

        public static XElement ArmExceptionsNode()
        {
            var excep = Globals.ThisAddIn.Application.ActiveProject.Calendar.Exceptions;
            XElement Exceptions = new XElement("Exceptions");
            
            for(int i = 1; i<= excep.Count; i++)
            {
                string pinesw = excep[i].Name.Replace(' ','_');
                if (pinesw == "")
                    pinesw = "Sin_Nombre";
                string inicio = excep[i].Start.ToString().Substring(0, 10);
                string final = excep[i].Finish.ToString().Substring(0, 10);
                XElement Except = new XElement("Exception", new XElement("ExcepName", pinesw), new XElement("StartException", inicio), new XElement("FinishException", final));
                     Exceptions.Add(Except);
            }
            return Exceptions;
        }
    }
}