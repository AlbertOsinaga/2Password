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

    public class MR_UpdateRegPwd
    {
        #region UpdateRegistro

        [Fact]
        public void ConCamposNulos()
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
            Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.UpdateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void NoInicializado()
        {
            // Preparar
            var regPwd = new RegistroPwd();

            // Ejecutar
            RegistroPwd regPwdAdd = MR.UpdateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{MR.KeyVacia}', en ManejadorRegistros.UpdateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void Nulo()
        {
            // Preparar
            RegistroPwd regPwd = null;

            // Ejecutar
            RegistroPwd regPwdAdd = MR.UpdateRegPwd(regPwd, enMaestro: false);

            // Probar
            Assert.Null(regPwdAdd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: key invalida: '{G.RegNull}', en ManejadorRegistros.UpdateRegPwd!", MR.MensajeError);
        }

        [Fact]
        public void Ok()
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
            MR.ClearTableMaestro();
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
        public void Ok_enMaestro()
        {
            // Preparar
            MR.EmptyMaestro();
            var regPwd = new RegistroPwd
            {
                UserNombre = "Peter Pan",
                Categoria = "Misteriosos",
                Producto = "1Password",
                UserId = "peter ustinov",
                UserPwd = "frase dificil",
                UserEMail = "peter@gmail.com"
            };
            RegistroPwd regPwdAdd = MR.CreateRegPwd(regPwd);
            regPwdAdd.UserId = "mario_bross";
            regPwdAdd.UserPwd = "la@clave*mas$secreta";

            // Ejecutar
            RegistroPwd regPwdUpd = MR.UpdateRegPwd(regPwdAdd); // == MR.UpdateRegPwd(regPwdAdd, enMaestro: true);

            // Probar
            Assert.NotNull(regPwdUpd);
            Assert.Equal(regPwdAdd.UserId, regPwdUpd.UserId);
            Assert.Equal(regPwdAdd.UserPwd, regPwdUpd.UserPwd);
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
            string rowPwdUpd = MR.UpdateRegPwd(rowPwd, enMaestro: false);

            // Probar
            Assert.Null(rowPwdUpd);
            Assert.True(MR.HayError);
            Assert.Equal($"Error: reg null, en ManejadorRegistros.RegistroPwdToRow!", MR.MensajeError);
        }

        [Fact]
        public void Row_nulo()
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
        public void Row_vacia()
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
        public void Row_ok()
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
            MR.ClearTableMaestro();
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

        [Fact]
        public void Row_ok_enMaestro()
        {
            // Preparar
            MR.EmptyMaestro();
            var regPwd = new RegistroPwd
            {
                UserNombre = "Amelia",
                Categoria = "Kards",
                Producto = "MasterCard",
                UserId = "Amalia Cadena",
                UserPwd = "8835",
                UserEMail = "acadena@gmail.com"
            };
            var regPwdAdd = MR.CreateRegPwd(regPwd);
            regPwdAdd.UserId = "Amalita";
            regPwdAdd.UserPwd = "1476";
            string rowPwdAdd = MR.RegistroPwdToRow(regPwdAdd);

            // Ejecutar
            string rowPwdUpd = MR.UpdateRegPwd(rowPwdAdd); // == MR.UpdateRegPwd(rowPwdAdd, enMaestro: true);

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
