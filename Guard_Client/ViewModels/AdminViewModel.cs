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

namespace Guard_Client.ViewModels
{
    public class AdminViewModel : BindableBase
    {
        private readonly UserAndKeyHandler _service;

        private ObservableCollection<DetailsView> users;
        private ObservableCollection<DetailsView> keys;
        private ObservableCollection<DetailsView> permissions;
        private ObservableCollection<DetailsView> userpermissions;

        private DetailsView _selectedUser;
        private DetailsView _selectedKey;
        private DetailsView _selectedPermission;
        public DetailsView SelectedUser { get => _selectedUser; set { _selectedUser = value; RaisePropertyChanged(); } }
        public DetailsView SelectedKey { get => _selectedKey; set { _selectedKey = value; RaisePropertyChanged(); } }
        public DetailsView SelectedPermission { get => _selectedPermission; set { _selectedPermission = value; RaisePropertyChanged(); } }

        public AdminViewModel(UserAndKeyHandler service)
        {
            _service = service;

        }

        public ObservableCollection<DetailsView> Users { get => users; set { users = value; RaisePropertyChanged(); } }
        public ObservableCollection<DetailsView> Keys { get => keys; set { keys = value; RaisePropertyChanged(); } }
        public ObservableCollection<DetailsView> Permissions { get => permissions; set { permissions = value; RaisePropertyChanged(); } }
        //public ObservableCollection<DetailsView> UserPermissions
        //{
        //    get
        //    {
        //        if (SelectedPermission != null)
        //        {
        //            return _service.Ge
        //        }
        //        return null;
        //    }
        //    set { userpermissions = value; RaisePropertyChanged(); }
        //}


        public ICommand GetAllLists => new AsyncCommand(async () =>
        {
            var UserColl = (IEnumerable<User>)await _service.GetAll<User>();
            Keys = (await _service.GetAll(false)).MapToDetailsView();
            Users = UserColl.MapToDetailsView();
            Permissions = ((IEnumerable<Permission>)await _service.GetAll<Permission>()).MapToDetailsView();
        });

    }
}
