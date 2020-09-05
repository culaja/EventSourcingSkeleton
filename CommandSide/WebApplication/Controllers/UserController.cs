using System.Threading.Tasks;
using ApplicationServices;
using ApplicationWireUp;
using Domain.ForUser;
using Domain.ForUser.Commands;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class UserController : Controller
    {
        private readonly CommandHandlerContainer _commandHandler;

        public UserController(ApplicationContainer applicationContainer)
        {
            _commandHandler = applicationContainer.CommandHandlerContainer;
        }
        
        [HttpPost]
        [Route(nameof(CreateNew))]
        public Task<IActionResult> CreateNew(string name) => 
            _commandHandler.Execute(new CreateUser(UserId.Of(name))).ToActionResult();
    }
}