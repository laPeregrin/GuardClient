using DevExpress.Mvvm;
using Guard_Client.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Guard_Client.ViewModels
{
    public class GeneralViewModel : BindableBase
    {
        private readonly UserAndKeyHandler _bigService;

        public GeneralViewModel(UserAndKeyHandler bigService)
        {
            _bigService = bigService;
        }

        private string lastName;
        private string auditory;

        public string LastName { get => lastName; set { lastName = value; RaisePropertyChanged(); } }
        public string Auditory { get => auditory; set { auditory = value; RaisePropertyChanged(); }}


        public ICommand AddBooking => new AsyncCommand(async()=>
        {
            try
            {
                await _bigService.AddBooking(LastName, Auditory);
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
    }
}
