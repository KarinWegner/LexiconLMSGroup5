using Domain.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Presemtation.Controllers
{

    public class ApiControllerBase : ControllerBase
    {
        [NonAction]
        public ActionResult ProcessError(ApiBaseResponse baseResponse)
        {
            return baseResponse switch
            {
                ApiNotFoundResponse => NotFound(
                (
                    Detail: ((ApiNotFoundResponse)baseResponse).Message,
                    StatusCode: StatusCodes.Status404NotFound,
                    Title: "Not Found",
                    Instance: HttpContext.Request.Path
                )),
                _ => throw new NotImplementedException()
            };
        }
    }
}
