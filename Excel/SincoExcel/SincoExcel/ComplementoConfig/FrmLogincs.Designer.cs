namespace SincoExcel
{
    partial class FrmLogincs
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
           this.pictureBox1 = new System.Windows.Forms.PictureBox();
           this.BtnIngresar = new System.Windows.Forms.Button();
           this.CbSucursales = new System.Windows.Forms.ComboBox();
           this.CbEmpresas = new System.Windows.Forms.ComboBox();
           this.label4 = new System.Windows.Forms.Label();
           this.label3 = new System.Windows.Forms.Label();
           this.TbContraseña = new System.Windows.Forms.TextBox();
           this.label2 = new System.Windows.Forms.Label();
           this.TbNombreUsuario = new System.Windows.Forms.TextBox();
           this.label1 = new System.Windows.Forms.Label();
           this.pictureBox2 = new System.Windows.Forms.PictureBox();
           this.ToolTipIniciarSesion = new System.Windows.Forms.ToolTip(this.components);
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
           this.SuspendLayout();
           // 
           // pictureBox1
           // 
           this.pictureBox1.Image = global::SincoExcel.Properties.Resources.logoSincoERP;
           this.pictureBox1.Location = new System.Drawing.Point(78, 12);
           this.pictureBox1.Name = "pictureBox1";
           this.pictureBox1.Size = new System.Drawing.Size(216, 47);
           this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
           this.pictureBox1.TabIndex = 0;
           this.pictureBox1.TabStop = false;
           // 
           // BtnIngresar
           // 
           this.BtnIngresar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.BtnIngresar.Location = new System.Drawing.Point(81, 200);
           this.BtnIngresar.Name = "BtnIngresar";
           this.BtnIngresar.Size = new System.Drawing.Size(213, 32);
           this.BtnIngresar.TabIndex = 18;
           this.BtnIngresar.Text = "Iniciar sesión";
           this.BtnIngresar.UseVisualStyleBackColor = true;
           this.BtnIngresar.Click += new System.EventHandler(this.BtnIngresar_Click);
           // 
           // CbSucursales
           // 
           this.CbSucursales.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
           this.CbSucursales.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
           this.CbSucursales.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
           this.CbSucursales.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.CbSucursales.FormattingEnabled = true;
           this.CbSucursales.Location = new System.Drawing.Point(81, 171);
           this.CbSucursales.Name = "CbSucursales";
           this.CbSucursales.Size = new System.Drawing.Size(213, 23);
           this.CbSucursales.TabIndex = 17;
           this.ToolTipIniciarSesion.SetToolTip(this.CbSucursales, "Seleccione la sucursal de la sesión.");
           // 
           // CbEmpresas
           // 
           this.CbEmpresas.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
           this.CbEmpresas.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
           this.CbEmpresas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
           this.CbEmpresas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.CbEmpresas.FormattingEnabled = true;
           this.CbEmpresas.Location = new System.Drawing.Point(81, 136);
           this.CbEmpresas.Name = "CbEmpresas";
           this.CbEmpresas.Size = new System.Drawing.Size(213, 23);
           this.CbEmpresas.TabIndex = 16;
           this.ToolTipIniciarSesion.SetToolTip(this.CbEmpresas, "Seleccione la empresa de sesión.");
           // 
           // label4
           // 
           this.label4.AutoSize = true;
           this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.label4.Location = new System.Drawing.Point(9, 174);
           this.label4.Name = "label4";
           this.label4.Size = new System.Drawing.Size(51, 15);
           this.label4.TabIndex = 15;
           this.label4.Text = "Sucursal";
           // 
           // label3
           // 
           this.label3.AutoSize = true;
           this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.label3.Location = new System.Drawing.Point(9, 139);
           this.label3.Name = "label3";
           this.label3.Size = new System.Drawing.Size(52, 15);
           this.label3.TabIndex = 14;
           this.label3.Text = "Empresa";
           // 
           // TbContraseña
           // 
           this.TbContraseña.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.TbContraseña.Location = new System.Drawing.Point(82, 101);
           this.TbContraseña.Name = "TbContraseña";
           this.TbContraseña.PasswordChar = '*';
           this.TbContraseña.Size = new System.Drawing.Size(212, 23);
           this.TbContraseña.TabIndex = 13;
           this.TbContraseña.UseSystemPasswordChar = true;
           // 
           // label2
           // 
           this.label2.AutoSize = true;
           this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.label2.Location = new System.Drawing.Point(9, 104);
           this.label2.Name = "label2";
           this.label2.Size = new System.Drawing.Size(67, 15);
           this.label2.TabIndex = 12;
           this.label2.Text = "Contraseña";
           // 
           // TbNombreUsuario
           // 
           this.TbNombreUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.TbNombreUsuario.Location = new System.Drawing.Point(81, 66);
           this.TbNombreUsuario.Name = "TbNombreUsuario";
           this.TbNombreUsuario.Size = new System.Drawing.Size(213, 23);
           this.TbNombreUsuario.TabIndex = 11;
           this.TbNombreUsuario.Leave += new System.EventHandler(this.TbNombreUsuario_Leave);
           // 
           // label1
           // 
           this.label1.AutoSize = true;
           this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.label1.Location = new System.Drawing.Point(9, 69);
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size(47, 15);
           this.label1.TabIndex = 10;
           this.label1.Text = "Usuario";
           // 
           // pictureBox2
           // 
           this.pictureBox2.Image = global::SincoExcel.Properties.Resources.ImgLogin;
           this.pictureBox2.Location = new System.Drawing.Point(7, 12);
           this.pictureBox2.Name = "pictureBox2";
           this.pictureBox2.Size = new System.Drawing.Size(48, 48);
           this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
           this.pictureBox2.TabIndex = 19;
           this.pictureBox2.TabStop = false;
           // 
           // ToolTipIniciarSesion
           // 
           this.ToolTipIniciarSesion.IsBalloon = true;
           this.ToolTipIniciarSesion.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
           this.ToolTipIniciarSesion.ToolTipTitle = "Login";
           // 
           // FrmLogincs
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.BackColor = System.Drawing.Color.White;
           this.ClientSize = new System.Drawing.Size(306, 238);
           this.Controls.Add(this.pictureBox2);
           this.Controls.Add(this.BtnIngresar);
           this.Controls.Add(this.CbSucursales);
           this.Controls.Add(this.CbEmpresas);
           this.Controls.Add(this.label4);
           this.Controls.Add(this.label3);
           this.Controls.Add(this.TbContraseña);
           this.Controls.Add(this.label2);
           this.Controls.Add(this.TbNombreUsuario);
           this.Controls.Add(this.label1);
           this.Controls.Add(this.pictureBox1);
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
           this.MaximizeBox = false;
           this.MinimizeBox = false;
           this.Name = "FrmLogincs";
           this.ShowIcon = false;
           this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
           this.Text = "Ingreso SINCO ERP";
           this.TopMost = true;
           this.Load += new System.EventHandler(this.FrmLogincs_Load);
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
           ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
           this.ResumeLayout(false);
           this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button BtnIngresar;
        private System.Windows.Forms.ComboBox CbSucursales;
        private System.Windows.Forms.ComboBox CbEmpresas;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TbContraseña;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TbNombreUsuario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolTip ToolTipIniciarSesion;
    }
}