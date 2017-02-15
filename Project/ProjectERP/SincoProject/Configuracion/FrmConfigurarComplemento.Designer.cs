namespace SincoProject
{
    partial class FrmConfigurarComplemento
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.BtnGuardarCambios = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnBuscarLicencia = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TbURL = new System.Windows.Forms.TextBox();
            this.ToolTipConfiguracion = new System.Windows.Forms.ToolTip(this.components);
            this.TbRutaTemporal = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnGuardarCambios
            // 
            this.BtnGuardarCambios.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGuardarCambios.Location = new System.Drawing.Point(406, 113);
            this.BtnGuardarCambios.Name = "BtnGuardarCambios";
            this.BtnGuardarCambios.Size = new System.Drawing.Size(182, 27);
            this.BtnGuardarCambios.TabIndex = 5;
            this.BtnGuardarCambios.Text = "Guardar Cambios";
            this.BtnGuardarCambios.UseVisualStyleBackColor = true;
            this.BtnGuardarCambios.Click += new System.EventHandler(this.BtnGuardarCambios_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnBuscarLicencia);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TbRutaTemporal);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TbURL);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(11, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(577, 95);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuración de SINCO Office";
            // 
            // BtnBuscarLicencia
            // 
            this.BtnBuscarLicencia.Location = new System.Drawing.Point(494, 28);
            this.BtnBuscarLicencia.Name = "BtnBuscarLicencia";
            this.BtnBuscarLicencia.Size = new System.Drawing.Size(75, 23);
            this.BtnBuscarLicencia.TabIndex = 4;
            this.BtnBuscarLicencia.Text = "Buscar";
            this.BtnBuscarLicencia.UseVisualStyleBackColor = true;
            this.BtnBuscarLicencia.Click += new System.EventHandler(this.BtnBuscarLicencia_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ruta Documentos \r\nTemporales";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Archivo Licencia";
            // 
            // TbURL
            // 
            this.TbURL.Enabled = false;
            this.TbURL.Location = new System.Drawing.Point(118, 28);
            this.TbURL.Name = "TbURL";
            this.TbURL.Size = new System.Drawing.Size(370, 23);
            this.TbURL.TabIndex = 1;
            this.ToolTipConfiguracion.SetToolTip(this.TbURL, "Seleccione el archivo de licencia.\r\n\r\nRecuerde que si no tiene seleccionado un ar" +
        "chivo de licencia válido, \r\nno puede iniciar sesión en el Complemento de excel.");
            // 
            // ToolTipConfiguracion
            // 
            this.ToolTipConfiguracion.AutomaticDelay = 100;
            this.ToolTipConfiguracion.AutoPopDelay = 10000;
            this.ToolTipConfiguracion.InitialDelay = 100;
            this.ToolTipConfiguracion.IsBalloon = true;
            this.ToolTipConfiguracion.ReshowDelay = 20;
            this.ToolTipConfiguracion.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ToolTipConfiguracion.ToolTipTitle = "Configuración";
            // 
            // TbRutaTemporal
            // 
            this.TbRutaTemporal.Location = new System.Drawing.Point(118, 57);
            this.TbRutaTemporal.Name = "TbRutaTemporal";
            this.TbRutaTemporal.ReadOnly = true;
            this.TbRutaTemporal.Size = new System.Drawing.Size(451, 23);
            this.TbRutaTemporal.TabIndex = 3;
            this.ToolTipConfiguracion.SetToolTip(this.TbRutaTemporal, "Carpeta de ubicación de archivos\r\ntemporales del complemento.");
            // 
            // FrmConfigurarComplemento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(595, 149);
            this.Controls.Add(this.BtnGuardarCambios);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmConfigurarComplemento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuración del complemento para Office de SINCO ERP";
            this.Load += new System.EventHandler(this.FrmConfigurarComplemento_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnGuardarCambios;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnBuscarLicencia;
        //private Microsoft.VisualBasic.PowerPacks.RectangleShape RecColorDescriptorObligatorio;
        //private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private System.Windows.Forms.ToolTip ToolTipConfiguracion;
        private System.Windows.Forms.TextBox TbRutaTemporal;
        //private Microsoft.VisualBasic.PowerPacks.RectangleShape RecColorDescriptorOpcional;
    }
}