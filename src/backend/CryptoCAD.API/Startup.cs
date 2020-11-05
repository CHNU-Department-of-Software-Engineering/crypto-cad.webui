using CryptoCAD.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoCAD.API
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
            services.AddControllers();

            var sqlConnectionString = Configuration["PostgreSqlConnectionString"];
            services.AddDbContext<PostgreSqlContext>(options => options.UseNpgsql("Host=ec2-54-155-22-153.eu-west-1.compute.amazonaws.com;Database=d8t2dpkjsm9aut;Username=wvpxlarxsmatbl;Password=ffb468082d1641aab267fb8fbf154dbfd58f3ac79fd8d9818278ea83604366cd"));

            services.AddSpaStaticFiles(configuration: options => { options.RootPath = "wwwroot"; });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<PostgreSqlContext>();
            //services.AddAuthentication(
            //    options =>
            //    {
            //        options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //    })
            //    .AddGoogle(options =>
            //    {
            //        options.ClientId = "625136470332-242md2h36pg5seipffud830v7ijsrrb8.apps.googleusercontent.com";
            //        options.ClientSecret = "6k20t4zbbDhJRDlqMvb_LnvZ";
            //    });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Okta:Authority"];
                    options.Audience = "api://default";
                });

            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoCAD API", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("VueCorsPolicy", builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("https://localhost:5001");
                });
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoCAD API v1");
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();

            app.UseCors("VueCorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSpaStaticFiles();
            app.UseSpa(configuration: builder =>
            {
                if (env.IsDevelopment())
                {
                    builder.UseProxyToSpaDevelopmentServer("http://localhost:8080");
                }
            });
        }
    }
}
