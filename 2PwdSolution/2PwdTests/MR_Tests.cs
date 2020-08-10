#region Header
using System;
using Xunit;

using _2PwdClasses;
using MR = _2PwdClasses.ManejadorRegistros;
using System.Threading.Tasks;

namespace _2PwdTests
{
#endregion
    
    public class MR_Tests
    {
        [Fact]
        public void OpenMaestro_blanco()
        {
            // Prepara
            MR.FileNameMaestro = string.Empty;

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
            MR.FileNameMaestro = "_MasterFileNoExiste";

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
            MR.FileNameMaestro = null;

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
            MR.FileNameMaestro = "_MasterFileEmpty";
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
    }

    #region Footer
}
#endregion
