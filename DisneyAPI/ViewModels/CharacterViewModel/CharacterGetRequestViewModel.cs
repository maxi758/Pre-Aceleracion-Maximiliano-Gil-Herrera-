using System;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels.CharacterViewModel
{
    public class CharacterGetRequestViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [Range(0, Int32.MaxValue)]
        public int Age { get; set; }
        [Range(0, Int32.MaxValue)]
        public int IdMovie { get; set; }
    }
}
