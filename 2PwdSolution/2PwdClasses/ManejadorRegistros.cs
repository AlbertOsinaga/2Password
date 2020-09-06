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
        public static bool Updated;

        public static string NameMaestro;
        public static string NameMaestro_Default = "_MasterFile";
        public static string KeyMaestro;
        public static string KeyVacia = "||||";

        public static bool IsMaestroReaded { get => MR.StatusMaestro == MR.StatusReaded; }
        public static string StatusWrited;
        public static string StatusMaestro;
        public static string StatusReaded;

        public static Dictionary<string, RegistroPwd> TableMaestro { get; set; }

        #endregion

        #region Metodos

        static ManejadorRegistros()
        {
            MR.HayError = false;
            MR.MensajeError = string.Empty;
            MR.Updated = false;

            MR.NameMaestro = MR.NameMaestro_Default;
            MR.KeyMaestro = "@2PwdMasterFile";

            MR.StatusWrited = "closed";
            MR.StatusReaded = "opened";
            MR.StatusMaestro = MR.StatusWrited;

            MR.TableMaestro = new Dictionary<string, RegistroPwd>();
        }

        public static RegistroPwd CreateRegPwd(RegistroPwd regPwd, bool enMaestro = true)
        {
            MR.InitMetodo();
            if (enMaestro)
                ReadMaestro();

            string key = MR.KeyOfRegistroPwd(regPwd);
            if (string.IsNullOrEmpty(key) || key == MR.KeyVacia || key == G.RegNull || MR.TableMaestro.ContainsKey(key))
            {
                MR.HayError = true;
                MR.MensajeError = $"Error: key invalida o duplicada: '{key??"null"}'" + 
                                  $", en {nameof(ManejadorRegistros)}.{nameof(CreateRegPwd)}!";
                return null;
            }
            
            regPwd.CreateDate = DateTime.Now;
            regPwd.UpdateDate = DateTime.MinValue;
            regPwd.RegId = (MR.TableMaestro.Keys.Count() + 1).ToString();
            MR.TableMaestro.Add(key, regPwd);
            MR.Updated = true;
            
            if (enMaestro)
                MR.WriteFile();
            
            return regPwd;
        }
        public static string CreateRegPwd(string rowPwd, bool enMaestro = true)
        {
            RegistroPwd regPwd = MR.RowToRegistroPwd(rowPwd);
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro);
            return MR.RegistroPwdToRow(regPwdAdd);
        }
        public static void ClearMaestro()
        { 
            MR.TableMaestro.Clear();
            MR.Updated = false;
            MR.StatusMaestro = MR.StatusWrited;
        }
        public static bool DeleteRegPwd(RegistroPwd regPwd, bool enMaestro = true)
        {
            MR.InitMetodo();
            if (enMaestro && !MR.ReadMaestro())
                return false;

            string key = MR.KeyOfRegistroPwd(regPwd);
            if (string.IsNullOrEmpty(key) || key == MR.KeyVacia || key == G.RegNull || !MR.TableMaestro.ContainsKey(key))
            {
                MR.HayError = true;
                MR.MensajeError = $"Error: key invalida o inexistente: '{key ?? "null"}'" +
                                  $", en {nameof(ManejadorRegistros)}.{nameof(DeleteRegPwd)}!";
                return false;
            }

            MR.TableMaestro.Remove(key);
            MR.Updated = true;

            if (enMaestro)
                MR.WriteMaestro();

            return true;
        }
        public static bool DeleteRegPwd(string rowPwd, bool enMaestro = true)
        {
            RegistroPwd regPwd = MR.RowToRegistroPwd(rowPwd);
            return MR.DeleteRegPwd(regPwd, enMaestro);
        }
        public static void InitMetodo()
        {
            MR.HayError = false;
            MR.MensajeError = string.Empty;
        }
        public static string KeyOfRegistroPwd(RegistroPwd regPwd) =>
            regPwd != null ?
            ((regPwd.UserNombre ?? "").Trim().ToLower() + G.SeparadorCSV) +
            ((regPwd.Categoria ?? "").Trim().ToLower() + G.SeparadorCSV) +
            ((regPwd.Empresa ?? "").Trim().ToLower() + G.SeparadorCSV) +
            ((regPwd.Producto ?? "").Trim().ToLower() + G.SeparadorCSV) +
            (regPwd.Numero ?? "").Trim().ToLower()
            : G.RegNull;

        //public static List<RegistroPwd> ListRegistros(string where = "")
        //{
        //    MR.InitMetodo();

        //    var regs = new List<RegistroPwd>();
        //    if(!MR.IsMaestroOpen)
        //    {
        //        MR.HayError = true;
        //        MR.MensajeError = $"Error: maestro no abierto, en {nameof(ManejadorRegistros)}.{nameof(ListRegistros)}!";
        //        return regs;
        //    }
        //    regs = MR.TableMaestro.Values.ToList();
        //    return regs;
        //}
        //public static List<string> ListRows(string where = "")
        //{
        //    var rows = new List<string>();
        //    List<RegistroPwd> regs = MR.ListRegistros(where);
        //    if (MR.HayError)
        //        return rows;

        //    foreach (var reg in regs)
        //        rows.Add(MR.RegistroPwdToRow(reg));

        //    return rows;
        //}
        //public static string ListRowsAsString(string where = "")
        //{
        //    var lista = MR.ListRows();
        //    var rows = "";
        //    for(int i = 0; i < lista.Count; i++)
        //    {
        //        if (!string.IsNullOrEmpty(lista[i]))
        //        {
        //            rows += lista[i];
        //            if(i < lista.Count - 1)
        //                rows += Environment.NewLine;
        //        }
        //    }
        //    return rows;
        //}

        public static string[] ReadFile() => File.ReadAllLines(MR.NameMaestro);
        public static bool ReadMaestro()
        {
            //
            // ReadMaestro()
            // - Solo 1 lectura efectiva, desde el archivo Maestro.
            // Si el archivo maestro ha sido ya leido (status readed),
            // devuelve true (y no error) sin hacer nada.
            // Para una nueva lectura efectiva, desde archivo Maestro,
            // debe estarse en status (writed = no readed)

            if (MR.IsMaestroReaded)
                return true;

            MR.InitMetodo();
            if (string.IsNullOrWhiteSpace(MR.NameMaestro))
            {
                string nulo_enblanco = MR.NameMaestro == null ? "nulo" : "en blanco";
                MR.HayError = true;
                MR.MensajeError = $"Nombre de archivo Maestro {nulo_enblanco}!";
                return false;
            }
            if (!File.Exists(MR.NameMaestro))
            {
                MR.HayError = true;
                MR.MensajeError = $"Archivo Maestro '{MR.NameMaestro}' no existe!";
                return false;
            }

            try
            {
                var lineas = MR.ReadFile();
                if (lineas == null || lineas.Count() == 0 || !(lineas[0] == MR.KeyMaestro))
                {
                    MR.HayError = true;
                    MR.MensajeError = $"'{MR.NameMaestro}' no tiene formato de Archivo Maestro!";
                    return false;
                }
                MR.RowsToTable(lineas);
                MR.StatusMaestro = MR.StatusReaded;
                MR.Updated = false;
            }
            catch (Exception ex)
            {
                MR.HayError = true;
                MR.MensajeError = Global.ArmaMensajeError($"Excepcion en metodo {nameof(ManejadorRegistros)}.{nameof(ReadMaestro)}", ex);
                return false;
            }

            return true;
        }
        public static string RegistroPwdToRow(RegistroPwd regPwd)
        {
            MR.InitMetodo();
            if (regPwd == null)
            {
                MR.HayError = true;
                MR.MensajeError = $"Error: reg null, en {nameof(ManejadorRegistros)}.{nameof(RegistroPwdToRow)}!";
                return null;
            }

            string row = string.Empty;
            row += regPwd.UserNombre != null ? regPwd.UserNombre : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.Categoria != null ? regPwd.Categoria : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.Empresa != null ? regPwd.Empresa : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.Producto != null ? regPwd.Producto : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.Numero != null ? regPwd.Numero : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.Web != null ? regPwd.Web : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.UserId != null ? regPwd.UserId : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.UserPwd != null ? regPwd.UserPwd : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.UserEMail != null ? regPwd.UserEMail : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.UserNota != null ? regPwd.UserNota : string.Empty;
            row += G.SeparadorCSV;
            row += regPwd.CreateDate.ToString(G.FormatoFecha);
            row += G.SeparadorCSV;
            row += regPwd.UpdateDate.ToString(G.FormatoFecha);
            row += G.SeparadorCSV;
            row += regPwd.RegId;

            return row;
        }
        public static RegistroPwd RetrieveRegPwd(RegistroPwd regPwd, bool enMaestro = true)
        {
            MR.InitMetodo();
            if (enMaestro)
                MR.ReadMaestro();

            string key = MR.KeyOfRegistroPwd(regPwd);
            if (string.IsNullOrEmpty(key) || key == MR.KeyVacia || key == G.RegNull)
            {
                MR.HayError = true;
                MR.MensajeError = $"Error: key invalida: '{key ?? "null"}'" +
                                  $", en {nameof(ManejadorRegistros)}.{nameof(RetrieveRegPwd)}!";
                return null;
            }
            if (!MR.TableMaestro.ContainsKey(key))
                return null;

            var regPwdGet = MR.TableMaestro[key];
            return regPwdGet;
        }
        public static string RetrieveRegPwd(string rowPwd, bool enMaestro = true)
        {
            RegistroPwd regPwd = MR.RowToRegistroPwd(rowPwd);
            var regPwdGet = MR.RetrieveRegPwd(regPwd, enMaestro);
            return MR.RegistroPwdToRow(regPwdGet);
        }
        public static bool RowsToTable(string[] rows)
        {
            MR.InitMetodo();
            if (rows == null)
            {
                MR.HayError = true;
                MR.MensajeError = $"Error: rows null, en {nameof(ManejadorRegistros)}.{nameof(RowsToTable)}!";
                return false;
            }
            if (rows.Length == 0)
            {
                MR.HayError = true;
                MR.MensajeError = $"Error: rows sin registros, en {nameof(ManejadorRegistros)}.{nameof(RowsToTable)}!";
                return false;
            }

            MR.ClearMaestro();
            try
            {
                foreach (var row in rows)
                {
                    if (row == MR.KeyMaestro)
                        continue;
                    var regPwd = MR.RowToRegistroPwd(row);
                    var key = MR.KeyOfRegistroPwd(regPwd);
                    if(!(key == MR.KeyVacia))
                        MR.TableMaestro.Add(key, regPwd);
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
        public static RegistroPwd RowToRegistroPwd(string row)
        {
            MR.InitMetodo();
            if (row == null)
            {
                MR.HayError = true;
                MR.MensajeError = $"Error: row nula, en {nameof(ManejadorRegistros)}.{nameof(RowToRegistroPwd)}!";
                return null;
            }

            var fields = row.Split(G.SeparadorCSV[0]);
            var regPwd = new RegistroPwd();

            int i = 0;
            regPwd.UserNombre = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.Categoria = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.Empresa = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.Producto = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.Numero = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.Web = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.UserId = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.UserPwd = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.UserEMail = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.UserNota = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty; i++;
            regPwd.CreateDate = DateTime.Parse(G.NoFecha);
            try
            {
                string strDate = fields.Length > i ? (fields?[i] != null ? fields[i] : G.NoFecha) : G.NoFecha; i++;
                regPwd.CreateDate = DateTime.Parse(strDate);
            }
            catch {}
            regPwd.UpdateDate = DateTime.Parse(G.NoFecha);
            try
            {
                string strDate = fields.Length > i ? (fields?[i] != null ? fields[i] : G.NoFecha) : G.NoFecha; i++;
                regPwd.UpdateDate = DateTime.Parse(strDate);
            }
            catch {}
            regPwd.RegId = fields.Length > i ? (fields?[i] != null ? fields[i] : string.Empty) : string.Empty;
            return regPwd;
        }
        public static RegistroPwd UpdateRegPwd(RegistroPwd regPwd, bool enMaestro = true)
        {
            MR.InitMetodo();
            if (enMaestro && !MR.ReadMaestro())
                return null;

            string key = MR.KeyOfRegistroPwd(regPwd);
            if (string.IsNullOrEmpty(key) || key == MR.KeyVacia || key == G.RegNull || !MR.TableMaestro.ContainsKey(key))
            {
                MR.HayError = true;
                MR.MensajeError = $"Error: key invalida o inexistente: '{key ?? "null"}'" +
                                  $", en {nameof(ManejadorRegistros)}.{nameof(UpdateRegPwd)}!";
                return null;
            }

            regPwd.UpdateDate = DateTime.Now;
            MR.TableMaestro[key] = regPwd;
            MR.Updated = true;

            if (enMaestro)
                MR.WriteMaestro();

            return MR.TableMaestro[key];
        }
        public static string UpdateRegPwd(string rowPwd, bool enMaestro = true)
        {
            RegistroPwd regPwd = MR.RowToRegistroPwd(rowPwd);
            RegistroPwd regPwdUpd = MR.UpdateRegPwd(regPwd, enMaestro);
            return MR.RegistroPwdToRow(regPwdUpd);
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
                return new string[] { };
            }

            return rows.ToArray();
        }
        public static void WriteFile()
        {
            var rows = MR.TableToRows();
            File.Delete(MR.NameMaestro);
            File.WriteAllLines(MR.NameMaestro, new[] { MR.KeyMaestro });
            File.AppendAllLines(MR.NameMaestro, rows);
        }
        public static bool WriteMaestro()
        {
            //
            // WriteMaestro()
            // - Solo 1 escritura efectiva, en el archivo Maestro.
            // Si el archivo maestro ha sido ya escrito (status writed = not readed),
            // devuelve false (y un mensaje de error) sin hacer nada.
            // Para una nueva escritura efectiva, en el archivo Maestro,
            // debe estarse en status (readed = no writed)
            // - Solo se escribe al archivo si el flag Updated es true.

            if (!MR.IsMaestroReaded)
            {
                MR.HayError = true;
                MR.MensajeError = "Maestro no leido o ya escrito con anterioridad!";
                return false;
            }

            MR.InitMetodo();

            try
            {
                if (MR.Updated)
                    MR.WriteFile();
                MR.ClearMaestro();
            }
            catch (Exception ex)
            {
                MR.HayError = true;
                MR.MensajeError = Global.ArmaMensajeError($"Excepcion en metodo {nameof(ManejadorRegistros)}" + 
                                                            $".{nameof(WriteMaestro)}", ex);
                return false;
            }

            return true;
        }

        #endregion
    }

    #region Footer
}
#endregion
