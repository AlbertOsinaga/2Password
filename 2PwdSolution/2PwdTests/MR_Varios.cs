#region Header
using System;
using Xunit;

using _2PwdClasses;
using G = _2PwdClasses.Global;
using MR = _2PwdClasses.ManejadorRegistros;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using System.IO;

namespace _2PwdTests
{
    #endregion

    public class MR_Varios
    {
        #region EmptyMaestro

        [Fact]
        public void EmptyMaestro_Ok()
        {
            // Prepara
            MR.DirMaestro = MR.DirMaestro_Default;
            MR.NameMaestro = MR.NameMaestro_Default;

            // Ejecuta
            bool ok = MR.EmptyMaestro();

            // Prueba
            Assert.True(ok);
            Assert.Equal(MR.StatusMaestro, MR.StatusWrited);
            Assert.False(MR.Updated);
            var texto = File.ReadAllText(MR.PathMaestro);
            Assert.Equal(MR.KeyMaestro, texto);
        }

        #endregion

        #region KeyOfRegistro

        [Fact]
        public void KeyOfRegistroPwd_conCamposNulos()
        {
            // Prepara
            RegistroPwd regPwd = new RegistroPwd();
            regPwd.Categoria = null;
            regPwd.Empresa = null;
            string keyEsperada = MR.KeyVacia;

            // Ejecuta
            string key = MR.KeyOfRegistroPwd(regPwd);

            // Prueba
            Assert.Equal(keyEsperada, key);
        }

        [Fact]
        public void KeyOfRegistroPwd_conCamposVacios()
        {
            // Prepara
            RegistroPwd regPwd = new RegistroPwd();
            regPwd.Categoria = "Software   ";
            regPwd.Empresa = "Microsoft    ";
            regPwd.Producto = "";
            regPwd.UserNombre = "Juana";
            string keyEsperada = "juana|software|microsoft|||";

            // Ejecuta
            string key = MR.KeyOfRegistroPwd(regPwd);

            // Prueba
            Assert.Equal(keyEsperada, key);
        }

        [Fact]
        public void KeyOfRegistroPwd_full()
        {
            // Prepara
            RegistroPwd regPwd = new RegistroPwd();
            regPwd.UserNombre = " Juan M  ";
            regPwd.Categoria = "Software   ";
            regPwd.Empresa = "Microsoft    ";
            regPwd.Producto = " OFFICE  ";
            string keyEsperada = "juan m|software|microsoft|office||";

            // Ejecuta
            string key = MR.KeyOfRegistroPwd(regPwd);

            // Prueba
            Assert.Equal(keyEsperada, key);
        }

        [Fact]
        public void KeyOfRegistroPwd_regNulo()
        {
            // Prepara
            string keyEsperada = G.RegNull;

            // Ejecuta
            string key = MR.KeyOfRegistroPwd(null);

            // Prueba
            Assert.Equal(keyEsperada, key);
        }

        #endregion

        #region List

        //        [Fact]
        //        public void ListRows_maestroNoOpen()
        //        {
        //            // Prepara

        //            // Ejecuta
        //            var rows = MR.ListRows();

        //            // Prueba
        //            Assert.Equal(0, rows.Count);
        //            Assert.True(MR.HayError);
        //            Assert.Equal("Error: maestro no abierto, en ManejadorRegistros.ListRegistros!", MR.MensajeError);
        //        }

        //        [Fact]
        //        public void ListRows_ok()
        //        {
        //            // Prepara
        //            MR.OpenMaestro();

        //            // Ejecuta
        //            var rows = MR.ListRows();

        //            // Prueba
        //            Assert.Equal(4, rows.Count);
        //            Assert.False(MR.HayError);
        //            Assert.Equal("", MR.MensajeError);
        //            MR.CloseMaestro();
        //        }

        //        [Fact]
        //        public void ListRowsAsString_maestroNoOpen()
        //        {
        //            // Prepara

