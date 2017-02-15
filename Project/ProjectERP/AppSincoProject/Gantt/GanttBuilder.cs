using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppSincoProject.WsProjectERPref;
using AppSincoProject.WsFestivosRef;
using System.Xml.Linq;
using System.Xml;

namespace AppSincoProject.Gantt
{
    public class GanttBuilder   
    {
        //WsProjectERPref.WSProjectERPtest ws = new WSProjectERPtest();
        //WsFestivosRef.WsCalcFestivos wsFest = new WsCalcFestivos();
        //public List<LoadComboBox> LoadModules ()
        //{
        //    var lista = ws.GetModules().ToList();
        //    return lista;
        //}

        //public List<LoadComboBox> LoadProjects(string usuarioId, string moduloId)
        //{
        //    var lista = ws.GetProjects(usuarioId, moduloId).ToList();
        //    return lista;
        //}

        //public UpperGanttNode GetProject(string prgId) 
        //{
        //    var upperNode = ws.LoadProject(prgId);
        //    return upperNode;
        //}

        //public List<string> SaveConf(string operacion, string PrgId, string PrgDescripcion, string PrgModulo, string PrgObservarciones, int EstadoEstado, string PrgUnidadesTrabajo, string PrgUnidadesDuracion,
        //                                string PrgHorasDia, string PrgHorasSemana, string PrgInicioProgramado, string PrgFinProgramado, int PrgDuracionProgramada, string PrgUsuario, string PrgWeekStart)
        //{
        //    var listSaveConfigs = ws.SaveConfigs(operacion, PrgId, PrgDescripcion, PrgModulo, PrgObservarciones, EstadoEstado, PrgUnidadesTrabajo, PrgUnidadesDuracion,
        //                                        PrgHorasDia, PrgHorasSemana, PrgInicioProgramado, PrgFinProgramado, PrgDuracionProgramada, PrgUsuario, PrgWeekStart).ToList();
        //    return listSaveConfigs;
        //}

        //public List<string> SaveWeekExcepts(ConfigDays dia)
        //{
        //    List<string> listaResult = new List<string>();
        //    var listaWeek = ws.SaveWeekExceptions(dia);
        //    listaResult = listaWeek.ToList();
        //    return listaResult;
        //}

        //public List<Festivo> FestivosColombia(int anoInicio, int anoFin)
        //{
        //    List<Festivo> listaFestivos = new List<Festivo>();
        //    var listaFest = wsFest.Calculate(anoInicio, anoFin);
        //    listaFestivos = listaFest.ToList();
        //    return listaFestivos;
        //}

        //public List<string> CompleteSaveProject(string xmlDoc)
        //{
        //    List<string> listResults = new List<string>();
        //    var lista = ws.SaveProject(xmlDoc);
        //    listResults = lista.ToList();
        //    return listResults;
        //}
    }
}
