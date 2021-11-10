using DisneyAPI.Entities;
using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System.Collections.Generic;

namespace DisneyAPI.ViewModels.GenreViewModel
{
    public class GenreGetResponseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<MovieOrSerieGetResponseViewModel> MovieOrSeries { get; set; } = new List<MovieOrSerieGetResponseViewModel>();
    }
}
