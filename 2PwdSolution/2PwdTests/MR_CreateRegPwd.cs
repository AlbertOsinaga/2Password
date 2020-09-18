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

namespace _2PwdTests
{
#endregion

    public class MR_CreateRegPwd
    {
        [Fact]
        public void ConCamposNulos()
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
            Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.CreateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void NoInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.CreateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void Nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{G.RegNull}', en ManejadorRegistros.CreateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void Ok()
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
            MR.ClearTableMaestro();

            // Ejecutar
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: false);

            // Probar
            var rowEsperada = $"LAOS|Licenciamientos||Audacity|||luigi_alberto|illimani|luigi@gmail.com||" +
                                $"{regPwd.CreateDate.ToString(G.FormatoFecha)}|0001/01/01 00:00:00";
            Assert.NotNull(regPwdAdd);
            Assert.Contains(rowEsperada, regPwdAdd.ToString());
        }

        [Fact]
        public void Ok_enMaestro()
        {
            // Preparar
            MR.NameMaestro = "_FileCreate_Ok";
            MR.EmptyMaestro();
            var regPwd = new RegistroPwd
            {
                UserNombre = "WARI",
                Categoria = "Licores",
                Producto = "Jony Wolker",
                UserId = "wilmer_luis",
                UserPwd = "ClaveSuperSecreta",
                UserEMail = "wari@gmail.com"
            };

            // Ejecutar
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd); // == RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd, enMaestro: true);
            //bool hayError = MR.HayError;
            //var mensajeError = MR.MensajeError;

            // Probar
            //var regPwdGet = MR.RetrieveRegPwd(regPwd); // ¡= MR.RetrieveRegPwd(regPwd, enMaestro: true);
            Assert.NotNull(regPwdAdd);
            //Assert.NotNull(regPwdGet);
            //Assert.Equal(regPwd.UserNombre, regPwdGet.UserNombre);
            //Assert.False(hayError);
            //Assert.Equal("", mensajeError);
            MR.NameMaestro = MR.NameMaestro_Default;
        }

        [Fact]
        public void Row_noInicializado()
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
        public void Row_nulo()
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
        public void Row_vacia()
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
        public void Row_ok()
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
            MR.ClearTableMaestro();

            // Ejecutar
            string rowPwdAdd = MR.CreateRegPwd(rowPwd, enMaestro: false);

            // Probar
            var regPwdAdd = MR.RowToRegistroPwd(rowPwdAdd);
            var rowEsperada = $"LAOS|Licencias||Audacity|||luis alberto|clave secreta|luis@gmail.com||" +
                                $"{regPwdAdd.CreateDate.ToString(G.FormatoFecha)}|0001/01/01 00:00:00";
            Assert.NotNull(rowEsperada);
            Assert.Contains(rowEsperada, rowPwdAdd);
        }

        [Fact]
        public void Row_ok_enMaestro()
        {
            // Preparar
            MR.NameMaestro = "_FileCreate_Rowok";
            MR.EmptyMaestro();
            var regPwd = new RegistroPwd
            {
                UserNombre = "VAnia",
                Categoria = "Otras Claves",
                Producto = "Mi Casa",
                UserPwd = "9537",
                UserEMail = "vania@gmail.com"
            };
            string rowPwd = MR.RegistroPwdToRow(regPwd);

            // Ejecutar
            string rowPwdAdd = MR.CreateRegPwd(rowPwd);   // == MR.CreateRegPwd(rowPwd, enMaestro: true);
            //bool hayError = MR.HayError;
            //var mensajeError = MR.MensajeError;

            // Probar
            //string rowPwdGet = MR.RetrieveRegPwd(rowPwd);
            Assert.NotNull(rowPwdAdd);
            //Assert.NotNull(rowPwdGet);
            //Assert.Equal(rowPwdGet, rowPwdAdd);
            //Assert.False(hayError);
            //Assert.Equal("", mensajeError);
            MR.NameMaestro = MR.NameMaestro_Default;
        }
    }

#region Footer
}
#endregion
