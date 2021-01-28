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

namespace Guard_Client.ViewModels
{
    public class CurrentViewModel : BindableBase
    {
        private readonly UserAndKeyHandler _service;


        #region Items for View
        private string _FName = "Піднесіть";
        private string _LName = "Картку";
        private string _ImageSource = @"W:\programs\Images\defaultIcon.jpg";


        public string FName { get { return _FName; } set { _FName = value; RaisePropertyChanged(); } }
        public string LName { get { return _LName; } set { _LName = value; RaisePropertyChanged(); } }
        public string ImageSource { get { return _ImageSource; } set { _ImageSource = value; RaisePropertyChanged(); } }



        public ObservableCollection<DetailsView> _keyCollection;

        private DetailsView SelectedKeyInList;
        //and add here stringMessage with all permission wich have current person
        public DetailsView SelectedKeyInList1 { get => SelectedKeyInList; set { SelectedKeyInList = value; RaisePropertyChanged(); } }
        #endregion Items for View

        public CurrentViewModel(UserAndKeyHandler service)
        {
            //here gonna be a listener of porter. And subscribing to serialPorterEvent
            _service = service;
            SetUserByInfoToView("3");
            var keyReader = new KeyReaderService();
            keyReader.serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPorter = sender as SerialPort;
            var buffer = new byte[serialPorter.BytesToRead];
            string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            SetUserByInfoToView(message);
        }

        private void SetUserByInfoToView(string messageId)
        {
            User user = Task.Run(async () => await _service.GetUserWithImageByCardId(messageId)).Result;
            var UserDetails = user.ConvertToDetailsViewWithImage();
            FName = UserDetails.FirstName;
            LName = UserDetails.LastName;
            ImageSource = UserDetails.KeyNumber;//in this field for this method i contain path to image
        }
    }
}
