using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace SincoOfficeLibrerias
{
    /// <summary>
    /// Clase encargada de efectuar los procedimientos de Impersonation para acceso a archivos y operaciones especiales
    /// </summary>
    public class Impersonation
    {
        #region Atributos y propiedades

        /// <summary>
        /// Contexto de impersonation. Clase base para invocar los métodos necesarios para la ejecución de procedimientos
        /// </summary>
        WindowsImpersonationContext m_impersonationContext;

        #endregion

        #region WIN32 definitions

        /// <summary>
        /// Constante de Win32
        /// </summary>
        public const int LOGON32_LOGON_INTERACTIVE = 2;

        /// <summary>
        /// Constante de Win32
        /// </summary>
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
        String lpszDomain,
        String lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        #endregion

        #region Métodos

        /// <summary>
        /// Constructor de clase
        /// </summary>
        public Impersonation()
        {

        }

        /// <summary>
        /// Procedimiento que ejecuta el 'impersonate' de un usuario entregado en los argumentos del método
        /// </summary>
        /// <param name="userName">Nombre del usuario reconocido en la máquina o directorio activo</param>
        /// <param name="domain">Dominio del equipo o de segmento de red</param>
        /// <param name="password">Clave de acceso del usuario para ese dominio</param>
        /// <returns>Especifica si la ejecución del procedimiento fue exitoso</returns>
        public bool ImpersonateUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        m_impersonationContext = tempWindowsIdentity.Impersonate();
                        if (m_impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        /// <summary>
        /// Procedimiento que se encarga de deshacer el 'impersonate' para el usuario.
        /// Se puede comparar con un cierre de sesión para una autenticación abierta previamente.
        /// </summary>
        public void UnImpersonation()
        {
            m_impersonationContext.Undo();
        }

        #endregion

    }
}
