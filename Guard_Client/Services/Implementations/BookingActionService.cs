using DTOs.Models;
using DTOs.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using testDAL;

namespace Guard_Client.Services.Implementations
{
    public class BookingActionService : IBookingActionService
    {
        private readonly DbTestContext _service;

        public BookingActionService(DbTestContext service)
        {
            _service = service;
        }
        /// <summary>
        /// ___Add Implicit Action___
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task Add(BookingAction obj)
        {
            if (!IsExist(obj))
            {
                await _service.BookingActions.AddAsync(obj);
                await _service.SaveChangesAsync();
            }
        }

        /// <summary>
        /// ___Add object with dateStart___
        /// </summary>
        /// <param name="bookingAction"></param>
        /// <returns></returns>
        public async Task StartSession(User user, KeyObject keyObject)
        {
            try
            {
                keyObject.IsBooked = true;
                keyObject.User = user;
                keyObject.UserId = user.Id;
                var res = new BookingAction().AddStartSessionBookingAction(user, keyObject, Guid.NewGuid());
                await Add(res);
                _service.KeyObjects.Update(keyObject);
                await _service.SaveChangesAsync();
            }
            catch(Exception e)
            {
                GC.Collect();
                throw;
            }
        }
        /// <summary>
        /// ___Add object with dateFinish for recording interaction with key in timeline
        /// </summary>
        /// <param name="bookingAction"></param>
        /// <returns></returns>
        public async Task EndSession(BookingAction bookingAction)
        {
            bookingAction.BookingFinish = DateTime.Now;
            await Task.Run(async () => await Update(bookingAction));
            bookingAction.KeyObject.IsBooked = false;
            bookingAction.KeyObject.User = null;
            bookingAction.KeyObject.UserId = null;
            await Task.Run(() => _service.KeyObjects.Update(bookingAction.KeyObject));
            await _service.SaveChangesAsync();
        }


        /// <summary>
        /// Method for getting collections
        /// all or some rule
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BookingAction>> GetAll()
        {
            return await _service.BookingActions.AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<BookingAction>> GetAll(Expression<Func<BookingAction, bool>> expression)
        {
            return await _service.Set<BookingAction>().Where(expression).AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<BookingAction>> GetAllByRule(Expression<Func<BookingAction, bool>> expression)
        {
            return await _service.BookingActions.Where(expression).Include(x => x.KeyObject).Include(x => x.User).AsNoTracking().ToArrayAsync();
        }
        /// <summary>
        /// Get all with including dtos object
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BookingAction>> GetAllFullValue()
        {
            return await _service.BookingActions
                .Include(x => x.KeyObject)
                .Include(x => x.User).ToListAsync();
        }
        //Get single element by expression
        public async Task<BookingAction> GetByRule(Expression<Func<BookingAction, bool>> expression)
        {
            return await _service.Set<BookingAction>().FirstOrDefaultAsync(expression);
        }

        public Task<BookingAction> GetBy(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Using for Add endDate to current session
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task Update(BookingAction obj)
        {
            if (IsExist(obj))
                _service.BookingActions.Update(obj);
            await _service.SaveChangesAsync();
        }

       

        private bool IsExist(BookingAction bookingAction)
        {
            return _service.BookingActions.Any(x => x.Id == bookingAction.Id);
        }

        public Task<bool> Delete(BookingAction obj)
        {
            throw new NotImplementedException();
        }
    }
}
