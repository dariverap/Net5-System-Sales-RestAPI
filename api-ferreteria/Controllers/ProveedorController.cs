using api_ferreteria.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_ferreteria.Controllers
{
    [ApiController]
    [Route("api-ferreteria/proveedor")]
    public class ProveedorController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProveedorController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Proveedor>>> FindAll()
        {
            return await context.Proveedor.ToListAsync();
        }

        [HttpGet("custom")]
        public async Task<ActionResult<List<Proveedor>>> FindAllCustom()
        {
            return await context.Proveedor.Where(x => x.estado == true).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Add(Proveedor proveedor)
        {
            bool exists = await context.Proveedor.AnyAsync(p => p.Ruc == proveedor.Ruc);
            if (exists)
            {
                return BadRequest(new { message = "Ya existe un proveedor con el mismo RUC." });
            }

            context.Add(proveedor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Proveedor>> FindById(int id)
        {
            var proveedor = await context.Proveedor.FirstOrDefaultAsync(x => x.Id == id);
            return proveedor;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(Proveedor proveedor, int id)
        {
            if (proveedor.Id != id)
            {
                return BadRequest("No se encontró el código correspondiente");
            }
            context.Update(proveedor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Proveedor.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            var proveedor = await context.Proveedor.FirstOrDefaultAsync(x => x.Id == id);
            proveedor.estado = false;
            context.Update(proveedor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("habilitar/{id:int}")]
        public async Task<ActionResult> habilitar(int id)
        {
            var existe = await context.Proveedor.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            var proveedor = await context.Proveedor.FirstOrDefaultAsync(x => x.Id == id);
            proveedor.estado = true;
            context.Update(proveedor);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
