#region Header

using System;
using PC = _2PwdClasses.ProcesadorComandos;
using MR = _2PwdClasses.ManejadorRegistros;
using System.Threading.Tasks;

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

        //private static string AddRegPwd(string row)
        //{
        //    var rowPwd = "";
        //    bool ok = MR.ReadMaestro();
        //    if (!ok)
        //        return rowPwd;
        //    MR.CreateRegPwd(row);
        //    if (ok)
        //        rowPwd = MR.RetrieveRegPwd(row);
        //    MR.WriteMaestro();
        //    return rowPwd;
        //}

        //private static string ListPwds()
        //{
        //    string pwds = "Categoría|Empresa|Producto|UsuarioNombre|UsuarioId|UsuarioPwd|EmpresaMail|EmpresaWeb|Notas|Fec.Creación|Fec.Actualización" + Environment.NewLine;
        //    bool ok = MR.OpenMaestro();
        //    if(ok)
        //    {
        //        pwds += MR.ListRowsAsString();
        //        MR.CloseMaestro();
        //    }
        //    return pwds;
        //}

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
            if(regComando.Cmd.Length != 3)
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

            Comando Parse(Comando regCmd)
            {
                switch (regComando.Cmd)
                {
                    case "add":
                        return ParseAdd(regCmd);
                    case "del":
                    case "get":
                    case "lst":
                    case "upd":
                    default:
                        PC.HayError = true;
                        PC.MensajeError = $"Error: comando '{regComando.Cmd}' no reconcido, " +
                                            $"en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                        return regCmd;
                }
            }

            Comando ParseAdd(Comando regCmd)
            {
                //
                // Este metodo arma el argumento en una row normalizada de la forma
                // UserNombre|Categoria|Empresa|Producto|Numero|WebEmpresa|UserId|UserPwd|UserEmail|userNotas|
                //

                string arg = "";
                int ixUno = regCmd.Arg.IndexOf("uno:"); // UserNombre
                if(ixUno >= 0)
                {
                    ixUno += 4;
                    if(ixUno < regCmd.Arg.Length)
                    {
                        int endUno = ixUno;
                        if(regCmd.Arg[ixUno] == '\"')
                        {
                            ixUno++;
                            endUno = regCmd.Arg.IndexOf("\"", ixUno);
                            if(endUno > ixUno)
                            {
                                arg += regCmd.Arg.Substring(ixUno, endUno - ixUno);
                                regCmd.Arg = arg;
                                regCmd.Ok = true;
                            }
                        }
                        else
                        {
                            endUno = regCmd.Arg.IndexOf(" ", ixUno);
                            endUno = endUno > 0 ? endUno : regCmd.Arg.Length;
                            if (endUno > ixUno)
                            {
                                arg += regCmd.Arg.Substring(ixUno, endUno - ixUno);
                                regCmd.Arg = arg;
                                regCmd.Ok = true;
                            }
                        }

                    }
                }

                return regCmd;
            }
        }


        //public static string Run(string comando)
        //{
        //    PC.InitMetodo();

        //    var respuesta = "";
        //    var regComando = PC.Parse(comando);
        //    if (!regComando.Ok)
        //        return "";

        //    switch (regComando.Cmd)
        //    {
        //        case "add":
        //            respuesta = PC.AddPwds(regComando.Arg);
        //            break;
        //        case "list":
        //            respuesta = PC.ListPwds();
        //            break;
        //        default:
        //            break;
        //    }

        //    return respuesta;
        //}

        #endregion
    }

#region Footer
}
#endregion
