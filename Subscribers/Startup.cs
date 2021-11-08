using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Subscribers
{
    public class Startup
    {
              
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   

            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                
                builder.WithOrigins(_configuration.GetValue<string>("connectionLocalHost")).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                }));

                 
            services.AddControllers();


            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddXmlDataContractSerializerFormatters();
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("ApiCorsPolicy");
            

            app.UseMvc();

            app.Run(async (context) => 
            {
                await context.Response.WriteAsync("MVC didn't find anything!");
            });

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
