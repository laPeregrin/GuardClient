using DevExpress.Mvvm;
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

            var keyReader = new KeyReaderService();
            keyReader.serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPorter = sender as SerialPort;
            var buffer = new byte[serialPorter.BytesToRead];
            string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
    }
}
