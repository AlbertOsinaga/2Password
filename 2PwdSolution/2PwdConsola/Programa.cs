#region Header
using System;
using static System.Console;
using _2PwdClasses;
using PC = _2PwdClasses.ProcesadorComandos;

namespace _2PwdConsola
{
#endregion
   
    static class Programa
    {
        static void Main(string[] args)
        {
            WriteLine("Hello 2Pwd!");
            WriteLine();

            var pwds = PC.Run("LIST");
            WriteLine(pwds);
            WriteLine();

            var adds = PC.Run("ADD: Categ|Empre|Produ|Nombre|usuario|clavesecreta|registro ejemplo!");
            WriteLine(adds);

            pwds = PC.Run("LIST");
            WriteLine(pwds);

            ReadKey();
        }
    }

#region Footer
}
#endregion
