using BackendApi.Models;


namespace BackendApi.Services.Contrato
{
    public interface IDepartamentoService
    {
        // Trabajaremos de manera síncrona usaremos Task
        Task<List<Departamento>> GetList();
    }
}
