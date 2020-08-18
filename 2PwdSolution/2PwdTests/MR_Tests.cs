#region Header
using System;
using Xunit;

using _2PwdClasses;
using MR = _2PwdClasses.ManejadorRegistros;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

namespace _2PwdTests
{
#endregion
    
    public class MR_Tests
    {
        [Fact]
        public void AddRegistro_Reg_conCamposNulos()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = null
            };

            // Ejecutar
            bool addOk = MR.AddRegistro(regPwd);

            // Probar
            Assert.False(addOk);
        }

        [Fact]
        public void AddRegistro_Reg_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            bool addOk = MR.AddRegistro(regPwd);

            // Probar
            Assert.False(addOk);
        }

        [Fact]
        public void AddRegistro_Reg_nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            bool addOk = MR.AddRegistro(regPwd);

            // Probar
            Assert.False(addOk);
        }

        [Fact]
        public void AddRegistro_Reg_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = "Programas",
                Producto = "Audacity",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com"
            };

            // Ejecutar
            bool addOk = MR.AddRegistro(regPwd);

            // Probar
            Assert.True(addOk);
            Assert.Equal(regPwd.ToString(), MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd)].ToString());
        }

        [Fact]
        public void AddRegistro_Str_keyNula()
        {
            // Preparar
            string row = null;

            // Ejecutar
            bool addOk = MR.AddRegistro(row);

            // Probar
            Assert.False(addOk);
        }

        [Fact]
        public void AddRegistro_Str_ok()
        {
            // Preparar
            var regPwdAdd = new RegistroPwd
            {
                Categoria = "Sistema",
                Producto = "MacOS",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com",
            };
            var key = MR.KeyOfRegistroPwd(regPwdAdd);
            var row = MR.RegistroPwdToRow(regPwdAdd);

            // Ejecutar
            bool addOk = MR.AddRegistro(row);

            // Probar
            Assert.True(MR.TableMaestro.ContainsKey(key));
            Assert.Equal(regPwdAdd.ToString(), MR.TableMaestro[key].ToString());
        }

        [Fact]
        public void AddRegistro_Str_rowCamposVacios()
        {
            // Preparar
            string row = ",,,,,,,,,,";

            // Ejecutar
            bool addOk = MR.AddRegistro(row);

            // Probar
            Assert.False(addOk);
        }

        [Fact]
        public void AddRegistro_Str_rowVacia()
        {
            // Preparar
            string row = "";

            // Ejecutar
            bool addOk = MR.AddRegistro(row);

            // Probar
            Assert.False(addOk);
        }

        [Fact]
        public void CloseMaestro_noAbierto()
        {
            // Prepara

            // Ejecuta
            bool okClosed = MR.CloseMaestro();

            // Prueba
            Assert.True(okClosed);
        }

        [Fact]
        public void CloseMaestro_vacio()
        {
            // Prepara
            MR.NameMaestro = "_MasterFileEmpty";
            var dir = Environment.CurrentDirectory;

            // Ejecuta
            bool openOk = MR.OpenMaestro();
            bool closeOk = MR.CloseMaestro();

            // Prueba
            Assert.True(closeOk);
            Assert.False(MR.HayError);
            Assert.Equal(string.Empty, MR.MensajeError);
            Assert.True(openOk);
        }

        [Fact]
        public void DelRegistro_Reg_conCamposNulos()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = null
            };

            // Ejecutar
            bool delOk = MR.DelRegistro(regPwd);

            // Probar
            Assert.False(delOk);
        }

        [Fact]
        public void DelRegistro_Reg_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            bool delOk = MR.DelRegistro(regPwd);

            // Probar
            Assert.False(delOk);
        }

        [Fact]
        public void DelRegistro_Reg_nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            bool delOk = MR.DelRegistro(regPwd);

            // Probar
            Assert.False(delOk);
        }

        [Fact]
        public void DelRegistro_Reg_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = "Soft",
                Empresa = "Adobe",
                Producto = "Photoshop",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com"
            };

            // Ejecutar
            MR.AddRegistro(regPwd);
            bool delOk = MR.DelRegistro(regPwd);

            // Probar
            Assert.True(delOk);
            Assert.False(MR.TableMaestro.ContainsKey(MR.KeyOfRegistroPwd(regPwd)));
        }

        [Fact]
        public void DelRegistro_Str_keyNula()
        {
            // Preparar
            string key = null;

            // Ejecutar
            bool delOk = MR.DelRegistro(key);

            // Probar
            Assert.False(delOk);
        }

        [Fact]
        public void DelRegistro_Str_KeyVacia()
        {
            // Preparar
            string key = "|||";

            // Ejecutar
            bool delOk = MR.DelRegistro(key);

            // Probar
            Assert.False(delOk);
        }

        [Fact]
        public void DelRegistro_Str_ok()
        {
            // Preparar
            var regPwdAdd = new RegistroPwd
            {
                Categoria = "PRog",
                Producto = "Photoshop",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com",
                Nota = "Licencia 534535353"
            };
            var key = MR.KeyOfRegistroPwd(regPwdAdd);

            // Ejecutar
            MR.AddRegistro(regPwdAdd);
            bool delOk = MR.DelRegistro(key);
            var regPwdGet = MR.GetRegistro(key);

            // Probar
            Assert.False(MR.TableMaestro.ContainsKey(key));
            Assert.Null(regPwdGet);
        }

        [Fact]
        public void GetRegistro_Reg_conCamposNulos()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = null
            };

            // Ejecutar
            RegistroPwd regPwdGet = MR.GetRegistro(regPwd);

            // Probar
            Assert.Null(regPwdGet);
        }

        [Fact]
        public void GetRegistro_Reg_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            var regPwdGet = MR.GetRegistro(regPwd);

            // Probar
            Assert.Null(regPwdGet);
        }

        [Fact]
        public void GetRegistro_Reg_nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            RegistroPwd regPwdGet = MR.GetRegistro(regPwd);

            // Probar
            Assert.Null(regPwdGet);
        }

        [Fact]
        public void GetRegistro_Reg_ok()
        {
            // Preparar
            var regPwdAdd = new RegistroPwd
            {
                Categoria = "Soft",
                Empresa = "Adobe",
                Producto = "Photoshop",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com",
                Nota = "Licencia 534535353"
            };
            var regPwd = new RegistroPwd
            {
                Categoria = "Soft",
                Empresa = "Adobe",
                Producto = "Photoshop",
            };

            // Ejecutar
            MR.AddRegistro(regPwdAdd);
            RegistroPwd regPwdGet = MR.GetRegistro(regPwd);

            // Probar
            Assert.True(MR.TableMaestro.ContainsKey(MR.KeyOfRegistroPwd(regPwd)));
            Assert.Equal(regPwdAdd.ToString(), regPwdGet.ToString());
        }

        [Fact]
        public void GetRegistro_Str_keyNula()
        {
            // Preparar
            string key = null;

            // Ejecutar
            RegistroPwd regPwdGet = MR.GetRegistro(key);

            // Probar
            Assert.Null(regPwdGet);
        }

        [Fact]
        public void GetRegistro_Str_KeyVacia()
        {
            // Preparar
            string key = "|||";

            // Ejecutar
            var regPwdGet = MR.GetRegistro(key);

            // Probar
            Assert.Null(regPwdGet);
        }

        [Fact]
        public void GetRegistro_Str_ok()
        {
            // Preparar
            var regPwdAdd = new RegistroPwd
            {
                Categoria = "PRog",
                Empresa = "Adobe",
                Producto = "Photoshop",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com",
                Nota = "Licencia 534535353"
            };
            var key = MR.KeyOfRegistroPwd(regPwdAdd);

            // Ejecutar
            MR.AddRegistro(regPwdAdd);
            RegistroPwd regPwdGet = MR.GetRegistro(key);

            // Probar
            Assert.True(MR.TableMaestro.ContainsKey(key));
            Assert.Equal(regPwdAdd.ToString(), regPwdGet.ToString());
        }

        [Fact]
        public void KeyOfRegistroPwd_conCamposNulos()
        {
            // Prepara
            RegistroPwd regPwd = new RegistroPwd();
            regPwd.Categoria = null;
            regPwd.Empresa = null;
            string keyEsperada = "|||";

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
            regPwd.Nombre = "";
            string keyEsperada = "software|microsoft||";

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
            regPwd.Categoria = "Software   ";
            regPwd.Empresa = "Microsoft    ";
            regPwd.Producto = " OFFICE  ";
            regPwd.Nombre = " Juan M  ";
            string keyEsperada = "software|microsoft|office|juan m";

            // Ejecuta
            string key = MR.KeyOfRegistroPwd(regPwd);

            // Prueba
            Assert.Equal(keyEsperada, key);
        }

        [Fact]
        public void KeyOfRegistroPwd_regNulo()
        {
            // Prepara
            string keyEsperada = "null!";

            // Ejecuta
            string key = MR.KeyOfRegistroPwd(null);

            // Prueba
            Assert.Equal(keyEsperada, key);
        }

        [Fact]
        public void OpenMaestro_blanco()
        {
            // Prepara
            MR.NameMaestro = string.Empty;

            // Ejecuta
            bool cargaOk = MR.OpenMaestro();

            // Prueba
            Assert.False(cargaOk);
            Assert.True(MR.HayError);
            Assert.Equal("Archivo Maestro en blanco!", MR.MensajeError);
        }
       
        [Fact]
        public void OpenMaestro_noExiste()
        {
            // Prepara
            MR.NameMaestro = "_MasterFileNoExiste";

            // Ejecuta
            bool cargaOk = MR.OpenMaestro();

            // Prueba
            Assert.False(cargaOk);
            Assert.True(MR.HayError);
            Assert.Equal("Archivo Maestro '_MasterFileNoExiste' no existe!", MR.MensajeError);
        }

        [Fact]
        public void OpenMaestro_nulo()
        {
            // Prepara
            MR.NameMaestro = null;

            // Ejecuta
            bool cargaOk = MR.OpenMaestro();

            // Prueba
            Assert.False(cargaOk);
            Assert.True(MR.HayError);
            Assert.Equal("Archivo Maestro nulo!", MR.MensajeError);
        }

        [Fact]
        public void OpenMaestro_ok()
        {
            // Prepara
            // MR.NameMaestro = "_MasterFile"; // default
            var key1 = "software|google|cuenta|laos";
            var key2 = "software|google|cuenta|mickey";
            var key3 = "software|microsoft|cuenta|laos";
            var key4 = "software|adobe|readers|";
            var rowEsperada1 = "Software,Google,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.google.com,,0001/01/01 00:00:00,0001/01/01 00:00:00";
            var rowEsperada2 = "Software,Google,Cuenta,Mickey,luis.osinaga@gmail.com,password,,www.google.com,,0001/01/01 00:00:00,0001/01/01 00:00:00";
            var rowEsperada3 = "Software,Microsoft,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.microsoft.com,,0001/01/01 00:00:00,0001/01/01 00:00:00";
            var rowEsperada4 = "Software,Adobe,Readers,,luisalberto,clave,,www.adobe.com,,2019/03/12 10:15:20,2020/08/13 16:49:10";
            
            // Ejecuta
            bool openOk = MR.OpenMaestro();

            // Prueba
            Assert.True(openOk);
            Assert.False(MR.HayError);
            Assert.Equal(string.Empty, MR.MensajeError);
            var regPwd = MR.TableMaestro[key1];
            Assert.Equal(rowEsperada1, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key2];
            Assert.Equal(rowEsperada2, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key3];
            Assert.Equal(rowEsperada3, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key4];
            Assert.Equal(rowEsperada4, MR.RegistroPwdToRow(regPwd));
            MR.CloseMaestro();
        }

        [Fact]
        public void OpenMaestro_vacio()
        {
            // Prepara
            MR.NameMaestro = "_MasterFileEmpty";
            var dir = Environment.CurrentDirectory;

            // Ejecuta
            bool openOk = MR.OpenMaestro();
            bool closeOk = MR.CloseMaestro();

            // Prueba
            Assert.True(openOk);
            Assert.False(MR.HayError);
            Assert.Equal(string.Empty, MR.MensajeError);
            Assert.True(closeOk);
        }

        [Fact]
        public void RegistroPwdToRow_camposConNulos()
        {
            // Prepara
            RegistroPwd regPwd = new RegistroPwd();
            regPwd.Categoria = "Software";
            regPwd.Empresa = "Google";
            regPwd.Producto = "Cuenta";
            regPwd.Nombre = "LAOS";
            regPwd.UserId = "luis.osinaga@gmail.com";
            regPwd.UserPwd = "password";
            regPwd.UserEMail = null;
            regPwd.WebEmpresa = "www.google.com";
            regPwd.Nota = null;
            regPwd.CreateDate = new DateTime(2019, 03, 12, 10, 15, 20);
            regPwd.UpdateDate = new DateTime(2020, 08, 13, 16, 49, 10);
            string rowEsperada = "Software,Google,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.google.com,,2019/03/12 10:15:20,2020/08/13 16:49:10";

            // Ejecuta
            string row = MR.RegistroPwdToRow(regPwd);

            // Prueba
            Assert.Equal(rowEsperada, row);
            Assert.False(MR.HayError);
            Assert.Empty(MR.MensajeError);
        }

        [Fact]
        public void RegistroPwdToRow_camposNoInicializados()
        {
            // Prepara
            RegistroPwd regPwd = new RegistroPwd();
            string rowEsperada = ",,,,,,,,,0001/01/01 00:00:00,0001/01/01 00:00:00";

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
            regPwd.Categoria = "Software";
            regPwd.Empresa = "Google";
            regPwd.Producto = "Cuenta";
            regPwd.Nombre = "LAOS";
            regPwd.UserId = "luis.osinaga@gmail.com";
            regPwd.UserPwd = "password";
            regPwd.UserEMail = "";
            regPwd.WebEmpresa = "www.google.com";
            regPwd.Nota = "";
            regPwd.CreateDate = new DateTime(2019, 03, 12, 10, 15, 20);
            regPwd.UpdateDate = new DateTime(2020, 08, 13, 16, 49, 10);
            string rowEsperada = "Software,Google,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.google.com,,2019/03/12 10:15:20,2020/08/13 16:49:10";

            // Ejecuta
            string row = MR.RegistroPwdToRow(regPwd);

            // Prueba
            Assert.Equal(rowEsperada, row);
            Assert.False(MR.HayError);
            Assert.Empty(MR.MensajeError);
        }

        [Fact]
        public void RegistroPwdToRow_regPwdNulo()
        {
            // Prepara
            string rowEsperada = string.Empty;

            // Ejecuta
            string row = MR.RegistroPwdToRow(null);

            // Prueba
            Assert.Equal(rowEsperada, row);
            Assert.True(MR.HayError);
            Assert.Equal("regPWd nulo en ManejadorRegistros.RegistroPwdToRow()!", MR.MensajeError);
        }

        [Fact]
        public void RowsToTable_rowsNulo()
        {
            // Prepara
            var rows = new string[] {};
            var mensajeEsperado = "Error: rows sin registros en ManejadorRegistros.RowsToTable!";

            // Ejecuta
            bool resultOk = MR.RowsToTable(rows);

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
                  "Software,Google,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.google.com,,2019/03/12 10:15:20,2020/08/13 16:49:10",
                  "Software,Google,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.google.com,,,",
                  "Software,Google,Cuenta,Mickey,luis.osinaga@gmail.com,password,,www.google.com,,,",
                  "Software,Microsoft,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.microsoft.com,,,",
                  "Software,Adobe,Readers,,luisalberto,clave,,www.adobe.com,,2019/03/12 10:15:20,2020/08/13 16:49:10"
                };
            var key1 = "software|google|cuenta|laos";
            var key2 = "software|google|cuenta|mickey";
            var key3 = "software|microsoft|cuenta|laos";
            var key4 = "software|adobe|readers|";
            var rowEsperada1 = "Software,Google,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.google.com,,2019/03/12 10:15:20,2020/08/13 16:49:10";
            var rowEsperada2 = "Software,Google,Cuenta,Mickey,luis.osinaga@gmail.com,password,,www.google.com,,0001/01/01 00:00:00,0001/01/01 00:00:00";
            var rowEsperada3 = "Software,Microsoft,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.microsoft.com,,0001/01/01 00:00:00,0001/01/01 00:00:00";
            var rowEsperada4 = "Software,Adobe,Readers,,luisalberto,clave,,www.adobe.com,,2019/03/12 10:15:20,2020/08/13 16:49:10";

            // Ejecuta
            bool resultOk = MR.RowsToTable(rows);

            // Prueba
            Assert.True(resultOk);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
            Assert.Equal(4, MR.TableMaestro.Count);
            var regPwd = MR.TableMaestro[key1];
            Assert.Equal(rowEsperada1, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key2];
            Assert.Equal(rowEsperada2, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key3];
            Assert.Equal(rowEsperada3, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key4];
            Assert.Equal(rowEsperada4, MR.RegistroPwdToRow(regPwd));
        }

        [Fact]
        public void RowsToTable_rowsSinRegistros()
        {
            // Prepara
            var mensajeEsperado = "Error: rows nulo en ManejadorRegistros.RowsToTable!";

            // Ejecuta
            bool resultOk = MR.RowsToTable(null);

            // Prueba
            Assert.False(resultOk);
            Assert.True(MR.HayError);
            Assert.Equal(mensajeEsperado, MR.MensajeError);
        }

        [Fact]
        public void RowsToTable_rowsVacias()
        {
            // Prepara
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
            Assert.Equal(0, MR.TableMaestro.Count);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void RowToRegistroPwd_camposOk()
        {
            // Prepara
            string rowEsperada = "Software,Google,Cuenta,LAOS,luis.osinaga@gmail.com,password,,www.google.com,,2019/03/12 10:15:20,2020/08/13 16:49:10";

            // Ejecuta
            RegistroPwd regPwd = MR.RowToRegistroPwd(rowEsperada);
            bool hayError = MR.HayError;
            bool isEmpty = MR.MensajeError == string.Empty;

            // Prueba
            string row = MR.RegistroPwdToRow(regPwd);
            Assert.Equal(rowEsperada, row);
            Assert.False(hayError);
            Assert.True(isEmpty);
        }

        [Fact]
        public void RowToRegistroPwd_camposIncompletos()
        {
            // Prepara
            string rowIn = ",,,,,,,";
            string rowEsperada = ",,,,,,,,,0001/01/01 00:00:00,0001/01/01 00:00:00";

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
            string rowIn = ",,,,,,,,,,";
            string rowEsperada = ",,,,,,,,,0001/01/01 00:00:00,0001/01/01 00:00:00";

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
            var rowEsperada1 = "Soft,,Doodly,,Albertosky,infinitamente,,,,0001/01/01 00:00:00,0001/01/01 00:00:00";
            var rowEsperada2 = "Card,,Mastercard,,,5323,,,,0001/01/01 00:00:00,0001/01/01 00:00:00";
            MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd1)] = regPwd1;
            MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd2)] = regPwd2;

            // Ejecuta
            var rows = MR.TableToRows();

            // Prueba
            Assert.False(MR.HayError);
            Assert.Empty(MR.MensajeError);
            Assert.Equal(rowEsperada1, MR.RegistroPwdToRow(MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd1)]));
            Assert.Equal(rowEsperada2, MR.RegistroPwdToRow(MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd2)]));
        }

        [Fact]
        public void UpdRegistro_Reg_conCamposNulos()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = null
            };

            // Ejecutar
            bool updOk = MR.UpdRegistro(regPwd);

            // Probar
            Assert.False(updOk);
        }

        [Fact]
        public void UpdRegistro_Reg_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            bool updOk = MR.UpdRegistro(regPwd);

            // Probar
            Assert.False(updOk);
        }

        [Fact]
        public void UpdRegistro_Reg_nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            bool updOk = MR.UpdRegistro(regPwd);

            // Probar
            Assert.False(updOk);
        }

        [Fact]
        public void UpdRegistro_Reg_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = "Soft",
                Empresa = "Microsoft",
                Producto = "Word",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com"
            };

            // Ejecutar
            MR.AddRegistro(regPwd);
            regPwd.UserPwd = "periquito";
            bool updOk = MR.UpdRegistro(regPwd);

            // Probar
            Assert.True(updOk);
            Assert.Equal("periquito", MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd)].UserPwd);
            Assert.Equal(regPwd.ToString(), MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd)].ToString());
        }
        [Fact]

        public void UpdRegistro_Str_keyNula()
        {
            // Preparar
            string row = null;

            // Ejecutar
            bool updOk = MR.UpdRegistro(row);

            // Probar
            Assert.False(updOk);
        }

        [Fact]
        public void UpdRegistro_Str_ok()
        {
            // Preparar
            var regPwdAdd = new RegistroPwd
            {
                Categoria = "Systems",
                Producto = "Windows",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com",
            };
            var key = MR.KeyOfRegistroPwd(regPwdAdd);
            MR.AddRegistro(regPwdAdd);
            regPwdAdd.UserPwd = "Kantutani";
            var row = MR.RegistroPwdToRow(regPwdAdd);

            // Ejecutar
            bool updOk = MR.UpdRegistro(row);

            // Probar
            Assert.True(updOk);
            Assert.Equal("Kantutani", MR.TableMaestro[key].UserPwd);
            Assert.Equal(regPwdAdd.ToString(), MR.TableMaestro[key].ToString());
        }

        [Fact]
        public void UpdRegistro_Str_rowCamposVacios()
        {
            // Preparar
            string row = ",,,,,,,,,,";

            // Ejecutar
            bool updOk = MR.UpdRegistro(row);

            // Probar
            Assert.False(updOk);
        }

        [Fact]
        public void UpdRegistro_Str_rowVacia()
        {
            // Preparar
            string row = "";

            // Ejecutar
            bool updOk = MR.UpdRegistro(row);

            // Probar
            Assert.False(updOk);
        }
    }

    #region Footer
}
#endregion
