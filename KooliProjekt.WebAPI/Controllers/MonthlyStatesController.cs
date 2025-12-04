using KooliProjekt.Application.Features.MonthlyStates;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class MonthlyStatesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> List(int page = 1, int pageSize = 10)
        {
            return Ok(await Mediator.Send(new List.Query { Page = page, PageSize = pageSize }));
        }
    }
}
