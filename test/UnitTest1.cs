using DTOs.Models;
using Guard_Client.BLL;
using Guard_Client.Services.Implementations;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using testDAL;

namespace test
{
    public class Tests
    {
        private DbTestContext _service;
        private UserService userService;
        private KeyObjectService keyObjectService;
        private BookingActionService bookingActionService;
        private UserAndKeyHandler _userHandler;
        [SetUp]
        public void Setup()
        {
            _service = new DbTestContext();
            userService = new UserService(_service);
            keyObjectService = new KeyObjectService(_service);
            bookingActionService = new BookingActionService(_service);
            _userHandler = new UserAndKeyHandler(keyObjectService, userService, bookingActionService);
        }


        [Test]
        public async Task AddUser_User_Success()
        {
            Guid id = Guid.NewGuid();
            //arrange
            var user = new User
            {
                Id = id,
                FirstName = "�����",
                MiddleName = "�����",
                LastName = "���������"
            };
            //Action
            await userService.Add(user);
            //Assert
            var exp = await userService.GetBy(id);
            Assert.AreEqual(id, exp.Id, "cant add user");
        }
        [Test]
        public async Task AdKey_User_Success()
        {
            Guid id = Guid.NewGuid();
            //arrange
            var key = new KeyObject
            {
                Id = id,
                AudNum = "017",
            };
            //Action
            await keyObjectService.Add(key);
            var exp = await keyObjectService.GetBy(id);
            //Assert
            Assert.AreEqual(id, exp.Id, "cant add key");
        }
        [Test]
        public async Task AddBookingSession_AddUserAndKey_ReturnSessionWithoutFinishTime()
        {
            //Arrange
            var user = userService.GetBy(new Guid("01d66862-ab84-4beb-98e8-78059307f19c")).Result;
            var key = keyObjectService.GetByAuditoryName("132").Result;
            //Action
            await bookingActionService.StartSession(user, key);
            var res = bookingActionService.GetAll().Result;
            bool succes = res.Count() != 0;
            //Assert
            Assert.IsTrue(succes);
        }
        [Test]
        public async Task GetAllWithNoTracking_ReturnKeyWithBookingTrue()
        {
            //Arrange
            User user = new User();
            //Action
            var collection = await keyObjectService.GetAll(x => x.IsBooked == true);
            user = collection.First().User;
            //Assert
            Assert.IsTrue(collection.Count() > 0);
        }

        [Test]
        public async Task GetAllWith_ReturnKeyWithBookingTrue()
        {
            //Arrange
            User user = new User();
            //Action
            var collection = await keyObjectService.GetAll();
            user = collection.FirstOrDefault(x => x.IsBooked == true).User;
            //Assert
            Assert.IsTrue(collection.Count() > 0);
        }
        [Test]
        public async Task GetBookingByAuditoryNumber_ReturnBooking()
        {
            //Arrange
            BookingAction ba = new BookingAction();
            //Action
            var key = keyObjectService.GetByAuditoryName("132").Result;
            var collection = await bookingActionService.GetAll(x => x.KeyObjectId == key.Id && x.BookingFinish == null);

            //Assert
            Assert.IsTrue(collection.Count() > 0);
        }
        [Test]
        public async Task GetBy_ReturnNull()
        {
            //Arrange

            //Action
            var user = await userService.GetBy(Guid.NewGuid());


            //Assert
            Assert.IsNull(user, "not null!");
        }
        [Test]
        public async Task GetByLastName_lastName_ReturnUser()
        {
            //Action
            var user = await userService.GetByLastName("���");
            //Assert
            Assert.AreEqual("���",user.LastName);
        }
        [Test]
        public async Task GetByAditory_auditory_ReturnKey()
        {
            //Action
            var key = await keyObjectService.GetByAuditoryName("132");
            //Assert
            Assert.AreEqual("132", key.AudNum);
        }
        [Test]
        public async Task GetAll__ReturnUsers()
        {
            //Action
            var users = await _userHandler.GetAll<User>();
            var user = users.First(); 
            //Assert
            Assert.AreEqual(typeof(User), user.GetType());
        }
        [Test]
        public async Task GetAll__ReturnKey()
        {
            //Action
            var users = await _userHandler.GetAll<KeyObject>();
            var user = users.First();
            //Assert
            Assert.AreEqual(typeof(KeyObject), user.GetType());
        }
        [Test]
        public async Task GetAll__ReturnBooking()
        {
            //Action
            var users = await _userHandler.GetAll<BookingAction>();
            var user = users.First();
            //Assert
            Assert.AreEqual(typeof(BookingAction), user.GetType());
        }
    }
}