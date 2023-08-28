using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_ferreteria.Entitys
{
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Ruc { get; set; }
        [Required]
        public string Razon { get; set; }
        
        public string Email { get; set; }
        [Required]
        public string NombreContacto { get; set; }
        [Required]
        public string TelefonoContacto { get; set; }

        [Required]
        public bool estado { get; set; }
        public List<Compra> compra { get; set; }

        public List<ProductoProveedor> productoProveedor { get; set; }
    }
}
