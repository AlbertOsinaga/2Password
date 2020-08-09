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

        public static string ArchivoMaestro;
        public static bool HayError;
        public static string MensajeError;
        public static Dictionary<string, string> TablaRegistros;

        #endregion

        #region Metodos

        static ManejadorRegistros()
        {
            MR.HayError = false;
            MR.ArchivoMaestro = "_MasterFile";
            MR.MensajeError = string.Empty;
            MR.TablaRegistros = new Dictionary<string, string>();
        }
        public static bool AbrirMaestro()
        {
            MR.HayError = false;
            MR.MensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(MR.ArchivoMaestro))
            {
                string nulo_enblanco = MR.ArchivoMaestro == null ? "nulo" : "en blanco";
                MR.HayError = true;
                MR.MensajeError = $"Archivo Maestro {nulo_enblanco}!";
                return false;
            }

            if(!File.Exists(MR.ArchivoMaestro))
            {
                MR.HayError = true;
                MR.MensajeError = $"Archivo Maestro '{MR.ArchivoMaestro}' no existe!";
                return false;
            }

            var lineas = File.ReadAllLines(MR.ArchivoMaestro);
            var lista = lineas.ToList();
            try
            {
                MR.TablaRegistros.Clear();
                lista.ForEach(s => MR.TablaRegistros[s.Substring(0, s.IndexOf(' '))] = s);
                MR.TablaRegistros["@status"] = "@status|abierto";
            }
            catch (Exception ex)
            {
                MR.HayError = true;
                MR.MensajeError = Global.ArmaMensajeError("Excepcion", ex);
                return false;
            }

            return true;
        }
        public static bool CerrarMaestro()
        {
            if (MR.MaestroAbierto())
            {
                try
                {
                    var lineas = MR.TablaRegistros.Values.ToArray();
                    File.WriteAllLines(MR.ArchivoMaestro, lineas);
                }
                catch (Exception ex)
                {
                    MR.MensajeError = Global.ArmaMensajeError("Excepcion ", ex);
                    return false;
                }
            }

            return true;
        }
        public static bool MaestroAbierto()
        {
            if(MR.TablaRegistros == null)
                return false;

            if (MR.TablaRegistros.Count == 0)
                return false;

            var keysStatus = from k in MR.TablaRegistros.Keys where k == "@status" select k;
            if (keysStatus.Count() < 0)
                return false;

            string keyStatus = keysStatus.FirstOrDefault();
            if (!(MR.TablaRegistros[keyStatus] == "@status|abierto"))
                return false;

            return true;
        }

        #endregion
    }

#region Footer
}
#endregion
