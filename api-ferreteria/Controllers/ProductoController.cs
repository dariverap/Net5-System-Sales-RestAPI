using api_ferreteria.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_ferreteria.Controllers
{
    //indiacmos que es un controlador
    [ApiController]
    //definir la ruta de acceso al controlador
    [Route("api-ferreteria/producto")]
    //Controller base es una herencia para que sea un controlador
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductoController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //cuando queremos obtener informacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListadoProductos>>> findAll()
        {
            var query = from p in context.Producto
                        join c in context.Categoria on p.CategoriaId equals c.id
                        join m in context.Marca on p.MarcaId equals m.Id
                        select new ListadoProductos
                        {
                            id = p.id,
                            nombre = p.nombre,
                            descripcion = p.descripcion,
                            stock = p.stock,
                            estado = p.estado,
                            precioVenta = p.precioVenta,
                            nombreCategoria = c.nombre,
                            nombreMarca = m.Nombre,
                            url = p.url
                        };
            return await query.ToListAsync();
        }
        //queremos obtener solo la informacion de los de estado "true" habilitados
        [HttpGet("custom")]
        public async Task<ActionResult<List<Producto>>> findAllCustom()
        {
            return await context.Producto.Where(x => x.estado == true).ToListAsync();
        }

        //cuando queremos guardar informacion
        [HttpPost]
        public async Task<ActionResult> add(Producto l)
        {// Verificar si ya existe un cliente con el mismo DNI

            var categoriaExiste = await context.Categoria.AnyAsync(x => x.id == l.CategoriaId);
            if (!categoriaExiste)
            {
                return BadRequest(new { message = "Selecciona una categoria" });
            }


            var marcaExiste = await context.Marca.AnyAsync(x => x.Id == l.MarcaId);
            if (!marcaExiste)
            {
                return BadRequest(new { message = "Selecciona una categoria" });
            }

            if (l.nombre == null || l.nombre == null)
            {
                return BadRequest(new { message = "Ingresa un nombre para el producto" });
            }

            if (string.IsNullOrEmpty(l.descripcion))
            {
                return BadRequest(new { message = "El campo descripción es requerido." });
            }

            if (l.stock < 0)
            {
                return BadRequest(new { message = "El campo stock no puede ser nulo o menor que cero." });
            }

            if (l.precioVenta < 0)
            {
                return BadRequest(new { message = "El campo precioVenta no puede ser menor que cero." });
            }


            context.Add(l);
            await context.SaveChangesAsync();
            return Ok();
        }

        //cuando queremos buscar informacion por el id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Producto>> findById(int id)
        {
            var producto = await context.Producto.FirstOrDefaultAsync(x => x.id == id);
            return producto;
        }

        //cuando queremos actualizar informaion
        [HttpPut("{id:int}")]
        public async Task<ActionResult> update(Producto l, int id)
        {
            if (l.id != id)
            {
                return BadRequest("No se encontro el codigo correspondiente");
            }
            context.Update(l);
            await context.SaveChangesAsync();
            return Ok();
        }

        //cuando queremos "eliminar" informacion, cambiar el estado de la entidad a FALSO
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> delete(int id)
        {
            var existe = await context.Producto.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }
            var producto = await context.Producto.FirstOrDefaultAsync(x => x.id == id);
            producto.estado = false;
            context.Update(producto);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("habilitar/{id:int}")]
        public async Task<ActionResult> habilitar(int id)
        {
            var existe = await context.Producto.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }
            var producto = await context.Producto.FirstOrDefaultAsync(x => x.id == id);
            producto.estado = true;
            context.Update(producto);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}