using DevExpress.Mvvm;
using DTOs.Models;
using Guard_Client.BLL;
using Guard_Client.DomainModels;
using Guard_Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guard_Client.Extensions;
using Guard_Client.Exceptions;
using System.Windows.Input;

namespace Guard_Client.ViewModels
{
    public class CurrentViewModel : BindableBase
    {
        private readonly UserAndKeyHandler _service;


        #region Items for View
        private string _FName;
        private string _LName;
        private string _ImageSource = @"W:\programs\Images\defaultIcon.jpg";
        private string _Permissions;

        private DetailsView _BookedKey;
        private DetailsView _SelectedKeyInList;
        private DetailsView _SelectedKeyInBookedList;

        public ObservableCollection<DetailsView> KeyCollection { get; set; }
        public ObservableCollection<DetailsView> BookedKeyCollection { get; set; }

        public string FName { get { return _FName; } set { _FName = value; RaisePropertyChanged(); } }
        public string LName { get { return _LName; } set { _LName = value; RaisePropertyChanged(); } }
        public string ImageSource { get { return _ImageSource; } set { _ImageSource = value; RaisePropertyChanged(); } }
        public string Permissions { get { return _Permissions; } set { _Permissions = value; RaisePropertyChanged(); } }
        public DetailsView BookedKey { get => _BookedKey; set { _BookedKey = value; RaisePropertyChanged(); } }


        public DetailsView SelectedKeyInBookedList { get => _SelectedKeyInBookedList; set { _SelectedKeyInBookedList = value; } }
        public DetailsView SelectedKeyInList { get => _SelectedKeyInList; set { _SelectedKeyInList = value; } }
        #endregion Items for View

        public CurrentViewModel(UserAndKeyHandler service)
        {
            //here gonna be a listener of porter. And subscribing to serialPorterEvent
            _service = service;
            try
            {


                var KeyUser = Task.Run(async () => await _service.GetAllKeys()).Result;

                KeyCollection = new ObservableCollection<DetailsView>(KeyUser.MapToDetailsView().OrderBy(x => x.KeyNumber));
                SetUserByInfoToView("3");
                Permissions = GetPermByLastName(LName);
                GetBookedKeyByCurrentUser();
                var keyReader = new KeyReaderService();
                keyReader.serialPort.DataReceived += SerialPort_DataReceived;
            }
            catch (Exception) { }
        }

        private string GetPermByLastName(string lName)
        {
            if (string.IsNullOrEmpty(lName))
                return "Прикладіть картку до зчитувача";
            var col = Task.Run(async () => await _service.GetAllPermissionByUserLastName(_LName)).Result;
            if (col == null)
                return "Не має спеціального доступу";
            StringBuilder permission = new StringBuilder();
            for (int i = 0; i < col.Length; i++)
            {
                if (i == col.Length - 1)
                    permission.Append(col[i]).Append(". ");
                else
                    permission.Append(col[i]).Append(", ");
            }
            return permission.ToString();
        }





        #region Commands
        public ICommand AddBooking => new AsyncCommand(async () =>
        {
            try
            {
                await _service.AddBooking(LName, SelectedKeyInList.KeyNumber);
                UpdateCollection(SelectedKeyInList);
            }

            catch (NotHaveAccessException e)
            {
                NotificationService.ShowNotification($"Викладач {e.UserName} не має доступу до цього ключа", "Помилка");
            }
            catch (ArgumentException)
            {
                NotificationService.ShowNotification("Обов'язково обирайте ключ", "Помилка");
            }

            catch (KeyIsBookingAlreadyException e)
            {
                NotificationService.ShowNotification($"Ключ зараз у {e.Username}", "Попередження");
            }
            catch (Exception e)
            {

                NotificationService.ShowNotification("Помилка маршрутизації інформації", "Помилка");
            }

        });
        public ICommand FinishBooking => new AsyncCommand(async () =>
        {
            try
            {
                await _service.FinishBooking(SelectedKeyInBookedList.KeyNumber);
                UpdateCollection(SelectedKeyInBookedList);
            }
            catch (KeyIsNotBooking)
            { NotificationService.ShowNotification("Цим ключем ніхто не володіє на данний момент", "Увага!"); await Task.Delay(3000); }
            catch (NullReferenceException)
            { NotificationService.ShowNotification("Варто отримати про нього повну інформацію", "Увага!"); await Task.Delay(3000); }
            catch (Exception e) { }

        });

        #endregion Commands







        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPorter = sender as SerialPort;
            var buffer = new byte[serialPorter.BytesToRead];
            string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            SetUserByInfoToView(message);
        }

        private void SetUserByInfoToView(string messageId)
        {
            if (string.IsNullOrEmpty(messageId))
                return;
            User user = Task.Run(async () => await _service.GetUserWithImageByCardId(messageId)).Result;
            if (user == null)
                return;
            var UserDetails = user.ConvertToDetailsViewWithImage();
            FName = UserDetails.FirstName;
            LName = UserDetails.LastName;
            ImageSource = UserDetails.KeyNumber;//in this field for this method i contain path to image
        }

        private void GetBookedKeyByCurrentUser()
        {
            var key = Task.Run(async () => { return await _service.KeyService.GetAll(x => x.User.LastName == LName && x.IsBooked == true); }).Result;
            BookedKeyCollection = key.MapToDetailsView();
        }
        private void UpdateCollection(DetailsView details)
        {
            if (!KeyCollection.Remove(details))
                BookedKeyCollection.Remove(details);
        }
    }
}
