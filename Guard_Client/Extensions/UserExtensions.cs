using DTOs.Models;
using Guard_Client.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.Extensions
{
    public static class UserExtensions
    {
        public static DetailsView ConvertToDetailsViewWithImage(this User user)
        {
            if (user == null)
                user.Image = new Image();
            var ViewUser = new DetailsView()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                KeyNumber = user.Image.Path // using here KeyNumber prop for containig path to Image
            };
            return ViewUser;
        }
    }
}
