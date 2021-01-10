using DevExpress.Mvvm;
using Guard_Client.BLL;
using Guard_Client.DomainModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.ViewModels
{
    public class CurrentViewModels : BindableBase
    {
        private readonly UserAndKeyHandler _service;


        #region Items for View
        public ObservableCollection<DetailsView> _keyCollection;

        private DetailsView SelectedItemInList;
        //and add here stringMessage with all permission wich have current person
        public DetailsView SelectedItemInList1 { get => SelectedItemInList; set { SelectedItemInList = value; RaisePropertyChanged(); } }
        #endregion Items for View

        public CurrentViewModels(UserAndKeyHandler service)
        {
            //here gonna be a listener of porter. And subscribing to serialPorterEvent
            _service = service;
        }

    }
}
