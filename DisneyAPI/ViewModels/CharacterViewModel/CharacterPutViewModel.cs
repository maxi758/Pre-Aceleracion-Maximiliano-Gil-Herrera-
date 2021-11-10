using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels.CharacterViewModel
{
    public class CharacterPutViewModel
    {
        [Required]
        [Range(1, Int32.MaxValue)]
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
        public List<int> MovieOrSeriesId { get; set; }
    }
}
