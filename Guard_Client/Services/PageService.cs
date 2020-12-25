using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Guard_Client.Services
{
    public class PageService
    {
        public event Action<Page> PageChanged;
        public void OnPageChanged(Page page) => PageChanged?.Invoke(page);
    }
}
