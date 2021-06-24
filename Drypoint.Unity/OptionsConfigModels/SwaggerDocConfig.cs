using System;
using System.Collections.Generic;
using System.Text;

namespace Drypoint.Unity.OptionsConfigModels
{
    public class SwaggerDocConfig
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public SwaggerDocContact Contact { get; set; }
        public SwaggerDocLicense License { get; set; }
        public SwaggerDocAuthorize Authorize { get; set; }
    }

    public class SwaggerDocContact
    {
        public string Name { get; set;}
        public string Email { get; set; }
        public string Url { get; set; }
    }
    public class SwaggerDocLicense
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class SwaggerDocAuthorize
    {
        public bool IsShow { get; set; }
        public SwaggerDocAuthorizeClient Client { get; set; }

    }
    public class SwaggerDocAuthorizeClient
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
