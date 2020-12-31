﻿using DTOs.Models;
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
            var res = new BookingAction().AddStartSessionBookingAction(user, keyObject, Guid.NewGuid());
            await Task.Run(async () => await Add(res));
            keyObject.IsBooked = true;
            keyObject.User = user;
            keyObject.UserId = user.Id;
            var changeKey = keyObject;
            await Task.Run(() => _service.KeyObjects.Update(changeKey));
            await _service.SaveChangesAsync();

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
            var changeKey = bookingAction.KeyObject;
            await Task.Run(() => _service.KeyObjects.Update(changeKey));
            await _service.SaveChangesAsync();
        }

        public async Task<IEnumerable<BookingAction>> GetAll()
        {
            return await _service.BookingActions.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<BookingAction>> GetAll(Expression<Func<BookingAction, bool>> expression)
        {
            return await _service.Set<BookingAction>().Where(expression).AsNoTracking().ToListAsync();
        }

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
            if (_service.BookingActions.Any(x => x.Id == bookingAction.Id))
                return true;
            return false;
        }
    }
}
