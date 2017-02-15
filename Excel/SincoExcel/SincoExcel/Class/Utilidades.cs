using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppExternas;

namespace SincoExcel
{
    public class Utilidades
    {
        public static void ReportarError(Exception exc)
        {
            string CodError = ControlErrores.ReportarErrorExterno(Globals.ThisAddIn.DatosUsuario, Globals.ThisAddIn.Conexion, exc);

            if (!string.IsNullOrEmpty(CodError))
            {
               MessageBox.Show(string.Format(Globals.ThisAddIn.MensajeError, CodError), Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
               MessageBox.Show(Globals.ThisAddIn.MensajeErrorNoReportado, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
