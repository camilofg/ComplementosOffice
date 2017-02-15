namespace SincoWord
{
   partial class WordRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
   {
      /// <summary>
      /// Variable del diseñador requerida.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      public WordRibbon()
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
            this.BtnLogin = this.Factory.CreateRibbonButton();
            this.GrpVariables = this.Factory.CreateRibbonGroup();
            this.button1 = this.Factory.CreateRibbonButton();
            this.TabPrincipal.SuspendLayout();
            this.GrpLogin.SuspendLayout();
            this.GrpVariables.SuspendLayout();
            // 
            // TabPrincipal
            // 
            this.TabPrincipal.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.TabPrincipal.Groups.Add(this.GrpLogin);
            this.TabPrincipal.Groups.Add(this.GrpVariables);
            this.TabPrincipal.Label = "Sinco ERP";
            this.TabPrincipal.Name = "TabPrincipal";
            // 
            // GrpLogin
            // 
            this.GrpLogin.Items.Add(this.BtnLogin);
            this.GrpLogin.Label = "Login";
            this.GrpLogin.Name = "GrpLogin";
            // 
            // BtnLogin
            // 
            this.BtnLogin.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.BtnLogin.Image = global::SincoWord.Properties.Resources.ImgLogin;
            this.BtnLogin.Label = "Iniciar Sesión";
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.ShowImage = true;
            this.BtnLogin.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnLogin_Click);
            // 
            // GrpVariables
            // 
            this.GrpVariables.Items.Add(this.button1);
            this.GrpVariables.Label = "Variables";
            this.GrpVariables.Name = "GrpVariables";
            this.GrpVariables.Visible = false;
            // 
            // button1
            // 
            this.button1.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.button1.Image = global::SincoWord.Properties.Resources.Jerarquia;
            this.button1.Label = "Arbol de Variables";
            this.button1.Name = "button1";
            this.button1.ShowImage = true;
            this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // WordRibbon
            // 
            this.Name = "WordRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.TabPrincipal);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.WordRibbon_Load);
            this.TabPrincipal.ResumeLayout(false);
            this.TabPrincipal.PerformLayout();
            this.GrpLogin.ResumeLayout(false);
            this.GrpLogin.PerformLayout();
            this.GrpVariables.ResumeLayout(false);
            this.GrpVariables.PerformLayout();

      }

      #endregion

      internal Microsoft.Office.Tools.Ribbon.RibbonTab TabPrincipal;
      internal Microsoft.Office.Tools.Ribbon.RibbonGroup GrpLogin;
      internal Microsoft.Office.Tools.Ribbon.RibbonButton BtnLogin;
      internal Microsoft.Office.Tools.Ribbon.RibbonGroup GrpVariables;
      internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
   }

   partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
   {
      internal WordRibbon WordRibbon
      {
         get { return this.GetRibbon<WordRibbon>(); }
      }
   }
}
