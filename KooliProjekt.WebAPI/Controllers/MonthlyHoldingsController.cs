using System.Threading.Tasks;
using KooliProjekt.Application.Features.MonthlyHoldings;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class MonthlyHoldingsController : ApiControllerBase
    {
        // 12.02.2026 - otsingu parameetrid lisatud
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List(int page = 1,
                                              int pageSize = 10,
                                              string assetName = null,
                                              int? assetID = null,
                                              int? stateID = null)
        {
            var query = new ListMonthlyHoldingsQuery
            {
                Page = page,
                PageSize = pageSize,
                AssetName = assetName,
                AssetID = assetID,
                StateID = stateID
            };

            return Result(await Mediator.Send(query));
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetMonthlyHoldingQuery { Id = id };
            var response = await Mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveMonthlyHoldingCommand command)
        {
            var response = await Mediator.Send(command);

            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteMonthlyHoldingCommand command)
        {
            var response = await Mediator.Send(command);

            return Result(response);
        }
    }
}
