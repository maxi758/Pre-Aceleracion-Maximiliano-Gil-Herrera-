using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModels.MovieOrSerieViewModel
{
    public class MovieOrSerieGetResponseViewModel
    {
        public string Imagen { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
