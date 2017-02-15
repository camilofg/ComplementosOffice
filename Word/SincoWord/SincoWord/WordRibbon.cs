using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using AppSincoWord;
using SincoWord.Configuracion;

namespace SincoWord
{
   public partial class WordRibbon
   {
      private void WordRibbon_Load(object sender, RibbonUIEventArgs e)
      {

      }

      private void BtnLogin_Click(object sender, RibbonControlEventArgs e)
      {
         try
         {
            FrmLogin Frm = new FrmLogin();
            Frm.Show();
         }
         catch
         {

         }
      }

      private void button1_Click(object sender, RibbonControlEventArgs e)
      {
          FrmSelectSP FrmSelSP = new FrmSelectSP();
          FrmSelSP.Text = "Seleccionar Aplicacion";
          FrmSelSP.Show();
          //FrmTreeView FrmTree = new FrmTreeView();
          //FrmTree.Text = "Arbol de Variables";
          //FrmTree.TopMost = true;
          //FrmTree.Show();
      }
   }
}
