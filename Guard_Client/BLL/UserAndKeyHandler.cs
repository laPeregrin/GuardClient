using DTOs.Models;
using DTOs.Services;
using Guard_Client.DomainModels;
using Guard_Client.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.BLL
{
    public class UserAndKeyHandler
    {
        private readonly KeyObjectService _keyService;
        private readonly UserService _userService;
        private readonly IBookingActionService _bookingAction;

        public UserAndKeyHandler(KeyObjectService keyService, UserService userService, IBookingActionService bookingActionService)
        {
            _keyService = keyService;
            _userService = userService;
            _bookingAction = bookingActionService;
        }

        public async Task<IEnumerable<DomainObject>> GetAll<T>() where T : DomainObject
        {
            if (typeof(T) == typeof(User))
            {
                return await _userService.GetAll();
            }
            else if (typeof(T) == typeof(KeyObject))
            {
                return await _keyService.GetAll();
            }
            else if (typeof(T) == typeof(BookingAction))
            {
                return await _bookingAction.GetAll();
            }
            throw new ArgumentException();
        }
        public async Task<IEnumerable<KeyObject>> GetAll(bool visible)
        {
            return await _keyService.GetAll(x => x.IsBooked == visible);
        }
        public async Task<DetailsView> GetCurrentInfoByKeyAuditory(string numberOfAuditoryOfKeys)
        {
            DetailsView model = null;
            if (numberOfAuditoryOfKeys != null && numberOfAuditoryOfKeys != "")
            {
                var currentKey = await _keyService.GetByAuditoryName(numberOfAuditoryOfKeys);
                var currentBooking = await _bookingAction.GetByRule(x => x.KeyObjectId == currentKey.Id && x.BookingFinish == null);
                var currentUser = await _userService.GetBy(currentBooking.UserId);
                model = new DetailsView()
                {
                    KeyNumber = numberOfAuditoryOfKeys,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    DateTaking = currentBooking.BookingBegine
                };
                return model;
            }
            return model;
        }

        public async Task AddBooking(string lastName, string auditoryNumber)
        {
            if (lastName == null || auditoryNumber == null)
                throw new ArgumentException();
            var user = await _userService.GetByLastName(lastName);
            var key = await _keyService.GetByAuditoryName(auditoryNumber);
            if (key.IsBooked == true)
                throw new FormatException("key alreadyBooked");
            var booking = new BookingAction(user.Id, user, key.Id, key, DateTime.Now, null, null);
            await _bookingAction.StartSession(booking);

        }



    }
}
