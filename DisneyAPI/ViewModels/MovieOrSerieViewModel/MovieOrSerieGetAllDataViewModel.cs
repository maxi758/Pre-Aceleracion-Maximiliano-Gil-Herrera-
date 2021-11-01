using DisneyAPI.ViewModels.CharacterViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModels.MovieOrSerieViewModel
{
    public class MovieOrSerieGetAllDataViewModel
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public int Score { get; set; }
        public List<CharacterGetResponseViewModel> Characters { get; set; } = new List<CharacterGetResponseViewModel>();
        public List<GenreGetResponseViewModel> Genres { get; set; } = new List<GenreGetResponseViewModel>();
    }
}
