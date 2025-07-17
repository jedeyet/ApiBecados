using ApiBecados.Data;
using ApiBecados.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            string nombre = resultado.ToString(); // ✅ Aquí lo convertís

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("F15_clave_jwt_secreta_segura_2025");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, loginRequest.Usuario),
                    new Claim("nombre", nombre)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                success = true,
                nombre = nombre,         // ✅ Esto sí va
                token = tokenString      // ✅ Este es el JWT
            });
        }
        else
        {
            return Unauthorized(new { success = false, mensaje = "Credenciales inválidas" });
        }
    }
}
