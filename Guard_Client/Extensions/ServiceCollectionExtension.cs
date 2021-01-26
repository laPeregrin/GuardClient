using DTOs.Services;
using Guard_Client.BLL;
using Guard_Client.Services.Abstactions;
using Guard_Client.Services.Implementations;
using Guard_Client.Views.Pages;
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
            services.AddSingleton<PermissionService>();

            services.AddSingleton<UserAndKeyHandler>();
            return services;
        }

        public static IServiceCollection AddDelegatePages(this IServiceCollection services)
        {
            services.AddTransient<CreatePage<CurrentPage>>(services =>
            {
                return () => services.GetService<CurrentPage>();
            });
            services.AddTransient<CreatePage<Details>>(services =>
            {
                return () => services.GetService<Details>();
            });
            services.AddTransient<CreatePage<GeneralPage>>(services =>
            {
                return () => services.GetService<GeneralPage>();
            });
            services.AddTransient<CreatePage<History>>(services =>
            {
                return () => services.GetService<History>();
            });
            services.AddTransient<CreatePage<AdminPage>>(services =>
            {
                return () => services.GetService<AdminPage>();
            });
            return services;
        }
    }
}
