using BluenitosToDo.Models;
using BluenitosToDo.Services;
using CodeBlue.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BluenitosToDo.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class AuthController: ControllerBase
    {
        AuthService _authService;
        SqlUsersService _sqlUsersService;

        public AuthController(AuthService authService, SqlUsersService sqlUsersService)
        {
            _authService = authService;
            _sqlUsersService = sqlUsersService;
        }
        /// <summary>
        /// Endpoint responsável pela criação de um novo usuário no banco de dados. Inserir no Json (username, email e passwordhash)
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] Users users)
        {
            try
            {
                var result = _authService.Create(users).Result;
                if (result.Succeeded)
                {
                    users.PasswordHash = default;
                    users.SecurityStamp = default;
                    users.ConcurrencyStamp = default;
                    return Ok(users);
                }

                return BadRequest();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Endpoint responsável por gerar o Token. Inserir no Json (email e passwordhash)
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Token")]
        public IActionResult Token([FromBody] Users users)
        {
            try
            {
                return Ok(_authService.GenerateToken(users));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Endpoint responsável por informar o perfil que está logado em determinada sessão.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LoggedUser")]
        public IActionResult Logged()
        {
            var user = User.Identity.Name;
            var userBD = _sqlUsersService.Get().ToList();
            var userID = userBD.FirstOrDefault(u => u.UserName == user);
            if (userID == null)
            {
                return NotFound("Nenhum usuário logado no momento");
            }
            return Ok(_sqlUsersService.Get(userID.Id));
        }


    }
}
