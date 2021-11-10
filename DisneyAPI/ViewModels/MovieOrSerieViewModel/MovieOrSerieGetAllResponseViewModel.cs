using DisneyAPI.ViewModels.CharacterViewModel;
using DisneyAPI.ViewModels.GenreViewModel;
using System;
using System.Collections.Generic;

namespace DisneyAPI.ViewModels.MovieOrSerieViewModel
{
    public class MovieOrSerieGetAllResponseViewModel
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public int Score { get; set; }
        public List<CharacterListGetViewModel> Characters { get; set; } = new List<CharacterListGetViewModel>();
        public List<GenreGetResponseViewModel> Genre { get; set; } = new List<GenreGetResponseViewModel>();
    }
}
