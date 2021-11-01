using DisneyAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModels.MovieOrSerieViewModel
{
    public class GenreGetResponseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<MovieOrSerie> MovieOrSeries { get; set; } = new List<MovieOrSerie>();
    }
}