        //            // Ejecuta
        //            var rows = MR.ListRowsAsString();

        //            // Prueba
        //            Assert.Equal("", rows);
        //            Assert.True(MR.HayError);
        //            Assert.Equal("Error: maestro no abierto, en ManejadorRegistros.ListRegistros!", MR.MensajeError);
        //        }

        //        [Fact]
        //        public void ListRowsAsString_ok()
        //        {
        //            // Prepara
        //            MR.CloseMaestro();
        //            MR.OpenMaestro();

        //            // Ejecuta
        //            var rows = MR.ListRowsAsString();

        //            // Prueba
        //            string[] rowsArray = rows.Split('\n','\r');
        //            var rowsLista = new List<string>();
        //            foreach (var item in rowsArray)
        //            {
        //                if (!string.IsNullOrEmpty(item))
        //                    rowsLista.Add(item);
        //            }
        //            Assert.True(rowsLista.Count == 4);
        //            Assert.False(MR.HayError);
        //            Assert.Equal("", MR.MensajeError);
        //            MR.CloseMaestro();
        //        }

        #endregion

        #region ReadMaestro

        [Fact]
        public void ReadMaestro_blanco()
        {
            // Prepara
            MR.DirMaestro = string.Empty;
            MR.NameMaestro = string.Empty;

            // Ejecuta
            MR.StatusMaestro = MR.StatusWrited;
            bool readedOk = MR.ReadMaestro();
            MR.DirMaestro = MR.DirMaestro_Default;
            MR.NameMaestro = MR.NameMaestro_Default;

            // Prueba
            Assert.False(readedOk);
            Assert.True(MR.HayError);
            Assert.Equal("Nombre de archivo Maestro en blanco!", MR.MensajeError);
        }

