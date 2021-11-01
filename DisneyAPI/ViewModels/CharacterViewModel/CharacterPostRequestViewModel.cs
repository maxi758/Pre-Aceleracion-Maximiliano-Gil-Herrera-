using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModels.CharacterViewModel
{
    public class CharacterPostRequestViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo edad es obligatorio")]
        [Range(1, Int32.MaxValue)]
        public int Age { get; set; }
        [MaxLength(150)]
        [Required(ErrorMessage = "El campo imagen es obligatorio")]
        public string Image { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        public string Name { get; set; }
        [Range(1, Int32.MaxValue)]
        public int Weight { get; set; }
        public string Story { get; set; }
        //[Range(0, Int32.MaxValue)]
        public List<int> MovieOrSeriesId { get; set; }
    }
}
