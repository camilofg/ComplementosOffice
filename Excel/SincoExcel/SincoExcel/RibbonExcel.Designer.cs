namespace SincoExcel
{
    partial class RibbonExcel : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RibbonExcel()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
           this.components = new System.ComponentModel.Container();
           Microsoft.Office.Tools.Ribbon.RibbonDialogLauncher ribbonDialogLauncherImpl1 = this.Factory.CreateRibbonDialogLauncher();
           System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RibbonExcel));
           this.TabSincoERP = this.Factory.CreateRibbonTab();
           this.GroupLogin = this.Factory.CreateRibbonGroup();
           this.BtnIngreso = this.Factory.CreateRibbonButton();
           this.GroupInfoUsuario = this.Factory.CreateRibbonGroup();
           this.label1 = this.Factory.CreateRibbonLabel();
           this.label2 = this.Factory.CreateRibbonLabel();
           this.label3 = this.Factory.CreateRibbonLabel();
           this.LbUsuario = this.Factory.CreateRibbonLabel();
           this.LbSucursal = this.Factory.CreateRibbonLabel();
           this.LbEmpresa = this.Factory.CreateRibbonLabel();
           this.GroupFormatos = this.Factory.CreateRibbonGroup();
           this.BtnCrearPlantilla = this.Factory.CreateRibbonButton();
           this.BtnGuardarPlantilla = this.Factory.CreateRibbonButton();
           this.BtnCerrarFormato = this.Factory.CreateRibbonButton();
           this.GroupElementos = this.Factory.CreateRibbonGroup();
           this.BtnCrearElemento = this.Factory.CreateRibbonButton();
           this.IconoAppExcelSisTray = new System.Windows.Forms.NotifyIcon(this.components);
           this.TabSincoERP.SuspendLayout();
           this.GroupLogin.SuspendLayout();
           this.GroupInfoUsuario.SuspendLayout();
           this.GroupFormatos.SuspendLayout();
           this.GroupElementos.SuspendLayout();
           // 
           // TabSincoERP
           // 
           this.TabSincoERP.Groups.Add(this.GroupLogin);
           this.TabSincoERP.Groups.Add(this.GroupInfoUsuario);
           this.TabSincoERP.Groups.Add(this.GroupFormatos);
           this.TabSincoERP.Groups.Add(this.GroupElementos);
           this.TabSincoERP.Label = "Sinco ERP";
           this.TabSincoERP.Name = "TabSincoERP";
           // 
           // GroupLogin
           // 
           this.GroupLogin.DialogLauncher = ribbonDialogLauncherImpl1;
           this.GroupLogin.Items.Add(this.BtnIngreso);
           this.GroupLogin.Label = "Login";
           this.GroupLogin.Name = "GroupLogin";
           this.GroupLogin.DialogLauncherClick += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.GroupLogin_DialogLauncherClick);
           // 
           // BtnIngreso
           // 
           this.BtnIngreso.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
           this.BtnIngreso.Image = global::SincoExcel.Properties.Resources.ImgLogin;
           this.BtnIngreso.Label = "Iniciar Sesión";
           this.BtnIngreso.Name = "BtnIngreso";
           this.BtnIngreso.ScreenTip = "Iniciar Sesión";
           this.BtnIngreso.ShowImage = true;
           this.BtnIngreso.SuperTip = "Permite Iniciar y/o cerrar  Sesión en Sinco  ERP";
           this.BtnIngreso.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnIngreso_Click);
           // 
           // GroupInfoUsuario
           // 
           this.GroupInfoUsuario.Items.Add(this.label1);
           this.GroupInfoUsuario.Items.Add(this.label2);
           this.GroupInfoUsuario.Items.Add(this.label3);
           this.GroupInfoUsuario.Items.Add(this.LbUsuario);
           this.GroupInfoUsuario.Items.Add(this.LbSucursal);
           this.GroupInfoUsuario.Items.Add(this.LbEmpresa);
           this.GroupInfoUsuario.Label = "Usuario";
           this.GroupInfoUsuario.Name = "GroupInfoUsuario";
           this.GroupInfoUsuario.Visible = false;
           // 
           // label1
           // 
           this.label1.Label = "Usuario:";
           this.label1.Name = "label1";
           // 
           // label2
           // 
           this.label2.Label = "Sucursal:";
           this.label2.Name = "label2";
           // 
           // label3
           // 
           this.label3.Label = "Empresa:";
           this.label3.Name = "label3";
           // 
           // LbUsuario
           // 
           this.LbUsuario.Label = "No ha iniciado sesión";
           this.LbUsuario.Name = "LbUsuario";
           // 
           // LbSucursal
           // 
           this.LbSucursal.Label = "No ha iniciado sesión";
           this.LbSucursal.Name = "LbSucursal";
           // 
           // LbEmpresa
           // 
           this.LbEmpresa.Label = "No ha iniciado sesión";
           this.LbEmpresa.Name = "LbEmpresa";
           // 
           // GroupFormatos
           // 
           this.GroupFormatos.Items.Add(this.BtnCrearPlantilla);
           this.GroupFormatos.Items.Add(this.BtnGuardarPlantilla);
           this.GroupFormatos.Items.Add(this.BtnCerrarFormato);
           this.GroupFormatos.Label = "Formatos";
           this.GroupFormatos.Name = "GroupFormatos";
           this.GroupFormatos.Visible = false;
           // 
           // BtnCrearPlantilla
           // 
           this.BtnCrearPlantilla.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
           this.BtnCrearPlantilla.Image = global::SincoExcel.Properties.Resources.ImgBuscar;
           this.BtnCrearPlantilla.Label = "Buscar";
           this.BtnCrearPlantilla.Name = "BtnCrearPlantilla";
           this.BtnCrearPlantilla.ScreenTip = "Búsqueda de formatos";
           this.BtnCrearPlantilla.ShowImage = true;
           this.BtnCrearPlantilla.SuperTip = "Busca formatos del Sistema de Gestión de Calidad (SGC), para registrar y/o dilige" +
               "nciar.";
           this.BtnCrearPlantilla.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnCrearPlantilla_Click);
           // 
           // BtnGuardarPlantilla
           // 
           this.BtnGuardarPlantilla.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
           this.BtnGuardarPlantilla.Image = global::SincoExcel.Properties.Resources.ImgBox;
           this.BtnGuardarPlantilla.Label = "Guardar";
           this.BtnGuardarPlantilla.Name = "BtnGuardarPlantilla";
           this.BtnGuardarPlantilla.ScreenTip = "Guardar Formato";
           this.BtnGuardarPlantilla.ShowImage = true;
           this.BtnGuardarPlantilla.SuperTip = "Guarda el formato actual.";
           this.BtnGuardarPlantilla.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnGuardarPlantilla_Click);
           // 
           // BtnCerrarFormato
           // 
           this.BtnCerrarFormato.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
           this.BtnCerrarFormato.Image = global::SincoExcel.Properties.Resources.ImgDelete;
           this.BtnCerrarFormato.Label = "Cerrar";
           this.BtnCerrarFormato.Name = "BtnCerrarFormato";
           this.BtnCerrarFormato.ScreenTip = "Cerrar Formato";
           this.BtnCerrarFormato.ShowImage = true;
           this.BtnCerrarFormato.SuperTip = "Cierra el formato actual.";
           this.BtnCerrarFormato.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnCerrarFormato_Click);
           // 
           // GroupElementos
           // 
           this.GroupElementos.Items.Add(this.BtnCrearElemento);
           this.GroupElementos.Label = "Elementos";
           this.GroupElementos.Name = "GroupElementos";
           this.GroupElementos.Visible = false;
           // 
           // BtnCrearElemento
           // 
           this.BtnCrearElemento.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
           this.BtnCrearElemento.Image = global::SincoExcel.Properties.Resources.ImgBrush;
           this.BtnCrearElemento.Label = "Diseñador";
           this.BtnCrearElemento.Name = "BtnCrearElemento";
           this.BtnCrearElemento.ScreenTip = "Diseñador de Elementos";
           this.BtnCrearElemento.ShowImage = true;
           this.BtnCrearElemento.SuperTip = "Herramienta que permite diseñar el formatos del Sistema de gestión de Calidad, ad" +
               "icionando descriptores enlazados con el Sistema de Gestón Documental SGD. ";
           this.BtnCrearElemento.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnCrearElemento_Click);
           // 
           // IconoAppExcelSisTray
           // 
           this.IconoAppExcelSisTray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
           this.IconoAppExcelSisTray.BalloonTipText = "Icono";
           this.IconoAppExcelSisTray.BalloonTipTitle = "Titulo del tip";
           this.IconoAppExcelSisTray.Icon = ((System.Drawing.Icon)(resources.GetObject("IconoAppExcelSisTray.Icon")));
           this.IconoAppExcelSisTray.Text = "IconoAppExcelSisTray";
           this.IconoAppExcelSisTray.Visible = true;
           this.IconoAppExcelSisTray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.IconoAppExcelSisTray_MouseDoubleClick);
           // 
           // RibbonExcel
           // 
           this.Name = "RibbonExcel";
           this.RibbonType = "Microsoft.Excel.Workbook";
           this.Tabs.Add(this.TabSincoERP);
           this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonExcel_Load);
           this.TabSincoERP.ResumeLayout(false);
           this.TabSincoERP.PerformLayout();
           this.GroupLogin.ResumeLayout(false);
           this.GroupLogin.PerformLayout();
           this.GroupInfoUsuario.ResumeLayout(false);
           this.GroupInfoUsuario.PerformLayout();
           this.GroupFormatos.ResumeLayout(false);
           this.GroupFormatos.PerformLayout();
           this.GroupElementos.ResumeLayout(false);
           this.GroupElementos.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnIngreso;
        internal Microsoft.Office.Tools.Ribbon.RibbonTab TabSincoERP;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GroupFormatos;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnCrearPlantilla;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnCrearElemento;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GroupInfoUsuario;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel label1;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel label2;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel label3;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel LbUsuario;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel LbSucursal;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel LbEmpresa;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GroupElementos;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnGuardarPlantilla;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnCerrarFormato;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GroupLogin;
        public System.Windows.Forms.NotifyIcon IconoAppExcelSisTray;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonExcel RibbonExcel
        {
            get { return this.GetRibbon<RibbonExcel>(); }
        }
    }
}