        [Fact]
        public void ReadMaestro_noExiste()
        {
            // Prepara
            MR.NameMaestro = "_MasterFileNoExiste";
            if(File.Exists(MR.PathMaestro))
                File.Delete(MR.PathMaestro);

            // Ejecuta
            bool readedOk = MR.ReadMaestro();

            // Prueba
            Assert.False(readedOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Archivo Maestro '{MR.PathMaestro}' no existe!", MR.MensajeError);
            MR.NameMaestro = MR.NameMaestro_Default;
        }

        [Fact]
        public void ReadMaestro_nulo()
        {
            // Prepara
            MR.PathMaestro = null;

            // Ejecuta
            bool readedOk = MR.ReadMaestro();
            MR.NameMaestro = MR.NameMaestro_Default;

            // Prueba
            Assert.False(readedOk);
            Assert.True(MR.HayError);
            Assert.Equal("Nombre de archivo Maestro nulo!", MR.MensajeError);
        }

        [Fact]
        public void ReadMaestro_ok()
        {
            // Prepara
            var key1 = "laos|software|google|cuenta|1234|";
            var key2 = "mickey|software|google|cuenta||";
            var key3 = "laos|software|microsoft|cuenta|9876|";
            var key4 = "|software|adobe|readers||";
            var rowEsperada1 = "LAOS|Software|Google|Cuenta|1234|www.google.com|luis.osinaga@gmail.com|password|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            var rowEsperada2 = "Mickey|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            var rowEsperada3 = "LAOS|Software|Microsoft|Cuenta|9876|www.microsoft.com|luis.osinaga@gmail.com|password|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            var rowEsperada4 = "|Software|Adobe|Readers||www.adobe.com|luisalberto|clave|||2019/03/12 10:15:20|2020/08/13 16:49:10|";
            MR.EmptyMaestro();
            MR.NameMaestro = "_MasterFile_Ok";

            // Ejecuta
            bool readedOk = MR.ReadMaestro();

            // Prueba
            MR.NameMaestro = MR.NameMaestro_Default;
            Assert.True(readedOk);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
            var regPwd = MR.TableMaestro[key1];
            Assert.Contains(rowEsperada1, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key2];
            Assert.Contains(rowEsperada2, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key3];
            Assert.Contains(rowEsperada3, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key4];
            Assert.Contains(rowEsperada4, MR.RegistroPwdToRow(regPwd));
        }

        [Fact]
        public void ReadMaestro_vacio()
        {
            // Prepara
            MR.EmptyMaestro();
            MR.NameMaestro = "_MasterFileEmpty";

            // Ejecuta
            bool readedOk = MR.ReadMaestro();

            // Prueba
            MR.NameMaestro = MR.NameMaestro_Default;
            Assert.True(readedOk);
            Assert.False(MR.HayError);
            Assert.Equal(string.Empty, MR.MensajeError);
        }

        #endregion

        #region RegistroPwdToRow

        [Fact]
        public void RegistroPwdToRow_camposConNulos()
        {
            // Prepara
            RegistroPwd regPwd = new RegistroPwd();
            regPwd.UserNombre = "LAOSS";
            regPwd.Categoria = "Software";
            regPwd.Empresa = "Google";
            regPwd.Producto = "Cuenta";
            regPwd.Numero = "1234567";
            regPwd.Web = "www.google.com";
            regPwd.UserId = "luis.osinaga@gmail.com";
            regPwd.UserPwd = "password";
            regPwd.UserEMail = null;
            regPwd.UserNota = null;
            regPwd.CreateDate = new DateTime(2019, 03, 12, 10, 15, 20);
            regPwd.UpdateDate = new DateTime(2020, 08, 13, 16, 49, 10);
            regPwd.RegId = "100";
            string rowEsperada = "LAOSS|Software|Google|Cuenta|1234567|www.google.com|luis.osinaga@gmail.com|password|||2019/03/12 10:15:20|2020/08/13 16:49:10";

            // Ejecuta
            string row = MR.RegistroPwdToRow(regPwd);

            // Prueba
            Assert.Contains(rowEsperada, row);
            Assert.False(MR.HayError);
            Assert.Empty(MR.MensajeError);
        }

        [Fact]
        public void RegistroPwdToRow_camposNoInicializados()
        {
            // Prepara
            RegistroPwd regPwd = new RegistroPwd();
            string rowEsperada = "||||||||||0001/01/01 00:00:00|0001/01/01 00:00:00||";

            // Ejecuta
            string row = MR.RegistroPwdToRow(regPwd);

            // Prueba
            Assert.Equal(rowEsperada, row);
            Assert.False(MR.HayError);
            Assert.Empty(MR.MensajeError);
        }

        [Fact]
        public void RegistroPwdToRow_camposOk()
        {
            // Prepara
            RegistroPwd regPwd = new RegistroPwd();
            regPwd.UserNombre = "LLAOS";
            regPwd.Categoria = "Software";
            regPwd.Empresa = "Google";
            regPwd.Producto = "Cuenta";
            regPwd.Numero = "333444";
            regPwd.Web = "www.google.com";
            regPwd.UserId = "luis.osinaga@gmail.com";
            regPwd.UserPwd = "password";
            regPwd.UserEMail = "";
            regPwd.UserNota = "";
            regPwd.CreateDate = new DateTime(2019, 03, 12, 10, 15, 20);
            regPwd.UpdateDate = new DateTime(2020, 08, 13, 16, 49, 10);
            regPwd.RegId = "0";
            string rowEsperada = "LLAOS|Software|Google|Cuenta|333444|www.google.com|luis.osinaga@gmail.com|password|||2019/03/12 10:15:20|2020/08/13 16:49:10";

            // Ejecuta
            string row = MR.RegistroPwdToRow(regPwd);

            // Prueba
            Assert.Contains(rowEsperada, row);
            Assert.False(MR.HayError);
            Assert.Empty(MR.MensajeError);
        }

        [Fact]
        public void RegistroPwdToRow_regPwdNulo()
        {
            // Prepara

            // Ejecuta
            string row = MR.RegistroPwdToRow(null);

            // Prueba
            Assert.Null(row);
            Assert.True(MR.HayError);
            Assert.Equal("Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        #endregion

        #region RowsToTable

        [Fact]
        public void RowsToTable_rowsNoRow()
        {
            // Prepara
            var rows = new string[] { };
            var mensajeEsperado = "Error: rows sin registros, en ManejadorRegistros.RowsToTable!";

            // Ejecuta
            bool resultOk = MR.RowsToTable(rows);

            // Prueba
            Assert.False(resultOk);
            Assert.True(MR.HayError);
            Assert.Equal(mensajeEsperado, MR.MensajeError);
        }

        [Fact]
        public void RowsToTable_rowsNull()
        {
            // Prepara
            var mensajeEsperado = "Error: rows null, en ManejadorRegistros.RowsToTable!";

            // Ejecuta
            bool resultOk = MR.RowsToTable(null);

            // Prueba
            Assert.False(resultOk);
            Assert.True(MR.HayError);
            Assert.Equal(mensajeEsperado, MR.MensajeError);
        }

        [Fact]
        public void RowsToTable_rowsOk()
        {
            // Prepara
            var rows = new string[]
                { $"{MR.KeyMaestro}",
                          "LAOS|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||2019/03/12 10:15:20|2020/08/13 16:49:10|",
                          "Mickey|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||||",
                          "LAOS|Software|Microsoft|Cuenta||www.microsoft.com|luis.osinaga@gmail.com|password|||||",
                          "|Software|Adobe|Readers||www.adobe.com|luisalberto|clave|||2019/03/12 10:15:20|2020/08/13 16:49:10|"
                };
            var key1 = "laos|software|google|cuenta||";
            var key2 = "mickey|software|google|cuenta||";
            var key3 = "laos|software|microsoft|cuenta||";
            var key4 = "|software|adobe|readers||";
            var rowEsperada1 = "LAOS|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||2019/03/12 10:15:20|2020/08/13 16:49:10|";
            var rowEsperada2 = "Mickey|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            var rowEsperada3 = "LAOS|Software|Microsoft|Cuenta||www.microsoft.com|luis.osinaga@gmail.com|password|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            var rowEsperada4 = "|Software|Adobe|Readers||www.adobe.com|luisalberto|clave|||2019/03/12 10:15:20|2020/08/13 16:49:10|";

            // Ejecuta
            bool resultOk = MR.RowsToTable(rows);

            // Prueba
            Assert.True(resultOk);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
            Assert.Equal(4, MR.TableMaestro.Count);
            var regPwd = MR.TableMaestro[key1];
            Assert.Contains(rowEsperada1, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key2];
            Assert.Contains(rowEsperada2, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key3];
            Assert.Contains(rowEsperada3, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key4];
            Assert.Contains(rowEsperada4, MR.RegistroPwdToRow(regPwd));
        }

        [Fact]
        public void RowsToTable_rowsVacias()
        {
            // Prepara
            MR.ClearTableMaestro();
            var rows = new string[]
                { $"{MR.KeyMaestro}",
                          "",
                          "",
                          "",
                          "",
                          ""
                };

            // Ejecuta
            bool resultOk = MR.RowsToTable(rows);

            // Prueba
            Assert.True(resultOk);
            Assert.True(MR.TableMaestro.Count == 0);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        #endregion

        #region RowToRegistroPwd

        [Fact]
        public void RowToRegistroPwd_camposOk()
        {
            // Prepara
            string rowEsperada = "LAOS|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||2019/03/12 10:15:20|2020/08/13 16:04:00";

            // Ejecuta
            RegistroPwd regPwd = MR.RowToRegistroPwd(rowEsperada);
            bool hayError = MR.HayError;
            bool isEmpty = (MR.MensajeError == string.Empty);

            // Prueba
            string row = MR.RegistroPwdToRow(regPwd);
            Assert.Contains(rowEsperada, row);
            Assert.False(hayError);
            Assert.True(isEmpty);
        }

        [Fact]
        public void RowToRegistroPwd_camposIncompletos()
        {
            // Prepara
            string rowIn = "|||||||";
            string rowEsperada = "||||||||||0001/01/01 00:00:00|0001/01/01 00:00:00||";

            // Ejecuta
            RegistroPwd regPwd = MR.RowToRegistroPwd(rowIn);
            bool hayError = MR.HayError;
            bool isEmpty = MR.MensajeError == string.Empty;

            // Prueba
            string row = MR.RegistroPwdToRow(regPwd);
            Assert.Equal(rowEsperada, row);
            Assert.False(hayError);
            Assert.True(isEmpty);
        }

        [Fact]
        public void RowToRegistroPwd_camposVacios()
        {
            // Prepara
            string rowIn = "||||||||||";
            string rowEsperada = "||||||||||0001/01/01 00:00:00|0001/01/01 00:00:00||";

            // Ejecuta
            RegistroPwd regPwd = MR.RowToRegistroPwd(rowIn);
            bool hayError = MR.HayError;
            bool isEmpty = MR.MensajeError == string.Empty;

            // Prueba
            string row = MR.RegistroPwdToRow(regPwd);
            Assert.Equal(rowEsperada, row);
            Assert.False(hayError);
            Assert.True(isEmpty);
        }

        #endregion

        #region TableToRows

        [Fact]
        public void TableToRows_regsOk()
        {
            // Prepara
            MR.TableMaestro.Clear();
            RegistroPwd regPwd1 = new RegistroPwd
            {
                Categoria = "Soft",
                Producto = "Doodly",
                UserId = "Albertosky",
                UserPwd = "infinitamente"
            };
            RegistroPwd regPwd2 = new RegistroPwd
            {
                Categoria = "Card",
                Producto = "Mastercard",
                UserId = "",
                UserPwd = "5323"
            };
            var rowEsperada1 = "|Soft||Doodly|||Albertosky|infinitamente|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            var rowEsperada2 = "|Card||Mastercard||||5323|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd1)] = regPwd1;
            MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd2)] = regPwd2;

            // Ejecuta
            var rows = MR.TableToRows();

            // Prueba
            Assert.False(MR.HayError);
            Assert.Empty(MR.MensajeError);
            Assert.Contains(rowEsperada1, MR.RegistroPwdToRow(MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd1)]));
            Assert.Contains(rowEsperada2, MR.RegistroPwdToRow(MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd2)]));
        }

        #endregion

        #region WriteMaestro

        [Fact]
        public void WriteMaestro_Ok()
        {
            // Prepara
            var vaciadoMaestroOk = MR.EmptyMaestro();
            Assert.True(vaciadoMaestroOk);
            bool readedMaestroOk = MR.ReadMaestro();
            Assert.True(readedMaestroOk);

            // Ejecuta
            bool writeOk = MR.WriteMaestro();

            // Prueba
            Assert.True(writeOk);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void WriteMaestro_vacio()
        {
            // Prepara
            var vaciadoMaestroOk = MR.EmptyMaestro();
            Assert.True(vaciadoMaestroOk);
            var readedMaestroOk = MR.ReadMaestro();
            Assert.True(readedMaestroOk);

            // Ejecuta
            bool writeOk = MR.WriteMaestro();

            // Prueba
            Assert.True(writeOk);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void WriteMaestro_writed()
        {
            // Nota:
            // El archivo Maestro solo puede escribirse una sola vez despues de leido.
            // Para volver a escribirse, debe ser primero leido.

            // Prepara
            MR.StatusMaestro = MR.StatusWrited;

            // Ejecuta
            bool okWrited = MR.WriteMaestro();

            // Prueba
            Assert.False(okWrited);
            Assert.True(MR.HayError);
            Assert.Equal("Maestro no leido o ya escrito con anterioridad!", MR.MensajeError);
        }

        #endregion
    }

    #region Footer
}
#endregion
