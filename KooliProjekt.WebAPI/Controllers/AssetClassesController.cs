using KooliProjekt.Application.Features.AssetClasses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class AssetClassesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> List()
        {
            return Ok(await Mediator.Send(new List.Query()));
        }
    }
}
