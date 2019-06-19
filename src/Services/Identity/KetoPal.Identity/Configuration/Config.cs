// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace KetoPal.Identity.Configuration
{
    public static class Config
    {
        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // ApiResources define the apis in your system
        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("ketopal-gateway", "Ketopal API")
            };
        }
        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // Postman 
                new Client
                {
                    ClientId = "postman",
                    ClientName = "Postman Testing",
                    ClientSecrets = { new Secret("511536EF-AAAA-CCCC-FFFF-1C89C192F69A".Sha256()) },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.Code,
                    
                    RedirectUris = new List<string>()
                    {
                        "https://www.getpostman.com/oauth2/callback"
                    },

                    AllowedScopes = new List<string>()
                    {
                        "openid", "profile", "ketopal-gateway"
                    }
                }
            };
        }

    }
}