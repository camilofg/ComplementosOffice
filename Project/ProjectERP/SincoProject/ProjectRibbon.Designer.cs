namespace SincoProject
{
    partial class ProjectRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ProjectRibbon()
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
            this.TabPrincipal = this.Factory.CreateRibbonTab();
            this.GrpLogin = this.Factory.CreateRibbonGroup();
            this.GrpFunciones = this.Factory.CreateRibbonGroup();
            this.GrpDatosSesion = this.Factory.CreateRibbonGroup();
            this.LblUsuario = this.Factory.CreateRibbonLabel();
            this.LblProgramacion = this.Factory.CreateRibbonLabel();
            this.LblNameProg = this.Factory.CreateRibbonLabel();
            this.BtnLogin = this.Factory.CreateRibbonButton();
            this.BtnSave = this.Factory.CreateRibbonButton();
            this.BtnLoad = this.Factory.CreateRibbonButton();
            this.TabPrincipal.SuspendLayout();
            this.GrpLogin.SuspendLayout();
            this.GrpFunciones.SuspendLayout();
            this.GrpDatosSesion.SuspendLayout();
            // 
            // TabPrincipal
            // 
            this.TabPrincipal.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.TabPrincipal.Groups.Add(this.GrpLogin);
            this.TabPrincipal.Groups.Add(this.GrpFunciones);
            this.TabPrincipal.Groups.Add(this.GrpDatosSesion);
            this.TabPrincipal.Label = "Sinco ERP";
            this.TabPrincipal.Name = "TabPrincipal";
            // 
            // GrpLogin
            // 
            this.GrpLogin.Items.Add(this.BtnLogin);
            this.GrpLogin.Label = "Login";
            this.GrpLogin.Name = "GrpLogin";
            // 
            // GrpFunciones
            // 
            this.GrpFunciones.Items.Add(this.BtnSave);
            this.GrpFunciones.Items.Add(this.BtnLoad);
            this.GrpFunciones.Name = "GrpFunciones";
            this.GrpFunciones.Visible = false;
            // 
            // GrpDatosSesion
            // 
            this.GrpDatosSesion.Items.Add(this.LblUsuario);
            this.GrpDatosSesion.Items.Add(this.LblProgramacion);
            this.GrpDatosSesion.Items.Add(this.LblNameProg);
            this.GrpDatosSesion.Name = "GrpDatosSesion";
            this.GrpDatosSesion.Visible = false;
            // 
            // LblUsuario
            // 
            this.LblUsuario.Label = "Empresa";
            this.LblUsuario.Name = "LblUsuario";
            // 
            // LblProgramacion
            // 
            this.LblProgramacion.Label = "Modulo";
            this.LblProgramacion.Name = "LblProgramacion";
            // 
            // LblNameProg
            // 
            this.LblNameProg.Label = "Programacion";
            this.LblNameProg.Name = "LblNameProg";
            this.LblNameProg.Visible = false;
            // 
            // BtnLogin
            // 
            this.BtnLogin.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.BtnLogin.Image = global::SincoProject.Properties.Resources.ImgLogin;
            this.BtnLogin.Label = "Iniciar Sesión";
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.ShowImage = true;
            this.BtnLogin.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnLogin_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.BtnSave.Image = global::SincoProject.Properties.Resources.database_save;
            this.BtnSave.Label = "Guardar";
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.ShowImage = true;
            this.BtnSave.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnSave_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.BtnLoad.Image = global::SincoProject.Properties.Resources.database_search;
            this.BtnLoad.Label = "Cargar";
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.ShowImage = true;
            this.BtnLoad.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnLoad_Click);
            // 
            // ProjectRibbon
            // 
            this.Name = "ProjectRibbon";
            this.RibbonType = "Microsoft.Project.Project";
            this.Tabs.Add(this.TabPrincipal);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ProjectRibbon_Load);
            this.TabPrincipal.ResumeLayout(false);
            this.TabPrincipal.PerformLayout();
            this.GrpLogin.ResumeLayout(false);
            this.GrpLogin.PerformLayout();
            this.GrpFunciones.ResumeLayout(false);
            this.GrpFunciones.PerformLayout();
            this.GrpDatosSesion.ResumeLayout(false);
            this.GrpDatosSesion.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab TabPrincipal;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GrpLogin;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnLogin;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GrpFunciones;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnSave;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnLoad;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GrpDatosSesion;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel LblUsuario;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel LblProgramacion;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel LblNameProg;
        //internal Microsoft.Office.Tools.Ribbon.RibbonGroup GrpVariables;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal ProjectRibbon ProjectRibbon
        {
            get { return this.GetRibbon<ProjectRibbon>(); }
        }
    }
}
