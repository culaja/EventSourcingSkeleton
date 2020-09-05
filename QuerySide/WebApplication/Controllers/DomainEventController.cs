using System.IO;
using System.Text;
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
        private readonly DomainEventProjectionsContainer _domainEventProjector;

        public DomainEventController(ApplicationContainer applicationContainer)
        {
            _domainEventResolver = applicationContainer.DomainEventResolver;
            _domainEventProjector = applicationContainer.DomainEventAppliers;
        }

        [HttpPost]
        [Route(nameof(Apply))]
        public async Task<IActionResult> Apply()
        {
            var domainEvent = await HttpContext.ReadRequestBodyAsString();
            await _domainEventProjector.Apply(_domainEventResolver.ResolveFrom(domainEvent));
            return Ok();
        }
    }
}