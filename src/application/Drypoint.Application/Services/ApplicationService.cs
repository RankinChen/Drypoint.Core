﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [ApiController]
    public class ApplicationService : IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        public static string[] CommonPostfixes = { "AppService", "ApplicationService" };
    }
}
