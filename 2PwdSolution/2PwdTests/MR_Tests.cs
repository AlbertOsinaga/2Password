#region Header
using System;
using Xunit;

using _2PwdClasses;
using MR = _2PwdClasses.ManejadorRegistros;
using System.Threading.Tasks;
using System.Reflection;

namespace _2PwdTests
{
#endregion
    
    public class MR_Tests
    {
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
    }

    #region Footer
}
#endregion
