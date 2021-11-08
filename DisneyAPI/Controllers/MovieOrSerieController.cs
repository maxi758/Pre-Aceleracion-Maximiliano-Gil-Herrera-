using Microsoft.AspNetCore.Mvc;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.ViewModels.CharacterViewModel;
using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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
        public MovieOrSerieController(IMovieOrSerieRepository movieOrSerieRepository, ICharacterRepository characterRepository, IGenreRepository genderRepository)
        {
            _movieOrSerieRepository = movieOrSerieRepository;
            _characterRepository = characterRepository;
            _genderRepository = genderRepository;
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
                foreach (var item in movieOrSeries)
                {
                    moviesOrSeriesVM.Add(new MovieOrSerieGetResponseViewModel
                    {
                        Imagen = item.Imagen,
                        CreationDate = item.CreationDate,
                        Title = item.Title

                    });
                }
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
                MovieOrSerieGetAllDataViewModel movieOrSerieVM = new MovieOrSerieGetAllDataViewModel
                {
                    Id = movieOrSerie.Id,
                    Imagen = movieOrSerie.Imagen,
                    CreationDate = movieOrSerie.CreationDate,
                    Title = movieOrSerie.Title,
                    Score = movieOrSerie.Score,

                };
                if (movieOrSerie.Characters.Any())
                {
                    foreach (var item in movieOrSerie.Characters)
                    {
                        var element = movieOrSerie.Characters.FirstOrDefault(x => x.Id == item.Id);
                        var characterVM = new CharacterGetResponseViewModel
                        {
                            Id = element.Id,
                            Name = element.Name,
                            Image = element.Image,
                            Age = element.Age,
                            Weight = element.Weight
                        };
                        if (element != null)
                        {
                            movieOrSerieVM.Characters.Add(characterVM);
                        }
                    }
                }
                if (movieOrSerie.Genre.Any())
                {

                    foreach (var item in movieOrSerie.Genre)
                    {
                        var element = movieOrSerie.Genre.FirstOrDefault(x => x.Id == item.Id);
                        var genreVM = new GenreGetResponseViewModel
                        {
                            Id = element.Id,
                            Name = element.Name,
                            Image = element.Image
                        };
                        if (element != null)
                        {
                            movieOrSerieVM.Genres.Add(genreVM);

                        }
                    }
                }
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
                    var auxMovieOrSerie = new List<MovieOrSerie>();
                    foreach (var item in movieOrSeriesList)
                    {
                        var element = item.Genre.Where(x => x.Id == viewModel.GenreId);
                        if (element != null)
                        {

                            auxMovieOrSerie.Add(item);

                        }
                    }
                    movieOrSeriesList = movieOrSeriesList.Intersect(auxMovieOrSerie).ToList();
                }
                if (!string.IsNullOrEmpty(viewModel.Name))
                {
                    movieOrSeriesList = movieOrSeriesList.Where(x => x.Title == viewModel.Name).ToList();
                }

                if (!movieOrSeriesList.Any()) return NoContent();

                var movieOrSerieVM = new List<MovieOrSerieGetResponseViewModel>();
                foreach (var item in movieOrSeriesList)
                {
                    movieOrSerieVM.Add(new MovieOrSerieGetResponseViewModel
                    {
                        Imagen = item.Imagen,
                        Title = item.Title,
                        CreationDate = item.CreationDate

                    });
                }
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
                MovieOrSerie movieOrSerie = new MovieOrSerie
                {
                    Id = movieOrSerieVM.Id,
                    CreationDate = movieOrSerieVM.CreationDate,
                    Imagen = movieOrSerieVM.Imagen,
                    Title = movieOrSerieVM.Title,
                    Score = movieOrSerieVM.Score
                };
                if (movieOrSerieVM.CharactersId.Any())
                {                  
                    var characterList = _characterRepository.GetAllCharacters();
                    if (characterList.Any())
                    {
                        if (movieOrSerie.Characters == null)
                        {
                            movieOrSerie.Characters = new List<Character>();
                        }
                        foreach (var item in movieOrSerieVM.CharactersId)
                        {
                            var element = characterList.FirstOrDefault(x => x.Id == item);
                            if (element != null)
                            {
                                movieOrSerie.Characters.Add(element);

                            }
                        }
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
                        foreach (var item in movieOrSerieVM.GenresId)
                        {
                            var element = gendersList.FirstOrDefault(x => x.Id == item);
                            if (element != null)
                            {
                                movieOrSerie.Genre.Add(element);

                            }
                        }

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
                originalMovieOrSerie.CreationDate = movieOrSerieVM.CreationDate;
                originalMovieOrSerie.Imagen = movieOrSerieVM.Imagen;
                originalMovieOrSerie.Score = movieOrSerieVM.Score;
                originalMovieOrSerie.Title = movieOrSerieVM.Title;
                if (movieOrSerieVM.CharactersId.Any())
                {
                    var characterList = _characterRepository.GetAllCharacters();
                    foreach (var item in movieOrSerieVM.CharactersId)
                    {
                        var element = characterList.FirstOrDefault(x => x.Id == item);
                        if (element != null)
                        {
                            originalMovieOrSerie.Characters.Add(element);

                        }
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
