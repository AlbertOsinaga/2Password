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
        private static string AddPwds(string arg)
        {
            var pwd = "";
            bool ok = MR.OpenMaestro();
            if (!ok)
                return pwd;
            ok = MR.CreateRegistro(arg);
            if (ok)
                pwd = MR.RetrieveRowRegistro(arg);
            MR.CloseMaestro();
            return pwd;
        }
        private static string ListPwds()
        {
            string pwds = "Categoría|Empresa|Producto|UsuarioNombre|UsuarioId|UsuarioPwd|EmpresaMail|EmpresaWeb|Notas|Fec.Creación|Fec.Actualización" + Environment.NewLine;
            bool ok = MR.OpenMaestro();
            if(ok)
            {
                pwds += MR.ListRowsAsString();
                MR.CloseMaestro();
            }
            return pwds;
        }
        public static Comando Parse(string comando)
        {
            PC.InitMetodo();
            var regComando = new Comando();

            if(comando == null)
            {
                PC.HayError = true;
                PC.MensajeError = $"Error: comando nulo, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                return regComando;
            }
            if (string.IsNullOrWhiteSpace(comando))
            {
                PC.HayError = true;
                PC.MensajeError = $"Error: cmd vacio, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                return regComando;
            }

            int i = comando.IndexOf(':');
            regComando.Cmd = comando.Trim().ToLower();
            regComando.Arg = "";
            if (i > 0)
            {
                regComando.Cmd = comando.Substring(0, i).Trim().ToLower();
                if(i+1 >= 0)
                    regComando.Arg = comando.Substring(i+1).Trim();
            }

            switch(regComando.Cmd)
            {
                case "add":
                    regComando.Cmd = "add";
                    regComando.Ok = true;
                    break;
                case "list":
                    regComando.Cmd = "list";
                    regComando.Ok = true;
                    break;
                default:
                    PC.HayError = true;
                    PC.MensajeError = $"Error: cmd no reconcido, en {nameof(ProcesadorComandos)}.{nameof(Parse)}!";
                    break;
            }

            return regComando;
        }
        public static string Run(string comando)
        {
            PC.InitMetodo();

            var respuesta = "";
            var regComando = PC.Parse(comando);
            if (!regComando.Ok)
                return "";
            
            switch (regComando.Cmd)
            {
                case "add":
                    respuesta = PC.AddPwds(regComando.Arg);
                    break;
                case "list":
                    respuesta = PC.ListPwds();
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
