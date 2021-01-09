using DTOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Services
{
    public interface IBookingActionService : IDataService<BookingAction>
    {
        Task StartSession(User user, KeyObject keyObject);
        Task EndSession(BookingAction bookingAction);
        Task<BookingAction> GetByRule(Expression<Func<BookingAction, bool>> expression);
        Task<IEnumerable<BookingAction>> GetAllByRule(Expression<Func<BookingAction, bool>> expression);
        Task<IEnumerable<BookingAction>> GetAllFullValue();
    }
}
