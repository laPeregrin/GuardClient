using Guard_Client.Services.Abstactions;
using Guard_Client.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Guard_Client.Services.Implementations
{
    public enum PageType : byte
    {
        General,
        Details,
        CurrentPage,
        History,
        AdminPage
    }
    public class PageFactory : IPageFactory
    {
        private readonly CreatePage<GeneralPage> _getGeneralePage;
        private readonly CreatePage<Details> _getDetailsPage;
        private readonly CreatePage<CurrentPage> _getCurrentPage;
        private readonly CreatePage<History> _getHistoryPage;
        private readonly CreatePage<AdminPage> _getAdminPage;

        public PageFactory(CreatePage<GeneralPage> getGeneralePage,
            CreatePage<Details> getDetailsPage,
            CreatePage<CurrentPage> getCurrentPage,
            CreatePage<History> getHistoryPage, 
            CreatePage<AdminPage> getAdminPage)
        {
            _getGeneralePage = getGeneralePage;
            _getDetailsPage = getDetailsPage;
            _getCurrentPage = getCurrentPage;
            _getHistoryPage = getHistoryPage;
            _getAdminPage = getAdminPage;
        }


        public async Task<Page> GetPage(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.General:
                    return _getGeneralePage();
                case PageType.CurrentPage:
                    return _getCurrentPage();
                case PageType.Details:
                    return _getDetailsPage();
                case PageType.History:
                    return _getHistoryPage();
                case PageType.AdminPage:
                    return _getAdminPage();
                default:
                    return _getGeneralePage();
            }
        }
    }
}
