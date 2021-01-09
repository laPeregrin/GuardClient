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
using System.Windows;
using System.Windows.Input;
using Guard_Client.Extensions;
using System.Windows.Controls;
using Guard_Client.Exceptions;

namespace Guard_Client.ViewModels
{
    public class GeneralViewModel : BindableBase
    {
        private readonly UserAndKeyHandler _bigService;

        public GeneralViewModel(UserAndKeyHandler bigService)
        {
            _bigService = bigService;

            var CollUser = (IEnumerable<User>)Task.Run(async () => await _bigService.GetAll<User>()).Result;
            users = new ObservableCollection<DetailsView>(CollUser.MapToDetailsView());
            var KeyUser = Task.Run(async () => await _bigService.GetAll(false)).Result;

            keys = new ObservableCollection<DetailsView>(KeyUser.MapToDetailsView());
        }
        public async Task UpdateCollection()
        {
            var KeyUser = await _bigService.GetAll(false);
            keys = new ObservableCollection<DetailsView>(KeyUser.MapToDetailsView());
            RaisePropertyChanged(nameof(keys));
        }

        private string lastName; //bind in textBox for searching in list of users
        private string auditory; //bind int textbox for searching a list of keys

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                CurrentUser = users.FirstOrDefault(x => x.LastName.StartsWith(LastName));
                RaisePropertyChanged();
            }
        }
        public string Auditory
        {
            get => auditory;
            set
            {
                auditory = value;
                CurrentKey = keys.FirstOrDefault(x => x.KeyNumber.StartsWith(Auditory));
                RaisePropertyChanged();
            }
        }

        private DetailsView _currentUser;
        public DetailsView _currentKey;
        public DetailsView CurrentUser { get { return _currentUser; } set { _currentUser = value; RaisePropertyChanged(); } }
        public DetailsView CurrentKey { get { return _currentKey; } set { _currentKey = value; RaisePropertyChanged(); } }

        public ObservableCollection<DetailsView> users { get; set; }
        public ObservableCollection<DetailsView> keys { get; set; }

        public ICommand AddBooking => new AsyncCommand(async () =>
        {
            try
            {
                await _bigService.AddBooking(CurrentUser.LastName, CurrentKey.KeyNumber);
                UpdateCollection(CurrentKey);
            }

            catch(NotHaveAccessException e)
            {
                MessageBox.Show($"Викладач {e.UserName} не має доступу до цього ключа", "Відмовлено у доступі", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Помилка маршрутизації інформації", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            catch (KeyIsBookingAlreadyException e)
            {
                MessageBox.Show($"Ключ зараз у {e.Username}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception e)
            {
                
                MessageBox.Show("Обязательно выбирайте нужное в обеих списках", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        });

        public ICommand UpdateKeys => new AsyncCommand(async () =>
        {
            await UpdateCollection();
        });


        #region ManipulationCollectionViaMethod
        private void UpdateCollection(DetailsView details)
        {
            keys.Remove(details);
        }
       
        #endregion
    }
}
