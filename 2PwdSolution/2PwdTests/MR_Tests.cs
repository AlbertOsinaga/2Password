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
        public void AbrirMaestro_blanco()
        {
            // Prepara
            MR.ArchivoMaestro = string.Empty;

            // Ejecuta
            bool cargaOk = MR.AbrirMaestro();

            // Prueba
            Assert.False(cargaOk);
            Assert.True(MR.HayError);
            Assert.Equal("Archivo Maestro en blanco!", MR.MensajeError);
        }
       
        [Fact]
        public void AbrirMaestro_noExiste()
        {
            // Prepara
            MR.ArchivoMaestro = "_MasterFileNoExiste";

            // Ejecuta
            bool cargaOk = MR.AbrirMaestro();

            // Prueba
            Assert.False(cargaOk);
            Assert.True(MR.HayError);
            Assert.Equal("Archivo Maestro '_MasterFileNoExiste' no existe!", MR.MensajeError);
        }

        [Fact]
        public void AbrirMaestro_nulo()
        {
            // Prepara
            MR.ArchivoMaestro = null;

            // Ejecuta
            bool cargaOk = MR.AbrirMaestro();

            // Prueba
            Assert.False(cargaOk);
            Assert.True(MR.HayError);
            Assert.Equal("Archivo Maestro nulo!", MR.MensajeError);
        }

        [Fact]
        public void AbrirMaestro_vacio()
        {
            // Prepara
            MR.ArchivoMaestro = "_MasterFileEmpty";
            var dir = Environment.CurrentDirectory;

            // Ejecuta
            bool cargaOk = MR.AbrirMaestro();

            // Prueba
            Assert.True(cargaOk);
            Assert.True(MR.MaestroAbierto());
            Assert.False(MR.HayError);
            Assert.Equal(string.Empty, MR.MensajeError);
        }
    }

    #region Footer
}
#endregion
