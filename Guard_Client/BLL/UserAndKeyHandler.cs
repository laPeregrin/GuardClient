using DTOs.Models;
using DTOs.Services;
using Guard_Client.DomainModels;
using Guard_Client.Exceptions;
using Guard_Client.Services.Implementations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Guard_Client.BLL
{
    public class UserAndKeyHandler
    {
        private readonly KeyObjectService _keyService;
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;
        private readonly IBookingActionService _bookingAction;
        #region propForAdminPannel
        public KeyObjectService KeyService => _keyService;

        public UserService UserService => _userService;

        public PermissionService PermissionService => _permissionService;

        public IBookingActionService BookingAction => _bookingAction;
        #endregion propForAdminPannel
        public UserAndKeyHandler(KeyObjectService keyService, UserService userService, IBookingActionService bookingActionService, PermissionService permissionService)
        {
            _keyService = keyService;
            _userService = userService;
            _bookingAction = bookingActionService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// Get all( dexterity method)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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
                return await _bookingAction.GetAllFullValue();
            }
            else if (typeof(T) == typeof(Permission))
            {
                return await _permissionService.GetAllWithUserCollectionAndKey();
            }
            throw new ArgumentException();
        }
        /// <summary>
        /// get keys via filtering(show just booked or beside)
        /// </summary>
        /// <param name="visible"></param>
        /// <returns></returns>
        public async Task<IEnumerable<KeyObject>> GetAllKeys(bool visible = false)
        {
            return await _keyService.GetAll(x => x.IsBooked == visible);
        }
        /// <summary>
        /// Attention asNoTracking!!!
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BookingAction>> GetAllByRule(Expression<Func<BookingAction, bool>> expression)
        {
            return await _bookingAction.GetAllByRule(expression);
        }
        /// <summary>
        /// Get info about not finished bookings
        /// </summary>
        /// <param name="numberOfAuditoryOfKeys"></param>
        /// <returns></returns>
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
        /// <summary>
        /// GetAll permission via multyThreading
        /// </summary>
        public async Task<string[]> GetAllPermissionByUserLastName(string lastName)
        {
            if(string.IsNullOrEmpty(lastName))
                return new Queue<string>().ToArray();
            var container = new ConcurrentQueue<string> { };
            var taskList = new ConcurrentQueue<Task>();

            var user = await _userService.GetByLastName(lastName);
            var permissions =  await _permissionService.GetAllWithUserCollectionAndKey();
            if (permissions == null)
                return new Queue<string>().ToArray();
            foreach (var permission in permissions)
            {
                taskList.Enqueue(Task.Run(() =>
                {
                    foreach (var userIter in permission.UsersWithPermissions)
                    {
                        if (user.Id == userIter.Id)
                        {
                            container.Enqueue(permission.Key.AudNum);
                            break;
                        }
                    }
                }));
            }
            if (taskList.Any())
            {
              await Task.WhenAll(taskList);
                taskList.Clear();
            }
                return container.ToArray();
        }

        public async Task<User> GetUserWithImageByCardId(string CardId)
        {
            return await _userService.GetByCardId(CardId);
        }





        public async Task<IEnumerable<User>> GetUsersByPermissionId(Guid id)
        {
            return (await _permissionService.GetBy(id)).UsersWithPermissions;
        }






        public async Task AddBooking(string lastName, string auditoryNumber)
        {
            if (lastName == null || auditoryNumber == null)
                throw new ArgumentException();

            var user = await _userService.GetByLastName(lastName);
            var key = await _keyService.GetByAuditoryName(auditoryNumber);
            if (key.IsBooked == true)
                throw new KeyIsBookingAlreadyException(user.LastName);
            var permission = await _permissionService.GetByKey(key);
            if (permission != null)
            {
                if (_permissionService.IsHaveAcces(permission, user))
                {
                    await _bookingAction.StartSession(user, key);
                    return;
                }
                throw new NotHaveAccessException(user.LastName);
            }
            await _bookingAction.StartSession(user, key);
        }






        public async Task FinishBooking(string AudName)
        {
            var key = await _keyService.GetByAuditoryName(AudName);
            if (key.IsBooked == true)
            {
                var currentBooking = await _bookingAction.GetByRule(x => x.KeyObjectId == key.Id && x.UserId == key.UserId && x.BookingFinish == null);
                if (currentBooking == null)
                    throw new Exception();

                await _bookingAction.EndSession(currentBooking);
            }
            else
            {
                throw new KeyIsNotBooking(key.AudNum);
            }
        }
    }
}
