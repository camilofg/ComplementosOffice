using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppExternas;
using System.IO;
using SincoProject.Classes;
using SincoOfficeLibrerias;



namespace SincoProject
{
   public partial class FrmLogin : Form
   {
      public FrmLogin()
      {
          try
          {
              InitializeComponent();
              bool ResLic = CargarLicencia();
              if (ResLic)
              { ConfiguracionInicial(); }
              else
              { MessageBox.Show(Properties.Settings.Default.MsErrorIniciarArchivoLicencia, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information); }
          }
                
          catch (Exception EXC)
          {
              Utilidades.ReportarError(EXC);
          }
      }


      private bool CargarLicencia()
      {
          bool Resultado = true;

          try
          {
              #region Cargar licencia archivo
              //Leer archivo de licencia
              string ArchivoLicencia = RegistroWindows.ConsultarEntradaRegistro("LICENSE", "pathProject");
              string TextPure = string.Empty;

              if (!string.IsNullOrEmpty(ArchivoLicencia))
              {
                  TextPure = ConfirmarLicencia(ArchivoLicencia);
              }


              else
              {
                  FrmConfigurarComplemento frmConfigLicencia = new FrmConfigurarComplemento();
                  frmConfigLicencia.Show();
                  this.Close();
              }

              //if (string.IsNullOrEmpty(TextPure))
              //{
              //    MessageBox.Show("No Existe la Licencia, o no se encuentra registrada", Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
              //    FrmConfigurarComplemento frmConfigLicencia = new FrmConfigurarComplemento();
              //    frmConfigLicencia.Show();
              //    this.Close();
              //    Resultado = false;
              //    //this.Hide();
              //}

              if (TextPure.Split('|').Length > 1)
              {
                  string[] Datos = TextPure.Split('|');
              }
              #endregion
          }
          catch
          {
              Resultado = false;
          }

          return Resultado;
      }

      private string ConfirmarLicencia(string ArchivoLicencia)
      {
          string TextoPuro = string.Empty;
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
                  ConexionesProject DatosConexion = new ConexionesProject();
                  bool IsCompleted = true;

                  if (Licencia.Propiedades.ContainsKey("urlWsAutenticacion"))
                  { DatosConexion.urlWsAutenticacion = Licencia.Propiedades["urlWsAutenticacion"]; }
                  else
                  { IsCompleted = false; }

                  if (Licencia.Propiedades.ContainsKey("urlWSProject"))
                  { DatosConexion.urlWSProject = Licencia.Propiedades["urlWSProject"]; }
                  else
                  { IsCompleted = false; }

                  if (Licencia.Propiedades.ContainsKey("urlWSCalcFestivo"))
                  { DatosConexion.urlWSCalcFestivo = Licencia.Propiedades["urlWSCalcFestivo"]; }
                  else
                  { IsCompleted = false; }

                  if (Licencia.Propiedades.ContainsKey("TimeOut"))
                  { DatosConexion.TimeOut = int.Parse(Licencia.Propiedades["TimeOut"]); }
                  else
                  { IsCompleted = false; }

                  Conexiones ConTemp = new Conexiones(Licencia.Propiedades["urlWsAutenticacion"]);
                  Conexiones ConexionGeneral = ConTemp;
                  Conexiones ConexionWsProject = new Conexiones(Licencia.Propiedades["urlWSProject"]);
                  Conexiones ConexionWsFestivos = new Conexiones(Licencia.Propiedades["urlWSCalcFestivo"]);

                  Globals.ThisAddIn.Conexion = ConexionGeneral;

                  Globals.ThisAddIn.ConexWSProject = ConexionWsProject;
                  Globals.ThisAddIn.ConexWSFestivos = ConexionWsFestivos;

                  if (!string.IsNullOrEmpty(Licencia.Key))
                  { DatosConexion.Licencia = Licencia.Key; }
                  else
                  { IsCompleted = false; }

                  if (IsCompleted)
                  {
                      Globals.ThisAddIn.DatosConexion = DatosConexion;
                  }
                  else
                  {
                      MessageBox.Show("Licencia no Válida", Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
                      RegistroWindows winReg = new RegistroWindows();
                      winReg.EliminarEntradaRegistro("LICENSE", "pathProject");
                      this.Close();
                  }

              }

              TextoPuro = DataEncryption.Decryption(ContenidoArchivo, Globals.ThisAddIn.newKeyFile, Globals.ThisAddIn.newIVFile);
          }
          return TextoPuro;
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

                     CargarOpcionesSession();

                      
                     //MessageBox.Show("Sesión iniciada. \n\n " + Globals.ThisAddIn.DatosUsuario.NomUsuario + "\n" + Globals.ThisAddIn.DatosUsuario.SucDesc);
                        Globals.Ribbons.ProjectRibbon.GrpFunciones.Visible = true;
                        Globals.Ribbons.ProjectRibbon.GrpLogin.Visible = false;
                     //Globals.Ribbons.ProjectRibbon.GrpFunciones.Label = User.EmpresaNombre + User.SucDesc + User.NomUsuario;
                        Globals.Ribbons.ProjectRibbon.LblUsuario.Label = User.EmpresaNombre +'-'+ User.SucDesc;
                        Globals.Ribbons.ProjectRibbon.LblProgramacion.Visible = false;
                        Globals.Ribbons.ProjectRibbon.GrpDatosSesion.Visible = true;

                     bool ResultadoSucursal = RegistroWindows.AgregarEntradaRegistro("Sucursal", "ConfiguracionInicial", CbSucursales.SelectedValue.ToString());
                     bool ResultadoEmpresa = RegistroWindows.AgregarEntradaRegistro("Empresa", "ConfiguracionInicial", CbEmpresas.SelectedValue.ToString());

                     this.Dispose(true);
                  }
                  else
                  {
                     //MessageBox.Show( Properties.Settings.Default.MsErrorDatosLoginUsuario, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);

                     TbNombreUsuario.Enabled = true;
                     TbContraseña.Enabled = true;
                     TbContraseña.Text = string.Empty;
                     CbEmpresas.Enabled = true;
                     CbSucursales.Enabled = true;

                  }
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
         //try
         //{
         //      Globals.Ribbons.RibbonExcel.TimerSessionExpired.Stop();

         //      Globals.Ribbons.RibbonExcel.TimerSessionExpired.Interval = Globals.Ribbons.RibbonExcel.TimeSessionExpired;
         //      Globals.Ribbons.RibbonExcel.TimerSessionExpired.Start();
         //}
         //catch (Exception EXC)
         //{
         //      Utilidades.ReportarError(EXC);
         //}
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
               //MessageBox.Show(Properties.Settings.Default.MsErrorCargarListaSucursalesLogin, Globals.ThisAddIn.MensajeTitulos, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
      }
             
      private bool CargarOpcionesSession()
      {
         bool resultado = true;
         try
         {
               //#region Validar acceso usuarios
               //Globals.ThisAddIn.ValidarMenusUsuario(Globals.ThisAddIn.DatosUsuario);
               //#endregion

               //ConfigurarSessionExpired();
         }
         catch
         {
               resultado = false;
         }

         return resultado;
      }
   }
}
