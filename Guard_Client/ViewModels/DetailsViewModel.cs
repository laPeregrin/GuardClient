using DevExpress.Mvvm;
using DTOs.Models;
using DTOs.Services;
using Guard_Client.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guard_Client.Extensions;
using testDAL;
using Guard_Client.DomainModels;
using Guard_Client.BLL;
using System.Windows.Input;
using System.Windows;

namespace Guard_Client.ViewModels
{
    public class DetailsViewModel : BindableBase
    {
        private IDataService<KeyObject> _keyObjService;
        private UserAndKeyHandler _userAndKeyHandler;


        private ObservableCollection<DetailsView> _keyCollections;

        public ObservableCollection<DetailsView> BookedKeyCollections { get { return _keyCollections; } set { _keyCollections = value; RaisePropertyChanged(); } }
        private DetailsView selectedKey;
        private DetailsView currentKey;
        public DetailsView? SelectedKey { get { return selectedKey; } set { selectedKey = value; RaisePropertyChanged(); } }
        public DetailsView? CurrentKey { get { return currentKey; } set { currentKey = value; RaisePropertyChanged(); } }
        public DetailsViewModel(KeyObjectService dataService, UserAndKeyHandler handler)
        {
            _keyObjService = dataService;
            _userAndKeyHandler = handler;

            Task.Run(async () =>
            {
                var items = await _keyObjService.GetAll(x => x.IsBooked == true);
                BookedKeyCollections = items.MapToDetailsView();

            }).GetAwaiter().GetResult();
        }

        public ICommand UpdateCurrentUser => new AsyncCommand(async () =>
        {
            if (SelectedKey != null)
            {
                CurrentKey = await _userAndKeyHandler.GetCurrentInfoByKeyAuditory(SelectedKey.KeyNumber);
            }
            else
            {
                MessageBox.Show("Для начала выберете ключ из списка");
            }
        });
    }
}
