using api_ferreteria.Entitys;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace api_ferreteria.Controllers
{
    //indiacmos que es un controlador
    [ApiController]
    //definir la ruta de acceso al controlador
    [Route("api-ferreteria/usuario")]
    //Controller base es una herencia para que sea un controlador
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UsuarioController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //MOSTRAR INFORMACION
        //cuando queremos obtener informacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListaUsuarios>>> findAll()
        {
            var query = from u in context.Usuario
                        join e in context.Empleado on u.EmpleadoId equals e.id
                        join r in context.Rol on e.RolId equals r.id
                        select new ListaUsuarios
                        {
                            id = u.id,
                            nombre = u.nombre,
                            contraseña = u.contraseña,
                            estado = u.estado,
                            nombreEmpleado = e.nombre,
                            apellidoEmpleado = e.apellido,
                            rol = r.nombre
                        };
            return await query.ToListAsync();
        }


        //MOSTRAR INFORMACION DE ESTADO TRUE
        //queremos obtener solo la informacion de los de estado "true" habilitados
        [HttpGet("custom")]
        public async Task<ActionResult<List<Usuario>>> findAllCustom()
        {
            return await context.Usuario.Where(x => x.estado == true).ToListAsync();
        }
        //GUARDAR
        //cuando queremos guardar informacion
        [HttpPost()]
        public async Task<ActionResult> add(Usuario l)
        {

            var empleadoexiste = await context.Empleado.AnyAsync(x => x.id == l.EmpleadoId);
            if (!empleadoexiste)
            {
                return BadRequest($"No existe el empleado con codigo : {l.EmpleadoId}");
            }

            var hashsalt = EncryptPassword(l.contraseña);
            l.contraseña = hashsalt.Hash;
            l.StoredSalt = hashsalt.Salt;

            context.Add(l);
            await context.SaveChangesAsync();
            return Ok();
        }

        public HashSalt EncryptPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return new HashSalt { Hash = encryptedPassw, Salt = salt };
        }

        public bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw == storedPassword;
        }
        //MOSTRAR POR ID
        //cuando queremos buscar informacion por el id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Usuario>> findById(int id)
        {
            var usuario = await context.Usuario.FirstOrDefaultAsync(x => x.id == id);
            return usuario;
        }

        //ACTUALIZAR
        //cuando queremos actualizar informaion
        [HttpPut("{id:int}")]
        public async Task<ActionResult> update(Usuario l, int id)
        {
            if (l.id != id)
            {
                return BadRequest("No se encontro el codigo correspondiente");
            }
            context.Update(l);
            await context.SaveChangesAsync();
            return Ok();
        }
        // ELIMINAR
        //cuando queremos "eliminar" informacion, cambiar el estado de la entidad a FALSO
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> delete(int id)
        {
            var existe = await context.Usuario.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }
            var usuario = await context.Usuario.FirstOrDefaultAsync(x => x.id == id);
            usuario.estado = false;
            context.Update(usuario);
            await context.SaveChangesAsync();
            return Ok();
        }

        //INICIAR SESION
        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<List<ListaUsuarios>>> GetIniciarSesion(string username, string password)
        {
            var query = from u in context.Usuario
                        join e in context.Empleado on u.EmpleadoId equals e.id
                        join r in context.Rol on e.RolId equals r.id
                        where u.nombre.Equals(username) && u.estado.Equals(true)
                        select new ListaUsuarios
                        {
                            id = u.id,
                            nombre = u.nombre,
                            StoredSalt = u.StoredSalt,
                            contraseña = u.contraseña,
                            estado = u.estado,
                            nombreEmpleado = e.nombre,
                            apellidoEmpleado = e.apellido,
                            rol = r.nombre
                        };

            var usuarios = await query.ToListAsync();

            if (usuarios == null || usuarios.Count == 0)
            {
                return NotFound();
            }
            var isPasswordMatched = VerifyPassword(password, usuarios[0].StoredSalt, usuarios[0].contraseña);
            if (isPasswordMatched)
            {
                return usuarios;
                //Login Successfull
            }
            else
            {
                return Unauthorized();
                //Login Failed
            }


        }

    }
}
