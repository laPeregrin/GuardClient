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
        public DateTime CurrentDateMin { get { return min; } set { min = value; RaisePropertyChanged(); } }
        public DateTime CurrentDateMax { get { return max; } set { max = value; RaisePropertyChanged(); } }
        #endregion DataSorting
        #region StringSorting
        private string _filter = string.Empty;

        public string FilterString { get => _filter; set { _filter = value; RaisePropertyChanged(); HistoryCollection.Refresh(); } }
        #endregion StringSorting
        private readonly ObservableCollection<DetailsView> _HistoryDetailsViewModels;
        public ICollectionView HistoryCollection { get; }

        public HistoryViewModel(UserAndKeyHandler userAndKeyHandler)
        {
            service = userAndKeyHandler;
            _HistoryDetailsViewModels = new ObservableCollection<DetailsView>();
            HistoryCollection = CollectionViewSource.GetDefaultView(_HistoryDetailsViewModels);
            HistoryCollection.Filter = FilterUser;
            HistoryCollection.SortDescriptions.Add(new SortDescription(nameof(DetailsView.DateTaking), ListSortDirection.Descending));
        }


        #region Filters
        private bool FilterUser(object obj)
        {
            if (obj is DetailsView detailsView)
            {
                return detailsView.LastName.Contains(FilterString, StringComparison.InvariantCultureIgnoreCase) ||
                    detailsView.KeyNumber.Contains(FilterString, StringComparison.InvariantCultureIgnoreCase) ||
                    detailsView.FirstName.Contains(FilterString, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
        #endregion Filters
        private async Task<IEnumerable<BookingAction>> GetData()
        {
            return (IEnumerable<BookingAction>)await service.GetAll<BookingAction>();
        }
        public ICommand UpdateCollectionCommand => new DelegateCommand(() =>
        {
            UpdateCollection();
        });


        private void UpdateCollection()
        {
            var res = Task.Run(async () => await GetData()).Result;
            DetailsView details = new DetailsView();
            foreach (DetailsView item in res.MapToDetailsView())
            {
                _HistoryDetailsViewModels.Add(item);
            }

        }
    }
}
