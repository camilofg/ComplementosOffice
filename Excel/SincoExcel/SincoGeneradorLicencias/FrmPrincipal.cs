using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SincoGeneradorLicencias
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception Exc)
            {
                MessageBox.Show("Error en la aplicación:\n\n" + Exc.ToString(), "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gaurdarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TbKeylicencia.Text)
                    && !string.IsNullOrEmpty(TbNombreArchivo.Text)
                    && !string.IsNullOrEmpty(TbUbicacionArchivo.Text)
                    && DgvPropiedades.Rows.Count > 0)
                {
                    #region Agregar atributos iniciales
                    Licencias Licencia = new Licencias();
                    Licencia.Nombre = TbNombreArchivo.Text;
                    Licencia.Key = TbKeylicencia.Text;
                    Licencia.Propiedades = new Dictionary<string, string>();

                    foreach (DataGridViewRow Fila in DgvPropiedades.Rows)
                    {
                        if (Fila.Cells["Propiedad"].Value != null && Fila.Cells["Valor"].Value != null)
                        {
                            if (!string.IsNullOrEmpty(Fila.Cells["Propiedad"].Value.ToString())
                                && !string.IsNullOrEmpty(Fila.Cells["Valor"].Value.ToString()))
                            {
                                Licencia.Propiedades.Add(Fila.Cells["Propiedad"].Value.ToString(), Fila.Cells["Valor"].Value.ToString());
                            }
                        }
                    }
                    #endregion

                    Byte[] ArchivoLicencia = Licencias.CrearArchivoLicencia(Licencia);

                    #region Crear Archivo
                    string rutaArchivo = TbUbicacionArchivo.Text + "/" + TbNombreArchivo.Text;
                    MemoryStream objstreaminput = new MemoryStream();
                    FileStream objfilestream = new FileStream(rutaArchivo, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(ArchivoLicencia, 0, ArchivoLicencia.Length);
                    objfilestream.Close();
                    #endregion

                    MessageBox.Show("Licencia creada Correctamente", "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Por favor Complete todos los campos.", "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Exc)
            {
                MessageBox.Show("Error en la aplicación:\n\n" + Exc.ToString(), "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Dispose(true);
            }
            catch (Exception Exc)
            {
                MessageBox.Show("Error en la aplicación:\n\n" + Exc.ToString(), "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAbrirUbicacionArchivo_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog BuscarUbicacion = new FolderBrowserDialog();
                //BuscarUbicacion.Description = "Seleccione la ubicación";
                BuscarUbicacion.RootFolder = Environment.SpecialFolder.MyComputer;
                BuscarUbicacion.ShowNewFolderButton = true;

                DialogResult Respuesta = BuscarUbicacion.ShowDialog();

                if (Respuesta == System.Windows.Forms.DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(BuscarUbicacion.SelectedPath))
                    {
                        TbUbicacionArchivo.Text = BuscarUbicacion.SelectedPath;
                    }
                }
            }
            catch (Exception Exc)
            {
                MessageBox.Show("Error en la aplicación:\n\n" + Exc.ToString(), "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string GuidLicencia = Guid.NewGuid().ToString();
                TbKeylicencia.Text = GuidLicencia;
            }
            catch (Exception Exc)
            {
                MessageBox.Show("Error en la aplicación:\n\n" + Exc.ToString(), "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cargarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog RutaLicencia = new OpenFileDialog();
                RutaLicencia.CheckFileExists = true;
                RutaLicencia.CheckPathExists = true;
                RutaLicencia.Multiselect = false;

                DialogResult Respuesta = RutaLicencia.ShowDialog();

                if (Respuesta == System.Windows.Forms.DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(RutaLicencia.FileName))
                    {
                        #region Cargar Archivo de licencia

                        Byte[] ContenidoArchivo = new Byte[0];
                        string rutaArchivo = RutaLicencia.FileName;
                        FileStream objfilestreamRead = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
                        int len = (int)objfilestreamRead.Length;
                        ContenidoArchivo = new Byte[len];
                        objfilestreamRead.Read(ContenidoArchivo, 0, len);
                        objfilestreamRead.Close();

                        Licencias Licencia = Licencias.LeerLicencia(ContenidoArchivo);

                        if (!string.IsNullOrEmpty(Licencia.Nombre) && !string.IsNullOrEmpty(Licencia.Key))
                        {
                            #region Cargar propiedades de Licencia
                            TbKeylicencia.Text = Licencia.Key;
                            TbNombreArchivo.Text = Licencia.Nombre;
                            TbUbicacionArchivo.Text = RutaLicencia.FileName.Replace(Licencia.Nombre, "");

                            DataTable DatosPropiedades = new DataTable();
                            DatosPropiedades.Columns.Add("Propiedad", typeof(string));
                            DatosPropiedades.Columns.Add("Valor", typeof(string));

                            foreach (KeyValuePair<string, string> Dict in Licencia.Propiedades)
                            {
                                DatosPropiedades.Rows.Add(Dict.Key, Dict.Value);
                            }

                            DgvPropiedades.Rows.Clear();

                            DgvPropiedades.DataSource = DatosPropiedades;
                            #endregion

                            MessageBox.Show("Licencia Cargada Correctamente.", "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("El archivo de licencia no es válido.", "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception Exc)
            {
                MessageBox.Show("Error en la aplicación:\n\n" + Exc.ToString(), "Sinco ERP", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {

        }


        #region Rutas Servicios
        //SincoCalidad
        //http://svrhosting/SincoCalidad/ERPNET/Comunicaciones/ServiciosWeb/Seguridad/wsAutenticacion.asmx
        //http://svrhosting/SincoCalidad/ERPNET/GestionDeCalidad/Comunicaciones/ServiciosWeb/wsSGCdocumentos.asmx
        //http://svrhosting/SincoCalidad/ERPNET/SGD/Comunicaciones/wsOfficeSGD.asmx

        //Desarrollo
        //http://desarrollo/sincook/ERPNET/Comunicaciones/ServiciosWeb/Seguridad/wsAutenticacion.asmx
        //http://desarrollo/sincook/ERPNET/SGD/Comunicaciones/wsOfficeSGD.asmx
        //http://desarrollo/sincook/ERPNET/GestionDeCalidad/Comunicaciones/ServiciosWeb/wsSGCdocumentos.asmx

        //LocalHost
        //http://localhost/ERPNET/Comunicaciones/ServiciosWeb/Seguridad/wsAutenticacion.asmx
        //http://localhost/ERPNET/SGD/Comunicaciones/wsOfficeSGD.asmx
        //http://localhost/ERPNET/GestionDeCalidad/Comunicaciones/ServiciosWeb/wsSGCdocumentos.asmx

        #endregion

    }
}
