﻿using System;
using System.Threading.Tasks;
using AuthenticationMicroservice.Core.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AuthenticationMicroservice.Core.Providers
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return base.GetPolicyAsync(policyName);
            }

            var permissionNames = policyName.Substring(PermissionAuthorizeAttribute.PolicyPrefix.Length).Split(',');

            var policy = new AuthorizationPolicyBuilder()
                .RequireClaim(Core.Config.PermissionsConfig.Permission, permissionNames)
                .Build();

            return Task.FromResult(policy);
        }
    }
}
