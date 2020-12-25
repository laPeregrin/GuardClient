using Guard_Client.Services;
using Guard_Client.Services.Abstactions;
using Guard_Client.Services.Implementations;
using Guard_Client.ViewModels;
using Guard_Client.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client
{
    public class DI_Container
    {
        private static ServiceProvider _provider;

        public static void Build()
        {
            var services = new ServiceCollection();


            services.AddSingleton<PageService>();


            services.AddScoped<CreatePage<CurrentPage>>(services =>
            {
                return () => services.GetRequiredService<CurrentPage>();
            });
            services.AddScoped<CreatePage<Details>>(services =>
            {
                return () => services.GetRequiredService<Details>();
            });
            services.AddScoped<CreatePage<GeneralPage>>(services =>
            {
                return () => services.GetRequiredService<GeneralPage>();
            });
            services.AddScoped<CreatePage<History>>(services =>
            {
                return () => services.GetRequiredService<History>();
            });

            services.AddScoped<CurrentPage>();
            services.AddScoped<Details>();
            services.AddScoped<GeneralPage>();
            services.AddScoped<History>();

            services.AddScoped<MainViewModel>();

            services.AddSingleton<IPageFactory, PageFactory>(services =>
            {
                return new PageFactory(services.GetRequiredService<CreatePage<GeneralPage>>(), services.GetRequiredService<CreatePage<Details>>(),
                services.GetRequiredService<CreatePage<CurrentPage>>(), services.GetRequiredService<CreatePage<History>>());
            });

            _provider = services.BuildServiceProvider();

            foreach (var item in services)
            {
                _provider.GetRequiredService(item.ServiceType);
            }
        }

        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();
    }
}
