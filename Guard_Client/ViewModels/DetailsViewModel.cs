using DevExpress.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Guard_Client.Extensions;
using Guard_Client.DomainModels;
using Guard_Client.BLL;
using System.Windows.Input;
using System.Windows;
using Guard_Client.Exceptions;

namespace Guard_Client.ViewModels
{
    public class DetailsViewModel : BindableBase
    {
        private UserAndKeyHandler _userAndKeyHandler;


        private ObservableCollection<DetailsView> _BookedKeyCollections;
        public ObservableCollection<DetailsView> BookedKeyCollections { get { return _BookedKeyCollections; } set { _BookedKeyCollections = value; RaisePropertyChanged(); } }


        private DetailsView selectedKey;
        private DetailsView currentKey;
        public DetailsView? SelectedKey { get { return selectedKey; } set { selectedKey = value; RaisePropertyChanged(); } }
        public DetailsView? CurrentKey { get { return currentKey; } set { currentKey = value; RaisePropertyChanged(); } }


        public DetailsViewModel(UserAndKeyHandler handler)
        {
            _userAndKeyHandler = handler;

            Task.Run(async () =>
            {
                await UpdateGenerealCollection();

            }).GetAwaiter().GetResult();
        }

        public ICommand AddMoreKey => new AsyncCommand(async () =>
        {
            try
            {
                await _userAndKeyHandler.AddBooking(CurrentKey.LastName, CurrentKey.KeyNumber);
            }
            catch (KeyIsBookingAlreadyException e)
            {
                MessageBox.Show($"Ключ на данний момент зайнятий викладачем {e.Username}", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception)
            { MessageBox.Show("Можливо ви не обрали викладача або ключа, перевірте чи корректно ви обрали інформацію в обох списках", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error); }


        });
        public ICommand GetInfoBySelectedItemInList => new AsyncCommand(async () =>
        {
            if (SelectedKey != null)
            {
                CurrentKey = await _userAndKeyHandler.GetCurrentInfoByKeyAuditory(SelectedKey.KeyNumber);
            }
            else
            {
                MessageBox.Show("Для початку виберіть ключ із списку", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        });
        public ICommand ReturnKey => new AsyncCommand(async () =>
        {
            try
            {
                await _userAndKeyHandler.FinishBooking(CurrentKey.KeyNumber);
                UpdateList(SelectedKey);
            }
            catch (KeyIsNotBooking)
            { MessageBox.Show("Цим ключем ніхто не володіє на данний момент", "Увага!", MessageBoxButton.OK, MessageBoxImage.Warning); }
            catch (Exception)
            { MessageBox.Show("Для повернення обранного ключа варто отримати про нього повну інформацію", "Увага!", MessageBoxButton.OK, MessageBoxImage.Information); }

        });
        public ICommand UpdateAll => new AsyncCommand(async () =>
            {
                await UpdateGenerealCollection();
            });

        public async Task UpdateGenerealCollection()
        {
            var items = await _userAndKeyHandler.GetAll(true);
            BookedKeyCollections = items.MapToDetailsView();
        }

        private void UpdateList(DetailsView view)
        {
            BookedKeyCollections.Remove(view);
            CurrentKey = null;
        }

    }
}
