using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoApi.Models;

namespace ToDoApi {
    public class Startup {
        public Startup( IConfiguration configuration ) {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services ) {
            // services.AddDbContext<TodoContext>( opt => opt.UseInMemoryDatabase( "TodoList" ) );
            services.AddDbContext<TodoContext>( opt => opt.UseSqlServer( Configuration.GetConnectionString( "TodoApiConnection" ) ) );
            services.AddControllers();
            services.AddCors( options =>
            options.AddPolicy( MyAllowSpecificOrigins,
            builder => { builder.WithOrigins( "http://localhost:4200" ).AllowAnyMethod().AllowAnyHeader(); }
            )
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env ) {

            if ( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors( MyAllowSpecificOrigins );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints( endpoints => {
                endpoints.MapControllers();
            } );

        }

    }
}
