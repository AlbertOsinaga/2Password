#region Header
using System;
using Xunit;

using _2PwdClasses;
using G = _2PwdClasses.Global;
using MR = _2PwdClasses.ManejadorRegistros;
using PC = _2PwdClasses.ProcesadorComandos;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace _2PwdTests
{
#endregion
    
    public class PC_Tests
    {
       //[Fact]
       //public void Test1()
       // {
       //     // Prepara
       //     var linea = "add -nom Luis Alberto -cat cat Software -emp Microsoft -cta Azure -nro Cuenta 1 " +
       //                 " -web web.azure.com -uid laos -pwd clavesecreta -ema luis.@gmail.com -not prueba de Registro";

       //     // Ejecuta
       //     PC.Parse(linea);

       //     // Prueba

       // }
        
        [Fact]
        public void Parse_add_ok_espacios()
        {
            // Prepara
            var lineaComando = "add -nom Juan Perez -uid jperez -pwd secreto!";

            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("add", regCmd.Cmd);
            Assert.Equal("Juan Perez||||||jperez|secreto!||", regCmd.Arg);
        }

        [Fact]
        public void Parse_add_ok_full()
        {
            // Prepara
            var lineaComando = "add -cta Microsoft Azure -nro #123001 -nom Luis Alberto -cat Software -emp Microsoft " +
                        " -web web.azure.com -uid laos -pwd supersecreta -ema luis.@gmail.com -not prueba de registro";

            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("add", regCmd.Cmd);
            Assert.Equal("Luis Alberto|Software|Microsoft|Microsoft Azure|#123001|web.azure.com|laos|supersecreta|luis.@gmail.com|prueba de registro", regCmd.Arg);
        }

        [Fact]
        public void Parse_add_ok_noEspacios()
        {
            // Prepara
            var lineaComando = "add -cta Camaleon -uid carlos.lopez -pwd facil_clave -web www.camaleon.com -not todo es igual!";
            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("add", regCmd.Cmd);
            Assert.Equal("|||Camaleon||www.camaleon.com|carlos.lopez|facil_clave||todo es igual!", regCmd.Arg);
        }

        [Fact]
        public void Parse_del_ok_full()
        {
            // Prepara
            var lineaComando = "del -nom Luis Alberto -cat Software -emp Microsoft -cta Microsoft Azure -nro 0001";

            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("del", regCmd.Cmd);
            Assert.Equal("Luis Alberto|Software|Microsoft|Microsoft Azure|0001", regCmd.Arg);
        }

        [Fact]
        public void Parse_get_ok_full()
        {
            // Prepara
            var lineaComando = "get -nom Luis Alberto -cat Software -emp Microsoft -cta Microsoft Azure -nro 0001";

            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("get", regCmd.Cmd);
            Assert.Equal("Luis Alberto|Software|Microsoft|Microsoft Azure|0001", regCmd.Arg);
        }

        [Fact]
        public void Parse_lst_ok_full()
        {
            // Prepara
            var lineaComando = "lst -nom Luis Alberto -cat Software -emp Microsoft -cta Microsoft Azure -nro 0001";

            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("lst", regCmd.Cmd);
            Assert.Equal("Luis Alberto|Software|Microsoft|Microsoft Azure|0001", regCmd.Arg);
        }

        [Fact]
        public void Parse_upd_ok_full()
        {
            // Prepara
            var lineaComando = "upd -cta Microsoft Azure -nro #123001 -nom Luis Alberto -cat Software -emp Microsoft " +
                        " -web web.azure.com -uid laos -pwd supersecreta -ema luis.@gmail.com -not prueba de registro " +
                        "-fcr -fup -rid";

            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("upd", regCmd.Cmd);
            Assert.Equal("Luis Alberto|Software|Microsoft|Microsoft Azure|#123001|web.azure.com|laos|supersecreta|luis.@gmail.com|prueba de registro|||", regCmd.Arg);
        }
    }

    #region Footer
}
#endregion
