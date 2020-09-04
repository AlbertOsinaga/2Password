#region Header
using System;

namespace _2PwdClasses
{
#endregion

    public class RegistroPwd
    {
        public RegistroPwd()
        {
            UserNombre = "";
            Categoria = "";
            Empresa = "";
            Producto = "";
            Numero = "";
            Web = "";
            UserId = "";
            UserPwd = "";
            UserEMail = "";
            UserNota = "";
            CreateDate = DateTime.MinValue;
            UpdateDate = DateTime.MinValue;
        }

        public string UserNombre { get; set; }
        public string Categoria { get; set; }
        public string Empresa { get; set; }
        public string Producto { get; set; }
        public string Numero { get; set; }

        public string Web { get; set; }
        public string UserId { get; set; }
        public string UserPwd { get; set; }
        public string UserEMail { get; set; }
        public string UserNota { get; set; }
       
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string RegId { get; set; }

        public override string ToString()
        {
            return $"{UserNombre}|{Categoria}|{Empresa}|{Producto}|{Numero}|{Web}|{UserId}|{UserPwd}|{UserEMail}|{UserNota}|" + 
                    $"{CreateDate.ToString(Global.FormatoFecha)}|{UpdateDate.ToString(Global.FormatoFecha)}|{RegId}";
        }
    }

#region Footer
}
#endregion
