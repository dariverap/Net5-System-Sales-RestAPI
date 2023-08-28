using System.ComponentModel.DataAnnotations;

namespace api_ferreteria.Entitys
{
    public class ListaEmpleado
    {
        public int id { get; set; }
      
        public string nombre { get; set; }

        public string apellido { get; set; }

        public bool estado { get; set; }

        public string nombreRol { get; set; }


    }
}
