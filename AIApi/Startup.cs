using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using Polly;

using AIApi.Events;
using AIApi.Messages;
using AIApi.Services;
using AIApi.CommandHandlers;
using AIApi.Classifier;

namespace AIApi
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AIApi", Version = "v1" });
            });

            services.Configure<AppSettings>(Configuration);

            services.AddSingleton<ISmsRequestService, SmsRequestService>();
            services.AddTransient<IThirdPartyService, ThirdPartyService>();
            services.AddTransient<ISmsSendCommandHandler, SmsSendCommandHandler>();
            services.AddTransient<ITensorFlowPredictionStrategy, TensorFlowPredictionStrategy>();

            RegisterEventBus(services);

            services.AddHttpClient("ThirdParty", c =>
            {
                c.BaseAddress = new Uri("http://localhost");
            }).AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(5, _ => TimeSpan.FromMilliseconds(500)
           ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AIApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<SmsSentEvent, IIntegrationEventHandler<SmsSentEvent>>();
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBus>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<EventBus>>();
                return new EventBus(logger);
            });
        }
    }
}
