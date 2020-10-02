using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace TokenServiceApi
{
    public class Config
    {
        public static Dictionary<string, string> GetUrls(IConfiguration configuration)
        {
            Dictionary<string, string> urls = new Dictionary<string, string>();
            urls.Add("Mvc", configuration.GetValue<string>("MvcClient"));

            return urls;
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("basket", "basket api"),
                new ApiScope("order", "order api"),
                new ApiScope("report", "report api")
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("basket", "Shopping Cart Api")
                {
                    Scopes = new List<string>
                    {
                        "basket"
                    }
                },
                new ApiResource("order", "Ordering Api")
                {
                    Scopes = new List<string>
                    {
                        "order"
                    }
                },
                new ApiResource("report", "Report Api"){
                    Scopes = new List<string>
                    {
                        "report"
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientUrls)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = new []{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = {$"{clientUrls["Mvc"]}/signin-oidc"},
                    PostLogoutRedirectUris = {$"{clientUrls["Mvc"]}/signout-callback-oidc"},
                    AllowAccessTokensViaBrowser = false,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    RequirePkce = false,
                    AlwaysIncludeUserClaimsInIdToken  =true,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "order",
                        "basket",
                        "report"
                    }
                }
            };
        }
    }
}
