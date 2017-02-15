using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppSincoWord.WsArbolVariablesRef;

namespace SincoWord.Classes
{
    public class WRootNodes : WordParentNode
    {
        private List<WRootNodes> nodosHijos = new List<WRootNodes>();

        public List<WRootNodes> NodosHijos
        {
            get { return nodosHijos; }
            set { nodosHijos = value; }
        }

        private string _Descripcion;

        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
        
    }
}
