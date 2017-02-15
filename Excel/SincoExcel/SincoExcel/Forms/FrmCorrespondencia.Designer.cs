namespace SincoExcel.Forms
{
    partial class FrmCorrespondencia
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DgvActividadResponsables = new System.Windows.Forms.DataGridView();
            this.BtnEnviar = new System.Windows.Forms.Button();
            this.Codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Actividad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Responsable = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvActividadResponsables)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DgvActividadResponsables);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(497, 260);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Responsables";
            // 
            // DgvActividadResponsables
            // 
            this.DgvActividadResponsables.AllowUserToAddRows = false;
            this.DgvActividadResponsables.AllowUserToDeleteRows = false;
            this.DgvActividadResponsables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DgvActividadResponsables.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvActividadResponsables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvActividadResponsables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Codigo,
            this.Actividad,
            this.Responsable});
            this.DgvActividadResponsables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvActividadResponsables.Location = new System.Drawing.Point(3, 19);
            this.DgvActividadResponsables.Name = "DgvActividadResponsables";
            this.DgvActividadResponsables.Size = new System.Drawing.Size(491, 238);
            this.DgvActividadResponsables.TabIndex = 4;
            this.DgvActividadResponsables.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DgvActividadResponsables_DataBindingComplete);
            this.DgvActividadResponsables.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DgvActividadResponsables_RowPostPaint);
            this.DgvActividadResponsables.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DgvActividadResponsables_RowsAdded);
            // 
            // BtnEnviar
            // 
            this.BtnEnviar.Location = new System.Drawing.Point(409, 275);
            this.BtnEnviar.Name = "BtnEnviar";
            this.BtnEnviar.Size = new System.Drawing.Size(97, 23);
            this.BtnEnviar.TabIndex = 1;
            this.BtnEnviar.Text = "Enviar";
            this.BtnEnviar.UseVisualStyleBackColor = true;
            this.BtnEnviar.Click += new System.EventHandler(this.BtnEnviar_Click);
            // 
            // Codigo
            // 
            this.Codigo.DataPropertyName = "Codigo";
            this.Codigo.HeaderText = "Codigo";
            this.Codigo.Name = "Codigo";
            this.Codigo.ReadOnly = true;
            this.Codigo.Width = 71;
            // 
            // Actividad
            // 
            this.Actividad.DataPropertyName = "Actividad";
            this.Actividad.HeaderText = "Actividad";
            this.Actividad.Name = "Actividad";
            this.Actividad.ReadOnly = true;
            this.Actividad.Width = 82;
            // 
            // Responsable
            // 
            this.Responsable.HeaderText = "Responsable";
            this.Responsable.Name = "Responsable";
            this.Responsable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Responsable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Responsable.Width = 98;
            // 
            // FrmCorrespondencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(521, 305);
            this.Controls.Add(this.BtnEnviar);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmCorrespondencia";
            this.Text = "Configuración de Correspondencia";
            this.Load += new System.EventHandler(this.FrmCorrespondencia_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvActividadResponsables)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView DgvActividadResponsables;
        private System.Windows.Forms.Button BtnEnviar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Actividad;
        private System.Windows.Forms.DataGridViewComboBoxColumn Responsable;

    }
}