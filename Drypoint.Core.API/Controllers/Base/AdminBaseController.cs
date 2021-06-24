using Drypoint.Unity;
using Microsoft.AspNetCore.Mvc;

namespace Drypoint.Core.Controllers.Base
{
    /// <summary>
    /// Admin组的Controller 用作SwaggerAPI分组
    /// </summary>
    [ApiExplorerSettings(GroupName = DrypointConsts.AdminAPIGroupName)]
    [Route(DrypointConsts.ApiPrefix + "[controller]")]
    public abstract class AdminBaseController : ControllerBase
    {
    }
}
