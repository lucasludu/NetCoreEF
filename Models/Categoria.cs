// using System.ComponentModel.DataAnnotations;

using System.Text.Json.Serialization;

namespace EF.NET.Models;
public class Categoria {

    // [Key]
    public Guid CategoriaId {get;set;}

    // [Required]
    // [MaxLength(150)]
    public string Nombre {get;set;}
    public string Descripcion {get;set;}
    public int Peso {get;set;} // Impacto que va tener la tarea a realizar.


    [JsonIgnore] // Al momento de traer los datos, no tiene que trer las tareas.
    public virtual ICollection<Tarea> Tareas {get;set;}

}   

