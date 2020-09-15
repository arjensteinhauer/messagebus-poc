using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace MB.Client.Gateway.Service.Controllers.V1
{
    [ApiController]
    [Route("api/v1")]
    public class ImageController : ImageControllerBase
    {
        public override Task<AddImageResponse> Add([BindRequired, FromBody] AddImageRequest body)
        {
            throw new System.NotImplementedException();
        }

        public override Task<DeleteImageResponse> Delete([BindRequired, FromBody] DeleteImageRequest body)
        {
            throw new System.NotImplementedException();
        }

        public override Task<GetImageDetailsResponse> GetDetails([BindRequired, FromBody] GetImageDetailsRequest body)
        {
            throw new System.NotImplementedException();
        }

        public override Task<ProcessImageResponse> Process([BindRequired, FromBody] ProcessImageRequest body)
        {
            throw new System.NotImplementedException();
        }

        public override Task<ImageSearchResponse> Search([BindRequired, FromBody] SearchRequest body)
        {
            throw new System.NotImplementedException();
        }

        public override Task<UpdateImageDetailsResponse> UpdateDetails([BindRequired, FromBody] UpdateImageDetailsRequest body)
        {
            throw new System.NotImplementedException();
        }
    }
}
