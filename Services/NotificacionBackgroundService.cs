using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Keepi.Services;

namespace Keepi.Services
{
    public class NotificacionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificacionBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<INotificacionService>();

                await service.GenerarNotificacionesAutomaticas();

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); //Cada 24 horas se fija si hay notificaciones para crear
            }
        }
    }
}
