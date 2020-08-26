using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Auth
{
    public class CanEditAlbumHandler : AuthorizationHandler<QualifiedUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, QualifiedUserRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "Edit Role"))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
