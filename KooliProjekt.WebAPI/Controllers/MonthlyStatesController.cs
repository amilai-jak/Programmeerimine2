using KooliProjekt.Application.Features.MonthlyStates;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class MonthlyStatesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> List()
        {
            return Ok(await Mediator.Send(new List.Query()));
        }
    }
}
