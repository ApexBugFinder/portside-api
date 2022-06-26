using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.EntityFrameworkCore;
using Portfolio.WebApp.Services;
using Portfolio.WebApp.Abstract;
using Portfolio.WebApp.Concrete;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System;
using Portfolio.WebApp.Helpers;

using Microsoft.AspNetCore.Authentication;
namespace Portfolio.WebApp
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {

      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    public readonly string MustOwnProject = "_mustOwnProject";

    public readonly string DefaultAuthorizedPolicy = "_defaultAuthorizedPolicy";

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();

services.AddAuthentication()
  .AddIdentityServerAuthentication(options => {
    options.Authority = "https://identity.portside.cyou";
    options.RequireHttpsMetadata = false;
    options.ApiName = "portfoliowebapi";

    })
  // .AddJwtBearer("Bearer", options => {
  //   options.Authority="https://identity.portside.cyou";
  //   options.RequireHttpsMetadata = false;
  //   options.Audience = "portfoliofront portfoliowebapi";

  // })
  // .AddCertificate(options => {
  //   options.AllowedCertificateTypes = Microsoft.AspNetCore.Authentication.Certificate.CertificateTypes.All;
  //   options.RevocationMode = X509RevocationMode.NoCheck;
  // })
  ;

// services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));

      // services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
      //     .AddIdentityServerAuthentication(options =>
      //     {
      //       options.Authority = "http://localhost:34004";
      //       options.ApiName = "portfoliowebapi";
      //       options.ApiSecret = "apisecret";
      //       options.RequireHttpsMetadata = false;
      //       // options.SaveToken = true;

      //     });
      services.AddControllers();
     IdentityModelEventSource.ShowPII = true;


    services.Configure<ForwardedHeadersOptions>(options => {
       options.ForwardLimit = 3;
       options.KnownProxies.Add(IPAddress.Parse("23.94.40.225"));
       options.ForwardedForHeaderName = "X-Forwarded-For-Webapi-Portside";
    });
     services.AddCertificateForwarding(options => {
       options.CertificateHeader = "X-SSL-CERT";

       options.HeaderConverter = (headerValue) => {
         X509Certificate2 clientCertificate = null;
         if (!string.IsNullOrWhiteSpace(headerValue))
         {
           var bytes = Encoding.UTF8.GetBytes(Uri.UnescapeDataString(headerValue));
           clientCertificate = new X509Certificate2(bytes);

         }
         return clientCertificate;
       };
     });


     services.AddHttpsRedirection(options => {
        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
        options.HttpsPort = 443;
      });

      services.AddAuthorization(authorizationOptions =>
      {
        // authorizationOptions.AddPolicy(DefaultAuthorizedPolicy, policy =>
        // {
        //   policy.Requirements.Add();
        // });
        authorizationOptions.AddPolicy(MustOwnProject,
                  policyBuilder =>
                  {

                    policyBuilder.RequireAuthenticatedUser();
                    // policyBuilder.AddRequirements();
                  });
      });




      services.AddCors(options =>
      {

        options.AddPolicy(MyAllowSpecificOrigins,
            builder =>
            {
                builder.WithOrigins(
                 //API

                "https://23.94.40.225:8085",
                "http://23.94.40.225:8086",
                "https://23.94.40.225",

                "https://webapi.portside.cyou",
                "http://webapi.portside.cyou",

                        "http://localhost:8000", //IDP


                        "https://23.94.40.225:8000",

                        "https://identity.portside.cyou",
                        "http://identity.portside.cyou",

                        "http://localhost:4200",//CLIENT
                        "https://localhost:4200",
                        "https://portside.cyou",

                        "http://localhost:3000",
                        "https://23.94.40.225",
                        "http://23.94.40.225:3000")

                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithMethods("GET", "PUT", "POST", "DELETE")
                        ;
              });
      });

      // var connectionString = Configuration["ConnectionStrings:PortfolioDomainDB"];
       var connectionString = "Server=localhost,1433; Database = PortfolioDB;User Id = sa;Password ='Apple&Pie79';MultipleActiveResultSets=true;Persist Security Info=True;";
      //  var connectionString = "Server=23.94.40.225,1433; Database = PortfolioDB;User Id = sa;Password = Apple&Pie79;MultipleActiveResultSets=true;Persist Security Info=True;";
      //var windowString = "Server=localhost,1433;Database=PortfolioDomainDB;User Id=Orville;Password=pass@123;Trusted_Connection=True;Persist Security Info=True;";
      // var linuxString = "Server=localhost,1433;Database=PortfolioDomainDB;User Id=sa;Password=Yukon900;Trusted_Connection=True;Persist Security Info=True;";
      services.AddDbContext<PortfolioContext>(o => o.UseSqlServer(connectionString));

      services.AddScoped<IProjectRepository, EFProjectRepository>();
      services.AddScoped<IProjectCreatorRepository, EFProjectCreatorRepository>();
      services.AddScoped<IProjectLinkRepository, EFProjectLinkRepository>();
      services.AddScoped<IProjectRequirementRepository, EFProjectRequirementRepository>();
      services.AddScoped<IPublishedHistoryRepository, EFPublishedHistoryRepository>();
      services.AddScoped<IExperienceRepository, EFExperienceRepository>();
      services.AddScoped<IRolesRepository, EFRoleRepository>();
      services.AddScoped<ICertificationRepository, EFCertificationRepository>();
      services.AddScoped<IDegreeRepository, EFDegreeRepository>();

      //services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCors(MyAllowSpecificOrigins);
      app.UseAuthorization();
     app.UseAuthentication();


      app.UseEndpoints(endpoints => endpoints.MapControllers()
		  .RequireCors(MyAllowSpecificOrigins)
		     );
    }
  }
}

