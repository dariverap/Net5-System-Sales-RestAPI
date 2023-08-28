using api_ferreteria.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_ferreteria.Controllers
{
    [ApiController]
    [Route("api-ferreteria/detallecompra")]
    public class DetalleCompraController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public DetalleCompraController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<DetalleCompra>>> FindAll()
        {
            return await context.DetalleCompra.ToListAsync();
        }

        [HttpGet("custom")]
        public async Task<ActionResult<List<DetalleCompra>>> FindAllCustom()
        {
            return await context.DetalleCompra.Where(x => x.estado == true).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Add(DetalleCompra detalleCompra)
        {
            var comprobanteExiste = await context.Compra.AnyAsync(x => x.numero == detalleCompra.CompraNumero);
            var productoExiste = await context.Producto.AnyAsync(x => x.id == detalleCompra.ProductoId);

            if (!comprobanteExiste)
            {
                return BadRequest($"No existe el comprobante de compra con número : {detalleCompra.CompraNumero}");
            }
            if (!productoExiste)
            {
                return BadRequest($"No existe el producto con ID : {detalleCompra.ProductoId}");
            }

            context.Add(detalleCompra);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleCompra>> FindById(int id)
        {
            var detalleCompra = await context.DetalleCompra.FirstOrDefaultAsync(x => x.id == id);
            return detalleCompra;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(DetalleCompra detalleCompra, int id)
        {
            if (detalleCompra.id != id)
            {
                return BadRequest("No se encontró el código correspondiente");
            }

            context.Update(detalleCompra);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var detalleCompra = await context.DetalleCompra.FirstOrDefaultAsync(x => x.id == id);
            if (detalleCompra == null)
            {
                return NotFound();
            }

            detalleCompra.estado = false;
            context.Update(detalleCompra);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("habilitar/{id:int}")]
        public async Task<ActionResult> habilitar(int id)
        {
            var existe = await context.DetalleCompra.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }
            var detalleCompra = await context.DetalleCompra.FirstOrDefaultAsync(x => x.id == id);
            detalleCompra.estado = true;
            context.Update(detalleCompra);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
