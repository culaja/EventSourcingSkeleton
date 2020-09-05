using System.Threading.Tasks;
using ApplicationServices;
using ApplicationWireUp;
using Microsoft.AspNetCore.Mvc;
using NewtonSoftAdapter;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class DomainEventController : Controller
    {
        private readonly NewtonSoftDomainEventResolver _domainEventResolver;
        private readonly DomainEventHandlerContainer _domainEventHandler;

        public DomainEventController(ApplicationContainer applicationContainer)
        {
            _domainEventResolver = applicationContainer.DomainEventResolver;
            _domainEventHandler = applicationContainer.DomainEventHandlerContainer;
        }

        [HttpPost]
        [Route(nameof(Apply))]
        public async Task<IActionResult> Apply()
        {
            var domainEvent = await HttpContext.ReadRequestBodyAsString();
            var result = await _domainEventHandler.Handle(_domainEventResolver.ResolveFrom(domainEvent)).ToActionResult();
            return result;
        }
    }
}