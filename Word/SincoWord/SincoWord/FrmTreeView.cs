using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppSincoWord.WsArbolVariablesRef;
using AppSincoWord.Librerias;
using SincoWord.Classes;
using AppSincoWord;
using SincoOfficeLibrerias;
using AppExternas;

namespace SincoWord
{
    public partial class FrmTreeView : Form
    {
        public FrmTreeView()
        {
            InitializeComponent();
        }

        private void FrmTreeview_Load(object sender, EventArgs e)
        {
            ConexionesWord conexiones = Globals.ThisAddIn.DatosConexion;
            Conexiones conexionWsTree = Globals.ThisAddIn.ConexionTree;

            if (conexionWsTree == null)
            {
                MessageBox.Show("No tiene conexion con Sinco Comunicaciones");
                this.Close();
                return;
            }

            ClienteWSTree wsClient = new ClienteWSTree(conexionWsTree);


            //WordGrandPaNode nodote = wsClient.GetTree("[ADP].[CF_WSArbolContratos]");
            WordGrandPaNode nodote = wsClient.GetTree(Globals.ThisAddIn.TreeviewStoreProc);
            var nPadres = nodote.NodosPadres.ToList();
            List<WRootNodes> listwPN = new List<WRootNodes>();
            foreach (var item in nPadres)
            {
                if (item.CVSCodigo.Length > 0)
                {
                    if (item.CVSCodigo.Length == 2)
                    {
                        WRootNodes rNodes = new WRootNodes
                        {
                            CVSCodigo = item.CVSCodigo,
                            CVSDescripcion = item.CVSDescripcion
                        };

                        listwPN.Add(rNodes);
                    }
                    else
                    { 
                        WRootNodes rNodes = new WRootNodes
                            {
                                CVSCodigo = item.CVSCodigo,
                                CVSDescripcion = item.CVSDescripcion
                            };
                        int indexMatch = item.CVSCodigo.Length - 2;
                        var idToSearch = item.CVSCodigo.Substring(0, indexMatch);
                        var nodeParent = listwPN.Where(n => n.CVSCodigo == idToSearch);
                        var nodeP = nodeParent.Count() > 0 ? nodeParent.First() : null;
                        if (nodeP != null)
                        {
                            nodeP.NodosHijos.Add(rNodes);
                        }
                        else {
                            encontrado = false;
                            SearchDad(listwPN, rNodes, idToSearch);
                            encontrado = false;
                        }

                    }

                }
            }
            //Buscar nodos hijos
             var nhijos = nodote.NodosHijos.ToList();
             var listH = from h in nhijos
                         group h by new { CVSCodigo = h.CVSCodigo } into g
                         where g.Count() >= 1
                         select g.Key;

             foreach (var item in listH)
             {
                 var codToCompare = item.CVSCodigo;
                 var lHijos = from hijo in nhijos
                              where hijo.CVSCodigo == codToCompare
                              select new WRootNodes { 
                                   CVSCodigo = hijo.CVSCodigo, 
                                   CVSDescripcion = hijo.CVSDescripcion,
                                   Descripcion = hijo.Descripcion
                              };
                 List<WRootNodes> listaSons = lHijos.ToList();
                 sonFind = false;
                 SonSearchDad(listwPN, listaSons, codToCompare);
                 sonFind = false;
             }


            ArmTree(listwPN);
            treeView1.ShowNodeToolTips = true;
            treeView1.Scrollable = true;
        }

        bool encontrado = false;
        private void SearchDad(List<WRootNodes> listwPN, WRootNodes rNodes, string idToSearch)
        {
            foreach (var item in listwPN)
            {
                if (encontrado) 
                    break;
                if (item.CVSCodigo == idToSearch)
                {
                    encontrado = true;
                    item.NodosHijos.Add(rNodes);
                    break;
                }
                if(item.NodosHijos.Count > 0)
                    SearchDad(item.NodosHijos, rNodes, idToSearch);
            }
        }

        bool sonFind = false;
        private void SonSearchDad(List<WRootNodes> listwPN, List<WRootNodes> ListHijos, string idToSearch)
        {
            foreach (var item in listwPN)
            {
                if (sonFind)
                    break;
                if (item.CVSCodigo == idToSearch)
                {
                    sonFind = true;
                    foreach (var itemAux in item.NodosHijos)
                    {
                        ListHijos.Add(itemAux);
                    }
                    item.NodosHijos = ListHijos.OrderBy(o=>o.CVSDescripcion).ToList();
                    break;
                }
                if (item.NodosHijos.Count > 0)
                    SonSearchDad(item.NodosHijos, ListHijos, idToSearch);
            }
        }

        private void ArmTree(List<WRootNodes> superNodo)
        {
            foreach (var nodo in superNodo)
            {
                TreeNode nodoH = new TreeNode(); 
                nodoH.Text = nodo.CVSDescripcion;
                if (nodo.NodosHijos.Count != 0) 
                {
                    ArmSons(nodo, nodoH);
                }
                treeView1.Nodes.Add(nodoH);   
            }
        }

        
        private void ArmSons(WRootNodes NodoPap, TreeNode nodoAb)
        {
            foreach (var item in NodoPap.NodosHijos)
            {
                TreeNode nodoH3 = new TreeNode();
                nodoH3.Text = item.CVSDescripcion;
                try
                {
                    if (!string.IsNullOrEmpty(item.Descripcion)) { nodoH3.ToolTipText = item.Descripcion; }
                    nodoAb.Nodes.Add(nodoH3);
                }
                catch (Exception ex) { throw ex; }

                if (item.NodosHijos.Count > 0)
                {
                    TreeNode tal = nodoH3;
                    ArmSons(item, tal);
                }
            }
        }

        void treeView1_NodeMouseDoubleClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ToolTipText == "")
                return;

            var tagName = e.Node.Text;
            Microsoft.Office.Interop.Word.Document docum = Globals.ThisAddIn.Application.ActiveDocument;
            var etiqueta = docum.ContentControls.Add(Microsoft.Office.Interop.Word.WdContentControlType.wdContentControlRichText, Type.Missing);
            etiqueta.Tag = tagName;
            etiqueta.Title = tagName;
            etiqueta.SetPlaceholderText(null, null, '_' + tagName);
        }

    }
}
