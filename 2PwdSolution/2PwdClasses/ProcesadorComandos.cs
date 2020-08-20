#region Header

using System;
using PC = _2PwdClasses.ProcesadorComandos;

namespace _2PwdClasses
{

#endregion

    public static class ProcesadorComandos
    {
        #region Propiedades

        public static bool HayError;
        public static string MensajeError;

        #endregion

        #region Metodos

        static ProcesadorComandos()
        {
            HayError = false;
            MensajeError = "";
        }

        public static void InitMetodo()
        {
            PC.HayError = false;
            PC.MensajeError = string.Empty;
        }


        public static string Run(string comando)
        {
            PC.InitMetodo();

            if(comando == null)
            {
                PC.HayError = false;
                PC.MensajeError = $"comando nulo, en {nameof(ProcesadorComandos)}.{nameof(Run)}!";
                return "";
            }
            var result = "";
            return result;
        }

        #endregion
    }

#region Footer
}
#endregion
