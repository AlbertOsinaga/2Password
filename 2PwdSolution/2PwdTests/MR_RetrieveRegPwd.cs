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

    public class MR_RetrieveRegPwd
    {
        #region RetrieveRegistro

        [Fact]
        public void ConCamposNulos()
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
        public void NoInicializado()
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
        public void Nulo()
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
        public void Ok()
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
            MR.ClearTableMaestro();
            MR.CreateRegPwd(regPwdAdd, enMaestro: false);

            // Ejecutar
            RegistroPwd regPwdGet = MR.RetrieveRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.NotNull(regPwdGet);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void Ok_enMaestro()
        {
            // Preparar
            MR.EmptyMaestro();
            var regPwdAdd = new RegistroPwd
            {
                Categoria = "Keys",
                Empresa = "Adobe",
                Producto = "Photosho",
                UserId = "lu_alberto",
                UserPwd = "illampu",
                UserEMail = "lu@gmail.com",
                UserNota = "Licencia 534535353"
            };
            var regPwd = new RegistroPwd
            {
                Categoria = "Keys",
                Empresa = "Adobe",
                Producto = "Photosho"
            };
            MR.CreateRegPwd(regPwdAdd);
            MR.WriteMaestro();

            // Ejecutar
            RegistroPwd regPwdGet = MR.RetrieveRegPwd(regPwd); // == MR.RetrieveRegPwd(regPwd, enMaestro: true);

            // Probar
            Assert.NotNull(regPwdGet);
            Assert.Equal(regPwdAdd.UserId, regPwdGet.UserId);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void Row_noInicializado()
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
        public void Row_nulo()
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
        public void Row_vacia()
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
        public void Row_ok()
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
            MR.ClearTableMaestro();
            MR.CreateRegPwd(regPwd, enMaestro: false);

            // Ejecutar
            string rowPwdGet = MR.RetrieveRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.NotNull(rowPwdGet);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        [Fact]
        public void Row_ok_enMaestro()
        {
            // Preparar
            MR.EmptyMaestro();
            var regPwd = new RegistroPwd
            {
                UserNombre = "PILATOS",
                Categoria = "Varios",
                Producto = "Excel",
                UserId = "luis alberto",
                UserPwd = "clave secreta",
                UserEMail = "luis@gmail.com"
            };
            string rowPwd = MR.RegistroPwdToRow(regPwd);
            string rowPwdAdd = MR.CreateRegPwd(rowPwd);
            MR.WriteMaestro();

            // Ejecutar
            string rowPwdGet = MR.RetrieveRegPwd(rowPwd); // == MR.RetrieveRegPwd(rowPwd, enMaestro: true);

            // Probar
            Assert.NotNull(rowPwdGet);
            Assert.Equal(rowPwdGet, rowPwdAdd);
            Assert.False(MR.HayError);
            Assert.Equal("", MR.MensajeError);
        }

        #endregion

    }

    #region Footer
}
#endregion
