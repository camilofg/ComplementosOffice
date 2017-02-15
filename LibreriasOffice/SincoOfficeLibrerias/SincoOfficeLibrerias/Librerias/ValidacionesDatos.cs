using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SincoOfficeLibrerias
{
   public class ValidacionesDatos
   {
      #region Constructor de propiedades
      public const string ValidarNumero = "Número";
      public const string ValidarFecha = "Fecha";
      public const string ValidarTexto = "Texto";
      public const string ValidarBool = "Booleano";
      public const string ValidarHora = "Hora";

      #endregion

      /// <summary>
      /// Devuelve el resultado de la validación de un texto.
      /// </summary>
      /// <param name="Texto"></param>
      /// <param name="TipoValidacion"></param>
      /// <returns>estado final de la operación</returns>
      public static bool ValidarInformacion(string Texto, string TipoValidacion)
      {
         try
         {
            bool Resultado = false;

            switch (TipoValidacion)
            {
               case ValidacionesDatos.ValidarNumero:
                  double Numero;
                  if (double.TryParse(Texto, out Numero))
                  { Resultado = true; }
                  else
                  { Resultado = false; }
                  break;
               case ValidacionesDatos.ValidarFecha:
                  try
                  {
                     DateTime fechaNumero = DateTime.FromOADate(double.Parse(Texto));
                     // 18264 -> 01/01/1095       *73415 -> 31/12/2100
                     if (double.Parse(Texto) > 18264 && double.Parse(Texto) < 73415)
                     {
                        Resultado = true;
                     }
                     else
                     {
                        Resultado = false;
                     }
                  }
                  catch
                  {
                     try
                     {
                        DateTime fecha = DateTime.Parse(Texto);
                        Resultado = true;
                     }
                     catch
                     {
                        Resultado = false;
                     }
                  }
                  break;
               case ValidacionesDatos.ValidarBool:
                  bool Boolean1;
                  if (bool.TryParse(Texto, out Boolean1))
                  { Resultado = true; }
                  else
                  { Resultado = false; }
                  break;
               case ValidacionesDatos.ValidarTexto:
                  if (Texto.Length > 0)
                  { Resultado = true; }
                  else
                  { Resultado = false; }
                  break;
               case ValidacionesDatos.ValidarHora:
                  Resultado = true;
                  try
                  {
                     //Formato: HH:MM:SS y Formato: HH:MM
                     Regex r = new Regex(@"(([0-1][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9])|(([0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9])|(([0-1][0-9]|2[0-3]):[0-5][0-9])|(([0-9]|2[0-3]):[0-5][0-9])");
                     Match m = r.Match(Texto);
                     Resultado = m.Success;

                     if (!Resultado)
                     {
                        //Formato: Número Excel
                        double Hora;
                        if (double.TryParse(Texto, out Hora))
                        {
                           if (Hora > 0 && Hora < 1)
                           {
                              Resultado = true;
                           }
                        }
                     }
                  }
                  catch
                  {
                     Resultado = false;
                  }

                  break;
               default:
                  Resultado = false;
                  break;
            }

            return Resultado;
         }
         catch
         {
            return false;
         }
      }

      #region Métodos utilizados para limpiar el nombre de un archivo, (compatibilidad con el servicio web de conversión de archivos a PDF y ZIP)
      public static string DarFormatoNombreArchivoSGC(string nombreDocumento)
      {
         string cadenaRetorno = string.Empty;

         // Le quita las tildes
         StringBuilder sb = new StringBuilder();

         nombreDocumento.Normalize(NormalizationForm.FormD).ToCharArray().ToList()
         .ForEach(caracter => sb.Append((CharUnicodeInfo.GetUnicodeCategory(caracter) != UnicodeCategory.NonSpacingMark) ? caracter.ToString() : ""));

         cadenaRetorno = (sb.ToString().Normalize(NormalizationForm.FormC));

         // Remplaza espacios y demas caracteres raros por barras bajas.
         cadenaRetorno = RemplazaCaracteres(cadenaRetorno);
         return cadenaRetorno;
      }

      private static string RemplazaCaracteres(string cadenaRetorno)
      {
         cadenaRetorno = cadenaRetorno.Replace(" ", "_");
         cadenaRetorno = cadenaRetorno.Replace("/", "_");
         cadenaRetorno = cadenaRetorno.Replace(":", "_");
         cadenaRetorno = cadenaRetorno.Replace(";", "_");
         cadenaRetorno = cadenaRetorno.Replace("?", "_");
         cadenaRetorno = cadenaRetorno.Replace("<", string.Empty);
         cadenaRetorno = cadenaRetorno.Replace(">", string.Empty);
         cadenaRetorno = cadenaRetorno.Replace(".", string.Empty);
         cadenaRetorno = cadenaRetorno.Replace(",", string.Empty);
         cadenaRetorno = cadenaRetorno.Replace("\"", string.Empty);
         cadenaRetorno = cadenaRetorno.Replace("-", string.Empty);
         cadenaRetorno = cadenaRetorno.Replace("–", string.Empty);
         cadenaRetorno = cadenaRetorno.Replace("—", string.Empty);
         cadenaRetorno = cadenaRetorno.Replace("\t", "_");
         return cadenaRetorno;
      }
      #endregion
   }
}
