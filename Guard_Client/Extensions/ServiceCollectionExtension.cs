using DTOs.Services;
using Guard_Client.BLL;
using Guard_Client.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testDAL;

namespace Guard_Client.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceHandler(this IServiceCollection services)
        {
            services.AddSingleton<UserService>();
            services.AddSingleton<KeyObjectService>();
            services.AddSingleton<IBookingActionService, BookingActionService>();

            services.AddSingleton<UserAndKeyHandler>();
            return services;
        }
    }
}
