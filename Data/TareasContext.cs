using EF.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace EF.NET.Data;

public class TareasContext : DbContext {

    public DbSet<Categoria> Categorias {get;set;}
    public DbSet<Tarea> Tareas {get;set;}


    public TareasContext(DbContextOptions<TareasContext> options) : base(options) {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        
        List<Categoria> categoriasInit = new List<Categoria>();
        categoriasInit.Add(new Categoria() { 
                CategoriaId = Guid.Parse("e9da9b6d-22f9-497d-ac73-2cc36b80c3a1"), 
                Nombre = "Actividades pendientes",
                Peso = 20
            });
        categoriasInit.Add(new Categoria() { 
                CategoriaId = Guid.Parse("e9da9b6d-22f9-497d-ac73-2cc36b80b4d2"), 
                Nombre = "Actividades personales",
                Peso = 50
            });


        // CATEGORIA
        modelBuilder.Entity<Categoria>(categoria => {
            categoria.ToTable("Categoria");
            categoria.HasKey(c => c.CategoriaId);
            categoria.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(150);
            categoria.Property(c => c.Descripcion)
                .IsRequired(false);
            categoria.Property(c => c.Peso);

            categoria.HasData(categoriasInit);
        });

        List<Tarea> tareaList = new List<Tarea>();
        tareaList.Add(new Tarea() {
                TareaId = Guid.Parse("c4e0d0e7-5f06-48c7-9246-11fe12f2c100"), 
                CategoriaId = Guid.Parse("e9da9b6d-22f9-497d-ac73-2cc36b80b4d2"),
                PrioridadTarea = Prioridad.Media,
                Titulo = "Payment of public services",
                FechaCreacion = DateTime.Now
            });
        tareaList.Add(new Tarea() {
                TareaId = Guid.Parse("c4e0d0e7-5f06-48c7-9246-11fe12f2c101"), 
                CategoriaId = Guid.Parse("e9da9b6d-22f9-497d-ac73-2cc36b80c3a1"),
                PrioridadTarea = Prioridad.Alta,
                Titulo = "Finish watching movie",
                FechaCreacion = DateTime.Now
            });

        // TAREA
        modelBuilder.Entity<Tarea>(tarea => {
            tarea.ToTable("Tarea");
            tarea.HasKey(t => t.TareaId);
            tarea.HasOne(t => t.Categoria)
                .WithMany(c => c.Tareas)
                .HasForeignKey(t => t.CategoriaId);
            tarea.Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(200);
            tarea.Property(t => t.Descripcion)
                .IsRequired(false);
            tarea.Property(t => t.PrioridadTarea);
            tarea.Property(t => t.FechaCreacion);
            tarea.Ignore(t => t.Resumen);

            tarea.HasData(tareaList);
        });

    }

}