using System.Collections.Generic;

namespace DisneyAPI.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<MovieOrSerie> MovieOrSeries { get; set; } = new List<MovieOrSerie>(); 

    }
}
