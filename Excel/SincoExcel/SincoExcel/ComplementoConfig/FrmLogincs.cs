using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SincoOfficeLibrerias;

using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using ExcelTools = Microsoft.Office.Tools.Excel;
using System.Threading;
using System.IO;

using AppExternas;
using System.ServiceModel;

namespace SincoExcel
{
    public partial class FrmLogincs : Form
    {
        public FrmLogincs()
        {
            try
            {
                InitializeComponent();
                bool ResLic = CargarLicencia();
                if (ResLic)
                {   ConfiguracionInicial();     }
                else
                {   MessageBox.Show(Properties.Settings.Default.MsErrorIniciarArchivoLicencia, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);     }
            }
            catch (Exception EXC)
            {
                Utilidades.ReportarError(EXC);
            }
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TbNombreUsuario.Text.Trim()) && !string.IsNullOrEmpty(TbContraseña.Text)
                        && CbEmpresas.SelectedValue.ToString() != "-1" && CbSucursales.SelectedValue.ToString() != "-1")
                {
                    TbNombreUsuario.Enabled = false;
                    TbContraseña.Enabled = false;
                    CbEmpresas.Enabled = false;
                    CbSucursales.Enabled = false;

                    Login User = Login.ValidarAutenticacion(Globals.ThisAddIn.Conexion, TbNombreUsuario.Text, TbContraseña.Text, CbSucursales.SelectedValue.ToString());
                    if ( !string.IsNullOrEmpty(User.CadenaConexion) && !string.IsNullOrEmpty(User.IdUsuario) && !string.IsNullOrEmpty(User.EmpresaId))
                    {
                        User.SucId = CbSucursales.SelectedValue.ToString();
                        User.SucDesc = CbSucursales.Text;

                        Globals.ThisAddIn.DatosUsuario = User;

                        Globals.Ribbons.RibbonExcel.LbUsuario.Label = User.NomUsuario;
                        Globals.Ribbons.RibbonExcel.LbSucursal.Label = User.SucDesc;
                        Globals.Ribbons.RibbonExcel.LbEmpresa.Label = User.EmpresaNombre;

                        Globals.Ribbons.RibbonExcel.TabSincoERP.Visible = true;
                        Globals.Ribbons.RibbonExcel.GroupElementos.Visible = true;
                        Globals.Ribbons.RibbonExcel.GroupFormatos.Visible = true;
                        Globals.Ribbons.RibbonExcel.GroupInfoUsuario.Visible = true;
                        Globals.Ribbons.RibbonExcel.BtnIngreso.Label = "Cerrar Sesión";

                        Globals.Ribbons.RibbonExcel.IconoAppExcelSisTray.BalloonTipText = "Sesión Iniciada por: \n" + User.NomUsuario + " - " + User.EmpresaNombre;
                        Globals.Ribbons.RibbonExcel.IconoAppExcelSisTray.BalloonTipTitle = Globals.ThisAddIn.MensajeTitulos;
                        Globals.Ribbons.RibbonExcel.IconoAppExcelSisTray.BalloonTipIcon = ToolTipIcon.Info;
                        Globals.Ribbons.RibbonExcel.IconoAppExcelSisTray.ShowBalloonTip(5000);

                        MetodosRibbon.CargarInformacionInicial();

                        CargarOpcionesSession();

                        bool ResultadoSucursal = RegistroWindows.AgregarEntradaRegistro("Sucursal", "ConfiguracionInicial", CbSucursales.SelectedValue.ToString());
                        bool ResultadoEmpresa = RegistroWindows.AgregarEntradaRegistro("Empresa", "ConfiguracionInicial", CbEmpresas.SelectedValue.ToString());

                        this.Dispose(true);
                    }
                    else
                    {
                        MessageBox.Show( Properties.Settings.Default.MsErrorDatosLoginUsuario, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        TbNombreUsuario.Enabled = true;
                        TbContraseña.Enabled = true;
                        TbContraseña.Text = string.Empty;
                        CbEmpresas.Enabled = true;
                        CbSucursales.Enabled = true;
                        Globals.Ribbons.RibbonExcel.TabSincoERP.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show(Properties.Settings.Default.MsCompletarDatosRequeridosLogin, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception EXC)
            {
                Utilidades.ReportarError(EXC);
            }
        }

        private void ConfiguracionInicial()
        {
            try
            {
                DataTable Empresas = Login.CargarListadoEmpresas(Globals.ThisAddIn.Conexion);

                if (Empresas.Rows.Count > 0)
                {
                    CbEmpresas.DisplayMember = "EmpresaNombre";
                    CbEmpresas.ValueMember = "EmpresaId";
                    CbEmpresas.DataSource = Empresas;

                    string UltimaEmpresa = RegistroWindows.ConsultarEntradaRegistro("Empresa", "ConfiguracionInicial");
                    if (!string.IsNullOrEmpty(UltimaEmpresa))
                    {
                        CbEmpresas.SelectedValue = UltimaEmpresa;
                    }
                }
            }
            catch (Exception EXC)
            {
                Utilidades.ReportarError(EXC);
            }
        }

        private void ConfigurarSessionExpired()
        {
            try
            {
                Globals.Ribbons.RibbonExcel.TimerSessionExpired.Stop();

                Globals.Ribbons.RibbonExcel.TimerSessionExpired.Interval = Globals.Ribbons.RibbonExcel.TimeSessionExpired;
                Globals.Ribbons.RibbonExcel.TimerSessionExpired.Start();
            }
            catch (Exception EXC)
            {
                Utilidades.ReportarError(EXC);
            }
        }

        private void FrmLogincs_Load(object sender, EventArgs e)
        {

        }

        private void TbNombreUsuario_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TbNombreUsuario.Text.Trim()))
                {
                    DataTable Sucursales = Login.CargarListadoSucursales(Globals.ThisAddIn.Conexion, TbNombreUsuario.Text);

                    if (Sucursales.Rows.Count > 0)
                    {
                        CbSucursales.DisplayMember = "SucDesc";
                        CbSucursales.ValueMember = "SucID";
                        CbSucursales.DataSource = Sucursales;

                        string UltimaSucursal = RegistroWindows.ConsultarEntradaRegistro("Sucursal", "ConfiguracionInicial");
                        if (!string.IsNullOrEmpty(UltimaSucursal))
                        {
                            CbSucursales.SelectedValue = UltimaSucursal;
                        }
                        else
                        {
                            CbSucursales.SelectedValue = -1;
                        }
                    }
                    else
                    {
                        CbSucursales.SelectedValue = -1;
                    }
                }
                else
                {
                    CbSucursales.SelectedValue = -1;
                }

