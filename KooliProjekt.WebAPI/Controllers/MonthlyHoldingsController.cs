using KooliProjekt.Application.Features.MonthlyHoldings;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class MonthlyHoldingsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> List()
        {
            return Ok(await Mediator.Send(new List.Query()));
        }
    }
}
