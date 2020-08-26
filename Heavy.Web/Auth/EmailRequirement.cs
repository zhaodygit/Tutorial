using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Auth
{
    public class EmailRequirement : IAuthorizationRequirement
    {
        public string RequiredEmail { get; set; }
        public EmailRequirement(string requiredEmail)
        {
            RequiredEmail = requiredEmail;
        }
    }
}
