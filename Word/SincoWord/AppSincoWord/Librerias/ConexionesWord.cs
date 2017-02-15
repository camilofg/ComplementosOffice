using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace AppSincoWord
{
    public class Conexiones2
    {
        public Binding BindingConexion { get; set; }
        public EndpointAddress InterfaceWordArbolVariables { get; set; }

        /// <summary>
        /// Crea una nueva instancia de Conexiones, a partir de string con la dirección del servicio WsWordArbolVariables del cliente
        /// </summary>
        /// <param name="UrlServicioAppExterna">Url WsWordArbolVariables</param>
        public Conexiones(string UrlServicioWsWordArbolVariables)
        {
            WSHttpBinding wsHttpBinding = new WSHttpBinding();

            wsHttpBinding.CloseTimeout = TimeSpan.Parse("00:01:00");
            wsHttpBinding.OpenTimeout = TimeSpan.Parse("00:01:00" );
            wsHttpBinding.ReceiveTimeout = TimeSpan.Parse("00:10:00" );
            wsHttpBinding.SendTimeout = TimeSpan.Parse("00:01:00");
            wsHttpBinding.BypassProxyOnLocal = false;
            wsHttpBinding.TransactionFlow = false;
            wsHttpBinding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            wsHttpBinding.MaxBufferPoolSize = 524288;
            wsHttpBinding.MaxReceivedMessageSize = 65536;
            wsHttpBinding.MessageEncoding = WSMessageEncoding.Text;
            wsHttpBinding.TextEncoding = System.Text.Encoding.UTF8;
            wsHttpBinding.UseDefaultWebProxy = true;
            wsHttpBinding.AllowCookies = false;
            wsHttpBinding.Security.Mode = SecurityMode.None;
            wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            BindingConexion = wsHttpBinding;

            InterfaceWordArbolVariables = new EndpointAddress(UrlServicioWsWordArbolVariables);

        }

        /// <summary>
        /// Crea una nueva instancia de Conexiones, a partir de string con la dirección del servicio WsWordArbolVariables del cliente
        /// </summary>
        /// <param name="UrlServicioAppExterna">Url wsAppExternas</param>
        /// <param name="BindingCon">Objeto de Binding, se utiliza para cambiar el tipo de objeto de conexión y las condiciones de autenticación</param>
        public Conexiones(string UrlServicioWsWordArbolVariables, Binding BindingCon)
        {
            BindingConexion = BindingCon;

            InterfaceWordArbolVariables = new EndpointAddress(UrlServicioWsWordArbolVariables);
        }


    }
}
