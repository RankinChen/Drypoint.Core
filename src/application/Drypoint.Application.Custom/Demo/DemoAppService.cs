using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drypoint.Application.Custom.Demo.Dto;
using Drypoint.Application.Services.Dto.Output;
using Drypoint.Core.Authorization.Users;
using Drypoint.EntityFrameworkCore.Repositories;
using Drypoint.Unity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Drypoint.Application.Custom.Demo
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route(DrypointConsts.ApiPrefix + "[controller]")]
    public class DemoAppService : IDemoAppService
    {
        private readonly ILogger _logger;
        private readonly IRepository<UserBase, long> _userBaseRepository;

        /// <summary>
        ///  构造函数
        /// </summary>
        public DemoAppService(ILogger<DemoAppService> logger,
            IRepository<UserBase, long> userBaseRepository)
        {
            _logger = logger;
            _userBaseRepository = userBaseRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ListResultDto<DemoOutputDto> GetAll()
        {
            var data = _userBaseRepository.GetAll().ToList();
            var resultData = data.Select(aa => new DemoOutputDto
            {
                Id = aa.Id,
                Name = aa.Name
            }).ToList();

            ListResultDto<DemoOutputDto> ltResult = new ListResultDto<DemoOutputDto>()
            {
                Items = resultData
            };

            return ltResult;
        }
    }
}
