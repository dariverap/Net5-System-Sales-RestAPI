using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace api_ferreteria.Entitys
{
    public class Producto
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string nombre { get; set; }
        [Required]
        public string descripcion { get; set; }
        [Required]
        public int stock { get; set; }
        [Required]
        public bool estado { get; set; }
        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public decimal precioVenta { get; set; }



        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public int MarcaId { get; set; }
        public string url { get; set; }
        public List<Detalle> detalle { get; set; }
        public List<DetalleCompra> detalleCompra { get; set; }
        public List<ProductoProveedor> productoProveedor { get; set; }

    }
}
