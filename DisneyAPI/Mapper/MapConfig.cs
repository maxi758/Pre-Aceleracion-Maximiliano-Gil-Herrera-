using AutoMapper;
using DisneyAPI.Entities;
using DisneyAPI.ViewModels.CharacterViewModel;
using DisneyAPI.ViewModels.GenreViewModel;
using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Mapper
{
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            //Character Controller
            CreateMap<CharacterPostRequestViewModel, Character>().ReverseMap(); //POST 
            CreateMap<CharacterPutViewModel, Character>();// PUT
            CreateMap<Character, CharacterGetResponseViewModel>(); //GET
            CreateMap<Character, CharacterGetRequestViewModel>(); //GET 
            CreateMap<Character, CharacterListGetViewModel>().ReverseMap();
            
            //Movie Controller
            CreateMap<MovieOrSeriePostRequestViewModel, MovieOrSerie>(); //POST 
            CreateMap<MovieOrSeriePutViewModel, MovieOrSerie>();// PUT
            CreateMap<MovieOrSerie, MovieOrSerieGetResponseViewModel>().ReverseMap();//GET
            CreateMap<MovieOrSerie, MovieOrSerieGetRequestViewModel>();//GET
            CreateMap<MovieOrSerie, MovieOrSerieGetAllResponseViewModel>().ReverseMap();//GET ALL
            CreateMap<MovieOrSerie, MovieOrSerieResponseViewModel>().ReverseMap();//GET 
            
            //Genre
            CreateMap<Genre, GenreGetResponseViewModel>();//GET
            CreateMap<GenrePostRequestViewModel, Genre>(); //POST
            CreateMap<GenrePutRequestViewModel, Genre>(); //PUT
        }
    }
}
