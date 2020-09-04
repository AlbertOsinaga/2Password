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

namespace _2PwdTests
{
#endregion
    
    public class MR_Tests
    {
        #region CloseMaestro

        [Fact]
        public void CloseMaestro_closed()
        {
            // Prepara
            MR.StatusMaestro = MR.StatusClosed;

            // Ejecuta
            bool okClosed = MR.CloseMaestro();

            // Prueba
            Assert.True(okClosed);
        }

        [Fact]
        public void CloseMaestro_vacio()
        {
            // Prepara
            MR.OpenMaestro("_MasterFileEmpty");

            // Ejecuta
            bool closeOk = MR.CloseMaestro();
            MR.NameMaestro = MR.NameMaestro_Default;

            // Prueba
            Assert.True(closeOk);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        #endregion

        #region CreateRegPwd

        [Fact]
        public void CreateRegPwd_conCamposNulos()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = null
            };

            // Ejecutar
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o duplicada: '{MR.KeyVacia}', en ManejadorRegistros.CreateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void CreateRegPwd_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o duplicada: '{MR.KeyVacia}', en ManejadorRegistros.CreateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void CreateRegPwd_nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o duplicada: '{G.RegNull}', en ManejadorRegistros.CreateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void CreateRegPwd_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                UserNombre = "LAOS",
                Categoria = "Licenciamientos",
                Producto = "Audacity",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com"
            };
            MR.ClearMaestro();

