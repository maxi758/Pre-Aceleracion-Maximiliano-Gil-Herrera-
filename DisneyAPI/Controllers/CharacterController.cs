using Microsoft.AspNetCore.Mvc;
using DisneyAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using DisneyAPI.ViewModels.CharacterViewModel;
using DisneyAPI.ViewModels.MovieOrSerieViewModel;
using System;
using DisneyAPI.Entities;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route(template:"api/[controller]")]  
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly IMovieOrSerieRepository _movieOrSerieRepository;
        public CharacterController(ICharacterRepository characterRepository, IMovieOrSerieRepository movieOrSerieRepository)
        {
            _characterRepository = characterRepository;
            _movieOrSerieRepository = movieOrSerieRepository;
        }
        
        [HttpGet]
        [Route("characters")]
        public IActionResult Get()
        {
            try
            {
                var characters = _characterRepository.GetAllCharacters();
                if (characters == null) return BadRequest("No se ha agregado contenido");
                var charactersVM = new List<CharacterListGetViewModel>();
                foreach (var item in characters)
                {
                    charactersVM.Add(new CharacterListGetViewModel
                    {
                        Image = item.Image,
                        Nombre = item.Name
                    
                    });
                }
                return Ok(charactersVM);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest($"Error: {ex.Message}");
            }
            
            
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var character = _characterRepository.GetCharacter(id);
            if (character == null) return BadRequest("El personaje no existe");

            try
            {
                
                var characterVM = new CharacterGetResponseViewModel
                {
                    Id = character.Id,
                    Age = character.Age,
                    Name = character.Name,
                    Image = character.Image,
                    Weight = character.Weight,
                    Story = character.Story
                };
                if (character.MovieOrSeries.Any())
                {
                    var movieOrSerieList = _movieOrSerieRepository.GetAllMoviesOrSeries();
                    
                    foreach (var item in character.MovieOrSeries)
                    {
                        var element = movieOrSerieList.FirstOrDefault(x => x.Id == item.Id);
                        var MovieOrSerieVM = new MovieOrSerieResponseViewModel()
                        {
                            Id = element.Id,
                            Score = element.Score,
                            Title = element.Title

                        };
                        
                        if (element != null)
                        {                           

                            characterVM.MovieOrSeries.Add(MovieOrSerieVM);

                        }
                    }
                }
                return Ok(characterVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest($"Error: {ex.Message}");
            }
            
            
        }
        [HttpGet]
        [Route("obtenerPersonajes")]
        public IActionResult Get([FromQuery] CharacterGetRequestViewModel viewModel)
        {
            var characters = _characterRepository.GetAllCharacters();

            if (!characters.Any()) return NoContent();

            try
            {
                if (viewModel.IdMovie != 0)
                {
                    characters = characters.Where(x => x.MovieOrSeries.FirstOrDefault(x => x.Id == viewModel.IdMovie) != null).ToList();
                   // characters.RemoveAll(x => x.MovieOrSeries.Where(x => x.Id != viewModel.IdMovie) !=null);
                }
                if (!string.IsNullOrEmpty(viewModel.Name))
                {
                    characters = characters.Where(x => x.Name == viewModel.Name).ToList();
                }
                if (viewModel.Age > 0)
                {
                    characters = characters.Where(x => x.Age == viewModel.Age).ToList();
                }

                var charactersVM = new List<CharacterGetResponseViewModel>();
                foreach (var item in characters)
                {
                    charactersVM.Add(new CharacterGetResponseViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Image = item.Image,
                        Age = item.Age,
                        Weight = item.Weight,
                        Story = item.Story,
                        MovieOrSeries = item.MovieOrSeries.Any() ? item.MovieOrSeries.Select(x => new MovieOrSerieResponseViewModel
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Score = x.Score

                        }).ToList() : null
                    });
                }

                return Ok(charactersVM.OrderBy(x => x.Id));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest($"Error: {ex.Message}");
            }

            
        }
        [HttpGet]
        [Route("InfoCompletaPersonajes")]
        public IActionResult GetAllCharacterInfo()
        {
            var characters = _characterRepository.GetAllCharacters();
            if (characters == null) return BadRequest("No hay contenido disponible");
            var charactersVM = new List<CharacterGetResponseViewModel>();
            foreach (var item in characters)
            {
                charactersVM.Add(new CharacterGetResponseViewModel {
                    Id = item.Id,
                    Name = item.Name,
                    Age = item.Age,
                    Weight = item.Weight,
                    Story = item.Story,
                    MovieOrSeries = item.MovieOrSeries.Any() ? item.MovieOrSeries.Select(x => new MovieOrSerieResponseViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Score = x.Score

                    }).ToList() : null
                });
            }
            
            return Ok(charactersVM);
        }
        [HttpPost]
        public IActionResult Post(CharacterPostRequestViewModel characterVM)
        {
            try
            {
                Character character = new Character
                {
                    Id = characterVM.Id,
                    Age = characterVM.Age,
                    Name = characterVM.Name,
                    Weight = characterVM.Weight,
                    Story = characterVM.Story,
                    Image = characterVM.Image
                };
                if (characterVM.MovieOrSeriesId.Any())
                {
                    var movieOrSerieList = _movieOrSerieRepository.GetAllEntities();
                    foreach (var item in characterVM.MovieOrSeriesId)
                    {
                        var element = movieOrSerieList.FirstOrDefault(x => x.Id == item);
                        if (element != null)
                        {
                            character.MovieOrSeries.Add(element);

                        }
                    }
                }

                return Ok(_characterRepository.Add(character));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest($"Error: {ex.Message}");
            }
            
        }
        [HttpPut]
        public IActionResult Put(CharacterPutViewModel characterVM)
        {
            var originalCharacter = _characterRepository.GetCharacter(characterVM.Id);
            if (originalCharacter == null) return BadRequest("El personaje no existe");

            try
            {
                originalCharacter.Age = characterVM.Age;
                originalCharacter.Image = characterVM.Image;
                originalCharacter.Name = characterVM.Name;
                originalCharacter.Story = characterVM.Story;
                originalCharacter.Weight = characterVM.Weight;

                if (characterVM.MovieOrSeriesId.Any())
                {
                    var movieOrSerieList = _movieOrSerieRepository.GetAllEntities();
                    foreach (var item in characterVM.MovieOrSeriesId)
                    {
                        var element = movieOrSerieList.FirstOrDefault(x => x.Id == item);
                        if (element != null)
                        {
                            originalCharacter.MovieOrSeries.Add(element);

                        }
                    }
                }
                _characterRepository.Update(originalCharacter);
                return Ok(originalCharacter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest($"Error: {ex.Message}");
            }
            
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var originalCharacter = _characterRepository.GetCharacter(id);
            if (originalCharacter == null) return BadRequest("El personaje no existe");
            try
            {
                _characterRepository.Delete(originalCharacter.Id);
                return Ok("Eliminado");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest($"Error: {ex.Message}");
            }
           
        }
       
        
    }
}
