using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppExternas;


namespace AppSincoWord.Librerias
{
    public class ClienteWSTree : WsArbolVariablesRef.WsWordArbolVariables
    {
        public ClienteWSTree(Conexiones conexion)
        {
            try
            {
                this.Timeout = int.Parse(conexion.BindingConexion.CloseTimeout.TotalMilliseconds.ToString());
                this.Url = conexion.InterfaceAppExterna.Uri.OriginalString;
            }
            catch
            {

            }
        }

        public void Close()
        {
            this.Abort();
            this.Dispose(true);
        }
    }
}
