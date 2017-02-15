using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SincoOfficeLibrerias;
using System.IO;
using AppExternas;
using SincoProject.Classes;
using SincoProject;

namespace SincoProject
{
    public partial class FrmConfigurarComplemento : Form
    {
        public FrmConfigurarComplemento()
        {
            try
            {
                InitializeComponent();
                                
                string ruta = RegistroWindows.ConsultarEntradaRegistro("Ruta", "Temporales");
                string ArchivoLicencia = RegistroWindows.ConsultarEntradaRegistro("LICENSE", "pathProject");

                if (!string.IsNullOrEmpty(ArchivoLicencia))
                {
                    TbURL.Text = ArchivoLicencia;
                }
                if (!string.IsNullOrEmpty(ruta))
                {
                    TbRutaTemporal.Text = ruta;
                }
            }
            catch (Exception EXC)
            {
                Utilidades.ReportarError(EXC); 
            }
        }

        private void BtnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                bool ResultadoURL = false;
                bool ResultadoRutaTemp = false;

                ResultadoURL = RegistroWindows.AgregarEntradaRegistro("URLLogin", "URL", TbURL.Text);
                ResultadoRutaTemp = RegistroWindows.AgregarEntradaRegistro("Ruta", "Temporales", TbRutaTemporal.Text);
                if (ResultadoURL && ResultadoRutaTemp)
                {
                    MessageBox.Show(SincoProject.Properties.Settings.Default.MsGuardarCambiosCompleto, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose(true);
                }

                FrmLogin Frm = new FrmLogin();
                Frm.Show();
            }
            catch (Exception EXC)
            {
                Utilidades.ReportarError(EXC);
            }
        }

        private void FrmConfigurarComplemento_Load(object sender, EventArgs e)
        {
            try
            {
                #region Cargar Colores
                //RecColorDescriptorObligatorio.BackColor = Globals.ThisAddIn.ColorDescriptorObligatorio;
                //RecColorDescriptorOpcional.BackColor = Globals.ThisAddIn.ColorDescriptorOpcional;
                //label3.ForeColor = Globals.ThisAddIn.ColorDescriptorObligatorio;
                //label4.ForeColor = Globals.ThisAddIn.ColorDescriptorOpcional;

                //PanelColorObligatorio.BackColor = Globals.ThisAddIn.ColorDescriptorObligatorio;
                //PanelColorOpcional.BackColor = Globals.ThisAddIn.ColorDescriptorOpcional;

                #endregion
            }
            catch (Exception EXC)
            {
                Utilidades.ReportarError(EXC);
            }
        }

        private void BtnBuscarLicencia_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog BuscarLicencia = new OpenFileDialog();
                BuscarLicencia.Multiselect = false;
                BuscarLicencia.CheckFileExists = true;

                BuscarLicencia.ShowDialog();

                if (!string.IsNullOrEmpty(BuscarLicencia.FileName))
                {
                    bool AgregarRutaLicencia = RegistroWindows.AgregarEntradaRegistro("LICENSE", "pathProject", BuscarLicencia.FileName);
                    if (!AgregarRutaLicencia)
                    {
                        MessageBox.Show(SincoProject.Properties.Settings.Default.MsErrorCargarArchivoLicencia, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        TbURL.Text = BuscarLicencia.FileName;
                    }
                }
            }
            catch
            {
                
            }
        }
    }
}
