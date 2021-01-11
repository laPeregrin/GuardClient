

using DevExpress.Mvvm;
using Guard_Client.BLL;
using Guard_Client.DomainModels;
using Guard_Client.Exceptions;
using Guard_Client.Extensions;
using Guard_Client.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Guard_Client.ViewModels
{
    public class DetailsViewModel : BindableBase
    {
        private UserAndKeyHandler _userAndKeyHandler;


        private ObservableCollection<DetailsView> _BookedKeyCollections;
        public ObservableCollection<DetailsView> BookedKeyCollections { get { return _BookedKeyCollections; } set { _BookedKeyCollections = value; } }


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
                NotificationService.ShowNotification($"Ключ на данний момент зайнятий викладачем {e.Username}", "Увага");
            }
            catch (Exception)
            { NotificationService.ShowNotification("Можливо ви не обрали викладача або ключа", "Помилка!"); }


        });
        public ICommand GetInfoBySelectedItemInList => new AsyncCommand(async () =>
        {
            if (SelectedKey != null)
            {
                CurrentKey = await _userAndKeyHandler.GetCurrentInfoByKeyAuditory(SelectedKey.KeyNumber);
            }
            else
            {
                NotificationService.ShowNotification("Для початку виберіть ключ із списку", "Помилка!");
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
            { NotificationService.ShowNotification("Цим ключем ніхто не володіє на данний момент", "Увага!"); }
            catch (NullReferenceException)
            { NotificationService.ShowNotification("Варто отримати про нього повну інформацію", "Увага!"); }
            catch (Exception e) { }

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
