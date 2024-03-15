using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.Utilities
{
    public static class DS
    {
        public const string Success = "Exitoso";
        public const string Error = "Error";

        public const string Role_Admin = "Admin";
        public const string Role_Owner = "Dueño";
        public const string Role_Employee = "Empleado";
        public const string Role_Secretary = "Secretario";

        public const string Audit_Delete = "Eliminar";
        public const string Audit_View = "Ver";
        public const string Audit_Upsert = "Upsert";
    }
}