            // Ejecutar
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);

            // Probar
            var rowEsperada = $"LAOS|Licenciamientos||Audacity|||luigi_alberto|illimani|luigi@gmail.com||" +
                                $"{regPwd.CreateDate.ToString(G.FormatoFecha)}|0001/01/01 00:00:00|1";
            Assert.NotNull(regPwdAdd);
            Assert.Equal(rowEsperada, regPwdAdd.ToString());
        }

        [Fact]
        public void CreateRegPwd_ok_enMaestro()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                UserNombre = "WARI",
                Categoria = "Licenciamientos",
                Producto = "Audacity",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com"
            };
            MR.CloseMaestro();

            // Ejecutar
            MR.OpenMaestro();
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd);
            MR.CloseMaestro();


            // Probar
            MR.OpenMaestro();
            var regPwdGet = MR.RetrieveRegPwd(regPwd);
            MR.DeleteRegPwd(regPwd);
            MR.CloseMaestro();

            Assert.NotNull(regPwdAdd);
            Assert.Equal(regPwd.UserNombre, regPwdGet.UserNombre);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void CreateRegPwd_row_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();
            string rowPwd = MR.RegistroPwdToRow(regPwd);

            // Ejecutar
            string rowPwdAdd = MR.CreateRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void CreateRegPwd_row_nulo()
        {
            // Preparar
            string rowPwd = null;

            // Ejecutar
            string rowPwdAdd = MR.CreateRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void CreateRegPwd_row_vacia()
        {
            // Preparar
            string rowPwd = "";

            // Ejecutar
            string rowPwdAdd = MR.CreateRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void CreateRegPwd_row_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                UserNombre = "LAOS",
                Categoria = "Licencias",
                Producto = "Audacity",
                UserId = "luis alberto",
                UserPwd = "clave secreta",
                UserEMail = "luis@gmail.com"
            };
            string rowPwd = MR.RegistroPwdToRow(regPwd);
            MR.ClearMaestro();

            // Ejecutar
            string rowPwdAdd = MR.CreateRegPwd(rowPwd, enMaestro: false);

            // Probar
            var regPwdAdd = MR.RowToRegistroPwd(rowPwdAdd);
            var rowEsperada = $"LAOS|Licencias||Audacity|||luis alberto|clave secreta|luis@gmail.com||" +
                                $"{regPwdAdd.CreateDate.ToString(G.FormatoFecha)}|0001/01/01 00:00:00|1";
            Assert.NotNull(rowEsperada);
            Assert.Equal(rowEsperada, rowPwdAdd);
        }

        #endregion

        #region DeleteRegistro

        [Fact]
        public void DeleteRegPwd_conCamposNulos()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = null
            };

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(regPwd);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o inexistente: '{MR.KeyVacia}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void DeleteRegPwd_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o inexistente: '{MR.KeyVacia}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void DeleteRegPwd_nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o inexistente: '{G.RegNull}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void DeleteRegPwd_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                UserNombre = "Luis",
                Categoria = "Licencias",
                Producto = "Audaciti",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com"
            };
            MR.ClearMaestro();
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);
            var regPwdDel = new RegistroPwd
            {
                UserNombre = "Luis",
                Categoria = "Licencias",
                Producto = "Audaciti"
            };

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(regPwdDel, enMaestro: false);

            // Probar
            Assert.True(deleteOk);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void DeleteRegPwd_row_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();
            string rowPwd = MR.RegistroPwdToRow(regPwd);

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o inexistente: '{MR.KeyVacia}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void DeleteRegPwd_row_nulo()
        {
            // Preparar
            string rowPwd = null;

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o inexistente: '{G.RegNull}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void DeleteRegPwd_row_vacia()
        {
            // Preparar
            string rowPwd = "";

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o inexistente: '{MR.KeyVacia}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void DeleteRegPwd_row_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                UserNombre = "Maria",
                Categoria = "Soft",
                Producto = "Kitchen",
                UserId = "maria",
                UserPwd = "clave secreta",
                UserEMail = "maria@gmail.com"
            };
            string rowPwd = MR.RegistroPwdToRow(regPwd);
            MR.ClearMaestro();
            string rowPwdAdd = MR.CreateRegPwd(rowPwd, enMaestro: false);

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(rowPwdAdd, enMaestro: false);

            // Probar
            Assert.True(deleteOk);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
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
            string keyEsperada = "juana|software|microsoft||";

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
            string keyEsperada = "juan m|software|microsoft|office|";

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

        #region OpenMaestro

        [Fact]
        public void OpenMaestro_blanco()
        {
            // Prepara
            MR.NameMaestro = string.Empty;

            // Ejecuta
            MR.StatusMaestro = MR.StatusClosed;
            bool openOk = MR.OpenMaestro();
            MR.NameMaestro = MR.NameMaestro_Default;

            // Prueba
            Assert.False(openOk);
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
            MR.NameMaestro = MR.NameMaestro_Default;

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
            MR.NameMaestro = MR.NameMaestro_Default;

            // Prueba
            Assert.False(cargaOk);
            Assert.True(MR.HayError);
            Assert.Equal("Archivo Maestro nulo!", MR.MensajeError);
        }

        [Fact]
        public void OpenMaestro_ok()
        {
            // Prepara
            var key1 = "laos|software|google|cuenta|1234";
            var key2 = "mickey|software|google|cuenta|";
            var key3 = "laos|software|microsoft|cuenta|9876";
            var key4 = "|software|adobe|readers|";
            var rowEsperada1 = "LAOS|Software|Google|Cuenta|1234|www.google.com|luis.osinaga@gmail.com|password|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            var rowEsperada2 = "Mickey|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            var rowEsperada3 = "LAOS|Software|Microsoft|Cuenta|9876|www.microsoft.com|luis.osinaga@gmail.com|password|||0001/01/01 00:00:00|0001/01/01 00:00:00|";
            var rowEsperada4 = "|Software|Adobe|Readers||www.adobe.com|luisalberto|clave|||2019/03/12 10:15:20|2020/08/13 16:49:10|";

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
            MR.NameMaestro = MR.NameMaestro_Default;

            // Prueba
            Assert.True(openOk);
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
            string rowEsperada = "LAOSS|Software|Google|Cuenta|1234567|www.google.com|luis.osinaga@gmail.com|password|||2019/03/12 10:15:20|2020/08/13 16:49:10|100";

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
            string rowEsperada = "||||||||||0001/01/01 00:00:00|0001/01/01 00:00:00|";

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
            string rowEsperada = "LLAOS|Software|Google|Cuenta|333444|www.google.com|luis.osinaga@gmail.com|password|||2019/03/12 10:15:20|2020/08/13 16:49:10|0";

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

            // Ejecuta
            string row = MR.RegistroPwdToRow(null);

            // Prueba
            Assert.Null(row);
            Assert.True(MR.HayError);
            Assert.Equal("Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        #endregion

        #region RetrieveRegistro

        [Fact]
        public void RetrieveRegPwd_conCamposNulos()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = null
            };

            // Ejecutar
            RegistroPwd regPwdGet = MR.RetrieveRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdGet);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.RetrieveRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void RetrieveRegPwd_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            RegistroPwd regPwdGet = MR.RetrieveRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdGet);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.RetrieveRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void RetrieveRegPwd_nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            RegistroPwd regPwdGet = MR.RetrieveRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdGet);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{G.RegNull}', en ManejadorRegistros.RetrieveRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void RetrieveRegPwd_ok()
        {
            // Preparar
            var regPwdAdd = new RegistroPwd
            {
                Categoria = "Softw",
                Empresa = "Adobe",
                Producto = "Photosho",
                UserId = "lu_alberto",
                UserPwd = "illampu",
                UserEMail = "lu@gmail.com",
                UserNota = "Licencia 534535353"
            };
            var regPwd = new RegistroPwd
            {
                Categoria = "Softw",
                Empresa = "Adobe",
                Producto = "Photosho"
            };
            MR.ClearMaestro();
            MR.CreateRegPwd(regPwdAdd);

            // Ejecutar
            RegistroPwd regPwdGet = MR.RetrieveRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.NotNull(regPwdGet);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void RetrieveRegPwd_row_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();
            string rowPwd = MR.RegistroPwdToRow(regPwd);

            // Ejecutar
            string rowPwdGet = MR.RetrieveRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdGet);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void RetrieveRegPwd_row_nulo()
        {
            // Preparar
            string rowPwd = null;

            // Ejecutar
            string rowPwdGet = MR.RetrieveRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdGet);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void RetrieveRegPwd_row_vacia()
        {
            // Preparar
            string rowPwd = "";

            // Ejecutar
            string rowPwdGet = MR.RetrieveRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdGet);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void RetrieveRegPwd_row_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                UserNombre = "LALOS",
                Categoria = "Varios",
                Producto = "Excel",
                UserId = "luis alberto",
                UserPwd = "clave secreta",
                UserEMail = "luis@gmail.com"
            };
            string rowPwd = MR.RegistroPwdToRow(regPwd);
            var regPwdAdd = new RegistroPwd
            {
                UserNombre = "LALOS",
                Categoria = "Varios",
                Producto = "Excel",
            };
            MR.ClearMaestro();
            MR.CreateRegPwd(regPwd, enMaestro: false);

            // Ejecutar
            string rowPwdGet = MR.RetrieveRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.NotNull(rowPwdGet);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
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
                          //"LAOS|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||||",
                          "Mickey|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||||",
                          "LAOS|Software|Microsoft|Cuenta||www.microsoft.com|luis.osinaga@gmail.com|password|||||",
                          "|Software|Adobe|Readers||www.adobe.com|luisalberto|clave|||2019/03/12 10:15:20|2020/08/13 16:49:10|"
                };
            var key1 = "laos|software|google|cuenta|";
            var key2 = "mickey|software|google|cuenta|";
            var key3 = "laos|software|microsoft|cuenta|";
            var key4 = "|software|adobe|readers|";
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
            Assert.Equal(rowEsperada1, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key2];
            Assert.Equal(rowEsperada2, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key3];
            Assert.Equal(rowEsperada3, MR.RegistroPwdToRow(regPwd));
            regPwd = MR.TableMaestro[key4];
            Assert.Equal(rowEsperada4, MR.RegistroPwdToRow(regPwd));
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
            string rowEsperada = "LAOS|Software|Google|Cuenta||www.google.com|luis.osinaga@gmail.com|password|||2019/03/12 10:15:20|2020/08/13 16:04:00|";

            // Ejecuta
            RegistroPwd regPwd = MR.RowToRegistroPwd(rowEsperada);
            bool hayError = MR.HayError;
            bool isEmpty = (MR.MensajeError == string.Empty);

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
            string rowIn = "|||||||";
            string rowEsperada = "||||||||||0001/01/01 00:00:00|0001/01/01 00:00:00|";

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
            string rowEsperada = "||||||||||0001/01/01 00:00:00|0001/01/01 00:00:00|";

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
            Assert.Equal(rowEsperada1, MR.RegistroPwdToRow(MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd1)]));
            Assert.Equal(rowEsperada2, MR.RegistroPwdToRow(MR.TableMaestro[MR.KeyOfRegistroPwd(regPwd2)]));
        }

        #endregion

        #region UpdateRegistro

        [Fact]
        public void UpdateRegPwd_conCamposNulos()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                Categoria = null
            };

            // Ejecutar
            RegistroPwd regPwdUpd = MR.UpdateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdUpd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o inexistente: '{MR.KeyVacia}', en ManejadorRegistros.UpdateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void UpdateRegPwd_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            RegistroPwd regPwdAdd = MR.UpdateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o inexistente: '{MR.KeyVacia}', en ManejadorRegistros.UpdateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void UpdateRegPwd_nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            RegistroPwd regPwdAdd = MR.UpdateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida o inexistente: '{G.RegNull}', en ManejadorRegistros.UpdateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void UpdateRegPwd_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                UserNombre = "Pedro",
                Categoria = "Cosas",
                Producto = "Audacity",
                UserId = "luigi_alberto",
                UserPwd = "illimani",
                UserEMail = "luigi@gmail.com"
            };
            MR.ClearMaestro();
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);
            regPwdAdd.UserId = "mario_bross";
            regPwdAdd.UserPwd = "la@clave*mas$secreta";

            // Ejecutar
            RegistroPwd regPwdUpd = MR.UpdateRegPwd(regPwdAdd, enMaestro: false);

            // Probar
            Assert.NotNull(regPwdUpd);
            Assert.Equal(regPwdAdd.UserId, regPwdUpd.UserId);
            Assert.Equal(regPwdAdd.UserPwd, regPwdUpd.UserPwd);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void UpdateRegPwd_row_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();
            string rowPwd = MR.RegistroPwdToRow(regPwd);

            // Ejecutar
            string rowPwdUpd = MR.UpdateRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdUpd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void UpdateRegPwd_row_nulo()
        {
            // Preparar
            string rowPwd = null;

            // Ejecutar
            string rowPwdUpd = MR.UpdateRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdUpd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void UpdateRegPwd_row_vacia()
        {
            // Preparar
            string rowPwd = "";

            // Ejecutar
            string rowPwdUpd = MR.UpdateRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdUpd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void UpdateRegPwd_row_ok()
        {
            // Preparar
            var regPwd = new RegistroPwd
            {
                UserNombre = "LAOSTRA",
                Categoria = "Tarjetas",
                Producto = "Visa",
                UserId = "luis alberto",
                UserPwd = "3433",
                UserEMail = "luis@gmail.com"
            };
            MR.ClearMaestro();
            var regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);
            regPwdAdd.UserId = "luis";
            regPwdAdd.UserPwd = "4523";
            string rowPwdAdd = MR.RegistroPwdToRow(regPwdAdd);


            // Ejecutar
            string rowPwdUpd = MR.UpdateRegPwd(rowPwdAdd, enMaestro: false);

            // Probar
            var regPwdUpd = MR.RowToRegistroPwd(rowPwdUpd);
            Assert.NotNull(rowPwdUpd);
            Assert.Equal(regPwdAdd.UserId, regPwdUpd.UserId);
            Assert.Equal(regPwdAdd.UserPwd, regPwdUpd.UserPwd);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        #endregion
    }

    #region Footer
}
#endregion
