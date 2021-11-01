using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Entities
{
    public class MovieOrSerie
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        //[Range(1,5)]
        public int Score { get; set; }
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        public ICollection<Genre> Genre { get; set; } = new List<Genre>();
    }
}
