using api_ferreteria.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_ferreteria.Controllers
{
    //indiacmos que es un controlador
    [ApiController]
    //definir la ruta de acceso al controlador
    [Route("api-ferreteria/comprobante")]
    //Controller base es una herencia para que sea un controlador
    public class ComprobanteController:ControllerBase
    {
            
        private readonly ApplicationDbContext context;

        public ComprobanteController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Comprobante>>> findAll()
        {
            return await context.Comprobante.ToListAsync();
        }
        //MOSTRAR INFORMACION
        //cuando queremos obtener informacion
        [HttpGet("personalizado")]
        public async Task<ActionResult<List<Comprobante>>> FindAllWithTotals()
        {
            // Paso 1: Obtener todos los comprobantes de la base de datos
            var comprobantes = await context.Comprobante.ToListAsync();

            // Paso 2: Filtrar solo los comprobantes con estado true
            var comprobantesConEstadoTrue = comprobantes.Where(c => c.estado == true).ToList();
            
            // Paso 3: Calcular la suma de los totales, subtotales e IGV de los comprobantes con estado true
            decimal totalSum = comprobantesConEstadoTrue.Sum(c => c.total);
            decimal subtotalSum = comprobantesConEstadoTrue.Sum(c => c.subtotal);
            decimal igvSum = comprobantesConEstadoTrue.Sum(c => c.igv);

            // Paso 4: Contar la cantidad de comprobantes con estado true
            int cantidadComprobantes = comprobantesConEstadoTrue.Count;

            // Crear un objeto anónimo que contenga la lista de comprobantes y los totales calculados
            var result = new
            {
                Comprobantes = comprobantes,
                TotalSum = totalSum,
                SubtotalSum = subtotalSum,
                IgvSum = igvSum,
                CantidadComprobantes = cantidadComprobantes
            };

            return Ok(result);
        }

        //BUSCAR COMPROBANTES ENTRE FECHAS

        [HttpGet("buscar/{fechaInicio:DateTime}/{fechaFin:DateTime}")]
        public async Task<ActionResult<List<Comprobante>>> FindByDates(DateTime fechaInicio, DateTime fechaFin)
        {
            // Asegurémonos de que las fechas estén en orden (fechaInicio debe ser antes que fechaFin)
            if (fechaInicio > fechaFin)
            {
                return BadRequest("La fecha de inicio debe ser anterior a la fecha de fin.");
            }
            var comprobantesEntreFechas = await context.Comprobante
                .Where(c => c.fecha >= fechaInicio && c.fecha <= fechaFin)
                .ToListAsync();

            // Paso 2: Filtrar solo los comprobantes con estado true
            var comprobantesConEstadoTrue = comprobantesEntreFechas.Where(c => c.estado == true).ToList();


            // Paso 3: Calcular la suma de los totales, subtotales e IGV de los comprobantes con estado true
            decimal totalSum = comprobantesConEstadoTrue.Sum(c => c.total);
            decimal subtotalSum = comprobantesConEstadoTrue.Sum(c => c.subtotal);
            decimal igvSum = comprobantesConEstadoTrue.Sum(c => c.igv);

            // Paso 4: Contar la cantidad de comprobantes con estado true
            int cantidadComprobantes = comprobantesConEstadoTrue.Count;


            // Filtrar los comprobantes según las fechas proporcionadas


            var result = new
            {
                Comprobantes = comprobantesEntreFechas,
                TotalSum = totalSum,
                SubtotalSum = subtotalSum,
                IgvSum = igvSum,
                CantidadComprobantes = cantidadComprobantes
            };

            return Ok(result);
        }

        //MOSTRAR INFORMACION DE ESTADO TRUE
        //queremos obtener solo la informacion de los de estado "true" habilitados
        [HttpGet("custom")]
        public async Task<ActionResult<List<Comprobante>>> findAllCustom()
        {
            return await context.Comprobante.Where(x => x.estado == true).ToListAsync();
        }
        //GUARDAR
        //cuando queremos guardar informacion
        /*[HttpPost]
        public async Task<ActionResult> add(Comprobante l)
        {
            var usuariooexiste = await context.Usuario.AnyAsync(x => x.id == l.UsuarioId);
            var clienteexiste = await context.Cliente.AnyAsync(x => x.id == l.ClienteId);

            if (!usuariooexiste)
            {
                return BadRequest($"No existe el usuario con codigo : {l.UsuarioId}");
            }
            if (!clienteexiste)
            {
                return BadRequest($"No existe el cliente con codigo : {l.ClienteId}");
            }

            context.Add(l);
            await context.SaveChangesAsync();
            return Ok();
        }*/

        [HttpPost]
        public async Task<IActionResult> CrearVenta(Comprobante comprobante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Comprobante.Add(comprobante);
                    await context.SaveChangesAsync();

                    foreach (var detalle in comprobante.detalle) // Utiliza DetalleCompra en lugar de detalleCompra
                    {
                       /* detalle.ComprobanteNumero = comprobante.numero;
                        context.Detalle.Add(detalle);*/

                        var producto = context.Producto.FirstOrDefault(p => p.id == detalle.ProductoId);
                        if (producto != null)
                        {
                            producto.stock -= detalle.cantidad;
                            
                        }
                    }

                    await context.SaveChangesAsync();

                    transaction.Commit();
                    return Ok("Compra creada exitosamente");

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, "Error interno del servidor: " + ex.Message);
                }
            }
        }

        //MOSTRAR POR ID
        //cuando queremos buscar informacion por el id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Comprobante>> findById(int id)
        {
            var comprobante = await context.Comprobante.FirstOrDefaultAsync(x => x.numero == id);
            return comprobante;
        }
        //ACTUALIZAR
        //cuando queremos actualizar informaion
        [HttpPut("{id:int}")]
        public async Task<ActionResult> update(Comprobante l, int id)
        {
            if (l.numero != id)
            {
                return BadRequest("No se encontro el codigo correspondiente");
            }
            context.Update(l);
            await context.SaveChangesAsync();
            return Ok();
        }
        // ELIMINAR
        //cuando queremos "eliminar" informacion, cambiar el estado de la entidad a FALSO
        [HttpPut("habilitar/{id:int}")]
        public async Task<ActionResult> habilitar(int id)
        {
            var existe = await context.Comprobante.AnyAsync(x => x.numero == id);
            if (!existe)
            {
                return NotFound();
            }
            var comprobante = await context.Comprobante.FirstOrDefaultAsync(x => x.numero == id);
            comprobante.estado = true;
            context.Update(comprobante);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("cancelarVenta")]
        public async Task<ActionResult> ActualizarEstadoComprobante(ComentarioDTO comentarioDTO)
        {
            int numeroComprobante = comentarioDTO.NumeroComprobante;
            string comentario = comentarioDTO.Comentario;

            // Buscar el comprobante por su número
            var comprobante = await context.Comprobante.FirstOrDefaultAsync(c => c.numero == numeroComprobante);

            if (comprobante == null)
            {
                return NotFound(comentarioDTO.NumeroComprobante);
            }

            // Agregar el comentario al comprobante
            comprobante.comentario = comentario;
            comprobante.estado = false;
            context.SaveChanges(); // Guardar los cambios en la base de datos

            // Cambiar el estado de los detalles relacionados al comprobante a false
            var detalles = context.Detalle.Where(d => d.ComprobanteNumero == comprobante.numero).ToList();
            foreach (var detalle in detalles)
            {
                detalle.estado = false;
                context.SaveChanges();
            }

            // Actualizar el stock de los productos relacionados a los detalles
            foreach (var detalle in detalles)
            {
                var producto = await context.Producto.FirstOrDefaultAsync(p => p.id == detalle.ProductoId);
                if (producto != null)
                {
                    producto.stock += detalle.cantidad;
                    context.SaveChanges();
                }
            }

            return Ok("El comprobante y sus detalles han sido actualizados correctamente.");
        }
    }
}
