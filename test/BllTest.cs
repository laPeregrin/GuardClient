using Guard_Client.BLL;
using Guard_Client.Services.Implementations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testDAL;

namespace test
{
    class BllTest
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
        public async Task GetPermissionsByLastName_lastName_return()
        {
            var lastName = "Рач";
            var strings = await _userHandler.GetAllPermissionByUserLastName(lastName);
            Assert.IsTrue(strings.Count() > 0);
        }
    }
}
