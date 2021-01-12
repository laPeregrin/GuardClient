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
using Guard_Client.Services;
using Guard_Client.Exceptions;

namespace Guard_Client.ViewModels
{
    public class AdminViewModel : BindableBase
    {
        private readonly UserAndKeyHandler service;
        #region PRIVATE FIELDS FOR MVVM
        private ObservableCollection<DetailsView> users;
        private ObservableCollection<DetailsView> keys;
        private ObservableCollection<DetailsView> permissions;
        private ObservableCollection<DetailsView> userpermissions;

        private DetailsView _selectedUser;
        private DetailsView _selectedKey;
        private DetailsView _selectedPermission = new DetailsView();
        private DetailsView _selectedUserInUsersPermission;

        private string newKeyNumber;
        private string _LastName;
        private string _MiddleName;
        private string _FirstName;
        #endregion PRIVATE FIELDS FOR MVVM

        #region PUBLIC INPUT PROP FOR MVVM
        public string LastName { get => _LastName; set { _LastName = value; RaisePropertyChanged(); } }
        public string MiddleName { get => _MiddleName; set { _MiddleName = value; RaisePropertyChanged(); } }
        public string FirstName { get => _FirstName; set { _FirstName = value; RaisePropertyChanged(); } }
        public string NewKeyNumber { get { return newKeyNumber; } set { newKeyNumber = value; RaisePropertyChanged(); } }
        #endregion PUBLIC INPUT PROP FOR MVVM
        public DetailsView SelectedUser { get { return _selectedUser; } set { _selectedUser = value; RaisePropertyChanged(); } }
        public DetailsView SelectedKey { get { return _selectedKey; } set { _selectedKey = value; RaisePropertyChanged(); } }
        public DetailsView SelectedPermission { get { return _selectedPermission; } set { _selectedPermission = value; RaisePropertyChanged(); } }
        public DetailsView SelecteduserInPermission { get { return _selectedUserInUsersPermission; } set { _selectedUserInUsersPermission = value; RaisePropertyChanged(); } }



        public AdminViewModel(UserAndKeyHandler service)
        {
            this.service = service;
        }
        #region PUBLIC LISTBOX DATA
        public ObservableCollection<DetailsView> Users { get => users; set { users = value; RaisePropertyChanged(); } }                    //UserCollectionForView
        public ObservableCollection<DetailsView> Keys { get => keys; set { keys = value; RaisePropertyChanged(); } }                       //KeysCollectionForView
        public ObservableCollection<DetailsView> Permissions { get => permissions; set { permissions = value; RaisePropertyChanged(); } }  //PermissionsCollectionForView
        public ObservableCollection<DetailsView> UserPermissions { get { return userpermissions; } set { userpermissions = value; RaisePropertyChanged(); } }
        #endregion PUBLIC LISTBOX DATA

        #region Commands

        public ICommand RefreshTableWithSelectedPermissionUser => new AsyncCommand(async () =>
        {
            if(SelectedPermission == null)
            {
                NotificationService.ShowNotification("Щоб додати дозвіл треба обрати його", "Помилка");
                return;
            }
            try
            {
                UserPermissions = await GetUsersListByPermissionId(SelectedPermission.FirstName);
            }
            catch(Exception e)
            {
                NotificationService.ShowNotification("Помилка обробки інформації", "Помилка");
            }
        });

        /// <summary>
        /// Remove selected item with lastname in selectedUserPermission
        /// </summary>
        public ICommand RemoveSelectedPermissionCollectionItem => new AsyncCommand(async () =>
        {
            if (SelectedPermission == null)
            {
                NotificationService.ShowNotification("Щоб видалити викладача,треба ОБРАТИ ДОЗВІЛ", "Помилка");
                return;
            }
            if (SelecteduserInPermission == null)
            {
                NotificationService.ShowNotification("Щоб видалити дозвіл викладача,треба ОБРАТИ ВИКЛАДАЧА", "Помилка");
                return;
            }
            try
            {
                var permission = await service.PermissionService.GetBy(new Guid(SelectedPermission.FirstName));
                var user = await service.UserService.GetByLastName(SelecteduserInPermission.LastName);
                bool IsALready = permission.UsersWithPermissions.Any(x => x.LastName == user.LastName);
                if (IsALready)
                {
                    var collection = permission.UsersWithPermissions.ToList();
                    collection.Remove(user);
                    permission.UsersWithPermissions = collection;
                    await service.PermissionService.Update(permission);
                    var item = new DetailsView() { LastName = user.LastName };
                    UserPermissions.Remove(item);
                    RaisePropertyChanged(nameof(UserPermissions));
                }
            }
            catch (Exception e)
            {
                NotificationService.ShowNotification("Помилка обробки інформації", "Помилка");
            }
        });


