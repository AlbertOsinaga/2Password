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
        [Fact]
        public void Parse_add_uno_ok_comillas()
        {
            // Prepara
            var lineaComando = "add uno:\"Juan Perez\"";
            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("add", regCmd.Cmd);
            Assert.Equal("Juan Perez", regCmd.Arg);
        }

        [Fact]
        public void Parse_add_uno_ok_noComillas_endLine()
        {
            // Prepara
            var lineaComando = "add uno:Juan_Perez";
            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("add", regCmd.Cmd);
            Assert.Equal("Juan_Perez", regCmd.Arg);
        }

        [Fact]
        public void Parse_add_uno_ok_noComillas_espacio()
        {
            // Prepara
            var lineaComando = "add uno:Juan Perez";
            // Ejecuta
            var regCmd = PC.Parse(lineaComando);

            // Prueba
            Assert.True(regCmd.Ok);
            Assert.Equal("add", regCmd.Cmd);
            Assert.Equal("Juan", regCmd.Arg);
        }
    }

    #region Footer
}
#endregion
