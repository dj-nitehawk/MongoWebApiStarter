using MongoWebApiStarter.Biz.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MongoWebApiStarter.Api.Auth
{
    public static class Authentication
    {
        private static AppSettings _settings = new AppSettings();

        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, AppSettings settings)
        {
            _settings = settings;

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_settings.Auth.SigningKey))
                        };
                    });

            return services;
        }

        public static IApplicationBuilder UseJWTAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            return app;
        }

        public static FilterCollection RequireAuthenticatedUser(this FilterCollection filters)
        {
            var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
            filters.Add(new AuthorizeFilter(policy));

            return filters;
        }

        public static JwtToken GenerateToken(IEnumerable<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(_settings.Auth.TokenValidityMinutes),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(
                                            new SymmetricSecurityKey(Convert.FromBase64String(_settings.Auth.SigningKey)),
                                            SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);

            return new JwtToken
            {
                Value = token,
                Expiry = tokenDescriptor.Expires.Value
            };
        }

        public static JwtToken GenerateToken(Action<ClaimBuilder> action)
        {
            var builder = new ClaimBuilder();
            action(builder);
            return GenerateToken(builder.GetClaims());
        }
    }
}
