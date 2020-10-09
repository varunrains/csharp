using IplServerSide.Enums;
using IplServerSide.Persistence;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IplServerSide.Core.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.CompletedTask;
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //TODO: Manually resolving see if there are any better approach from Startup classes
            var bettingContext = new BettingContext();
            var uow= new UnitOfWork(bettingContext);
            using  (uow)
            {
                var user = uow.Users.ValidateUser(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return Task.CompletedTask;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRoleEnum), user.UserRole)));
                identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserId.ToString()));
                context.Validated(identity);
            }
            return  Task.CompletedTask;
        }
    }
}