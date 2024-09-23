using EF.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace EF.NET.Data;

public class TareasContext : DbContext {

    public DbSet<Categoria> Categorias {get;set;}
    public DbSet<Tarea> Tareas {get;set;}


    public TareasContext(DbContextOptions<TareasContext> options) : base(options) {
        
    }

}