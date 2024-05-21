using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MS.Core.MM.Infrastructures.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace MS.Core.MM.Infrastructures.Extensions
{
    /// <summary>
    /// <see cref="SwaggerGenOptions"/>擴充類別
    /// </summary>
    public static class SwaggerGenOptionsExtension
    {
        /// <summary>
        /// 增加JWT Bearer Token驗證UI
        /// <example>
        /// <code>
        /// var builder = ..........;
        /// builder.Services.AddJwtBearerTokenAuthentication(<see cref="JwtSetting"/>);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="swaggerGenOptions"><see cref="SwaggerGenOptions"/></param>
        public static void AddJwtBearerTokenUI(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization"
                });

            swaggerGenOptions.OperationFilter<AuthorizationOperationFilter>();
        }

        /// <summary>
        /// 新增Xml註解檔案
        /// </summary>
        /// <param name="swaggerGenOptions"></param>
        public static void AddIncludeXmlComments(this SwaggerGenOptions swaggerGenOptions, IEnumerable<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

                if (File.Exists(filePath))
                {
                    swaggerGenOptions.IncludeXmlComments(filePath);
                }
            }
        }

        /// <summary>
        /// 註冊JWT Bearer Token驗證
        /// <example>
        /// <code>
        /// builder.Services.AddSwaggerGen(options => options.AddJwtBearerTokenUI(); );
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="issuer"><see cref="issuer"/></param>
        /// /// <param name="signingKey"><see cref="signingKey"/></param>
        public static void AddJwtBearerTokenAuthentication(this IServiceCollection services, string? issuer, string? signingKey)
        {
            ArgumentNullException.ThrowIfNull(issuer);
            ArgumentNullException.ThrowIfNull(signingKey);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                    options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey ?? ""))
                    };
                });
        }
    }
}