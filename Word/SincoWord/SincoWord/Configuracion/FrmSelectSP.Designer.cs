namespace SincoWord.Configuracion
{
    partial class FrmSelectSP
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
            this.CBModulo = new System.Windows.Forms.ComboBox();
            this.CBAplicacion = new System.Windows.Forms.ComboBox();
            this.BtnAceptar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CBModulo
            // 
            this.CBModulo.FormattingEnabled = true;
            this.CBModulo.Location = new System.Drawing.Point(91, 32);
            this.CBModulo.Name = "CBModulo";
            this.CBModulo.Size = new System.Drawing.Size(158, 21);
            this.CBModulo.TabIndex = 0;
            this.CBModulo.SelectedValueChanged += new System.EventHandler(this.CBModulo_SelectedValueChanged);
            // 
            // CBAplicacion
            // 
            this.CBAplicacion.FormattingEnabled = true;
            this.CBAplicacion.Location = new System.Drawing.Point(91, 93);
            this.CBAplicacion.Name = "CBAplicacion";
            this.CBAplicacion.Size = new System.Drawing.Size(158, 21);
            this.CBAplicacion.TabIndex = 1;
            this.CBAplicacion.SelectedValueChanged += new System.EventHandler(this.CBAplicacion_SelectedValueChanged);
            // 
            // BtnAceptar
            // 
            this.BtnAceptar.Location = new System.Drawing.Point(174, 150);
            this.BtnAceptar.Name = "BtnAceptar";
            this.BtnAceptar.Size = new System.Drawing.Size(75, 23);
            this.BtnAceptar.TabIndex = 2;
            this.BtnAceptar.Text = "Aceptar";
            this.BtnAceptar.UseVisualStyleBackColor = true;
            this.BtnAceptar.Click += new System.EventHandler(this.BtnAceptar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Modulo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Aplicación";
            // 
            // FrmSelectSP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 194);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnAceptar);
            this.Controls.Add(this.CBAplicacion);
            this.Controls.Add(this.CBModulo);
            this.Name = "FrmSelectSP";
            this.Text = "FrmSelectSP";
            this.Load += new System.EventHandler(this.FrmSelectSP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CBModulo;
        private System.Windows.Forms.ComboBox CBAplicacion;
        private System.Windows.Forms.Button BtnAceptar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}