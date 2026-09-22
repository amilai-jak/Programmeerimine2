using System.Threading.Tasks;
using KooliProjekt.Application.Features.Assets;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class AssetsController : ApiControllerBase
    {
        // 12.02.2026 - otsingu parameetrid lisatud
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List(int page = 1,
                                              int pageSize = 10,
                                              string name = null,
                                              string ticker = null,
                                              int? assetClassID = null,
                                              bool? isRealEstate = null)
        {
            var query = new ListAssetsQuery
            {
                Page = page,
                PageSize = pageSize,
                Name = name,
                Ticker = ticker,
                AssetClassID = assetClassID,
                IsRealEstate = isRealEstate
            };

            return Result(await Mediator.Send(query));
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetAssetQuery { Id = id };
            var response = await Mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveAssetCommand command)
        {
            var response = await Mediator.Send(command);

            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteAssetCommand command)
        {
            var response = await Mediator.Send(command);

            return Result(response);
        }
    }
}
