using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiBecados.Modelos;
using ApiBecados.Data;

namespace ApiBecados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PapeleriaController : ControllerBase
    {
        [HttpGet("por-carrera")]
        public async Task<ActionResult<List<PapeleriaDTO>>> GetPapeleriaPorCarrera([FromQuery] string carrera)
        {
            try
            {
                var lista = new List<PapeleriaDTO>();

                using (var conn = new MySqlConnection(Conexion.connectionStringCloud))
                {
                    await conn.OpenAsync();

                    string query = @"
                SELECT idPapeleria, idBecados, Carnet, NombreCompleto, Carrera, Facultad,
                       ano, semestreAnual, cartaCompromiso, cartaSolicitud, anteproyecto,
                       estado, informeFinal, observacion1, observacion2, fechaRegistro
                FROM Vista_PapeleriaBecados
                WHERE TRIM(Carrera) LIKE TRIM(@Carrera)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Carrera", carrera);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                lista.Add(new PapeleriaDTO
                                {
                                    idPapeleria = reader.GetInt32("idPapeleria"),
                                    idBecados = reader.GetInt32("idBecados"),
                                    Carnet = reader.GetString("Carnet"),
                                    NombreCompleto = reader.GetString("NombreCompleto"),
                                    Carrera = reader.GetString("Carrera"),
                                    Facultad = reader.GetString("Facultad"),
                                    ano = reader.GetInt32("ano"),
                                    semestreAnual = reader.GetString("semestreAnual"),

                                    cartaCompromiso = reader.GetBoolean("cartaCompromiso").ToString(),
                                    cartaSolicitud = reader.GetBoolean("cartaSolicitud").ToString(),
                                    anteproyecto = reader.GetBoolean("anteproyecto").ToString(),
                                    informeFinal = reader.GetBoolean("informeFinal").ToString(),

                                    estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? "" : reader.GetString("estado"),
                                    observacion1 = reader.IsDBNull(reader.GetOrdinal("observacion1")) ? "" : reader.GetString("observacion1"),
                                    observacion2 = reader.IsDBNull(reader.GetOrdinal("observacion2")) ? "" : reader.GetString("observacion2"),
                                    fechaRegistro = reader.GetDateTime("fechaRegistro")
                                });
                            }
                        }
                    }
                }

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }





        [HttpGet("carreras")]
        public async Task<ActionResult<List<string>>> GetCarreras()
        {
            var lista = new List<string>();

            using (var conn = new MySqlConnection(Conexion.connectionStringCloud))
            {
                await conn.OpenAsync();

                string query = @"SELECT DISTINCT Carrera FROM Vista_PapeleriaBecados ORDER BY Carrera";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(reader.GetString("Carrera"));
                    }
                }
            }

            return Ok(lista);
        }

    }
}
