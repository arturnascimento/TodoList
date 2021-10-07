using BluenitosToDo.Models;
using BluenitosToDo.Roles;
using BluenitosToDo.Services;
using CodeBlue.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BluenitosToDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController: ControllerBase
    {
        SqlTodoService _sqlTodoService;
        SqlUsersService _sqlUsersService;

        public TodoController(SqlTodoService sqlTodoService, SqlUsersService sqlUsersService)
        {
            _sqlTodoService = sqlTodoService;
            _sqlUsersService = sqlUsersService;
        }

        /// <summary>
        /// Endpoint responsável pela criação de uma nova tarefa. Inserir no Json (nome da tarefa, tarefa, prazo para execução e prioridade)
        /// </summary>
        /// <param name="todoModel"></param>
        /// <returns></returns>
        [HttpPost]
        [RoleAuthorize(RoleTypes.SuperAdmin, RoleTypes.Usuario)]
        public IActionResult Create([FromBody] TodoModel todoModel)
        {
            var userlogged = User.Identity.Name;
            var UserTask = _sqlUsersService.Get().ToList();
            todoModel.User = UserTask.FirstOrDefault(u => u.UserName == userlogged);
            return _sqlTodoService.Create(todoModel) ? Ok(_sqlTodoService.Get(todoModel.IdTask)) : NotFound("Erro ao Criar Tarefa");
        }

        /// <summary>
        /// Endpoint responsável por buscar no banco de tados todas as tarefas criadas pelo usuário que está logado.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(RoleTypes.SuperAdmin, RoleTypes.Usuario)]

        public IActionResult GetAll()
        {
            var userlogged = User.Identity.Name;
            return Ok(_sqlTodoService.Get(userlogged));
        }

        /// <summary>
        /// Endpoint responsável por buscar tarefas do usuário logado por ordem de prioridade. Endpoint só lista tarefas com Status false(incompleta) (Alta - Média - Baixa)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [RoleAuthorize(RoleTypes.SuperAdmin, RoleTypes.Usuario)]
        [Route("GetPriority")]
        public IActionResult GetByPriority()
        {
            var userlogged = User.Identity.Name;
            return Ok(_sqlTodoService.GetPriority(userlogged));
        }


        /// <summary>
        /// Endpoint responsável por buscar uma tarefa no banco de dados, recebendo o ID como parâmetro da busca.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [RoleAuthorize(RoleTypes.SuperAdmin, RoleTypes.Usuario)]
        public IActionResult Get(int id) => _sqlTodoService.Get(id) != null ? Ok(_sqlTodoService.Get(id)) : NotFound("Tarefa não existe");

        /// <summary>
        /// Endpoint responsável por mudar o Status da tarefa com o ID como parâmetro se o Status for true a tarefa foi concluída.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("status/{id}")]
        [RoleAuthorize(RoleTypes.SuperAdmin, RoleTypes.Usuario)]
        public IActionResult ChangeStatus(int id) => _sqlTodoService.ChangeStatus(id) != null ? Ok(_sqlTodoService.ChangeStatus(id)) : NotFound("Tarefa não existe");

        /// <summary>
        /// Endpoint responsável por editar as tarefas já cadastradas no banco de dados.
        /// </summary>
        /// <param name="todoModel"></param>
        /// <returns></returns>
        [HttpPut]
        [RoleAuthorize(RoleTypes.SuperAdmin, RoleTypes.Usuario)]
        public IActionResult Update([FromBody] TodoModel todoModel)
        {
           
            return _sqlTodoService.Update(todoModel) ? Ok(_sqlTodoService.Get(todoModel.IdTask)) : NotFound("Erro ao atualizar Tarefa");
        }

        /// <summary>
        /// Endpoint responsável por deletar as tarefas, caso seja desejo do usuário.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [RoleAuthorize(RoleTypes.SuperAdmin, RoleTypes.Usuario)]

        public IActionResult Delete(int id) => _sqlTodoService.Delete(id) ? Ok() : NotFound("Tarefa não existe");


    }
}