        /// <summary>
        /// Add to selected permission
        /// </summary>
        public ICommand AddToSelectedPermissionCollectionItem => new AsyncCommand(async () =>
        {
            if (SelectedPermission == null)
            {
                NotificationService.ShowNotification("Щоб додати дозвіл викладачу, треба ОБРАТИ ДОЗВІЛ", "Помилка");
                return;
            }
            if (SelectedUser == null)
            {
                NotificationService.ShowNotification("Щоб додати дозвіл викладачу, треба ОБРАТИ ВИКЛАДАЧА", "Помилка");
                return;
            }
            try
            {
                var permission = await service.PermissionService.GetBy(new Guid(SelectedPermission.FirstName));
                var user = await service.UserService.GetByLastName(SelectedUser.LastName);
                bool IsALready = permission.UsersWithPermissions.Any(x => x.LastName == user.LastName);
                if (IsALready)
                {
                    NotificationService.ShowNotification("Такий викладач вже існує", "Помилка");
                    return;
                }
                else
                {
                    var collection = permission.UsersWithPermissions.ToList();
                    collection.Add(user);
                    permission.UsersWithPermissions = collection;
                    await service.PermissionService.Update(permission);
                    var item = new DetailsView() { LastName = user.LastName };
                    UserPermissions.Remove(item);
                    RaisePropertyChanged(nameof(UserPermissions));
                }
            }
            catch (Exception e)
            {
                NotificationService.ShowNotification("Помилка обробки інформації", "Помилка");
            }
        });

        /// <summary>
        /// Remove selected Permission
        /// </summary>
        public ICommand DeleteSelectedPermission => new AsyncCommand(async () =>
        {
            if (SelectedPermission == null)
            {
                NotificationService.ShowNotification("Щоб видалити спеціальний дозвіл, треба його обрати", "Помилка");
                return;
            }
            try
            {
                var key = await service.KeyService.GetByAuditoryName(SelectedPermission.KeyNumber);
                var permission = await service.PermissionService.GetByKey(key);
                if (permission == null)
                {
                    NotificationService.ShowNotification("Обранного доступу не існує в базі, оновіть списки", "Помилка");
                    return;
                }

                await service.PermissionService.Delete(permission);
            }
            catch (Exception e)
            {
                NotificationService.ShowNotification("Помилка обробки інформації", "Помилка");
            }


        });

        /// <summary>
        /// Add new PErmission via selected Key
        /// </summary>
        public ICommand AddNewPermission => new AsyncCommand(async () =>
        {
            if (SelectedKey == null)
            {
                NotificationService.ShowNotification("Щоб зробити ключу спеціальний дозвіл, треба його обрати", "Помилка");
                return;
            }
            try
            {
                var key = await service.KeyService.GetByAuditoryName(SelectedKey.KeyNumber);
                if (key == null)
                {
                    NotificationService.ShowNotification("Обранного ключа не існує в базі або він зараз у викладача, оновіть списки", "Помилка");
                    return;
                }

                var userCollection = new List<User>();
                var NewPermission = new Permission(key, userCollection);//addig empty and than can add selected user in AddUserToPermissionCommand
                NewPermission.Id = Guid.NewGuid();
                await service.PermissionService.Add(NewPermission);
            }
            catch (KeyAlreadyExist e)
            {
                NotificationService.ShowNotification("Для обранного ключа вже існує спеціальний доступ, оберіть його так змінюйте список Викладачів", "Помилка");
            }
            catch (Exception e)
            {
                NotificationService.ShowNotification("Помилка обробки інформації", "Помилка");
            }
        });


