using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.ViewModels.Auth.Login;
using DisneyAPI.ViewModels.Auth.Register;
using DisneyAPI.ViewModels.Services.MailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DisneyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;
        


        public AuthenticationController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mailService = mailService;
        }

        //Registro de usuarios SIN roles

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterRequestViewModel model)
        {
            //Revisa si existe el usuario
            var userExist = await _userManager.FindByNameAsync(model.Username);
            var emailExist = await _userManager.FindByEmailAsync(model.Email);
            String registrationDataError = "";
            //Si existe, devolver un error
            if (userExist != null || emailExist != null)
            {
                if (userExist != null && emailExist != null)
                {
                    registrationDataError += $"Usuario {model.Username} y Email {model.Email}";
                }

                else if (userExist != null)
                {
                    registrationDataError += $"Usuario {model.Username}";
                }

                else if (emailExist != null)
                {
                    registrationDataError += $"Email {model.Email} ";
                }
                return StatusCode(StatusCodes.Status400BadRequest, $"{registrationDataError} en uso");
            }
            
            //Si no existe, lo crea
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "Error",
                        Message = $"User creation failed! Errors: {string.Join(",", result.Errors.Select(x => x.Description))}"
                    });

            }

            MailServiceRequestViewModel mailVM = new MailServiceRequestViewModel
            {
                Email = user.Email,
                Username = user.UserName
            };
            await _mailService.SendEmail(mailVM);
            return Ok(new
            {
                Status = "Success",
                Message = "User Created Successfully!"
            });
        }

        // Registro de usuarios CON roles

        [HttpPost]
        [Route("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterRequestViewModel model)
        {
            //Revisa si ya estan en uso usuario y/o mail
            var userExist = await _userManager.FindByNameAsync(model.Username);
            var emailExist = await _userManager.FindByEmailAsync(model.Email);
            //Si existe, devolver un error

            String registrationDataError = "";
            if (userExist != null || emailExist != null)
            {
                if (userExist != null && emailExist != null)
                {
                    registrationDataError += $"Usuario {model.Username} y Email {model.Email}";
                }

                else if (userExist != null)
                {
                    registrationDataError += $"Usuario {model.Username}";
                }

                else if (emailExist != null)
                {
                    registrationDataError += $"Email {model.Email} ";
                }
                return StatusCode(StatusCodes.Status400BadRequest, $"{registrationDataError} en uso");
            }

            //Si no existe, lo crea
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "Error",
                        Message = $"User creation failed! Errors: {string.Join(",", result.Errors.Select(x => x.Description))}"
                    });

            }
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _userManager.AddToRoleAsync(user, "User");

            MailServiceRequestViewModel mailVM = new MailServiceRequestViewModel
            {
                Email = user.Email,
                Username = user.UserName
            };
            await _mailService.SendEmail(mailVM);
            return Ok(new
            {
                Status = "Success",
                Message = "User Created Successfully!"
            });

        }
        //Login con autenticación de usuario y creación de token
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequestViewModel model)
        {
            //Verificar que los datos de logueo sea correctos
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (result.Succeeded)
            {
                var currentUser = await _userManager.FindByNameAsync(model.Username);
                if (currentUser.IsActive)
                {
                    return Ok(await GetToken(currentUser));
                }
                
            }
          
            return StatusCode(StatusCodes.Status401Unauthorized,
                new
                {
                    Status = "Error",
                    Message = $"El usuario {model.Username} no esta autorizado"
                }); ;
        }
        //Generación de token
        private async Task<LoginResponseViewModel> GetToken(User currentUser)
        {
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, currentUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            authClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));
            string jwtKey = Startup.Configuration["JwtKey"];
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );
            return new LoginResponseViewModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };
        }
    }
    
}
