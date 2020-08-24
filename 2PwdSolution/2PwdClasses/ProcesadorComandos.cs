#region Header

using System;
using PC = _2PwdClasses.ProcesadorComandos;
using MR = _2PwdClasses.ManejadorRegistros;

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
        public static Comando Parse(string cmd)
        {
            PC.InitMetodo();
            var comando = new Comando();

            if(cmd == null)
            {
                PC.HayError = true;
                PC.MensajeError = $"Error: cmd nulo, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                return comando;
            }
            if (string.IsNullOrWhiteSpace(cmd))
            {
                PC.HayError = true;
                PC.MensajeError = $"Error: cmd vacio, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                return comando;
            }

            switch(cmd.Trim().ToLower())
            {
                case "list":
                    comando.Cmd = "list";
                    comando.Ok = true;
                    break;
                default:
                    PC.HayError = true;
                    PC.MensajeError = $"Error: cmd no reconcido, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                    break;
            }

            return comando;
        }
        public static string Run(string cmd)
        {
            PC.InitMetodo();

            var respuesta = "";
            var comando = PC.Parse(cmd);
            if (!comando.Ok)
                return "";
            
            switch (comando.Cmd)
            {
                case "list":
                    respuesta = MR.ListRowsAsString();
                    break;
                default:
                    break;
            }

            return respuesta;
        }

        #endregion
    }

#region Footer
}
#endregion
