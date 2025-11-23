using KooliProjekt.Application.Features.Assets;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class AssetsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> List()
        {
            return Ok(await Mediator.Send(new List.Query()));
        }
    }
}
