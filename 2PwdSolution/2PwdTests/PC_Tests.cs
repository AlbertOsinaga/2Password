#region Header
using System;
using System.Collections.Generic;
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
            var comando = PC.Parse(cmd);

            // Prueba
            Assert.NotNull(comando);
            Assert.False(comando.Ok);
            Assert.True(PC.HayError);
            Assert.Equal("Error: cmd no reconcido, en ProcesadorComandos.Parse!", PC.MensajeError);
        }
        [Fact]
        public void Parse_cmdEmpty()
        {
            // Prepara
            string cmd = "";

            // Ejecuta
            var comando = PC.Parse(cmd);

            // Prueba
            Assert.NotNull(comando);
            Assert.False(comando.Ok);
            Assert.True(PC.HayError);
            Assert.Equal("Error: cmd vacio, en ProcesadorComandos.Parse!", PC.MensajeError);
        }
        [Fact]
        public void Parse_cmdNull()
        {
            // Prepara
            string cmd = null;

            // Ejecuta
            var comando = PC.Parse(cmd);

            // Prueba
            Assert.NotNull(comando);
            Assert.False(comando.Ok);
            Assert.True(PC.HayError);
            Assert.Equal("Error: cmd nulo, en ProcesadorComandos.Parse!", PC.MensajeError);
        }
        [Fact]
        public void Parse_cmdList_ok()
        {
            // Prepara
            string cmd = "LiSt";

            // Ejecuta
            var comando = PC.Parse(cmd);

            // Prueba
            Assert.NotNull(comando);
            Assert.Equal("list", comando.Cmd);
            Assert.False(PC.HayError);
            Assert.Equal("", PC.MensajeError);
        }
        [Fact]
        public void Run_cmdList_ok()
        {
            // Prepara
            //ManejadorRegistros.CloseMaestro();
            string cmd = "LiSt";

            // Ejecuta
            var pwds = PC.Run(cmd);

            // Prueba
            Assert.False(string.IsNullOrEmpty(pwds));
            string[] rowsArray = pwds.Split('\n', '\r');
            var rowsLista = new List<string>();
            foreach (var item in rowsArray)
            {
                if (!string.IsNullOrEmpty(item))
                    rowsLista.Add(item);
            }
            Assert.True(rowsLista.Count == 4);
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
            Assert.Equal("Error: cmd nulo, en ProcesadorComandos.Parse!", PC.MensajeError);
        }
    }

#region Footer
}
#endregion