#region Header
using System;
using Xunit;

using _2PwdClasses;
using PC = _2PwdClasses.ProcesadorComandos;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;

namespace _2PwdTests
{
    #endregion

    public class PC_Tests
    {
        [Fact]
        public void Parse_cmdBad()
        {
            // Prepara
            string cmd = "dadadasd";

            // Ejecuta
            string[] result = PC.Parse(cmd);

            // Prueba
            Assert.NotNull(result);
            Assert.True(result.Length == 0);
            Assert.True(PC.HayError);
            Assert.Equal("Error: cmd no reconcido, en ProcesadorComandos.Parse!", PC.MensajeError);
        }
        [Fact]
        public void Parse_cmdEmpty()
        {
            // Prepara
            string cmd = "";

            // Ejecuta
            string[] result = PC.Parse(cmd);

            // Prueba
            Assert.Null(result);
            Assert.True(PC.HayError);
            Assert.Equal("Error: cmd vacio, en ProcesadorComandos.Parse!", PC.MensajeError);
        }
        [Fact]
        public void Parse_cmdNull()
        {
            // Prepara
            string cmd = null;

            // Ejecuta
            string[] result = PC.Parse(cmd);

            // Prueba
            Assert.Null(result);
            Assert.True(PC.HayError);
            Assert.Equal("Error: cmd nulo, en ProcesadorComandos.Parse!", PC.MensajeError);
        }
        [Fact]
        public void Parse_cmdList_ok()
        {
            // Prepara
            string cmd = "LiSt";

            // Ejecuta
            string[] result = PC.Parse(cmd);

            // Prueba
            Assert.NotNull(result);
            Assert.True(result.Length == 1);
            Assert.Equal("list", result[0]);
            Assert.False(PC.HayError);
            Assert.Equal("", PC.MensajeError);
        }
        [Fact]
        public void Run_cmdNull()
        {
            // Prepara
            string cmd = null;

            // Ejecuta
            string result = PC.Run(cmd);

            // Prueba
            Assert.Empty(result);
            Assert.True(PC.HayError);
            Assert.Equal("Error: cmd nulo, en ProcesadorComandos.Run!", PC.MensajeError);
        }
    }

    #region Footer
}
#endregion