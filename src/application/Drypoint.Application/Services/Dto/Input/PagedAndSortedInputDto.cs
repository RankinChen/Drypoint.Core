using Drypoint.Application.Services.Dto.Interface;
using Drypoint.Unity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Application.Services.Dto.Input
{
    /// <summary>
    /// 
    /// </summary>
    public class PagedAndSortedInputDto : PagedInputDto, ISortedBaseInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Sorting { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PagedAndSortedInputDto()
        {
            MaxResultCount = DrypointConsts.DefaultPageSize;
        }
    }
}
