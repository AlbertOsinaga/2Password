#region Header

using System;
using PC = _2PwdClasses.ProcesadorComandos;
using MR = _2PwdClasses.ManejadorRegistros;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace _2PwdClasses
{

#endregion

    public static class ProcesadorComandos
    {
        #region Propiedades

        public static bool HayError;
        public static string MensajeError;
        public static char SeparadorCmdArg = ' ';

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

        public static Comando Parse(string lineaCmd)
        {
            // El objetivo de este metodo es convertir el formato en linea de comandos
            // al registro de comando, con cmd identificando el comando,
            // arg identificando la row sobre la que se debe operar
            // y manejando el flag Ok para indicar si el parse (conversion) fue exitoso

            PC.InitMetodo();
            var regComando = new Comando();

            if (string.IsNullOrWhiteSpace(lineaCmd))
            {
                PC.HayError = true;
                PC.MensajeError = $"Error: linea comando nula o vacia, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                return regComando;
            }

            int i = lineaCmd.IndexOf(PC.SeparadorCmdArg);  // ' '
            if (i > 0)
            {
                regComando.Cmd = lineaCmd.Substring(0, i).Trim().ToLower();
                if (i + 1 >= 4)
                    regComando.Arg = lineaCmd.Substring(i + 1);
            }
            if (regComando.Cmd.Length != 3)
            {
                PC.HayError = true;
                PC.MensajeError = $"Error: Comando '{regComando.Cmd}' debe ser de 3 caracteres," +
                                    $" en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                return regComando;
            }

            switch (regComando.Cmd)
            {
                case "add":
                case "del":
                case "get":
                case "lst":
                case "upd":
                    return Parse(regComando);
                default:
                    PC.HayError = true;
                    PC.MensajeError = $"Error: comando '{regComando.Cmd}' no reconcido, " +
                                        $"en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                    return regComando; ;
            }

            #region Funciones

            Comando Parse(Comando regCmd)
            {
                switch (regComando.Cmd)
                {
                    case "add":
                    case "del":
                    case "get":
                    case "lst":
                    case "upd":
                        return ParseCmd(regCmd);
                    default:
                        PC.HayError = true;
                        PC.MensajeError = $"Error: comando '{regCmd.Cmd}' no reconcido, " +
                                            $"en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                        return regCmd;
                }
            }

            Comando ParseCmd(Comando regCmd)
            {
                //
                // Este metodo arma el argumento del cmd add en una row normalizada de la forma
                // UserNombre|Categoria|Empresa|Producto|Numero|WebEmpresa|UserId|UserPwd|UserEmail|userNotas|
                //

                var cmds = new string[] {};
                switch(regCmd.Cmd)
                {
                    case "add":
                        cmds = new[] { "nom", "cat", "emp", "cta", "nro", "web", "uid", "pwd", "ema", "not" };
                        break;
                    case "del":
                    case "get":
                    case "lst":
                        cmds = new[] { "nom", "cat", "emp", "cta", "nro" };
                        break;
                    case "upd":
                        cmds = new[] { "nom", "cat", "emp", "cta", "nro", "web", "uid", "pwd", "ema", "not", "fcr", "fup", "rid" };
                        break;
                    default:
                        break;
                }

                var partes = regCmd.Arg.Split('-');
                var campos = new Dictionary<string, string>();
                foreach (var campo in partes)
                {
                    if (campo.Length > 4 && cmds.Where(cmd => cmd == campo.Substring(0, 3)).Any() && campo[3] == ' ')
                        campos[campo.Substring(0, 3)] = campo.Substring(4).Trim();
                }

                var arg = "";
                foreach (var cmd in cmds)
                {
                    if (campos.ContainsKey(cmd))
                        arg += campos[cmd];
                    arg += cmd == cmds[cmds.Length - 1] ? "" : "|";
                }

                regCmd.Arg = arg;
                regCmd.Ok = true;
                return regCmd;
            }


            #endregion
        }

        public static string Run(string comando)
        {
            PC.InitMetodo();

            var respuesta = "";
            var regComando = PC.Parse(comando);
            if (!regComando.Ok)
                return "Error en Parse!";

            switch (regComando.Cmd)
            {
                case "add":
                case "get":
                case "upd":
                    if (!MR.HayError)
                    {
                        respuesta = MR.CreateRegPwd(regComando.Arg, enMaestro: false);
                    }
                    else
                    {
                        respuesta = $"*** {MR.MensajeError} ***";
                    }
                    break;
                case "del":
                    if (!MR.HayError)
                    {
                        bool deleteOk = MR.DeleteRegPwd(regComando.Arg);
                        respuesta = deleteOk ? "*** registro borrado! ***" : "*** no borrado! ***";
                    }
                    else
                    {
                        respuesta = $"*** {MR.MensajeError} ***";
                    }
                    break;
                default:
                    respuesta = $"*** Comando '{regComando.Cmd}' no reconocido! ***";
                    break;
            }

            return respuesta;
        }

        #endregion
    }

#region Footer
}
#endregion
