using Microsoft.AspNetCore.Mvc;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.ViewModels.CharacterViewModel;
using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using DisneyAPI.ViewModels.GenreViewModel;
using AutoMapper;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    [Authorize]
    public class MovieOrSerieController : ControllerBase
    {
        private readonly IMovieOrSerieRepository _movieOrSerieRepository;
        private readonly IGenreRepository _genderRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;
        public MovieOrSerieController(IMovieOrSerieRepository movieOrSerieRepository, ICharacterRepository characterRepository, 
            IGenreRepository genderRepository, IMapper mapper)
        {
            _movieOrSerieRepository = movieOrSerieRepository;
            _characterRepository = characterRepository;
            _genderRepository = genderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("movies")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                var movieOrSeries = _movieOrSerieRepository.GetAllMoviesOrSeries();
                if (movieOrSeries == null) return NoContent();
                var moviesOrSeriesVM = new List<MovieOrSerieGetResponseViewModel>();
                _mapper.Map<List<MovieOrSerie>, List<MovieOrSerieGetResponseViewModel>>(movieOrSeries, moviesOrSeriesVM);
                return Ok(moviesOrSeriesVM);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
        }

        [HttpGet]
        [Route("movieDetails")]
        public IActionResult Get(int id)
        {
            try
            {
                var movieOrSerie = _movieOrSerieRepository.GetMovieOrSerie(id);

                if (movieOrSerie == null) return BadRequest("La pelicula o serie  no existe");
                var movieOrSerieVM = _mapper.Map<MovieOrSerie, MovieOrSerieGetAllResponseViewModel>(movieOrSerie);
                
                return Ok(movieOrSerieVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
        }
        
        [HttpGet]
        [Route("BusquedaPeliculasOSeries")]
        public IActionResult Get([FromQuery] MovieOrSerieGetRequestViewModel viewModel)
        {
            try
            {
                var movieOrSeriesList = _movieOrSerieRepository.GetAllMoviesOrSeries();
                if (!movieOrSeriesList.Any()) return NoContent();

                if (viewModel.GenreId != 0)
                {
                    movieOrSeriesList = movieOrSeriesList.Where(x => x.Genre.FirstOrDefault(x => x.Id == viewModel.GenreId) != null).ToList();
                }
                if (!string.IsNullOrEmpty(viewModel.Name))
                {
                    movieOrSeriesList = movieOrSeriesList.Where(x => x.Title == viewModel.Name).ToList();
                }

                if (!movieOrSeriesList.Any()) return NoContent();

                var movieOrSerieVM = new List<MovieOrSerieGetResponseViewModel>();
                _mapper.Map<List<MovieOrSerie>, List<MovieOrSerieGetResponseViewModel>>(movieOrSeriesList, movieOrSerieVM);

                if (!string.IsNullOrEmpty(viewModel.Order)) return (viewModel.Order.ToUpper() == "DESC") ? Ok(movieOrSerieVM.OrderByDescending(x => x.CreationDate))
                     : (viewModel.Order.ToUpper() == "ASC") ? Ok(movieOrSerieVM.OrderBy(x => x.CreationDate)): Ok(movieOrSerieVM);
                return Ok(movieOrSerieVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }


        }
        [HttpPost]
        public IActionResult Post(MovieOrSeriePostRequestViewModel movieOrSerieVM)
        {
            try
            {
                var movieOrSerie = _mapper.Map<MovieOrSeriePostRequestViewModel, MovieOrSerie>(movieOrSerieVM);
                
                if (movieOrSerieVM.CharactersId.Any())
                {                  
                    var characterList = _characterRepository.GetAllCharacters();
                    if (characterList.Any())
                    {
                        if (movieOrSerie.Characters == null)
                        {
                            movieOrSerie.Characters = new List<Character>();
                        }
                        movieOrSerie.Characters = characterList.Where(x => x.Id == movieOrSerieVM.CharactersId.FirstOrDefault(y => y == x.Id)).ToList();
                       
                    }
                  
                }
                if (movieOrSerieVM.GenresId.Any())
                {
                    var gendersList = _genderRepository.GetAllGenders();
                    if (gendersList.Any())
                    {
                        if (movieOrSerie.Genre == null)
                        {
                            movieOrSerie.Genre = new List<Genre>();
                        }
                        movieOrSerie.Genre = gendersList.Where(x => x.Id == movieOrSerieVM.GenresId.FirstOrDefault(y => y == x.Id)).ToList();
                        
                    }
                    
                }
                return Ok(_movieOrSerieRepository.Add(movieOrSerie));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }           
        }

        [HttpPut]
        public IActionResult Put(MovieOrSeriePutViewModel movieOrSerieVM)
        {
            var originalMovieOrSerie = _movieOrSerieRepository.GetMovieOrSerie(movieOrSerieVM.Id);
            if (originalMovieOrSerie == null) return BadRequest("La pelicula o serie no existe");
            try
            {
                _mapper.Map<MovieOrSeriePutViewModel, MovieOrSerie>(movieOrSerieVM, originalMovieOrSerie);

                if (movieOrSerieVM.CharactersId.Any())
                {
                    var characterList = _characterRepository.GetAllCharacters();
                    originalMovieOrSerie.Characters = characterList.Where(x => x.Id == movieOrSerieVM.CharactersId.FirstOrDefault(y => y == x.Id)).ToList();
                    
                }
                if (movieOrSerieVM.GenresId.Any())
                {
                    var gendersList = _genderRepository.GetAllGenders();
                    if (gendersList.Any())
                    {
                        originalMovieOrSerie.Genre = gendersList.Where(x => x.Id == movieOrSerieVM.GenresId.FirstOrDefault(y => y == x.Id)).ToList();
                    }

                }
                return Ok(_movieOrSerieRepository.Update(originalMovieOrSerie));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult Delete(int id)
        {
            var originalMovieOrSerie = _movieOrSerieRepository.GetMovieOrSerie(id);
            if (originalMovieOrSerie == null) return BadRequest("La pelicula o serie no existe");
            try
            {
                _movieOrSerieRepository.Delete(originalMovieOrSerie.Id);
                return Ok(" Eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            
        }
    }
}
