#region Header

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MR = _2PwdClasses.ManejadorRegistros;

namespace _2PwdClasses
{
#endregion
    
    public static class ManejadorRegistros
    {
        #region Propiedades

        public static string FileNameMaestro;
        public static bool HayError;
        public static string KeyFileMaestro;
        public static string KeyStatus;
        public static string MensajeError;
        public static string StatusClosed;
        public static string StatusOpened;
        public static Dictionary<string, string> TablaRegistros;

        #endregion

        #region Metodos

        static ManejadorRegistros()
        {
            MR.FileNameMaestro = "_MasterFile";
            MR.HayError = false;
            MR.KeyFileMaestro = "@2PwdMasterFile";
            MR.KeyStatus = "@status";
            MR.MensajeError = string.Empty;
            MR.StatusClosed = "closed";
            MR.StatusOpened = "opened";
            MR.TablaRegistros = new Dictionary<string, string>();
        }
        public static bool CloseMaestro()
        {
            if (MR.IsMaestroOpen())
            {
                try
                {
                    if (MR.TablaRegistros[MR.KeyStatus] != null)
                        MR.TablaRegistros.Remove(MR.KeyStatus);
                    var lineas = MR.TablaRegistros.Values.ToArray();
                    File.Delete(MR.FileNameMaestro);
                    File.WriteAllLines(MR.FileNameMaestro, new[] { MR.KeyFileMaestro });
                    File.AppendAllLines(MR.FileNameMaestro, lineas);
                }
                catch (Exception ex)
                {
                    MR.MensajeError = Global.ArmaMensajeError("Excepcion ", ex);
                    return false;
                }
            }

            return true;
        }
        public static bool IsMaestroOpen() => StatusMaestro() == MR.StatusOpened;
        public static bool OpenMaestro()
        {
            MR.HayError = false;
            MR.MensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(MR.FileNameMaestro))
            {
                string nulo_enblanco = MR.FileNameMaestro == null ? "nulo" : "en blanco";
                MR.HayError = true;
                MR.MensajeError = $"Archivo Maestro {nulo_enblanco}!";
                return false;
            }

            if(!File.Exists(MR.FileNameMaestro))
            {
                MR.HayError = true;
                MR.MensajeError = $"Archivo Maestro '{MR.FileNameMaestro}' no existe!";
                return false;
            }

            var lineas = File.ReadAllLines(MR.FileNameMaestro);
            if(lineas == null || lineas.Count() == 0 || !(lineas[0] == MR.KeyFileMaestro))
            {
                MR.HayError = true;
                MR.MensajeError = $"'{MR.FileNameMaestro}' no tiene formato de Archivo Maestro!";
                return false;
            }

            try
            {
                MR.TablaRegistros.Clear();
                MR.TablaRegistros[MR.KeyStatus] = MR.StatusOpened;
                _ArrayToDictionary(lineas, MR.TablaRegistros);
            }
            catch (Exception ex)
            {
                MR.HayError = true;
                MR.MensajeError = Global.ArmaMensajeError($"Excepcion en metodo {nameof(ManejadorRegistros)}.{nameof(OpenMaestro)}", ex);
                return false;
            }

            return true;

            #region funciones

            void _ArrayToDictionary(string[] registros, Dictionary<string, string> tablaRegistros)
            {
                foreach (string reg in registros)
                {
                    if (string.IsNullOrWhiteSpace(reg) || reg.IndexOf(' ') < 1)
                        continue;
                    var key = reg.Substring(0, reg.IndexOf(' '));
                    MR.TablaRegistros[key] = reg;
                }
            }

            #endregion
        }

        public static string StatusMaestro() => MR.TablaRegistros[MR.KeyStatus] != null ? MR.TablaRegistros[MR.KeyStatus] : MR.StatusClosed;

        #endregion
    }

#region Footer
}
#endregion
