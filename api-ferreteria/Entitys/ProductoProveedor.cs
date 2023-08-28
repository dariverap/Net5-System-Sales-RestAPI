using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_ferreteria.Entitys
{
    public class ProductoProveedor
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public decimal precioCompra { get; set; }


        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int ProveedorId { get; set; }
    }
}
