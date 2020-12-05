using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQPoc.BackgroundServices;
using RabbitMQPoc.Infra.RabbitMQ;
using RabbitMQPoc.Infra.RabbitMQ.Consumers;
using RabbitMQPoc.Infra.RabbitMQ.Contracts;
using RabbitMQPoc.Infra.RabbitMQ.Producers;
using RabbitMQPoc.ModelSettings;

namespace RabbitMQPoc
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RabbitMQSettings>(_configuration.GetSection("RabbitMQ"));

            services.AddSingleton<IRabbitMQContext, RabbitMQContext>();

            services.AddScoped<IMainProducer, MainProducer>();

            services.AddTransient<IMainConsumer, MainConsumer>();
            services.AddTransient<ILogConsumer, LogConsumer>();

            services.AddHostedService<RabbitMQBackgroundService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RabbitMQPoc", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RabbitMQPoc v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
