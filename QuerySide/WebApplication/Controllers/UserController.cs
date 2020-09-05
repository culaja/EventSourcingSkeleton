using System.Threading.Tasks;
using ApplicationServices.ProjectionDefinitions.ForUser;
using ApplicationWireUp;
using Framework;
using Framework.Querying;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class UserController : Controller
    {
        private readonly IProjection<CreatedUsersView> _createdUsersViewProjection;

        public UserController(ApplicationContainer applicationContainer)
        {
            _createdUsersViewProjection = applicationContainer.CreatedUsersViewProjection;
        }

        [HttpGet]
        [Route(nameof(Get))]
        public async Task<IActionResult> Get()
        {
            var maybeView = await _createdUsersViewProjection.Fetch(string.Empty);
            return Ok(maybeView.Unwrap());
        }
    }
}