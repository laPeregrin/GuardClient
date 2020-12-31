using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Models
{
    public class BookingAction : DomainObject
    {
        public BookingAction(Guid userId,
            User user,
            Guid keyId,
            KeyObject keyObject,
            DateTime bookingBegine,
            DateTime? bookingFinish,
            Guid id)
        {
            UserId = userId;
            User = user;
            KeyObjectId = keyId;
            KeyObject = keyObject;
            BookingBegine = bookingBegine;
            BookingFinish = bookingFinish;
            UserId = id;
        }

        public BookingAction()
        {

        }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid KeyObjectId { get; set; }
        public KeyObject KeyObject { get; set; }
        public DateTime BookingBegine { get; set; }
        public DateTime? BookingFinish { get; set; }

        public BookingAction AddStartSessionBookingAction(User user, KeyObject keyObject, Guid id)
        {
            return new BookingAction(user.Id, user, keyObject.Id, keyObject, DateTime.Now, null, id);
        }
        public BookingAction AddStartSessionBookingAction(BookingAction bookingAction)
        {
            return new BookingAction(bookingAction.UserId, bookingAction.User, bookingAction.KeyObjectId, bookingAction.KeyObject, bookingAction.BookingBegine, null, bookingAction.Id);
        }
    }
}
