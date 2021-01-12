﻿using DTOs.Models;
using DTOs.Services;
using Guard_Client.BLL;
using Guard_Client.Extensions;
using Guard_Client.Services;
using Guard_Client.Services.Abstactions;
using Guard_Client.Services.Implementations;
using Guard_Client.ViewModels;
using Guard_Client.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testDAL;

namespace Guard_Client
{
    public class DI_Container
    {
        private static ServiceProvider _provider;

        public static void Build()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DbTestContext>();
            services.AddSingleton<PageService>();



            services.AddServiceHandler();

            services.AddTransient<CreatePage<CurrentPage>>(services =>
                {
                    return () => services.GetRequiredService<CurrentPage>();
                });
            services.AddTransient<CreatePage<Details>>(services =>
            {
                return () => services.GetRequiredService<Details>();
            });
            services.AddTransient<CreatePage<GeneralPage>>(services =>
            {
                return () => services.GetRequiredService<GeneralPage>();
            });
            services.AddTransient<CreatePage<History>>(services =>
            {
                return () => services.GetRequiredService<History>();
            });
            services.AddTransient<CreatePage<AdminPage>>(services =>
            {
                return () => services.GetRequiredService<AdminPage>();
            });

            services.AddTransient<CurrentPage>();
            services.AddTransient<Details>();
            services.AddTransient<GeneralPage>();
            services.AddSingleton<History>();
            services.AddTransient<AdminPage>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<DetailsViewModel>();
            services.AddTransient<GeneralViewModel>();
            services.AddTransient<AdminViewModel>();
            services.AddTransient<HistoryViewModel>();

            services.AddSingleton<IPageFactory, PageFactory>();

            

            _provider = services.BuildServiceProvider();

            foreach (var item in services)
            {
                _provider.GetRequiredService(item.ServiceType);
            }
        }

        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();
        public DetailsViewModel DetailsViewModel => _provider.GetRequiredService<DetailsViewModel>();
        public GeneralViewModel GeneralViewModel => _provider.GetRequiredService<GeneralViewModel>();
        public HistoryViewModel HistoryViewModel => _provider.GetRequiredService<HistoryViewModel>();
        public AdminViewModel AdminViewModel => _provider.GetRequiredService<AdminViewModel>();
    }
}
