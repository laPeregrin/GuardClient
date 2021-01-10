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
using System.Globalization;
using System.Windows;
using Guard_Client.Services;

namespace Guard_Client.ViewModels
{
    public class HistoryViewModel : BindableBase
    {
        private readonly UserAndKeyHandler service;

        private static CultureInfo cultureInfo;

        #region DataSorting
        private string filterDate = DateTime.Now.ToString("g", cultureInfo);
        public string FilterDate { get { return filterDate; } set { filterDate = value; RaisePropertyChanged(); } }
        #endregion DataSorting
        #region StringSorting
        private string _filter = string.Empty;

        public string FilterString { get => _filter; set { _filter = value; RaisePropertyChanged(); HistoryCollection.Refresh(); } }
        #endregion StringSorting
        private readonly ObservableCollection<DetailsView> _HistoryDetailsViewModels;
        public ICollectionView HistoryCollection { get; }

        public HistoryViewModel(UserAndKeyHandler userAndKeyHandler)
        {
            cultureInfo = new CultureInfo("uk-UA");
            service = userAndKeyHandler;

            _HistoryDetailsViewModels = new ObservableCollection<DetailsView>();

            HistoryCollection = CollectionViewSource.GetDefaultView(_HistoryDetailsViewModels);

            HistoryCollection.Filter = FilterUser;
            HistoryCollection.SortDescriptions.Add(new SortDescription(nameof(DetailsView.DateTaking), ListSortDirection.Descending));
        }



        public ICommand UpdateCollectionCommand => new DelegateCommand(() =>
        {
            UpdateCollection();
        });
        public ICommand UpdateByDate => new AsyncCommand(async () =>
        {
            await TakeDataByFilterDate();
        });


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



        #region HelpWorkersNoMVVMstyle((((
        private void UpdateCollection()
        {
            _HistoryDetailsViewModels.Clear();
            var res = Task.Run(async () => await GetData()).Result;
            foreach (DetailsView item in res.MapToDetailsView())
            {
                _HistoryDetailsViewModels.Add(item);
            }
        }
        private void UpdateCollection(IEnumerable<BookingAction> collection)
        {
            _HistoryDetailsViewModels.Clear();
            foreach (DetailsView item in collection.MapToDetailsView())
            {
                _HistoryDetailsViewModels.Add(item);
            }
        }
        private async Task TakeDataByFilterDate()
        {
            try
            {
                DateTime importantDate = DateTime.Parse(FilterDate, cultureInfo);
                var collection = await service.GetAllByRule(x => x.BookingFinish.Value.Hour == importantDate.Hour
                && x.BookingFinish.Value.Minute == importantDate.Minute);
                if (!collection.Any())
                {
                    collection = await service.GetAllByRule(x => x.BookingFinish.Value.Hour == importantDate.Hour);
                    if (!collection.Any())
                    {
                        collection = await service.GetAllByRule(x => x.BookingFinish.Value.Day == importantDate.Day);
                    }
                }
                UpdateCollection(collection);
            }
            catch (Exception e) 
            {
                NotificationService.ShowNotification("Некоректна інформація в полі для дат", "Увага!");
            }
        }
        private async Task<IEnumerable<BookingAction>> GetData()
        {
            return await service.GetAllByRule(x => DateTime.Equals(x.BookingBegine.Day, DateTime.Today.Day));
        }
        #endregion HelpWorkersNoMVVMstyle((((
    }
}
