using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Entities
{
    public class Character
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Story { get; set; }
        public ICollection<MovieOrSerie> MovieOrSeries { get; set; } 
    }
}
