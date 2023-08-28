using api_ferreteria.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_ferreteria.Controllers
{
    [ApiController]
    [Route("api-ferreteria/ordencompra")]
    public class CompraController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        Compra compra = new Compra();

        public CompraController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("personalizado")]
        public async Task<ActionResult<List<Compra>>> FindAllWithTotals()
        {
            // Paso 1: Obtener todos los comprobantes de la base de datos
            var compras = await context.Compra.ToListAsync();

            var listaCompras = new List<ListaCompra>();

            foreach (var compra in compras)
            {
                var proveedor = await context.Proveedor.FindAsync(compra.ProveedorId);

                if (proveedor != null)
                {
                    var listaCompra = new ListaCompra
                    {
                        numero = compra.numero,
                        fechaCompra = compra.fechaCompra,
                        igv = compra.igv,
                        subtotal = compra.subtotal,
                        total = compra.total,
                        estado = compra.estado,
                        ProveedorRazon = proveedor.Razon, // Aquí obtenemos la razón social del proveedor
                        ProveedorId = compra.ProveedorId,
                        DetalleCompra = compra.DetalleCompra
                    };

                    listaCompras.Add(listaCompra);
                }
            }

            // Paso 2: Filtrar solo las compras con estado true
            var comprasConEstadoTrue = listaCompras.Where(c => c.estado == true).ToList();

            // Paso 3: Calcular la suma de los totales, subtotales e IGV de los comprobantes con estado true
            decimal totalSum = comprasConEstadoTrue.Sum(c => c.total);
            decimal subtotalSum = comprasConEstadoTrue.Sum(c => c.subtotal);
            decimal igvSum = comprasConEstadoTrue.Sum(c => c.igv);

            // Paso 4: Contar la cantidad de comprobantes con estado true
            int cantidadCompras = comprasConEstadoTrue.Count;

            // Crear un objeto anónimo que contenga la lista de comprobantes y los totales calculados


            return Ok(new {
                Compras = listaCompras,
                TotalSum = totalSum,
                SubtotalSum = subtotalSum,
                IgvSum = igvSum,
                CantidadComprobantes = cantidadCompras });
        }
        [HttpPost]
        public async Task<IActionResult> CrearCompra(RegistroCompra registroCompra)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    compra.fechaCompra = registroCompra.fechaCompra;
                    compra.igv = registroCompra.igv;
                    compra.subtotal = registroCompra.subtotal;
                    compra.total = registroCompra.total;
                    compra.estado = registroCompra.estado;
                    compra.DetalleCompra = registroCompra.DetalleCompra;
                    compra.ProveedorId = registroCompra.ProveedorId;

                    context.Compra.Add(compra);
                    await context.SaveChangesAsync();



                    transaction.Commit();

                    return Ok("Compra creada exitosamente");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Generar un mensaje con información de los detalles de compra
                    var detalleMessage = "Detalles de compra: ";
                    foreach (var detalle in compra.DetalleCompra)
                    {
                        detalleMessage += $"ProductoId: {detalle.ProductoId}, Cantidad: {detalle.cantidad}, Precio: {detalle.precioCompra}, numerocompra: {detalle.CompraNumero}, estado:{detalle.estado}, importe:{detalle.importe}; ";
                    }

                    return StatusCode(500, "Error interno del servidor: " + detalleMessage);

                }
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<Compra>>> FindAll()
        {
            return await context.Compra.ToListAsync();
        }

        [HttpGet("custom")]
        public async Task<ActionResult<List<Compra>>> FindAllCustom()
        {
            return await context.Compra.Where(x => x.estado == true).ToListAsync();
        }
        /*
        [HttpPost]
        public async Task<ActionResult> Add(Compra compra)
        {
            context.Add(compra);
            await context.SaveChangesAsync();
            return Ok();
        }
        */
        [HttpPost("ingreso/{numero:int}")]
        public async Task<IActionResult> ingresarProductos(int numero) {
            var compra = await context.Compra
                            .Include(c => c.DetalleCompra)
                            .FirstOrDefaultAsync(c => c.numero == numero);
            compra.estado = true;
            context.Update(compra);
            await context.SaveChangesAsync();

            foreach (var detalle in compra.DetalleCompra)
            {
                var producto = context.Producto.FirstOrDefault(p => p.id == detalle.ProductoId);
                if (producto != null)
                {
                    producto.stock += detalle.cantidad;

                    // Verificar si existe un registro en ProductoProveedor con la combinación ProveedorId y ProductoId
                    var productoProveedor = context.ProductoProveedor.FirstOrDefault(pp => pp.ProveedorId == compra.ProveedorId && pp.ProductoId == detalle.ProductoId);

                    if (productoProveedor == null)
                    {
                        // Si no existe, crea un nuevo registro en ProductoProveedor
                        productoProveedor = new ProductoProveedor
                        {
                            ProveedorId = compra.ProveedorId,
                            ProductoId = detalle.ProductoId,
                            precioCompra = detalle.precioCompra
                        };
                        context.ProductoProveedor.Add(productoProveedor);
                    }
                    else
                    {
                        // Si existe, actualiza el precio de compra
                        productoProveedor.precioCompra = detalle.precioCompra;
                        context.Update(productoProveedor);

                    }
                }
            }

            await context.SaveChangesAsync();
            return Ok(compra);
        }
        [HttpGet("{numero:int}")]
        public async Task<ActionResult<object>> FindByNumero(int numero)
        {
            var listaCompra = await (from c in context.Compra
                        join p in context.Proveedor on c.ProveedorId equals p.Id
                        where c.numero == numero
                        select new ListaCompra
                        {
                            numero = c.numero,
                            fechaCompra = c.fechaCompra,
                            igv = c.igv,
                            subtotal = c.subtotal,
                            total = c.total,
                            estado = c.estado,
                            ProveedorRazon = p.Razon,
                            ProveedorId = c.ProveedorId
                        }).ToListAsync();

            var listaDetalleCompra = await (from c in context.DetalleCompra
                        join p in context.Producto on c.ProductoId equals p.id
                        where c.CompraNumero == numero
                        select new ListaDetalleCompra
                        {
                            id=c.id,
                            nombreProducto=p.nombre,
                            cantidad=c.cantidad,
                            precioCompra=c.precioCompra,
                            importe=c.importe,
                            estado=c.estado,
                            CompraNumero=c.CompraNumero,
                            ProductoId=c.ProductoId,
                        }).ToListAsync();
            var resultado = new
            {
                ListaCompra = listaCompra,
                ListaDetalleCompra = listaDetalleCompra
            };

            return Ok(resultado);
        }


        [HttpPut("{numero:int}")]
        public async Task<ActionResult> Update(Compra compra, int numero)
        {
            if (compra.numero != numero)
            {
                return BadRequest("No se encontró el código correspondiente");
            }

            context.Update(compra);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{numero:int}")]
        public async Task<ActionResult> Delete(int numero)
        {
            var comprobanteCompra = await context.Compra.FirstOrDefaultAsync(x => x.numero == numero);
            if (comprobanteCompra == null)
            {
                return NotFound();
            }

            comprobanteCompra.estado = false;
            context.Update(comprobanteCompra);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("habilitar/{id:int}")]
        public async Task<ActionResult> habilitar(int id)
        {
            var existe = await context.Compra.AnyAsync(x => x.numero == id);
            if (!existe)
            {
                return NotFound();
            }
            var comprobante = await context.Compra.FirstOrDefaultAsync(x => x.numero == id);
            comprobante.estado = true;
            context.Update(comprobante);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
