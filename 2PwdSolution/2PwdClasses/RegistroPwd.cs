#region Header
using System;

namespace _2PwdClasses
{
#endregion

    public class RegistroPwd
    {
        public string Categoria { get; set; }
        public string Empresa { get; set; }
        public string Producto { get; set; }
        public string Nombre { get; set; }
        
        public string UserId { get; set; }
        public string UserPwd { get; set; }
        public string UserEMail { get; set; }
        public string WebEmpresa { get; set; }
        public string Nota { get; set; }
       
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public override string ToString()
        {
            return $"{Categoria}|{Empresa}|{Producto}|{Nombre}|{UserId}|{UserPwd}|{UserEMail}|{WebEmpresa}|{Nota}|" + 
                    $"{CreateDate.ToString(Global.FormatoFecha)}|{UpdateDate.ToString(Global.FormatoFecha)}";
        }
    }

#region Footer
}
#endregion
