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

        private string lastName; //bind in textBox for searching in list of users
        private string auditory; //bind int textbox for searching a list of keys

        public string LastName
        {
            get => lastName;
            set
            {
                try
                {
                    lastName = value;
                    CurrentUser = users.FirstOrDefault(x => x.LastName.StartsWith(LastName));
                    RaisePropertyChanged();
                }
                catch (NullReferenceException e) { }
                catch (ArgumentException e) { }
            }
        }
        public string Auditory
        {
            get => auditory;
            set
            {
                try
                {
                    auditory = value;
                    CurrentKey = keys.FirstOrDefault(x => x.KeyNumber.StartsWith(Auditory));
                    RaisePropertyChanged();
                }
                catch (NullReferenceException e) { }
                catch (ArgumentException e) { }

            }
        }

        private DetailsView _currentUser;
        public DetailsView _currentKey;
        public DetailsView CurrentUser { get { return _currentUser; } set { _currentUser = value; RaisePropertyChanged(); } }
        public DetailsView CurrentKey { get { return _currentKey; } set { _currentKey = value; RaisePropertyChanged(); } }

        public ObservableCollection<DetailsView> users { get; set; }
        public ObservableCollection<DetailsView> keys { get; set; }

        public ICommand AddBooking => new DelegateCommand(() =>
        {
            try
            {
                Task.Run(async () => await _bigService.AddBooking(CurrentUser.LastName, CurrentKey.KeyNumber));
                UpdateCollection(CurrentKey);
            }
            catch (FormatException e)
            {
                MessageBox.Show("Ключ уже занят");
            }
            catch (Exception e)
            {
                MessageBox.Show("Проверьте правильно ли вы ввели информацию про преподователя и номер ключа");
            }
        });


        #region MoveToSelectedItemScrollEvent
        private void UpdateCollection(DetailsView details)
        {
            keys.Remove(details);
        }
        #endregion
    }
}
