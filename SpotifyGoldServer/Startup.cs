using Microsoft.OpenApi.Models;

namespace SpotifyGoldServer {
    public partial class Startup {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseSwagger();

            app.UseSwaggerUI();
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpotifyGoldServer", Version = "v1" });
            });
        }
    }
}
