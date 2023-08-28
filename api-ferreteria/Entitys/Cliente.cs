using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace api_ferreteria.Entitys
{
    public class Cliente
    {
        //clave primaria
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength:30)]
        public string nombre { get; set; }
        [Required]
        public int numdocumento { get; set; }
        [Required]
        public bool estado { get; set; }

        public List<Comprobante> comprobante { get; set; }

    }
}
