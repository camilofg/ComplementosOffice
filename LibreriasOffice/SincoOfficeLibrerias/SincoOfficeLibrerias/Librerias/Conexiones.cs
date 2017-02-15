
namespace SincoOfficeLibrerias
{
    /// <summary>
    /// Clase que define las propiedades Necesarias para el funcionamiento del Complemento de MS Excel
    /// </summary>
    public class ConexionesExcel
    {
        public string IdEmpresaConexion { get; set; }
        public string Licencia { get; set; }
        public string urlWsOfficeSGD { get; set; }
        public string urlWsAutenticacion { get; set; }
        public string urlwsSGCdocumentos { get; set; }
        public int TimeOut { get; set; }
    }

    /// <summary>
    /// Clase que define las propiedades Necesarias para el funcionamiento del Complemento de MS Word
    /// </summary>
    public class ConexionesWord
    {
        public string IdEmpresaConexion { get; set; }
        public string Licencia { get; set; }
        public string urlWsAutenticacion { get; set; }
        public string urlwsArbolVariables { get; set; }
        public int TimeOut { get; set; }
    }


    /// <summary>
    /// Clase que define las propiedades Necesarias para el funcionamiento del Complemento de MS Project
    /// </summary>
    public class ConexionesProject
    {
        public string IdEmpresaConexion { get; set; }
        public string Licencia { get; set; }
        public string urlWsAutenticacion { get; set; }
        public string urlWSProject { get; set; }
        public string urlWSCalcFestivo { get; set; }
        public int TimeOut { get; set; }
    }

    /// <summary>
    /// Clase que define las propiedades Necesarias para el funcionamiento del Complemento de MS Outlook
    /// </summary>
    public class ConexionesOutlook
    {
        public string IdEmpresaConexion { get; set; }
        public string Licencia { get; set; }
        public string urlWsAutenticacion { get; set; }
        public string urlWsCRM { get; set; }
        public string urlWsMenu { get; set; }
        public int TimeOut { get; set; }
    }
}
