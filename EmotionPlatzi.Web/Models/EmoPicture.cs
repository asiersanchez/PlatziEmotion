using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Web.Models
{ 
    public class EmoPicture
    {
        // Todos los modelos tienen que tener un Id por convención
        public int Id { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        //DataAnotations, validaciones basadas en atributos
        [Required]
        public string Path { get; set; }

        // a esta propiedad en Entity Framework se la llama propiedad de navegación
        // que son propiedades que están en el modelo pero no equivalen a un campo real del modelo
        // Entity Framework las va a coger para general la navegación o dependencia entre una tabla y otra
        // entre maestro y detalle por ejemplo
        // se tiene que marcar como virtual. Como necesitamos una lista de emoface ya que una foto puede tener varias
        // caras asociadas usamos una colección, en este caso ObservableCollection, que es una clase genérica
        // No formará parte de EmoPicture igual que Id, Name y Path sino que establecerá una relación donde Emopicture es el maestro
        // y Emofaces es el detalle, es decir por cada Emopicture hay muchos Emofaces
        public virtual ObservableCollection<EmoFace> Faces { get; set; }

    }
}