        /// <summary>
        /// Add new key via input data (NewKeyNumber)
        /// </summary>
        public ICommand AddNewKey => new AsyncCommand(async () =>
        {
            if (NewKeyNumber == null)
            {
                NotificationService.ShowNotification("напишіть новий номер для нового ключа", "Помилка");
                return;
            }

            try
            {
                await service.KeyService.Add(NewKeyNumber);
            }
            catch (KeyAlreadyExist e)
            {
                NotificationService.ShowNotification($"ключ з таким номером вже існує! {e.KeyNumber}", "Помилка");
            }
            catch (Exception)
            {
                NotificationService.ShowNotification("при оновленні ключа виникла помилка!", "Помилка");
            }
        });

        /// <summary>
        /// Update selected key
        /// </summary>
        public ICommand UpdateKey => new AsyncCommand(async () =>
        {
            if (SelectedKey == null)
            {
                NotificationService.ShowNotification("оберіть ключ із списку!", "Помилка");
                return;
            }
            if (NewKeyNumber == null)
            {
                NotificationService.ShowNotification("напишіть новий номер ключа", "Помилка");
                return;
            }
            try
            {
                var key = await service.KeyService.GetByAuditoryName(SelectedKey.KeyNumber);
                var checkKey = await service.KeyService.GetByAuditoryName(NewKeyNumber);
                if (checkKey != null)
                    throw new KeyAlreadyExist(SelectedKey.KeyNumber);
                key.AudNum = NewKeyNumber;
                await service.KeyService.Update(key);
            }
            catch (KeyAlreadyExist e)
            {
                NotificationService.ShowNotification($"ключ з таким номером вже існує! {e.KeyNumber}", "Помилка");
            }
            catch (Exception)
            {
                NotificationService.ShowNotification("при оновленні ключа виникла помилка!", "Помилка");
            }
        });



        /// <summary>
        /// Command wich add new User
        /// </summary>
        public ICommand AddNewUser => new AsyncCommand(async () =>
        {
            if (FirstName == null || MiddleName == null || LastName == null)
            {
                NotificationService.ShowNotification("у викладача не може не бути якогось ініціалу!", "Помилка");
                return;
            }

            var user = new User() { FirstName = FirstName, MiddleName = MiddleName, LastName = LastName, Id = Guid.NewGuid() };
            try
            {
                await service.UserService.Add(user);
            }
            catch (Exception)
            {
                NotificationService.ShowNotification("при додаванні викладача виникла помилка!", "Помилка");
            };
        });

        /// <summary>
        /// Command for removing Selected user
        /// </summary>
        public ICommand UpdateSelectedUser => new AsyncCommand(async () =>
        {
            if (SelectedUser == null)
            {
                NotificationService.ShowNotification("оберіть викладача із списку", "Помилка");

                return;
            }
            if (FirstName == null || MiddleName == null || LastName == null)
            {
                NotificationService.ShowNotification("у викладача не може не бути якогось ініціалу!", "Помилка");

                return;
            }
            try
            {
                var user = await service.UserService.GetByLastName(SelectedUser.LastName);
                user.FirstName = FirstName;
                user.MiddleName = MiddleName;
                user.LastName = LastName;
                await service.UserService.Update(user);
            }
            catch (Exception)
            {
                NotificationService.ShowNotification("при оновленні викладача виникла помилка!", "Помилка");
            };
        });


        public ICommand GetAllLists => new AsyncCommand(async () =>
        {
            var UserColl = (IEnumerable<User>)await service.GetAll<User>();
            Keys = (await service.GetAll(false)).MapToDetailsView();
            Users = UserColl.MapToDetailsView();
            Permissions = ((IEnumerable<Permission>)await service.GetAll<Permission>()).MapToDetailsView();
        });

        #endregion Commands
        private Task<ObservableCollection<DetailsView>> GetUsersListByPermissionId(string id)
        {
            var collUsers = Task.Run(async () => await service.GetUsersByPermissionId(new Guid(id)));
            var res = collUsers.ContinueWith(x => { return collUsers.Result.MapToDetailsView(); });
            return res;
        }
    }
}
