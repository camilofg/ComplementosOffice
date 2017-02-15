namespace SincoExcel.Forms
{
    partial class FrmFormatos
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
           this.tabControl1 = new System.Windows.Forms.TabControl();
           this.TabCrearFormato = new System.Windows.Forms.TabPage();
           this.BtnValidarRequisitosPlantillas = new System.Windows.Forms.Button();
           this.BtnCargarFormatoVerificacion = new System.Windows.Forms.Button();
           this.CbSubProcesoCrearFormato = new System.Windows.Forms.ComboBox();
           this.label4 = new System.Windows.Forms.Label();
           this.CbSubserieFormato = new System.Windows.Forms.ComboBox();
           this.label3 = new System.Windows.Forms.Label();
           this.BtnCrearFormato = new System.Windows.Forms.Button();
           this.groupBox1 = new System.Windows.Forms.GroupBox();
           this.DgvInformacionFormato = new System.Windows.Forms.DataGridView();
           this.Variable = new System.Windows.Forms.DataGridViewTextBoxColumn();
           this.Contenido = new System.Windows.Forms.DataGridViewTextBoxColumn();
           this.CbFormato = new System.Windows.Forms.ComboBox();
           this.label1 = new System.Windows.Forms.Label();
           this.TabCompletarFormato = new System.Windows.Forms.TabPage();
           this.CbSubProcesoCompletar = new System.Windows.Forms.ComboBox();
           this.label5 = new System.Windows.Forms.Label();
           this.BtnConsultarFormato = new System.Windows.Forms.Button();
           this.groupBox2 = new System.Windows.Forms.GroupBox();
           this.DgvInfoFormatoConsulta = new System.Windows.Forms.DataGridView();
           this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
           this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
           this.CbFormatosConsulta = new System.Windows.Forms.ComboBox();
           this.label2 = new System.Windows.Forms.Label();
           this.ToolTipFormatos = new System.Windows.Forms.ToolTip(this.components);
           this.tabControl1.SuspendLayout();
           this.TabCrearFormato.SuspendLayout();
           this.groupBox1.SuspendLayout();
           ((System.ComponentModel.ISupportInitialize)(this.DgvInformacionFormato)).BeginInit();
           this.TabCompletarFormato.SuspendLayout();
           this.groupBox2.SuspendLayout();
           ((System.ComponentModel.ISupportInitialize)(this.DgvInfoFormatoConsulta)).BeginInit();
           this.SuspendLayout();
           // 
           // tabControl1
           // 
           this.tabControl1.Controls.Add(this.TabCrearFormato);
           this.tabControl1.Controls.Add(this.TabCompletarFormato);
           this.tabControl1.Location = new System.Drawing.Point(3, 5);
           this.tabControl1.Name = "tabControl1";
           this.tabControl1.Padding = new System.Drawing.Point(7, 7);
           this.tabControl1.SelectedIndex = 0;
           this.tabControl1.ShowToolTips = true;
           this.tabControl1.Size = new System.Drawing.Size(306, 331);
           this.tabControl1.TabIndex = 0;
           // 
           // TabCrearFormato
           // 
           this.TabCrearFormato.Controls.Add(this.BtnValidarRequisitosPlantillas);
           this.TabCrearFormato.Controls.Add(this.BtnCargarFormatoVerificacion);
           this.TabCrearFormato.Controls.Add(this.CbSubProcesoCrearFormato);
           this.TabCrearFormato.Controls.Add(this.label4);
           this.TabCrearFormato.Controls.Add(this.CbSubserieFormato);
           this.TabCrearFormato.Controls.Add(this.label3);
           this.TabCrearFormato.Controls.Add(this.BtnCrearFormato);
           this.TabCrearFormato.Controls.Add(this.groupBox1);
           this.TabCrearFormato.Controls.Add(this.CbFormato);
           this.TabCrearFormato.Controls.Add(this.label1);
           this.TabCrearFormato.Location = new System.Drawing.Point(4, 32);
           this.TabCrearFormato.Name = "TabCrearFormato";
           this.TabCrearFormato.Padding = new System.Windows.Forms.Padding(3);
           this.TabCrearFormato.Size = new System.Drawing.Size(298, 295);
           this.TabCrearFormato.TabIndex = 0;
           this.TabCrearFormato.Text = "Crear Formato";
           this.TabCrearFormato.ToolTipText = "Permite asignar o modificar un  formato al sistema de gestión  de calidad.";
           this.TabCrearFormato.UseVisualStyleBackColor = true;
           // 
           // BtnValidarRequisitosPlantillas
           // 
           this.BtnValidarRequisitosPlantillas.Location = new System.Drawing.Point(90, 265);
           this.BtnValidarRequisitosPlantillas.Name = "BtnValidarRequisitosPlantillas";
           this.BtnValidarRequisitosPlantillas.Size = new System.Drawing.Size(79, 27);
           this.BtnValidarRequisitosPlantillas.TabIndex = 8;
           this.BtnValidarRequisitosPlantillas.Text = "Validar";
           this.ToolTipFormatos.SetToolTip(this.BtnValidarRequisitosPlantillas, "En modo de Vista preliminar, comprueba las \r\nvalidaciones que son requeridas para" +
                   " guardar el formato.\r\n\r\nRecomendación: Utilice esta opción antes de aprobar el f" +
                   "ormato.");
           this.BtnValidarRequisitosPlantillas.UseVisualStyleBackColor = true;
           this.BtnValidarRequisitosPlantillas.Visible = false;
           this.BtnValidarRequisitosPlantillas.Click += new System.EventHandler(this.BtnValidarRequisitosPlantillas_Click);
           // 
           // BtnCargarFormatoVerificacion
           // 
           this.BtnCargarFormatoVerificacion.Enabled = false;
           this.BtnCargarFormatoVerificacion.Location = new System.Drawing.Point(6, 265);
           this.BtnCargarFormatoVerificacion.Name = "BtnCargarFormatoVerificacion";
           this.BtnCargarFormatoVerificacion.Size = new System.Drawing.Size(78, 27);
           this.BtnCargarFormatoVerificacion.TabIndex = 7;
           this.BtnCargarFormatoVerificacion.Text = "Vista Previa";
           this.ToolTipFormatos.SetToolTip(this.BtnCargarFormatoVerificacion, "Permite obtener una vista preliminar del formato.\r\n\r\nPuede revisar la información" +
                   " de  los descriptores asociados a una fuente\r\ny las validaciones de datos de cad" +
                   "a descriptor.");
           this.BtnCargarFormatoVerificacion.UseVisualStyleBackColor = true;
           this.BtnCargarFormatoVerificacion.Click += new System.EventHandler(this.BtnCargarFormatoVerificacion_Click_1);
           // 
           // CbSubProcesoCrearFormato
           // 
           this.CbSubProcesoCrearFormato.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
           this.CbSubProcesoCrearFormato.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
           this.CbSubProcesoCrearFormato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
           this.CbSubProcesoCrearFormato.FormattingEnabled = true;
           this.CbSubProcesoCrearFormato.Location = new System.Drawing.Point(60, 35);
           this.CbSubProcesoCrearFormato.Name = "CbSubProcesoCrearFormato";
           this.CbSubProcesoCrearFormato.Size = new System.Drawing.Size(235, 23);
           this.CbSubProcesoCrearFormato.TabIndex = 6;
           this.CbSubProcesoCrearFormato.SelectedIndexChanged += new System.EventHandler(this.CbSubProcesoCrearFormato_SelectedIndexChanged);
           // 
           // label4
           // 
           this.label4.AutoSize = true;
           this.label4.Location = new System.Drawing.Point(3, 38);
           this.label4.Name = "label4";
           this.label4.Size = new System.Drawing.Size(54, 15);
           this.label4.TabIndex = 5;
           this.label4.Text = "Subproc.";
           this.ToolTipFormatos.SetToolTip(this.label4, "Subproceso\r\n\r\nSeleccione el subproceso del Sistema de Gestión de Calidad \r\nque ti" +
                   "ene registrado el formato.");
           // 
           // CbSubserieFormato
           // 
           this.CbSubserieFormato.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
           this.CbSubserieFormato.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
           this.CbSubserieFormato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
           this.CbSubserieFormato.FormattingEnabled = true;
           this.CbSubserieFormato.Location = new System.Drawing.Point(60, 6);
           this.CbSubserieFormato.Name = "CbSubserieFormato";
           this.CbSubserieFormato.Size = new System.Drawing.Size(235, 23);
           this.CbSubserieFormato.TabIndex = 1;
           // 
           // label3
           // 
           this.label3.AutoSize = true;
           this.label3.Location = new System.Drawing.Point(3, 9);
           this.label3.Name = "label3";
           this.label3.Size = new System.Drawing.Size(51, 15);
           this.label3.TabIndex = 4;
           this.label3.Text = "Subserie";
           this.ToolTipFormatos.SetToolTip(this.label3, "Subserie\r\n\r\nSeleccione la subserie del Sistema de Gestión Documental\r\nque estará " +
                   "asociada al formato.");
           // 
           // BtnCrearFormato
           // 
           this.BtnCrearFormato.Location = new System.Drawing.Point(184, 265);
           this.BtnCrearFormato.Name = "BtnCrearFormato";
           this.BtnCrearFormato.Size = new System.Drawing.Size(103, 27);
           this.BtnCrearFormato.TabIndex = 4;
           this.BtnCrearFormato.Text = "Crear";
           this.BtnCrearFormato.UseVisualStyleBackColor = true;
           this.BtnCrearFormato.Click += new System.EventHandler(this.BtnCrearFormato_Click);
           // 
           // groupBox1
           // 
           this.groupBox1.Controls.Add(this.DgvInformacionFormato);
           this.groupBox1.Location = new System.Drawing.Point(3, 93);
           this.groupBox1.Name = "groupBox1";
           this.groupBox1.Size = new System.Drawing.Size(287, 166);
           this.groupBox1.TabIndex = 2;
           this.groupBox1.TabStop = false;
           this.groupBox1.Text = "Información de Formato";
           // 
           // DgvInformacionFormato
           // 
           this.DgvInformacionFormato.AllowUserToAddRows = false;
           this.DgvInformacionFormato.AllowUserToDeleteRows = false;
           this.DgvInformacionFormato.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
           this.DgvInformacionFormato.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
           this.DgvInformacionFormato.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
           this.DgvInformacionFormato.ColumnHeadersVisible = false;
           this.DgvInformacionFormato.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Variable,
            this.Contenido});
           this.DgvInformacionFormato.Dock = System.Windows.Forms.DockStyle.Fill;
           this.DgvInformacionFormato.Location = new System.Drawing.Point(3, 19);
           this.DgvInformacionFormato.Name = "DgvInformacionFormato";
           this.DgvInformacionFormato.Size = new System.Drawing.Size(281, 144);
           this.DgvInformacionFormato.TabIndex = 3;
           // 
           // Variable
           // 
           this.Variable.HeaderText = "Variable";
           this.Variable.Name = "Variable";
           this.Variable.ReadOnly = true;
           this.Variable.Width = 5;
           // 
           // Contenido
           // 
           this.Contenido.HeaderText = "Contenido";
           this.Contenido.Name = "Contenido";
           this.Contenido.ReadOnly = true;
           this.Contenido.Width = 5;
           // 
           // CbFormato
           // 
           this.CbFormato.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
           this.CbFormato.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
           this.CbFormato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
           this.CbFormato.FormattingEnabled = true;
           this.CbFormato.Location = new System.Drawing.Point(60, 64);
           this.CbFormato.Name = "CbFormato";
           this.CbFormato.Size = new System.Drawing.Size(235, 23);
           this.CbFormato.TabIndex = 2;
           this.CbFormato.SelectedIndexChanged += new System.EventHandler(this.CbFormato_SelectedIndexChanged);
           // 
           // label1
           // 
           this.label1.AutoSize = true;
           this.label1.Location = new System.Drawing.Point(3, 67);
           this.label1.Name = "label1";
           this.label1.Size = new System.Drawing.Size(52, 15);
           this.label1.TabIndex = 0;
           this.label1.Text = "Formato";
           // 
           // TabCompletarFormato
           // 
           this.TabCompletarFormato.Controls.Add(this.CbSubProcesoCompletar);
           this.TabCompletarFormato.Controls.Add(this.label5);
           this.TabCompletarFormato.Controls.Add(this.BtnConsultarFormato);
           this.TabCompletarFormato.Controls.Add(this.groupBox2);
           this.TabCompletarFormato.Controls.Add(this.CbFormatosConsulta);
           this.TabCompletarFormato.Controls.Add(this.label2);
           this.TabCompletarFormato.Location = new System.Drawing.Point(4, 32);
           this.TabCompletarFormato.Name = "TabCompletarFormato";
           this.TabCompletarFormato.Size = new System.Drawing.Size(298, 295);
           this.TabCompletarFormato.TabIndex = 1;
           this.TabCompletarFormato.Text = "Crear Registro";
           this.TabCompletarFormato.ToolTipText = "Permite diligenciar un formato del sistema de gestión de calidad, que se encuentr" +
               "e en estado Vigente.";
           this.TabCompletarFormato.UseVisualStyleBackColor = true;
           // 
           // CbSubProcesoCompletar
           // 
           this.CbSubProcesoCompletar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
           this.CbSubProcesoCompletar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
           this.CbSubProcesoCompletar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
           this.CbSubProcesoCompletar.FormattingEnabled = true;
           this.CbSubProcesoCompletar.Location = new System.Drawing.Point(60, 3);
           this.CbSubProcesoCompletar.Name = "CbSubProcesoCompletar";
           this.CbSubProcesoCompletar.Size = new System.Drawing.Size(235, 23);
           this.CbSubProcesoCompletar.TabIndex = 9;
           this.CbSubProcesoCompletar.SelectedIndexChanged += new System.EventHandler(this.CbSubProcesoCompletar_SelectedIndexChanged);
           // 
           // label5
           // 
           this.label5.AutoSize = true;
           this.label5.Location = new System.Drawing.Point(3, 6);
           this.label5.Name = "label5";
           this.label5.Size = new System.Drawing.Size(54, 15);
           this.label5.TabIndex = 8;
           this.label5.Text = "Subproc.";
           this.ToolTipFormatos.SetToolTip(this.label5, "Subproceso\r\n\r\nSeleccione el subproceso del Sistema de Gestión de Calidad \r\nque ti" +
                   "ene registrado el formato.\r\n\r\nRecuerde:\r\n\r\nSolo se muestran los formatos que est" +
                   "én aprobados.");
           // 
           // BtnConsultarFormato
           // 
           this.BtnConsultarFormato.Location = new System.Drawing.Point(187, 263);
           this.BtnConsultarFormato.Name = "BtnConsultarFormato";
           this.BtnConsultarFormato.Size = new System.Drawing.Size(103, 27);
           this.BtnConsultarFormato.TabIndex = 7;
           this.BtnConsultarFormato.Text = "Abrir";
           this.BtnConsultarFormato.UseVisualStyleBackColor = true;
           this.BtnConsultarFormato.Click += new System.EventHandler(this.BtnConsultarFormato_Click);
           // 
           // groupBox2
           // 
           this.groupBox2.Controls.Add(this.DgvInfoFormatoConsulta);
           this.groupBox2.Location = new System.Drawing.Point(6, 61);
           this.groupBox2.Name = "groupBox2";
           this.groupBox2.Size = new System.Drawing.Size(287, 196);
           this.groupBox2.TabIndex = 4;
           this.groupBox2.TabStop = false;
           this.groupBox2.Text = "Información de Formato";
           // 
           // DgvInfoFormatoConsulta
           // 
           this.DgvInfoFormatoConsulta.AllowUserToAddRows = false;
           this.DgvInfoFormatoConsulta.AllowUserToDeleteRows = false;
           this.DgvInfoFormatoConsulta.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
           this.DgvInfoFormatoConsulta.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
           this.DgvInfoFormatoConsulta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
           this.DgvInfoFormatoConsulta.ColumnHeadersVisible = false;
           this.DgvInfoFormatoConsulta.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
           this.DgvInfoFormatoConsulta.Dock = System.Windows.Forms.DockStyle.Fill;
           this.DgvInfoFormatoConsulta.Location = new System.Drawing.Point(3, 19);
           this.DgvInfoFormatoConsulta.Name = "DgvInfoFormatoConsulta";
           this.DgvInfoFormatoConsulta.Size = new System.Drawing.Size(281, 174);
           this.DgvInfoFormatoConsulta.TabIndex = 6;
           // 
           // dataGridViewTextBoxColumn1
           // 
           this.dataGridViewTextBoxColumn1.HeaderText = "Variable";
           this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
           this.dataGridViewTextBoxColumn1.ReadOnly = true;
           this.dataGridViewTextBoxColumn1.Width = 5;
           // 
           // dataGridViewTextBoxColumn2
           // 
           this.dataGridViewTextBoxColumn2.HeaderText = "Contenido";
           this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
           this.dataGridViewTextBoxColumn2.ReadOnly = true;
           this.dataGridViewTextBoxColumn2.Width = 5;
           // 
           // CbFormatosConsulta
           // 
           this.CbFormatosConsulta.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
           this.CbFormatosConsulta.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
           this.CbFormatosConsulta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
           this.CbFormatosConsulta.FormattingEnabled = true;
           this.CbFormatosConsulta.Location = new System.Drawing.Point(60, 32);
           this.CbFormatosConsulta.Name = "CbFormatosConsulta";
           this.CbFormatosConsulta.Size = new System.Drawing.Size(235, 23);
           this.CbFormatosConsulta.TabIndex = 5;
           this.CbFormatosConsulta.SelectedIndexChanged += new System.EventHandler(this.CbFormatosConsulta_SelectedIndexChanged);
           // 
           // label2
           // 
           this.label2.AutoSize = true;
           this.label2.Location = new System.Drawing.Point(3, 35);
           this.label2.Name = "label2";
           this.label2.Size = new System.Drawing.Size(52, 15);
           this.label2.TabIndex = 2;
           this.label2.Text = "Formato";
           // 
           // ToolTipFormatos
           // 
           this.ToolTipFormatos.AutomaticDelay = 200;
           this.ToolTipFormatos.AutoPopDelay = 20000;
           this.ToolTipFormatos.InitialDelay = 200;
           this.ToolTipFormatos.IsBalloon = true;
           this.ToolTipFormatos.ReshowDelay = 40;
           this.ToolTipFormatos.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
           this.ToolTipFormatos.ToolTipTitle = "Formatos";
           // 
           // FrmFormatos
           // 
           this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
           this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           this.BackColor = System.Drawing.Color.White;
           this.ClientSize = new System.Drawing.Size(309, 339);
           this.Controls.Add(this.tabControl1);
           this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
           this.MaximizeBox = false;
           this.MinimizeBox = false;
           this.Name = "FrmFormatos";
           this.ShowIcon = false;
           this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
           this.Text = "Formatos";
           this.TopMost = true;
           this.Load += new System.EventHandler(this.FrmFormatos_Load);
           this.tabControl1.ResumeLayout(false);
           this.TabCrearFormato.ResumeLayout(false);
           this.TabCrearFormato.PerformLayout();
           this.groupBox1.ResumeLayout(false);
           ((System.ComponentModel.ISupportInitialize)(this.DgvInformacionFormato)).EndInit();
           this.TabCompletarFormato.ResumeLayout(false);
           this.TabCompletarFormato.PerformLayout();
           this.groupBox2.ResumeLayout(false);
           ((System.ComponentModel.ISupportInitialize)(this.DgvInfoFormatoConsulta)).EndInit();
           this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabCrearFormato;
        private System.Windows.Forms.ComboBox CbFormato;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView DgvInformacionFormato;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contenido;
        private System.Windows.Forms.Button BtnCrearFormato;
        private System.Windows.Forms.TabPage TabCompletarFormato;
        private System.Windows.Forms.Button BtnConsultarFormato;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView DgvInfoFormatoConsulta;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ComboBox CbFormatosConsulta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CbSubserieFormato;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CbSubProcesoCrearFormato;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CbSubProcesoCompletar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtnCargarFormatoVerificacion;
        private System.Windows.Forms.Button BtnValidarRequisitosPlantillas;
        private System.Windows.Forms.ToolTip ToolTipFormatos;
    }
}