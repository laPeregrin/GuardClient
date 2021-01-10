using DevExpress.Mvvm;
using DTOs.Models;
using Guard_Client.DomainModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.Extensions
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<DetailsView> MapToDetailsView(this IEnumerable<KeyObject> keyObjects)
        {
            DetailsView mappedItem;
            var collection = new ObservableCollection<DetailsView>();
            foreach(var item in keyObjects)
            {
                mappedItem = new DetailsView();
                mappedItem.KeyNumber = item.AudNum;
                // mappedItem.DateTaking = item.
                collection.Add(mappedItem);
            }
            return collection;
        }
        public static ObservableCollection<DetailsView> MapToDetailsView(this IEnumerable<User> keyObjects)
        {
            DetailsView mappedItem;
            var collection = new ObservableCollection<DetailsView>();
            foreach (var item in keyObjects)
            {
                mappedItem = new DetailsView();
                mappedItem.LastName = item.LastName;
                // mappedItem.DateTaking = item.
                collection.Add(mappedItem);
            }
            return collection;
        }
        public static ObservableCollection<DetailsView> MapToDetailsView(this IEnumerable<BookingAction> @object)
        {
            DetailsView mappedItem = new DetailsView();
            var collection = new ObservableCollection<DetailsView>();
            foreach (var item in @object.ToArray())
            {
                mappedItem = new DetailsView();
                mappedItem.FirstName = item.User.FirstName;
                mappedItem.LastName = item.User.LastName;
                mappedItem.KeyNumber = item.KeyObject.AudNum;
                mappedItem.DateTaking = item.BookingBegine;
                mappedItem.DateBringin = item.BookingFinish;
                collection.Add(mappedItem);
            }
            return collection;
        }
    }
}
