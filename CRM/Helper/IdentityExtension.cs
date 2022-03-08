using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CRM.Helper
{
    public static class IdentityExtension
    {
        public static string GetId(this IIdentity identity, string claimName, string key)
        {
            try
            {
                ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
                // Claim claim = claimsIdentity.FindFirst(claimName);

                return AesOperation.DecryptString(key, claimsIdentity.FindFirst(claimName).Value);
            }
            catch
            {

                return "not found claim";
            }

        }
        public static string GetId(this IIdentity identity, string claimName)
        {
            try
            {
                ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
                // Claim claim = claimsIdentity.FindFirst(claimName);
                string tg = claimsIdentity.FindFirst(claimName).Value;

                return claimsIdentity.FindFirst(claimName).Value;
            }
            catch
            {


                return "not found claim";
            }

        }
    }
}
