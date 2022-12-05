using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VolunteerListAPI.Models;

namespace VolunteersAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHeaderPropagation(o =>
            {
                o.Headers.Add("Authorization");
            });

            services
           .AddAuthentication()
           .AddJwtBearer("AzureAdB2CSAML", options =>
           {
               //options.Authority = $"{Configuration["AzureAdB2CSAML:Instance"]}/{Configuration["AzureAdB2CSAML:TenantId"]}/v2.0";
               options.Authority = $"{Configuration["AzureAdB2CSAML:Instance"]}/tfp/{Configuration["AzureAdB2CSAML:TenantId"]}/{Configuration["AzureAdB2CSAML:SignUpSignInPolicyId"]}/v2.0/";
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidIssuer = $"{Configuration["AzureAdB2CSAML:Instance"]}/{Configuration["AzureAdB2CSAML:TenantId"]}/v2.0",
                   ValidateAudience = true,
                   ValidAudience = Configuration["AzureAdB2CSAML:ClientId"],
                   ValidateLifetime = true
               };
           })
           .AddJwtBearer("AzureAdB2C", options =>
           {
               options.Authority = $"{Configuration["AzureAdB2C:Instance"]}/tfp/{Configuration["AzureAdB2C:TenantId"]}/{Configuration["AzureAdB2C:SignUpSignInPolicyId"]}/v2.0/";
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidIssuer = $"{Configuration["AzureAdB2C:Instance"]}/{Configuration["AzureAdB2C:TenantId"]}/v2.0/",
                   ValidateAudience = true,
                   ValidAudience = Configuration["AzureAdB2C:ClientId"],
                   ValidateLifetime = true
               };
           })
           .AddJwtBearer("FAMSConfig", options =>
         {
             options.Authority = Configuration["FAMSConfig:Authority"];
             options.Audience = Configuration["FAMSConfig:Audience"]; ;

             options.RequireHttpsMetadata = bool.Parse(Configuration["FAMSConfig:RequireHttpsMetadata"]);

         });

            services
                .AddAuthorization(options =>
                {
                    //Applies to [Authorize] header in controller - ie "AzureAdB2CSAML", "AzureAdB2C", "FAMSConfig" token accepted.
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("AzureAdB2CSAML", "AzureAdB2C", "FAMSConfig")
                        .Build();
                    //[Authorize(Policy = "InternalUsers")] - When this header used in controller, only AAD token accepted.
                    options.AddPolicy("InternalUsers", new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("FAMSConfig")
                        //.RequireClaim("role", "admin") //could be role/scope
                        .Build());
                });


            #region Investigate
            // services.AddTransient<MultipleSchemaAuthenticationMiddleware>();

            // services.AddAuthentication("AzureAdB2C")
            //        .AddMicrosoftIdentityWebApi(options =>
            //        {
            //            Configuration.Bind("AzureAdB2C", options);

            //            options.TokenValidationParameters.NameClaimType = "name";
            //        },
            //options => { Configuration.Bind("AzureAdB2C", options); });


            //services.AddMicrosoftIdentityWebApiAuthentication
            //    (Configuration, "AzureAdB2C", "myADscheme");
            ////                (options =>
            //{
            //    Configuration.Bind("AzureAdB2C", options);

            //    options.TokenValidationParameters.NameClaimType = "name";
            //},
            //           options => { Configuration.Bind("AzureAdB2C", options); });

            //services.AddAuthentication(
            //    OpenIdConnectDefaults.AuthenticationScheme).AddJwtBearer(options =>
            //    {
            //        Configuration.Bind("AzureAdB2CSAML", options);
            //    });

            // services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddJwtBearer("Bearer", options =>
            //{
            //    options.Authority = Configuration["FAMSConfig:Authority"];
            //    options.Audience = Configuration["FAMSConfig:Audience"]; ;
            //    options.RequireHttpsMetadata = bool.Parse(Configuration["FAMSConfig:RequireHttpsMetadata"]);
            //}); ;

            // services.AddMicrosoftIdentityWebApiAuthentication(Configuration, "AzureAdB2CSAML", Microsoft.Identity.Web.Constants.AzureAdB2C);
            //services.AddMicrosoftIdentityWebApiAuthentication(Configuration, "AzureAdB2C", Microsoft.Identity.Web.Constants.AzureAdB2C);

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2CSAML"), "AzureAdB2CSAML")
            //.EnableTokenAcquisitionToCallDownstreamApi()
            //.AddInMemoryTokenCaches();

            //services.AddAuthentication()
            //        .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"), "AzureAdB2C", "B2CScheme")
            //            .EnableTokenAcquisitionToCallDownstreamApi();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(options =>
            //    {
            //        Configuration.Bind("AzureAdB2CSAML", options);

            //        options.TokenValidationParameters.NameClaimType = "name";
            //    },
            //options => { Configuration.Bind("AzureAdB2CSAML", options); },
            //"Bearer");

            //services.AddAuthentication()
            //    .AddJwtBearer(Microsoft.Identity.Web.Constants.AzureAdB2C, options =>
            //     {
            //         Configuration.Bind("AzureAdB2C", options);
            //         options.TokenValidationParameters.NameClaimType = "name";
            //     });

            //.AddCookie(options =>
            //{
            //    options.ExpireTimeSpan = new TimeSpan(7, 0, 0, 0);
            //}).AddJwtBearer("AzureAdB2CSAML", options =>
            // {
            //     Configuration.Bind("AzureAdB2CSAML", options);
            //     //    // options.Authority = "https://devvrb2c.b2clogin.com/devvrb2c.onmicrosoft.com/";
            //     //     //options.Authority = "https://devvrb2c.b2clogin.com";
            //     //     //options.Audience = "http://localhost:4200/";
            //     //     //options.TokenValidationParameters.NameClaimType = "name";
            // });


            //services.AddAuthentication()
            //       .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAdB2C"), Microsoft.Identity.Web.Constants.AzureAdB2C, null);
            //services.AddAuthentication()
            //       .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAdB2CSAML"), Microsoft.Identity.Web.Constants.AzureAd, null);

            //// Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
            //services.AddAuthentication()
            //        .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2CSAML"),
            //"AzureAdB2CSAML", "Bearer");

            // services.AddAuthentication()
            //        .AddMicrosoftIdentityWebApi(options =>
            //        {
            //            Configuration.Bind("AzureAdB2C", options);

            //            options.TokenValidationParameters.NameClaimType = "name";
            //        },
            //options => { Configuration.Bind("AzureAdB2C", options); });

            // Code omitted for brevity
            //services.AddAuthentication("AzureAdB2C")
            //.AddJwtBearer("AzureAdB2C", options =>
            // {
            //     Configuration.Bind("AzureAdB2C", options);
            //    // options.Authority = "https://devvrb2c.b2clogin.com/devvrb2c.onmicrosoft.com/";
            //     //options.Authority = "https://devvrb2c.b2clogin.com";
            //     //options.Audience = "http://localhost:4200/";
            //     //options.TokenValidationParameters.NameClaimType = "name";
            // });
            //.AddJwtBearer("AzureAdB2CSAML", options =>
            //{
            //    Configuration.Bind("AzureAdB2CSAML", options);
            //    options.Authority = "https://devvrb2c.b2clogin.com/devvrb2c.onmicrosoft.com/";
            //    //options.TokenValidationParameters.NameClaimType = "name";
            //    //options.Authority = "https://devvrb2c.b2clogin.com";
            //    options.Audience = "http://localhost:4400/";
            //});


            //    services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme) // This means default scheme is "OpenIdConnect"
            //.AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAdB2C"), OpenIdConnectDefaults.AuthenticationScheme);

            //    services.AddAuthentication() // Note that we don't provide the default scheme (there is only one default)
            //.AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAdB2CSAML"), "B2CSAML", "cookiesB2C");


            // Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
            //var authBuilder = services.AddAuthentication();

            //authBuilder.AddJwtBearer("AzureAdB2C", "AzureAdB2C",
            //options => { Configuration.Bind("AzureAdB2C", options); })
            //           ;

            //authBuilder.AddJwtBearer("AzureAdB2CSAML", "AzureAdB2CSAML",
            //options => { Configuration.Bind("AzureAdB2CSAML", options); })
            //           ;

            //var authBuilder = services.AddAuthentication();

            //// Sign-in users with the Microsoft identity platform
            //authBuilder.AddJwtBearer(Configuration, "AzureAd", "AzureAd", "AzureAdCookies");

            //// Sign-in users with AzureADB2C
            //authBuilder.AddSignIn(Configuration, "AzureAdB2C", "AzureAdB2C", "AzureAdB2CCookies");


            // Creating policies that wraps the authorization requirements
            //services.AddAuthorization(options =>
            //{
            //    options.DefaultPolicy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "AzureAdB2C")
            //        .Build();
            //});
            #endregion




            services.AddDbContext<VolunteerContext>(opt => opt.UseInMemoryDatabase("VolunteerList"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VolunteerAPI", Version = "v1" });
            });

            // Allowing CORS for all domains and methods for the purpose of the sample
            // In production, modify this with the actual domains you want to allow
            services.AddCors(o => o.AddPolicy("default", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VolunteerAPI v1"));
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("default");
            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseMyMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
