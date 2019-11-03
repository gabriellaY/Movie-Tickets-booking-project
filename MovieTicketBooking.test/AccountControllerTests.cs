using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieTicketBooking.Controllers;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services;
using Newtonsoft.Json;

namespace MovieTicketBooking.test
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void ChangePassword_Successfully_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToChangePassword();

            accountServiceMock.Setup(x => x.ChangePassword(user));

            var controller = new AccountController(accountServiceMock.Object);
            var actualResult = controller.ChangePassword(user);

            var okResult = (OkObjectResult) actualResult;

            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool) deserialized["Success"]);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ChangePassword_NullValue_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToChangePassword();

            accountServiceMock.Setup(x => x.ChangePassword(user))
                .Throws<ArgumentNullException>();
            var controller = new AccountController(accountServiceMock.Object);

            var actualResult = controller.ChangePassword(user);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ChangePassword_WrongPassword_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToChangePassword();

            accountServiceMock.Setup(x => x.ChangePassword(user))
                .Throws(new InvalidOperationException("Wrong password."));

            var controller = new AccountController(accountServiceMock.Object);

            var actualResult = controller.ChangePassword(user);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.AreEqual("Wrong password.", deserialized["Error"]);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ChangePassword_ConfirmedPasswordNotEqualToTheNewPassword_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToChangePassword();

            accountServiceMock.Setup(x => x.ChangePassword(user))
                .Throws(new InvalidOperationException("Confirmation password is not equal to the password."));

            var controller = new AccountController(accountServiceMock.Object);

            var actualResult = controller.ChangePassword(user);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.AreEqual("Confirmation password is not equal to the password.", deserialized["Error"]);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ChangePassword_InternalServerError_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToChangePassword();

            accountServiceMock.Setup(x => x.ChangePassword(user)).Throws<Exception>();

            var controller = new AccountController(accountServiceMock.Object);

            var actualResult = controller.ChangePassword(user);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ChangeEmail_Successfully_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToChangeEmail();

            accountServiceMock.Setup(x => x.ChangeEmail(user));

            var controller = new AccountController(accountServiceMock.Object);
            var actualResult = controller.ChangeEmail(user);

            var okResult = (OkObjectResult)actualResult;

            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void ChangeEmail_NullValue_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToChangeEmail();

            accountServiceMock.Setup(x => x.ChangeEmail(user))
                .Throws<ArgumentNullException>();
            var controller = new AccountController(accountServiceMock.Object);

            var actualResult = controller.ChangeEmail(user);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteAccount_Successfully_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToDelete();

            accountServiceMock.Setup(x => x.DeleteAccount(user));

            var controller = new AccountController(accountServiceMock.Object);
            var actualResult = controller.DeleteAccount(user);

            var okResult = (OkObjectResult)actualResult;

            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteAccount_NullValue_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToDelete();

            accountServiceMock.Setup(x => x.DeleteAccount(user))
                .Throws<ArgumentNullException>();

            var controller = new AccountController(accountServiceMock.Object);
            var actualResult = controller.DeleteAccount(user);

            var badRequestResult = (BadRequestObjectResult)actualResult;

            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteAccount_WrongPassword_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToDelete();

            accountServiceMock.Setup(x => x.DeleteAccount(user))
                .Throws(new InvalidOperationException("Wrong password."));

            var controller = new AccountController(accountServiceMock.Object);
            var actualResult = controller.DeleteAccount(user);

            var badRequestResult = (BadRequestObjectResult)actualResult;

            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.AreEqual("Wrong password.", deserialized["Error"]);

            accountServiceMock.VerifyAll();
        }

        [TestMethod]
        public void DeleteAccount_InternalServerError_Test()
        {
            var accountServiceMock = new Mock<IAccountService>();

            var user = new UserToDelete();

            accountServiceMock.Setup(x => x.DeleteAccount(user)).Throws<Exception>();

            var controller = new AccountController(accountServiceMock.Object);

            var actualResult = controller.DeleteAccount(user);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            accountServiceMock.VerifyAll();
        }
    }
}
