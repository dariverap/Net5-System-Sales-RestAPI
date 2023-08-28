using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace api_ferreteria.Entitys
{
    public class Compra
    {

        [Key]
        public int numero { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime fechaCompra { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal igv { get; set; }
        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public decimal subtotal { get; set; }
        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public decimal total { get; set; }

        [Required]
        public bool estado { get; set; }

        public int ProveedorId { get; set; }
        public List<DetalleCompra> DetalleCompra { get; set; }
    }
    public class RegistroCompra
    {
        public int numero { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime fechaCompra { get; set; }
        public decimal igv { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
        public bool estado { get; set; }

        public int ProveedorId { get; set; }
        public List<DetalleCompra> DetalleCompra { get; set; }
    }

    public class ListaCompra
    {
        public int numero { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime fechaCompra { get; set; }
        public decimal igv { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
        public bool estado { get; set; }
        public string ProveedorRazon { get; set; }
        public int ProveedorId { get; set; }
        public List<DetalleCompra> DetalleCompra { get; set; }
    }
}
