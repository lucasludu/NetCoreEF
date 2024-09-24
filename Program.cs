using EF.NET.Data;
using EF.NET.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* No se puede usar las dos porque puede haber conflictos
builder.Services.AddDbContext<TareasContext>(ops => {
    ops.UseInMemoryDatabase("TareasDB");
});
*/

builder.Services.AddDbContext<TareasContext>(Options => 
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("connectionEF"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) => {
    dbContext.Database.EnsureCreated();
    return Results.Ok("BD en memoria: " + dbContext.Database.IsInMemory());
});

app.MapGet("/api/tareas", async ([FromServices] TareasContext dbContext) => {
    return Results.Ok(dbContext.Tareas
        .Include(c => c.Categoria));
        // .Where(t => t.PrioridadTarea == Prioridad.Media));
});

app.MapPost("/api/tareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea) => {
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;

    await dbContext.AddAsync(tarea);
    // await dbContext.Tareas.AddAsync(tarea);
    await dbContext.SaveChangesAsync();

    return Results.Ok();
});

app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea, [FromRoute] Guid id) => {
    var tareaActual = dbContext.Tareas.Find(id);

    if(tareaActual != null) {
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        tareaActual.Descripcion = tarea.Descripcion;

        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }
    return Results.NotFound();
});

app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id) => {
    var tareaActual = dbContext.Tareas.Find(id);

    if(tareaActual != null) {
        dbContext.Remove(tareaActual);
        await dbContext.SaveChangesAsync();
        
        return Results.Ok();
    }
    return Results.NotFound();
});

app.Run();
