using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModels.CharacterViewModel
{
    public class CharacterGetResponseViewModel
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Image { get; set; }
        public string Story { get; set; }
        public List<MovieOrSerieResponseViewModel> MovieOrSeries { get; set; } = new List<MovieOrSerieResponseViewModel>();
    }
}
