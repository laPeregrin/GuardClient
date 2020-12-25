using Guard_Client.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Guard_Client.Services.Abstactions
{
    public delegate TPage CreatePage<TPage>() where TPage : Page;
    public interface IPageFactory
    {
        Task<Page> GetPage(PageType pageType);

    }
}
