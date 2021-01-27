using DTOs.Models;
using Guard_Client.BLL;
using Guard_Client.Exceptions;
using Guard_Client.Services.Implementations;
using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using testDAL;

namespace test
{
    public class Tests
    {
        private PermissionService _permService;
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
            _permService = new PermissionService(_service);
            _userHandler = new UserAndKeyHandler(keyObjectService, userService, bookingActionService, _permService);
        }


        [Test]
        public async Task AddUser_User_Success()
        {
            Guid id = Guid.NewGuid();
            //arrange
            var user = new User
            {
                Id = id,
                FirstName = "Михал",
                MiddleName = "Палыч",
                LastName = "Терентьев"
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
        public async Task AddBookingSession_AddUserAndKey_ReturnNoAccessException()
        {
            //Action
            try
            {
              await _userHandler.AddBooking("Сасім", "132-1");
            }
            catch(NotHaveAccessException e)
            {
                Assert.AreEqual("Сасім", e.UserName);
            }
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
            var user = await userService.GetByLastName("Рач");
            //Assert
            Assert.AreEqual("Рач",user.LastName);
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
        [Test]
        public async Task AddPermission_usersAndKey__()
        {
            //Arrange
            var user = await userService.GetByLastName("Рач");
            var user1 = await userService.GetByLastName("Хорошенюк");
            var key = await keyObjectService.GetByAuditoryName("132-1");
            var perm = new Permission(key, new User[] { user, user1 });
            //Action
            await _permService.Add(perm);
            var @explicit = await _permService.GetByKey(key);

            //Assert
            Assert.AreEqual(perm.UsersWithPermissions.Count(), @explicit.UsersWithPermissions.Count());
        }
        [Test]
        public void GetByKeyPermission_usersAndKey__()
        {
            var key = new KeyObject();
            key.Id = Guid.NewGuid();
            var result = _permService.GetByKey(key);

            Assert.IsNull(result);
        }
        [Test]
        public void DateTimeTest()
        {
            //Arrange 
            var date = DateTime.Now;
            var date1 = DateTime.UtcNow;
            var date2 = DateTime.Today;
            var date3 = date2.ToString("MM dd, yyyy");
            CultureInfo cultureInfo = new CultureInfo("uk-UA");
            DateTime tempDate = Convert.ToDateTime("1/1/2021 20:10:15", cultureInfo);
            var res = DateTime.Now.ToString(cultureInfo);
        }
        [Test]
        public async Task GetPermissionForSomeUser_user_returnStringWithKey()
        {
           var res = await _userHandler.GetAllPermissionByUserLastName("Рач");
            Assert.IsTrue(res.Any());
        }
        [Test]
        public async Task GetUsersByPermissionId_id_returnStringWithKey()
        {
           var res = await _userHandler.GetUsersByPermissionId(new Guid("480be0b1-03ac-49cf-9b77-f3431ddc7d5c"));
            Assert.IsTrue(res.Any());
        }

        [Test]
        public void TestText()
        {
            FileStream fs = new FileStream("Guard_Client//bin//Images//тест.txt", FileMode.Open);
            using(StreamWriter wr = new StreamWriter(fs))
            {
                wr.Write("TEST PATH");
            }
            using (StreamReader wr = new StreamReader(fs))
            {
               var res = wr.ReadToEnd();
            }
        }
    }
}