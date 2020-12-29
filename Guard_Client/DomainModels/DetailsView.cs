using DevExpress.Mvvm;
using DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.DomainModels
{
    public class DetailsView : BindableBase
    {
        private string _KeyNumber;
        private string _LastName;
        private string _FirstName;
        private DateTime _DateTaking;
        private DateTime? _DateBringin;

        public string KeyNumber { get { return _KeyNumber; } set { _KeyNumber = value; RaisePropertyChanged(); } }
        public string LastName { get => _LastName; set { _LastName = value; RaisePropertyChanged(); } }
        public string FirstName { get => _FirstName; set { _FirstName = value; RaisePropertyChanged(); } }
        public DateTime DateTaking { get => _DateTaking; set { _DateTaking = value; RaisePropertyChanged(); } }
        public DateTime? DateBringin { get => _DateBringin; set { _DateBringin = value; RaisePropertyChanged(); } }
    }
}
