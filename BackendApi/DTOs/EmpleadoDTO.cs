namespace BackendApi.DTOs
{
    public class EmpleadoDTO
    {
        

        // Este codigo debe ser casi similar a la de modelo
        public int IdEmpleado { get; set; }
        public string? NombreCompleto { get; set; }
        public int? IdDepartamento { get; set; }
        public string? NombreDepartamento { get; set; }
        public int? Sueldo { get; set; }
        public string? FechaContrato { get; set; }
    }
}
