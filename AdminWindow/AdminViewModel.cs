using DevExpress.Mvvm;
using DTOs.Models;
using Guard_Client.BLL;
using Guard_Client.DomainModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Guard_Client.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Guard_Client.Services.Implementations;
using testDAL;

namespace AdminWindow
{
    public class AdminViewModel : BindableBase
    {
        private readonly UserAndKeyHandler _service;

        private DetailsView _selectedUser;
        private DetailsView _selectedKey;


        public AdminViewModel(UserAndKeyHandler service)
        {
            _service = service;
        }
        public AdminViewModel()
        {
            DbTestContext context = new DbTestContext();
            _service = new UserAndKeyHandler(new UserService(context), new KeyObjectService() );
        }

        public ObservableCollection<DetailsView> Users;
        public ObservableCollection<DetailsView> Keys;



        public ICommand GetAllLists => new AsyncCommand(async () =>
        {
            var UserColl = (IEnumerable<User>)await _service.GetAll<User>();
            Keys = (await _service.GetAll(false)).MapToDetailsView();
            Users = UserColl.MapToDetailsView();
        });

        public DetailsView SelectedUser { get => _selectedUser; set { _selectedUser = value; RaisePropertyChanged(); } }
        public DetailsView SelectedKey { get => _selectedKey; set { _selectedKey = value; RaisePropertyChanged(); } }
    }
}
