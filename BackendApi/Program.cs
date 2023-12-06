using Microsoft.EntityFrameworkCore;
using BackendApi.Models;
using BackendApi.Services.Contrato;
using BackendApi.Utilidades;
using BackendApi.Services.Implementacion;
using BackendApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBEmpleadoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});

//Con esto podremos inyectar directamento en nuestros
// métodos apirest
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

// De esta manera agg la referencia de AutoMapperProfile
// que contiene todo el mapeo para la configuración de los mapeos
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


#region PETICIONES API REST
app.MapGet("/departamento/lista", async (
    // inyectar servicio y mapper
    IDepartamentoService _departamentoServicio,
    IMapper _mapper
    ) =>
    {
        // En ves de List<Departamento> puede simplificarse a var
        var listaDepartamento = await _departamentoServicio.GetList(); // este modelo hay que convertiro en una clase DTO
        var listaDepartamentoDTO = _mapper.Map<List<DepartamentoDTO>>(listaDepartamento);

        if (listaDepartamentoDTO.Count > 0)
            return Results.Ok(listaDepartamentoDTO);
        else
            return Results.NotFound();
});

app.MapGet("/empleado/lista", async (
    // inyectar servicio y mapper
    IEmpleadoService _empleadoServicio,
    IMapper _mapper
    ) =>
{
    // En ves de List<Departamento> puede simplificarse a var
    var listaEmpleado = await _empleadoServicio.GetList(); // este modelo hay que convertiro en una clase DTO
    var listaEmpleadoDTO = _mapper.Map<List<EmpleadoDTO>>(listaEmpleado);

    if (listaEmpleadoDTO.Count > 0)
        return Results.Ok(listaEmpleadoDTO);
    else
        return Results.NotFound();
});

app.MapPost("/empleado/guardar", async (
    EmpleadoDTO modelo,
    IEmpleadoService _empleadoServicio,
    IMapper _mapper
    ) => {
        var _empleado = _mapper.Map<Empleado>(modelo);
        var _empleadoCreado = await _empleadoServicio.Add(_empleado);

        if (_empleadoCreado.IdEmpleado != 0)
            return Results.Ok(_mapper.Map<EmpleadoDTO>(_empleadoCreado)); //Siempre debemos devolverlo como DTO
        else
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
    });



app.MapPut("/empleado/actualizar/{idEmpleado}", async(
    int idEmpleado,
    EmpleadoDTO modelo,
    IEmpleadoService _empleadoServicio,
    IMapper _mapper
    ) => {
        var _encontrado = await _empleadoServicio.Get(idEmpleado);

        if (_encontrado is null) return Results.NotFound();
        
        var _empleado = _mapper.Map<Empleado>(modelo);
        _encontrado.NombreCompleto = _empleado.NombreCompleto;
        _encontrado.IdDepartamento = _empleado.IdDepartamento;
        _encontrado.Sueldo = _empleado.Sueldo;
        _encontrado.FechaContrato = _empleado.FechaContrato;

        var respuesta = await _empleadoServicio.Update(_encontrado);
        if (respuesta)
            return Results.Ok(_mapper.Map<EmpleadoDTO>(_encontrado));
        else
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
    });


app.MapDelete("/empleado/eliminar/{idEmpelado}", async (
    int idEmpleado,
    IEmpleadoService _empleadoServicio
    ) => {
        var _encontrado = await _empleadoServicio.Get(idEmpleado);

        if (_encontrado is null) return Results.NotFound();

        var respuesta = await _empleadoServicio.Delete(_encontrado);

        if (respuesta)
            return Results.Ok();
        else
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
    });
#endregion

app.UseCors("NuevaPolitica");
app.Run();

