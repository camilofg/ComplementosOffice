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

namespace SincoExcel
{
    public partial class FrmConfigurarComplemento : Form
    {
        public FrmConfigurarComplemento()
        {
            try
            {
                InitializeComponent();
                                
                string ruta = RegistroWindows.ConsultarEntradaRegistro("Ruta", "Temporales");
                string ArchivoLicencia = RegistroWindows.ConsultarEntradaRegistro("LICENSE", "path");

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
                    MessageBox.Show(Properties.Settings.Default.MsGuardarCambiosCompleto, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose(true);
                }
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

                PanelColorObligatorio.BackColor = Globals.ThisAddIn.ColorDescriptorObligatorio;
                PanelColorOpcional.BackColor = Globals.ThisAddIn.ColorDescriptorOpcional;

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
                    bool AgregarRutaLicencia = RegistroWindows.AgregarEntradaRegistro("LICENSE", "path", BuscarLicencia.FileName);
                    if (!AgregarRutaLicencia)
                    {
                        MessageBox.Show(Properties.Settings.Default.MsErrorCargarArchivoLicencia, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void PanelColorOpcional_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ColorDialog ColorNuevo = new ColorDialog();
                ColorNuevo.AllowFullOpen = true;
                ColorNuevo.AnyColor = true;
                ColorNuevo.FullOpen = true;

                #region Cargar Colores
                //string ColorDescriptorObli = RegistroWindows.ConsultarEntradaRegistro("Color", "DescriptorObligatorio");
                string ColorDescriptorOpci = RegistroWindows.ConsultarEntradaRegistro("Color", "DescriptorOpcional");

                if (!string.IsNullOrEmpty(ColorDescriptorOpci))
                {
                    ColorNuevo.Color = Color.FromArgb(int.Parse(ColorDescriptorOpci));
                }
                #endregion

                DialogResult Respuesta = ColorNuevo.ShowDialog();
                if (Respuesta == System.Windows.Forms.DialogResult.OK)
                {
                    if (ColorNuevo.Color != null)
                    {
                        PanelColorOpcional.BackColor = ColorNuevo.Color;
                        bool AgregarRutaLicencia = RegistroWindows.AgregarEntradaRegistro("Color", "DescriptorOpcional", ColorNuevo.Color.ToArgb().ToString());
                        if (!AgregarRutaLicencia)
                        {
                            MessageBox.Show(Properties.Settings.Default.MsErrorCambiarColorConfiguracion, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Globals.ThisAddIn.ColorDescriptorOpcional = ColorNuevo.Color;
                        }
                    }
                }
            }
            catch (Exception EXC)
            {
                Utilidades.ReportarError(EXC);
            }
        }
        
        private void PanelColorObligatorio_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ColorDialog ColorNuevo = new ColorDialog();
                ColorNuevo.AllowFullOpen = true;
                ColorNuevo.AnyColor = true;
                ColorNuevo.FullOpen = true;

                #region Cargar Colores
                string ColorDescriptorObli = RegistroWindows.ConsultarEntradaRegistro("Color", "DescriptorObligatorio");
                //string ColorDescriptorOpci = RegistroWindows.ConsultarEntradaRegistro("Color", "DescriptorOpcional");

                if (!string.IsNullOrEmpty(ColorDescriptorObli))
                {
                    ColorNuevo.Color = Color.FromArgb(int.Parse(ColorDescriptorObli));
                }
                #endregion

                DialogResult Respuesta = ColorNuevo.ShowDialog();
                if (Respuesta == System.Windows.Forms.DialogResult.OK)
                {
                    if (ColorNuevo.Color != null)
                    {
                        PanelColorObligatorio.BackColor = ColorNuevo.Color;
                        bool AgregarRutaLicencia = RegistroWindows.AgregarEntradaRegistro("Color", "DescriptorObligatorio", ColorNuevo.Color.ToArgb().ToString());
                        if (!AgregarRutaLicencia)
                        {
                            MessageBox.Show(Properties.Settings.Default.MsErrorCambiarColorConfiguracion, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Globals.ThisAddIn.ColorDescriptorObligatorio = ColorNuevo.Color;
                        }
                    }
                }
            }
            catch (Exception EXC)
            {
                Utilidades.ReportarError(EXC);
            }
        }
    }
}
