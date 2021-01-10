using DevExpress.Mvvm;
using Guard_Client.Services;
using Guard_Client.Services.Abstactions;
using Guard_Client.Services.Implementations;
using Guard_Client.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Guard_Client.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly IPageFactory _pageFactory;
        private readonly IServiceProvider _serviceProvider;

        private Page _source;
        public Page PageSource { get { return _source; } set { _source = value; RaisePropertiesChanged(); } }
        public MainViewModel(PageService pageService, IPageFactory pageFactory, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _pageFactory = pageFactory;
            _pageService = pageService;
            _pageService.PageChanged += (page) => PageSource = page;
            _pageService.OnPageChanged(_pageFactory.GetPage(PageType.General).Result);
        }


        #region Commands
        public ICommand MoveToGeneralPage => new AsyncCommand(async () =>
        {
            PageSource = await _pageFactory.GetPage(PageType.General);
        });
        public ICommand MoveToCurrentPage => new AsyncCommand(async () =>
        {
            PageSource = await _pageFactory.GetPage(PageType.CurrentPage);
        });
        public ICommand MoveToHistoryPage => new AsyncCommand(async () =>
        {
            PageSource = await _pageFactory.GetPage(PageType.History);
        });
        public ICommand MoveToDetailsPage => new AsyncCommand(async () =>
        {
            PageSource = await _pageFactory.GetPage(PageType.Details);
        });
        #endregion Commands
    }
}
