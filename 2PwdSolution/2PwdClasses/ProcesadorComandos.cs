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
        public static string[] Parse(string cmd)
        {
            PC.InitMetodo();

            if(cmd == null)
            {
                PC.HayError = true;
                PC.MensajeError = $"Error: cmd nulo, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                return null;
            }
            if (string.IsNullOrWhiteSpace(cmd))
            {
                PC.HayError = true;
                PC.MensajeError = $"Error: cmd vacio, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                return null;
            }

            switch(cmd.Trim().ToLower())
            {
                case "list":
                    return new[] { "list" };
                default:
                    PC.HayError = true;
                    PC.MensajeError = $"Error: cmd no reconcido, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                    break;
            }

            return new string[] {};
        }
        public static string Run(string comando)
        {
            PC.InitMetodo();

            if(comando == null)
            {
                PC.HayError = true;
                PC.MensajeError = $"Error: cmd nulo, en {nameof(ProcesadorComandos)}.{nameof(Run)}!";
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
