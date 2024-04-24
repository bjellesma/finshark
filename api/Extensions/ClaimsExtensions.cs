using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Extensions
{
    /// <summary>
    /// class for getting claims of currently logged in user
    /// </summary>
    public static class ClaimsExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user){
             var userClaims = user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"));
             if(userClaims == null){
                return null;
             }
             return userClaims.Value;
        }
    }
}