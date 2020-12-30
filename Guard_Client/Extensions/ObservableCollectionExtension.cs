using DTOs.Models;
using Guard_Client.DomainModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.Extensions
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<DetailsView> MapToDetailsView(this IEnumerable<KeyObject> keyObjects)
        {
            var mappedItem = new DetailsView();
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
            var mappedItem = new DetailsView();
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
    }
}
