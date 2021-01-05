using DevExpress.Mvvm;
using Guard_Client.BLL;
using Guard_Client.DomainModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guard_Client.Extensions;
using System.Windows.Data;
using DTOs.Models;
using System.Windows.Input;

namespace Guard_Client.ViewModels
{
    public class HistoryViewModel : BindableBase
    {
        private readonly UserAndKeyHandler service;

        
        #region DataSorting
        private DateTime min;
        private DateTime max;
        public DateTime CurrentDateMin { get { return min; } set{ min = value;RaisePropertyChanged(); } }
        public DateTime CurrentDateMax { get { return max; } set { max = value; RaisePropertyChanged(); } }
        #endregion DataSorting
        #region StringSorting
        private string lastName;
        private string firstName;
        private string keyNumber;

        public string LastName { get => lastName; set { lastName = value; RaisePropertyChanged(); } }
        public string FirstName { get => firstName; set { firstName = value; RaisePropertyChanged(); } }
        public string KeyNumber { get => keyNumber; set { keyNumber = value; RaisePropertyChanged(); } }
        #endregion StringSorting
        private readonly ObservableCollection<DetailsView> _HistoryDetailsViewModels;
        public ICollectionView HistoryCollection { get; }

        public HistoryViewModel(UserAndKeyHandler userAndKeyHandler)
        {
            service = userAndKeyHandler;
            _HistoryDetailsViewModels = new ObservableCollection<DetailsView>();
            HistoryCollection = CollectionViewSource.GetDefaultView(_HistoryDetailsViewModels);
            
        }
        private void UpdateCollection()
        {
            var res = Task.Run(async () => await GetData()).Result;
            DetailsView details = new DetailsView();
                foreach(DetailsView item in res.MapToDetailsView())
                {
                    _HistoryDetailsViewModels.Add(item);
                }

        }
        private async Task<IEnumerable<BookingAction>> GetData()
        {
            return (IEnumerable<BookingAction>)await service.GetAll<BookingAction>();
        }
        public ICommand UpdateCollectionCommand => new DelegateCommand(()=>
        {
            UpdateCollection();
        });

        
    }
}
