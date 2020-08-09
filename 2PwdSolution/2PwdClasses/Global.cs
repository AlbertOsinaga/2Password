#region Header
using System;
using System.Diagnostics;
using static System.Console;

namespace _2PwdClasses
{
#endregion

    public static class Global
    {
        public static string ArmaMensajeError(string mensaje, Exception ex)
        {
            StackFrame CallStack = new StackFrame(1, true);
            return $"{mensaje} en archivo '{CallStack.GetFileName()}', " + 
                    $"linea # {CallStack.GetFileLineNumber()}: " + 
                    $"'{ex.GetType().Name}' - '{ex.Message}'";
        }

        public static void WriteHola() => WriteLine("Hola desde Global!");
    }

#region Footer
}
#endregion
