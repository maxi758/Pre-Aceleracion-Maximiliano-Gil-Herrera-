using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModels.CharacterViewModel
{
    public class CharacterPutViewModel
    {
        public int Id { get; set; }
        [Range(1, Int32.MaxValue)]
        public int Age { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [Range(1, Int32.MaxValue)]
        public int Weight { get; set; }
        [MaxLength(150)]
        public string Image { get; set; }
        public string Story { get; set; }
        //[Range(0, Int32.MaxValue)]
        public List<int> MovieOrSeriesId { get; set; }
    }
}
