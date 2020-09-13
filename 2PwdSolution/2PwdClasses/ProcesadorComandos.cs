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
        //            respuesta = PC.AddPwds(regComando.Arg);https://api.taboola.com/2.0/json/msn-anaheim-us/recommendations.notify-click?app.type=desktop&app.apikey=dd914485a5ed62ec1086bc1f372e410b851bc1e0&response.id=__57dbf6376eb42ccc5f4fcae52edb7879__ea64cd5a5101de84268a61dd6b307d65&response.session=v2_0afbed84ab22413805f7a94cf9362b7d_109624A5BCFD6E122D462A5FBDE96FC1_1599918471_1599918471_CIi3jgYQsORKGMz669e4ipfamgEgASgFMB44htMHQOCHEEi02dgDUP___________wFYAGAAaJHhgryfr6Tf2QE&item.id=%7E%7EV1%7E%7E-1206419116654905771%7E%7ELapUE77QL7-UbwMbvLG3OfBnf6AgXMqtwCFBTr-GeVYndpXq_nTToVci-tV_1bYyPVPbFHdycXfyr1VxmozLcV7JbGFbjtizQN29Zpin8p1JlawpbYFtoRw_FVSAoDPy8yoVX_ZV1DUzrU9mgEqIxUUWQYJbJAZQ_PxdsyidTxwHevLFaRMtkz52WhIKlmx7-csEBSsOEU6ufprwD-3fSi9Ezl800pG9duHuzBPjTUSAhDH4QnJoSS1idJX-Rd20Sh1b8LRPczWnT2TkP21u0fHnd3F8OVk2bR-QZRW3RyT-S5sN_yP-AY0beD0JthKK&item.type=text&sig=119fe907cc58e7c19f4e32bc2d3a55265ca250c5dc4f&redir=https%3A%2F%2Fwww.vpnmentor.com%2Fpopular%2Fwatch-disney-plus-online-anywhere%2F%3Futm_source%3Dtaboola%26utm_medium%3Dpaid%26tblcid%3DGiAc8YD07K6TzKrK-qmzpcVlVWEr6Ia40h5EYeyoYNdQHyCs8U4%26campaign%3DDisney%2BPlus%2B-%2BPremium%2B-%2BNew%26campaignid%3D5755473%26site%3Dmsn-anaheim-us%26siteid%3D1225264%26title%3DUnblock%2BDisney%2BPlus%2BFrom%2BBolivia%2521%26image%3Dhttp%253A%252F%252Fcdn.taboola.com%252Flibtrc%252Fstatic%252Fthumbnails%252FGETTY_IMAGES%252FSKP%252F1032516774__UGubyCf8.jpg%26accountid%3D1292460%26ad%3D2918819387%26tblci%3DGiAc8YD07K6TzKrK-qmzpcVlVWEr6Ia40h5EYeyoYNdQHyCs8U4%23tblciGiAc8YD07K6TzKrK-qmzpcVlVWEr6Ia40h5EYeyoYNdQHyCs8U4&ui=109624A5BCFD6E122D462A5FBDE96FC1
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
