using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
