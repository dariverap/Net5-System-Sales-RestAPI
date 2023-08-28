using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api_ferreteria.Entitys
{
    public class DetalleCompra
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int cantidad { get; set; }
        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public decimal precioCompra { get; set; }
        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public decimal importe { get; set; }
        [Required]
        public bool estado { get; set; }
        [Required]
        public int CompraNumero { get; set; }
        [Required]
        public int ProductoId { get; set; }

    }

    public class ListaDetalleCompra
    {
     
        public int id { get; set; }
        public string nombreProducto { get; set; }
        public int cantidad { get; set; }
        public decimal precioCompra { get; set; }
        public decimal importe { get; set; }
        public bool estado { get; set; }
        public int CompraNumero { get; set; }
        public int ProductoId { get; set; }

    }
}
