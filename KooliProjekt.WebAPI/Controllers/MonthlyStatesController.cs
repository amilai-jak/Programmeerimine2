using System.Threading.Tasks;
using KooliProjekt.Application.Features.MonthlyStates;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class MonthlyStatesController : ApiControllerBase
    {
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List(int page = 1, int pageSize = 10)
        {
            return Ok(await Mediator.Send(new List.Query { Page = page, PageSize = pageSize }));
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetMonthlyStateQuery { Id = id };
            var response = await Mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveMonthlyStateCommand command)
        {
            var response = await Mediator.Send(command);

            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteMonthlyStateCommand command)
        {
            var response = await Mediator.Send(command);

            return Result(response);
        }
    }
}