                TbContraseña.Text = string.Empty;
            }
            catch
            {
                MessageBox.Show(Properties.Settings.Default.MsErrorCargarListaSucursalesLogin, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool CargarLicencia()
        {
            bool Resultado = true;

            try
            {
                #region Cargar licencia archivo
                //Leer archivo de licencia
                string ArchivoLicencia = RegistroWindows.ConsultarEntradaRegistro("LICENSE", "path");

                if (!string.IsNullOrEmpty(ArchivoLicencia))
                {
                    Byte[] ContenidoArchivo = new Byte[0];
                    string rutaArchivo = ArchivoLicencia;
                    FileStream objfilestreamRead = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
                    int len = (int)objfilestreamRead.Length;
                    ContenidoArchivo = new Byte[len];
                    objfilestreamRead.Read(ContenidoArchivo, 0, len);
                    objfilestreamRead.Close();

                    Licencias Licencia = Licencias.LeerLicencia(ContenidoArchivo);

                    if (!string.IsNullOrEmpty(Licencia.Nombre) && !string.IsNullOrEmpty(Licencia.Key) && Licencia.Propiedades.Count > 0)
                    {
                        ConexionesExcel DatosConexion = new ConexionesExcel();
                        bool IsCompleted = true;

                        if (Licencia.Propiedades.ContainsKey("urlWsAutenticacion"))
                        {   DatosConexion.urlWsAutenticacion = Licencia.Propiedades["urlWsAutenticacion"];  }
                        else
                        {   IsCompleted = false;    }

                        if (Licencia.Propiedades.ContainsKey("urlWsOfficeSGD"))
                        {   DatosConexion.urlWsOfficeSGD = Licencia.Propiedades["urlWsOfficeSGD"];     }
                        else
                        {   IsCompleted = false;    }

                        if (Licencia.Propiedades.ContainsKey("urlwsSGCdocumentos"))
                        { DatosConexion.urlwsSGCdocumentos = Licencia.Propiedades["urlwsSGCdocumentos"]; }
                        else
                        { IsCompleted = false; }

                        if (Licencia.Propiedades.ContainsKey("IdEmpresaConexion"))
                        {   DatosConexion.IdEmpresaConexion = Licencia.Propiedades["IdEmpresaConexion"];    }
                        else
                        {   IsCompleted = false;    }

                        if (Licencia.Propiedades.ContainsKey("TimeOut"))
                        {   DatosConexion.TimeOut = int.Parse(Licencia.Propiedades["TimeOut"]);     }
                        else
                        {   IsCompleted = false; }


                        #region Cargar URL APP EXTERNAS
                        if (Licencia.Propiedades.ContainsKey("urlAppExternas"))
                        {
                            string Url = Licencia.Propiedades["urlAppExternas"];
                            WSHttpBinding wsHttpBinding = new WSHttpBinding();

                            wsHttpBinding.CloseTimeout = TimeSpan.Parse("00:01:00");
                            wsHttpBinding.OpenTimeout = TimeSpan.Parse("00:01:00" );
                            wsHttpBinding.ReceiveTimeout = TimeSpan.Parse("00:10:00" );
                            wsHttpBinding.SendTimeout = TimeSpan.Parse("00:01:00");
                            wsHttpBinding.BypassProxyOnLocal = false;
                            wsHttpBinding.TransactionFlow = false;
                            wsHttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
                            wsHttpBinding.MaxBufferPoolSize = 524288;
                            wsHttpBinding.MaxReceivedMessageSize = 65536;
                            wsHttpBinding.MessageEncoding = WSMessageEncoding.Text;
                            wsHttpBinding.TextEncoding = System.Text.Encoding.UTF8;
                            wsHttpBinding.UseDefaultWebProxy = true;
                            wsHttpBinding.AllowCookies = false;
                            wsHttpBinding.Security.Mode = SecurityMode.Transport;


                            //Conexiones ConTemp = new Conexiones(Url, wsHttpBinding);

                            Conexiones ConTemp = new Conexiones(Url);

                            Conexiones ConexionGeneral = ConTemp;

                            Globals.ThisAddIn.Conexion = ConexionGeneral;
                        }
                        else
                        { IsCompleted = false; }
                        #endregion


                        if (!string.IsNullOrEmpty(Licencia.Key) )
                        { DatosConexion.Licencia = Licencia.Key; }
                        else
                        {   IsCompleted = false;    }

                        if (IsCompleted)
                        {
                            Globals.ThisAddIn.DatosConexion = DatosConexion;
                        }
                        else
                        {
                            MessageBox.Show("Licencia no Válida", Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }                       


                    string TextoPuro = DataEncryption.Decryption(ContenidoArchivo, Globals.ThisAddIn.newKeyFile, Globals.ThisAddIn.newIVFile);

                    if (TextoPuro.Split('|').Length > 1)
                    {
                        string[] Datos = TextoPuro.Split('|');
                        
                    }
                }

                #endregion
            }
            catch
            {
                Resultado = false;
            }

            return Resultado;
        }

        private bool CargarOpcionesSession()
        {
            bool resultado = true;
            try
            {
                #region Validar acceso usuarios
                Globals.ThisAddIn.ValidarMenusUsuario(Globals.ThisAddIn.DatosUsuario);
                #endregion

                ConfigurarSessionExpired();
            }
            catch
            {
                resultado = false;
            }

            return resultado;
        }
    }
}
