using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using ApiBecados.Modelos;
using ApiBecados.Data; // para usar Conexion

namespace ApiBecados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Usuarios loginRequest)
        {
            using var conexion = new MySqlConnection(Conexion.connectionStringCloud);
            await conexion.OpenAsync();

            string query = "SELECT UsuariosNombre FROM Usuarios WHERE Usuario = @usuario AND Password = SHA2(@password, 256)";
            using var cmd = new MySqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@usuario", loginRequest.Usuario);
            cmd.Parameters.AddWithValue("@password", loginRequest.Password);

            var resultado = await cmd.ExecuteScalarAsync();

            if (resultado != null)
            {
                return Ok(new { success = true, nombre = resultado.ToString() });
            }
            else
            {
                return Unauthorized(new { success = false, mensaje = "Credenciales inválidas" });
            }
        }
    }
}
