using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using ApiBecados.Modelos;
using ApiBecados.Data;
using Microsoft.AspNetCore.Authorization;


namespace ApiBecados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BecadosController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBecados()
        {
            var lista = new List<Becado>();

            using var conexion = new MySqlConnection(Conexion.connectionStringCloud);
            await conexion.OpenAsync();

            string query = @"SELECT b.idBecados, b.Apellidos, b.Nombres, b.Carnet, b.email,
                                    c.Carreras AS NombreCarrera, x.Beca as Beca
                             FROM becados b
                             JOIN carreras c ON b.idCarreras = c.idCarreras
                             JOIN becas x ON b.idBecas = x.idBecas
                             ORDER BY b.apellidos, b.nombres;";

            using var cmd = new MySqlCommand(query, conexion);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var becado = new Becado
                {
                    IdBecados = reader.GetInt32("idBecados"),
                    Apellidos = reader.GetString("Apellidos"),
                    Nombres = reader.GetString("Nombres"),
                    Carnet = reader.GetString("Carnet"),
                    Email = reader.GetString("email"),
                    NombreCarrera = reader.GetString("NombreCarrera"),
                    Beca = reader.GetString("Beca")
                };

                lista.Add(becado);
            }

            return Ok(lista);
        }
    }
}
