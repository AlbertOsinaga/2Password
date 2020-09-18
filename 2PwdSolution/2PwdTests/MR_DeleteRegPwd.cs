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

    public class MR_DeleteRegPwd
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
            bool deleteOk = MR.DeleteRegPwd(regPwd);

            // Probar
            Assert.False(deleteOk);
            //Assert.True(MR.HayError);
            //Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void NoInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void Nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{G.RegNull}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void Ok()
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
            MR.ClearTableMaestro();
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
        public void Ok_enMaestro()
        {
            // Preparar
            MR.NameMaestro = "_FileDelete_Ok";
            MR.EmptyMaestro();
            var regPwd = new RegistroPwd
            {
                UserNombre = "Erick",
                Categoria = "Programas",
                Producto = "SharePoint",
                UserId = "erick_ronald",
                UserPwd = "Xanadu",
                UserEMail = "erick@gmail.com"
            };
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd);
            var regPwdDel = new RegistroPwd
            {
                UserNombre = "Erick",
                Categoria = "Programas",
                Producto = "SharePoint"
            };

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(regPwdDel);  // == MR.CreateRegPwd(regPwd, enMaestro: true);

            // Probar
            //var regPwdGet = MR.RetrieveRegPwd(regPwdDel);
            Assert.True(deleteOk);
            //Assert.Null(regPwdGet);
            //Assert.False(MR.HayError);
            //Assert.Equal("", MR.MensajeError);
            MR.NameMaestro = MR.NameMaestro_Default;
        }

        [Fact]
        public void Row_noInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();
            string rowPwd = MR.RegistroPwdToRow(regPwd);

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void Row_nulo()
        {
            // Preparar
            string rowPwd = null;

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{G.RegNull}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void Row_vacia()
        {
            // Preparar
            string rowPwd = "";

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.False(deleteOk);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.DeleteRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void Row_ok()
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
            MR.ClearTableMaestro();
            string rowPwdAdd = MR.CreateRegPwd(rowPwd, enMaestro: false);

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(rowPwdAdd, enMaestro: false);

            // Probar
            Assert.True(deleteOk);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void Row_ok_enMaestro()
        {
            // Preparar
            MR.NameMaestro = "_FileDelete_RowOk";
            MR.EmptyMaestro();
            var regPwd = new RegistroPwd
            {
                UserNombre = "Xime_" + Guid.NewGuid().ToString()[0..8],
                Categoria = "Tools",
                Producto = "Photo",
                UserId = "ximenita",
                UserPwd = "clave ULTRA secreta",
                UserEMail = "xime@gmail.com"
            };
            string rowPwd = MR.RegistroPwdToRow(regPwd);
            MR.CreateRegPwd(rowPwd);

            // Ejecutar
            bool deleteOk = MR.DeleteRegPwd(rowPwd); // == MR.DeleteRegPwd(rowPwd, enMaestro: true);

            // Probar
            Assert.True(deleteOk);
            //Assert.False(MR.HayError);
            //Assert.Equal("", MR.MensajeError);
            //string rowPwdGet = MR.RetrieveRegPwd(rowPwd);
            //Assert.Null(rowPwdGet);
            MR.NameMaestro = MR.NameMaestro_Default;
        }
    }

    #region Footer
}
#endregion
