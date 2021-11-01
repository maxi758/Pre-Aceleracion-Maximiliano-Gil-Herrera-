using DisneyAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.ViewModels.MovieOrSerieViewModel
{
    public class MovieOrSerieResponseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Range(1,5)]
        public int Score { get; set; }
    }
}
