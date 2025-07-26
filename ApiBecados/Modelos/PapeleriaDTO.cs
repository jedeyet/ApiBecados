namespace ApiBecados.Modelos
{
    public class PapeleriaDTO
    {
        public int idPapeleria { get; set; }
        public int idBecados { get; set; }
        public string Carnet { get; set; }
        public string NombreCompleto { get; set; }
        public string Carrera { get; set; }
        public string Facultad { get; set; }
        public int ano { get; set; }
        public string semestreAnual { get; set; }
        public string cartaCompromiso { get; set; }
        public string cartaSolicitud { get; set; }
        public string anteproyecto { get; set; }
        public string estado { get; set; }
        public string informeFinal { get; set; }
        public string observacion1 { get; set; }
        public string observacion2 { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
