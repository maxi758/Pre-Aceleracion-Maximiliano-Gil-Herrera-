using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModels.MovieOrSerieViewModel
{
    public class MovieOrSeriePutViewModel
    {
        public int Id { get; set; }
        [MaxLength(150)]
        public string Imagen { get; set; }
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "El título debe contener entre 2 y 100 caracteres")]
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        [Range(1, 5)]
        public int Score { get; set; }
        //[Range(0, Int32.MaxValue)]
        public List<int> CharactersId { get; set; } 
    }
}
