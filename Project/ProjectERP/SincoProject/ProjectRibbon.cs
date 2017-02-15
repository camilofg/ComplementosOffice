using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using SincoProject.Classes;
using Microsoft.Office.Interop.MSProject;
using System.Windows.Forms;

namespace SincoProject
{
    public partial class ProjectRibbon
    {
        private void ProjectRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            //Globals.ThisAddIn.Application.ProjectBeforeTaskChange +=new _EProjectApp2_ProjectBeforeTaskChangeEventHandler(Application_ProjectBeforeTaskChange);
        }

        private void BtnLogin_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                FrmLogin frm = new FrmLogin();
                frm.Show();
            }
            catch
            {

            }
        }

        private void BtnLoad_Click(object sender, RibbonControlEventArgs e)
        {
            FrmSelectProject frmSelect = new FrmSelectProject();
            
            frmSelect.Show();
            //var Nodes = gb.Get();
            #region DemoCollection
            ////List<GanttNode> Nodes = new List<GanttNode>
            ////{
            ////    new GanttNode{ Id=1, ItemName="Tarea 1", BeginDate=DateTime.Now, EndDate=DateTime.Now.AddDays(5),  Resources="1", PercentAdvanced=25d },
            ////    new GanttNode{ Id=2,ItemName="Tarea 2", BeginDate=DateTime.Now.AddDays(10), EndDate=  DateTime.Now.AddDays(15), Resources="2", PercentAdvanced=50d},
            ////    new GanttNode{ Id=3,ItemName="Tarea 3", BeginDate=DateTime.Now.AddDays(15), EndDate=DateTime.Now.AddDays(20), Resources="3", PercentAdvanced=75d },
            ////    new GanttNode{ Id=4,ItemName="Tarea 4", BeginDate=DateTime.Now.AddDays(20), EndDate=DateTime.Now.AddDays(25), Resources="1", PercentAdvanced=2d },
            ////    new GanttNode{ Id=5,ItemName="Tarea 5", BeginDate=DateTime.Now.AddDays(25), EndDate=DateTime.Now.AddDays(30), Resources="1", PercentAdvanced=10d },
            ////    new GanttNode{ Id=6,ItemName="Tarea 6", BeginDate=DateTime.Now.AddDays(30), EndDate=DateTime.Now.AddDays(40), Resources="2", PercentAdvanced=40d },
            ////    new GanttNode{ Id=7,ItemName="Tarea 7", BeginDate=DateTime.Now.AddDays(40), EndDate=DateTime.Now.AddDays(50), Resources="1", PercentAdvanced=50d },
            ////    new GanttNode{ Id=8,ItemName="Tarea 8", BeginDate=DateTime.Now.AddDays(50), EndDate=DateTime.Now.AddDays(60), Resources="2", PercentAdvanced=60d },
            ////    new GanttNode{ Id=9,ItemName="Tarea 9", BeginDate=DateTime.Now.AddDays(60), EndDate=DateTime.Now.AddDays(70), Resources="2", PercentAdvanced=70d },
            ////    new GanttNode{ Id=10,ItemName="Tarea 10", BeginDate=DateTime.Now.AddDays(70), EndDate=DateTime.Now.AddDays(80), Resources="1", PercentAdvanced=80d },
            ////    new GanttNode{ Id=11,ItemName="Tarea 11", BeginDate=DateTime.Now.AddDays(80), EndDate=DateTime.Now.AddDays(90), Resources="3", PercentAdvanced=90d }
            ////}; 
            #endregion
            //GanttHelper.BuildProject(Nodes);
            
        }

        private void BtnSave_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                var nodesCount = GanttHelper.GetGanttNodes();
                if (nodesCount.Count > 0)
                {
                    if (nodesCount.Count == 1)
                        MessageBox.Show("Se guardaron " + nodesCount[0].ToString() + " Tareas");

                    else
                        MessageBox.Show("Se guardaron " + nodesCount[1].ToString() + " tareas y " + nodesCount[0].ToString() + " excepciones");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + ex.InnerException);
            }
        }
    }
}
