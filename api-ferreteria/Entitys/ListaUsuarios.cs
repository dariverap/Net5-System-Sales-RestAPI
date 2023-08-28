using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_ferreteria.Entitys
{
    public class ListaUsuarios
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string contraseña { get; set; }
        public bool estado { get; set; }
        public string nombreEmpleado { get; set; }
        public string apellidoEmpleado { get; set; }
        public string rol { get; set; }
        public byte[] StoredSalt
        {
            get; set;
        }
    }
}
