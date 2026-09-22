using System.Threading.Tasks;
using KooliProjekt.Application.Features.AssetClasses;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class AssetClassesController : ApiControllerBase
    {
        // 12.02.2026 - otsingu parameeter lisatud
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List(int page = 1, int pageSize = 10, string name = null)
        {
            var query = new ListAssetClassesQuery
            {
                Page = page,
                PageSize = pageSize,
                Name = name
            };

            return Result(await Mediator.Send(query));
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetAssetClassQuery { Id = id };
            var response = await Mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveAssetClassCommand command)
        {
            var response = await Mediator.Send(command);

            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteAssetClassCommand command)
        {
            var response = await Mediator.Send(command);

            return Result(response);
        }
    }
}
