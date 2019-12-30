using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using GenHelper;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LookAPI
{
    public class Startup
    {
        string myCORS = "GenProjCORS";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //Remember to test the db for the first load
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();
            /*For setting later please check to Manager.KeyStorage*/
            services.Configure<APISetting>(Configuration.GetSection("APISettings"));
            string TokenKey = Configuration.GetSection("APISettings:TokenKey").Value;
            string TokenIssuer = Configuration.GetSection("APISettings:TokenIssuer").Value;
            string TokenAudience = Configuration.GetSection("APISettings:TokenAudience").Value;
            string TokenAlgo = Configuration.GetSection("APISettings:TokenAlgo").Value;
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());


            //});
            //services.AddAntiforgery(options =>
            //{
            //    options.HeaderName = "X-CSRF-TOKEN";
            //    options.SuppressXFrameOptionsHeader = false;
            //});

            string[] allowedOrigin = { "http://localhost:61138/" };
            services.AddCors(options =>
            {
                options.AddPolicy(myCORS,
                    builder =>
                    {

                        var allowedDomains = new[] { "http://localhost:61138/" };
                        builder.AllowAnyHeader();
                        builder.AllowAnyOrigin();
                        //builder.WithOrigins(allowedOrigin);
                        builder.AllowAnyMethod();
                        builder.AllowCredentials();
                        builder.SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
                    });
            });
            var tokenValidationParameters = new TokenValidationParameters
            {


                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = TokenIssuer,
                ValidAudience = TokenAudience,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(TokenKey)),
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = tokenValidationParameters;
                 options.Events = new JwtBearerEvents()
                 {

                     OnTokenValidated = context =>
                     {
                         string identityName = context.Principal.Identity.Name;
                         JwtSecurityToken accessToken = context.SecurityToken as JwtSecurityToken;
                         SecurityTokenCheck tokenCheck = SecurityTokenVerification(accessToken);
                         if (tokenCheck != null)
                         {
                             if (tokenCheck.Issuer == TokenIssuer & tokenCheck.Audience == TokenAudience & tokenCheck.Algoritma == TokenAlgo & !string.IsNullOrEmpty(tokenCheck.Sid))
                             {
                                
                                 //DALMtUser mgruser = new DALMtUser();
                                 //var checkAuth = mgruser.IsAuthorizedUser(identityName, tokenCheck.Issuer, tokenCheck.Audience, tokenCheck.Algoritma, tokenCheck.Sid).GetAwaiter().GetResult();
                                 ////var checkAuth = mgruser.IsAuthorizedUser(identityName, tokenCheck.Issuer, tokenCheck.Audience, tokenCheck.Algoritma, tokenCheck.Sid,helper.RemoteIPAddress()).GetAwaiter().GetResult();
                                 ////if(check to db based on identit name and sid//verificationcode
                                 //if (checkAuth.Message != MessageInfo.APISuccess)
                                 //{
                                 //    context.Fail(MessageInfo.UnAuthorized);
                                 //}
                                
                                 return Task.CompletedTask;
                             }
                             else
                             {

                                 context.Fail(MessageInfo.UnAuthorized);
                                 return Task.CompletedTask;
                             }
                         }
                         else
                         {
                             context.Fail(MessageInfo.UnAuthorized);
                             return Task.CompletedTask;
                         }
                     }
                      ,
                     OnAuthenticationFailed = context =>
                     {

                         context.Fail("OnAuthenticationFailed: " +
                             context.Exception.Message);
                         return Task.CompletedTask;
                     }

                 };
                 options.SaveToken = true;

             });
        }

        private string CreateSIDCookie(string SID, double tokenAge)
        {
            MemoryCacher memory = new MemoryCacher();
            string cookieValue = string.Empty;
            Guid guid = Guid.NewGuid();
            cookieValue = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString() + ";" + Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();
            //CookieOptions co = new CookieOptions();
            //co.HttpOnly = true;
            //co.MaxAge = TimeSpan.FromMinutes(tokenAge);
            //co.SameSite = SameSiteMode.Strict;
            //HttpContext.Request.HttpContext.Response.Cookies.Append(SID, cookieValue, co);
            memory.Add(SID, cookieValue, DateTimeOffset.Now.AddMinutes(2));
            return cookieValue;
        }
        private bool VerifiedBySID(string SID)
        {
            MemoryCacher memory = new MemoryCacher();
            var hasil = memory.GetValue(SID);
            if (hasil != null)
                return true;
            else
                return false;
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IAntiforgery antiforgery)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    //app.UseBrowserLink();
            //}
            //else
            //{
            //    // app.UseExceptionHandler("/Home/Error");
            //    app.UseHsts();

            //}
            /*http://localhost:51089https://localhost:44390/*/
            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(myCORS);
            app.UseMvc();
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.Run(async (context) =>
            {

                string div = "<div style='display:table;width:100%;height:100%;'>"
               + "<div style='display:table-cell;vertical-align:middle;text-align:center;color:white;font-weight:bold '>"
                + "<label style='font-family:Segoe Script;font-size:40px;font-weight:bold'> Gen Webs API</label><br/>"
                + "<label style='font-size:20px'api </label>"
                + "</div>"
                + "</div>";

                string body = "<div style='position:absolute;width:100%;height:100%;left:0px;top:0px;background-color:gray'>" + div + "</div>";

                await context.Response.WriteAsync(body);

            });



        }
        private SecurityTokenCheck SecurityTokenVerification(JwtSecurityToken accessToken)
        {
            SecurityTokenCheck securityTokenCheck = null;
            if (accessToken != null)
            {
                securityTokenCheck = new SecurityTokenCheck();
                securityTokenCheck.Issuer = accessToken.Issuer;
                securityTokenCheck.Algoritma = accessToken.SignatureAlgorithm;
                if (accessToken.Audiences != null)
                {
                    foreach (string myaudience in accessToken.Audiences)
                    {
                        securityTokenCheck.Audience = myaudience;
                        break;
                    }
                }
                JwtPayload payLoad = accessToken.Payload;
                if (payLoad != null)
                {
                    if (payLoad.ContainsKey("sid"))
                    {
                        object result = null;
                        payLoad.TryGetValue("sid", out result);
                        if (result != null) securityTokenCheck.Sid = result.ToString();
                    }
                }

            }
            return securityTokenCheck;
        }


    }
    public class SecurityTokenCheck
    {
        public string Issuer { get; set; }
        public string Algoritma { get; set; }
        public string Audience { get; set; }
        public string Sid { get; set; }

    }
}
