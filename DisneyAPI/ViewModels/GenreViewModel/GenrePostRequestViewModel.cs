using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModels.MovieOrSerieViewModel
{
    public class GenrePostRequestViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage = "El genero debe contener entre 2 y 50 caracteres")]
        public string Name { get; set; }
        [Required]
        [StringLength(2048, MinimumLength = 8,
            ErrorMessage = "imagen no valida")]
        public string Image { get; set; }
        //[Range(0, Int32.MaxValue)]
        public List<int> MovieOrSeriesId { get; set; } 
    }
}
