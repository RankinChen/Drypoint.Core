using Drypoint.Unity;
using Microsoft.AspNetCore.Mvc;

namespace Drypoint.Core.Controllers.Base
{
    /// <summary>
    /// App组的Controller 用作SwaggerAPI分组
    /// </summary>
    [ApiExplorerSettings(GroupName = DrypointConsts.AppAPIGroupName)]
    [Route(DrypointConsts.ApiPrefix + "[controller]")]
    public abstract class AppBaseController : ControllerBase
    {

    }
}
