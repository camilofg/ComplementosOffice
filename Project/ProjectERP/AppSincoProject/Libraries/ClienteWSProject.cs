﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppExternas;


namespace AppSincoProject.Libraries
{
    public class ClienteWSProject : WsProjectERPref.WSProjectERPtest
    {
        public ClienteWSProject(Conexiones conexion)
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
