using Microsoft.Extensions.DependencyInjection;
using System;

namespace TJL.ActiveSessionTracker
{
    public static class Setup
    {
        public static IServiceCollection AddActiveSessionTracker(this IServiceCollection services, long millisecondsBeforeSessionIsConsideredCold)
        {
            return services.AddSingleton<IActiveSessionTracker>((services) =>
            {
                var serviceProvider = services.GetService<IServiceProvider>();
                return new ActiveSessionTracker(services, millisecondsBeforeSessionIsConsideredCold);
            });
        }
    }
}
