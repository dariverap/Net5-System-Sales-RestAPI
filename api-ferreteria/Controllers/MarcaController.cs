using api_ferreteria.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_ferreteria.Controllers
{
    [ApiController]
    [Route("api-ferreteria/marca")]
    public class MarcaController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public MarcaController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Marca>>> FindAll()
        {
            return await context.Marca.ToListAsync();
        }

        [HttpGet("custom")]
        public async Task<ActionResult<List<Marca>>> FindAllCustom()
        {
            return await context.Marca.Where(x => x.estado == true).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Add(Marca marca)
        {
            context.Add(marca);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Marca>> FindById(int id)
        {
            var marca = await context.Marca.FirstOrDefaultAsync(x => x.Id == id);
            return marca;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(Marca marca, int id)
        {
            if (marca.Id != id)
            {
                return BadRequest("No se encontró el código correspondiente");
            }
            context.Update(marca);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Marca.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            var marca = await context.Marca.FirstOrDefaultAsync(x => x.Id == id);
            marca.estado = false;
            context.Update(marca);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("habilitar/{id:int}")]
        public async Task<ActionResult> habilitar(int id)
        {
            var existe = await context.Marca.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            var marca = await context.Marca.FirstOrDefaultAsync(x => x.Id == id);
            marca.estado = true;
            context.Update(marca);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}