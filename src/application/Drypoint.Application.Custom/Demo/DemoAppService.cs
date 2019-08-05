﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Drypoint.Application.Custom.Demo.Dto;
using Drypoint.Application.Services;
using Drypoint.Application.Services.Dto.Output;
using Drypoint.Core.Authorization.Users;
using Drypoint.EntityFrameworkCore.Repositories;
using Drypoint.Unity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Drypoint.Application.Custom.Demo
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "admin")]
    [Produces("application/json")]
    [Route(DrypointConsts.ApiPrefix + "Demo")]
    public class DemoAppService : ApplicationService, IDemoAppService
    {
        private readonly ILogger _logger;
        private readonly IRepository<User, long> _userBaseRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        /// <summary>
        ///  构造函数
        /// </summary>
        public DemoAppService(ILogger<DemoAppService> logger,
            IRepository<User, long> userBaseRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userBaseRepository = userBaseRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 测试获取
        /// </summary>
        /// <returns>无参 有返回值</returns>
        [HttpGet]
        [Authorize]
        public ListResultDto<DemoOutputDto> GetAll()
        {
            //ClaimsPrincipal Principal = Thread.CurrentPrincipal as ClaimsPrincipal;
            ClaimsPrincipal Principal = _httpContextAccessor.HttpContext?.User;

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

        /// <summary>
        /// 测试获取根据Id
        /// </summary>
        /// <param name="id">测试编号</param>
        /// <returns>有参 有返回值</returns>
        [HttpGet("{id}")]
        public DemoOutputDto GetById(int id)
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

            return resultData.FirstOrDefault();
        }
    }
}
