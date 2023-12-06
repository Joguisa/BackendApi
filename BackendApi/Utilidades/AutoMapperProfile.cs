using AutoMapper;
using BackendApi.DTOs;
using BackendApi.Models;
using System.Globalization;

namespace BackendApi.Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // configuración para convertir nuestro modelo a una clase DTO
            #region Departamento
            CreateMap<Departamento, DepartamentoDTO>().ReverseMap();
            #endregion

            #region Empleado
            CreateMap<Empleado, EmpleadoDTO>()
                // especificar cómo se van a trabaja las columnas
                // el destino viene a ser el EmpleadoDTO
                .ForMember(destino =>
                    destino.NombreDepartamento,
                    opt => opt.MapFrom(origen => origen.IdDepartamentoNavigation.Nombre)
                )
                .ForMember(destino =>
                    destino.FechaContrato,
                    opt => opt.MapFrom(origen => origen.FechaContrato.Value.ToString("dd/MM/yyyy"))
                );
            // Ahora lo haremos al revés de EmleadoDTO a Empleado, DTO a modelo
            CreateMap<EmpleadoDTO, Empleado>()
                .ForMember(destino =>
                    destino.IdDepartamentoNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(destino =>
                    destino.FechaContrato,
                    opt => opt.MapFrom(origen => DateTime.ParseExact(origen.FechaContrato, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                );
            #endregion

        }
    }
}
