#region Header

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;

using G = _2PwdClasses.Global;
using MR = _2PwdClasses.ManejadorRegistros;

namespace _2PwdClasses
{
    #endregion

    public static class ManejadorRegistros
    {
        #region Propiedades

        public static bool HayError;
        public static string MensajeError;

        public static string NameMaestro;
        public static string KeyMaestro;

        public static bool IsMaestroOpen { get => MR.StatusMaestro == MR.StatusOpened; }
        public static string StatusClosed;
        public static string StatusMaestro;
        public static string StatusOpened;

        public static Dictionary<string, RegistroPwd> TableMaestro { get; set; }

        #endregion

        #region Metodos

        static ManejadorRegistros()
        {
            MR.HayError = false;
            MR.MensajeError = string.Empty;

            MR.NameMaestro = "_MasterFile";
            MR.KeyMaestro = "@2PwdMasterFile";

            MR.StatusClosed = "closed";
            MR.StatusOpened = "opened";
            MR.StatusMaestro = MR.StatusClosed;

            MR.TableMaestro = new Dictionary<string, RegistroPwd>();
        }
        public static bool CloseMaestro()
        {
            MR.InitMetodo();

            if (MR.IsMaestroOpen)
            {
                try
                {
                    var rows = MR.TableToRows();
                    File.Delete(MR.NameMaestro);
                    File.WriteAllLines(MR.NameMaestro, new[] { MR.KeyMaestro });
                    File.AppendAllLines(MR.NameMaestro, rows);
                    MR.StatusMaestro = MR.StatusClosed;
                }
                catch (Exception ex)
                {
                    MR.HayError = true;
                    MR.MensajeError = Global.ArmaMensajeError($"Excepcion en metodo {nameof(ManejadorRegistros)}.{nameof(CloseMaestro)}", ex);
                    return false;
                }
            }

            return true;
        }
        public static void InitMetodo()
        {
            MR.HayError = false;
            MR.MensajeError = string.Empty;
        }
        public static string RegistroPwdToRow(RegistroPwd regPwd)
        {
            MR.InitMetodo();
            if(regPwd == null)
            {
                MR.HayError = true;
                MR.MensajeError = $"regPWd nulo en {nameof(ManejadorRegistros)}.{nameof(RegistroPwdToRow)}()!";
                return string.Empty;
            }

            string row = string.Empty;
            row += regPwd.Categoria != null ? regPwd.Categoria : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.Empresa != null ? regPwd.Empresa : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.Producto != null ? regPwd.Producto : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.Nombre != null ? regPwd.Nombre : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.UserId != null ? regPwd.UserId : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.UserPwd != null ? regPwd.UserPwd : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.UserEMail != null ? regPwd.UserEMail : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.WebEmpresa != null ? regPwd.WebEmpresa : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.Nota != null ? regPwd.Nota : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.CreateDate.ToString(G.FormatoFecha);
            row += G.SeparadorCSV;
            row += regPwd.UpdateDate.ToString(G.FormatoFecha);
            
            return row;
        }
        public static RegistroPwd RowToRegistroPwd(string row)
        {
            MR.InitMetodo();

            var fields = row.Split(G.SeparadorCSV[0]);
            var regPwd = new RegistroPwd();
            
            regPwd.Categoria = fields.Length > 0 ? (fields?[0] != null ? fields[0] : string.Empty) : string.Empty;
            regPwd.Empresa = fields.Length > 1 ? (fields?[1] != null ? fields[1] : string.Empty) : string.Empty;
            regPwd.Producto = fields.Length > 2 ? (fields?[2] != null ? fields[2] : string.Empty) : string.Empty;
            regPwd.Nombre = fields.Length > 3 ? (fields?[3] != null ? fields[3] : string.Empty) : string.Empty;
            regPwd.UserId = fields.Length > 4 ? (fields?[4] != null ? fields[4] : string.Empty) : string.Empty;
            regPwd.UserPwd = fields.Length > 5 ? (fields?[5] != null ? fields[5] : string.Empty) : string.Empty;
            regPwd.UserEMail = fields.Length > 6 ? (fields?[6] != null ? fields[6] : string.Empty) : string.Empty;
            regPwd.WebEmpresa = fields.Length > 7 ? (fields?[7] != null ? fields[7] : string.Empty) : string.Empty;
            regPwd.Nota = fields.Length > 8 ? (fields?[8] != null ? fields[8] : string.Empty) : string.Empty;
            regPwd.CreateDate = DateTime.Parse(G.NoFecha);
            try
            {
                string strDate = fields.Length > 9 ? (fields?[9] != null ? fields[9] : G.NoFecha) : G.NoFecha;
                regPwd.CreateDate = DateTime.Parse(strDate);
            }
            catch {}
            regPwd.UpdateDate = DateTime.Parse(G.NoFecha); ;
            try
            {
                string strDate = fields.Length > 10 ? (fields?[10] != null ? fields[10] : G.NoFecha) : G.NoFecha;
                regPwd.UpdateDate = DateTime.Parse(strDate);
            }
            catch {}
            return regPwd;
        }
        public static string KeyOfRegistroPwd(RegistroPwd regPwd) =>
            regPwd.Categoria + "|" +
            regPwd.Empresa + "|" +
            regPwd.Producto + "|" +
            regPwd.Nombre;
        public static bool OpenMaestro()
        {
            MR.InitMetodo();

            if (string.IsNullOrWhiteSpace(MR.NameMaestro))
            {
                string nulo_enblanco = MR.NameMaestro == null ? "nulo" : "en blanco";
                MR.HayError = true;
                MR.MensajeError = $"Archivo Maestro {nulo_enblanco}!";
                return false;
            }

            if(!File.Exists(MR.NameMaestro))
            {
                MR.HayError = true;
                MR.MensajeError = $"Archivo Maestro '{MR.NameMaestro}' no existe!";
                return false;
            }

            var lineas = File.ReadAllLines(MR.NameMaestro);
            if(lineas == null || lineas.Count() == 0 || !(lineas[0] == MR.KeyMaestro))
            {
                MR.HayError = true;
                MR.MensajeError = $"'{MR.NameMaestro}' no tiene formato de Archivo Maestro!";
                return false;
            }

            try
            {
                MR.RowsToTable(lineas); 
                MR.StatusMaestro = MR.StatusOpened;
            }
            catch (Exception ex)
            {
                MR.HayError = true;
                MR.MensajeError = Global.ArmaMensajeError($"Excepcion en metodo {nameof(ManejadorRegistros)}.{nameof(OpenMaestro)}", ex);
                return false;
            }

            return true;
        }
        public static bool RowsToTable(string[] rows)
        {
            MR.InitMetodo();
            MR.TableMaestro.Clear();
            try
            {
                foreach (var row in rows)
                {
                    if (row == MR.KeyMaestro)
                        continue;
                    var regPwd = MR.RowToRegistroPwd(row);
                    TableMaestro.Add(MR.KeyOfRegistroPwd(regPwd), regPwd);
                }
                return true;
            }
            catch (Exception ex)
            {
                MR.HayError = true;
                MR.MensajeError = Global.ArmaMensajeError($"Excepcion en metodo {nameof(ManejadorRegistros)}.{nameof(RowsToTable)}", ex);
                return false;
            }
        }
        public static string[] TableToRows()
        {
            MR.InitMetodo();

            var rows = new List<string>();
            try
            {
                foreach (var regPwd in MR.TableMaestro.Values)
                    rows.Add(MR.RegistroPwdToRow(regPwd));
            }
            catch (Exception ex)
            {
                MR.HayError = true;
                MR.MensajeError = Global.ArmaMensajeError($"Excepcion en metodo {nameof(ManejadorRegistros)}.{nameof(TableToRows)}", ex);
                return new string[] {};
            }

            return rows.ToArray();
        }

        #endregion
    }

#region Footer
}
#endregion
