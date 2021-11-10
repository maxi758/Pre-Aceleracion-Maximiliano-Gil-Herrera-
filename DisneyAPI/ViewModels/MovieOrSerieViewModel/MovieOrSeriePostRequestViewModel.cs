using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels.MovieOrSerieViewModel
{
    public class MovieOrSeriePostRequestViewModel
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        //[Required]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage ="El título debe contener entre 2 y 100 caracteres")]
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        [Range(1,5)]
        public int Score { get; set; }
        //[Range(0, Int32.MaxValue)]
        public List<int> CharactersId { get; set; } = new List<int>();
        //[Range(0, Int32.MaxValue)]
        public List<int> GenresId { get; set; } = new List<int>();
    }
}
