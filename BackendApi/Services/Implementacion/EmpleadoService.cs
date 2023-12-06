using Microsoft.EntityFrameworkCore;
using BackendApi.Models;
using BackendApi.Services.Contrato;

namespace BackendApi.Services.Implementacion
{
    public class EmpleadoService : IEmpleadoService
    {
        private DBEmpleadoContext _dbContext;
        public EmpleadoService(DBEmpleadoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Empleado>> GetList()
        {
            // Devolver la lista de empleados
            try
            {
                List<Empleado> lista = new List<Empleado>();
                lista = await _dbContext.Empleados.Include(dpt => dpt.IdDepartamentoNavigation).ToListAsync();

                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Empleado> Get(int idEmpleado)
        {
            // Devolver empleado por id
            try
            {
                Empleado? empleado = new Empleado();
                empleado = await _dbContext.Empleados.Include(dpt => dpt.IdDepartamentoNavigation)
                    .Where(e => e.IdEmpleado == idEmpleado).FirstOrDefaultAsync();
                return empleado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Empleado> Add(Empleado modelo)
        {
            try
            {
                _dbContext.Empleados.Add(modelo);
                await _dbContext.SaveChangesAsync();
                return modelo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Update(Empleado modelo)
        {
            try
            {
                _dbContext.Empleados.Update(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(Empleado modelo)
        {
            try
            {
                _dbContext.Empleados.Remove(modelo);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
