﻿using Microsoft.AspNetCore.Builder;

namespace AuthenticationMicroservice.Core.Extentions
{
    public static class MiddlewareExtensions
    {
        static MiddlewareExtensions()
        {
        }

        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            // UseMiddleware -> Extension Method -> using Microsoft.AspNetCore.Builder;
            return builder
                .UseMiddleware<Middlewares.ExceptionHandlingMiddleware>();
        }

    }
}
