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
    public class AuthenticationControllerTests
    {
        [TestMethod]
        public void Register_NullValue_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            authenticationServiceMock.Setup(x => x.Register(null))
                                                        .Throws<ArgumentNullException>();
            
            var controller = new AuthenticationController(authenticationServiceMock.Object);
            var actualResult = controller.Register(null);

            var badRequestResult = (BadRequestObjectResult) actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

             Assert.IsTrue((bool)deserialized["Success"] == false);

             authenticationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Register_InternalServerError_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var newUser = new UserForRegistrationDto();

            authenticationServiceMock.Setup(x => x.Register(newUser)).Throws<Exception>();

            var controller = new AuthenticationController(authenticationServiceMock.Object);

            var actualResult = controller.Register(newUser);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            authenticationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Register_SuccessfulRegistration_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var newUser = new UserForRegistrationDto();

            authenticationServiceMock.Setup(x => x.Register(newUser));

            var controller = new AuthenticationController(authenticationServiceMock.Object);

            var actualResult = controller.Register(newUser);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            authenticationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Register_PasswordAndConfirmationPasswordNotEqual_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var newUser = new UserForRegistrationDto();

            authenticationServiceMock.Setup(x => x.Register(newUser))
                                        .Throws(new InvalidOperationException("Confirmation password is not equal to the password."));

            var controller = new AuthenticationController(authenticationServiceMock.Object);

            var actualResult = controller.Register(newUser);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.AreEqual("Confirmation password is not equal to the password.", deserialized["Error"]);

            authenticationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Register_UsernameOrEmailNotUnique_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var newUser = new UserForRegistrationDto();

            authenticationServiceMock.Setup(x => x.Register(newUser))
                .Throws(new InvalidOperationException("User with this username or email already exists."));

            var controller = new AuthenticationController(authenticationServiceMock.Object);

            var actualResult = controller.Register(newUser);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.AreEqual("User with this username or email already exists.", deserialized["Error"]);

            authenticationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Login_SuccessfulLogin_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var user = new UserForLoginDto();

            authenticationServiceMock.Setup(x => x.Login(user));

            var controller = new AuthenticationController(authenticationServiceMock.Object);

            var actualResult = controller.Login(user);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            authenticationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Login_NullValue_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            authenticationServiceMock.Setup(x => x.Login(null))
                .Throws<ArgumentNullException>();

            var controller = new AuthenticationController(authenticationServiceMock.Object);
            var actualResult = controller.Login(null);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            authenticationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Login_InternalServerError_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var user = new UserForLoginDto();

            authenticationServiceMock.Setup(x => x.Login(user)).Throws<Exception>();

            var controller = new AuthenticationController(authenticationServiceMock.Object);

            var actualResult = controller.Login(user);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            authenticationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Login_InvalidUsername_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var user = new UserForLoginDto();

            authenticationServiceMock.Setup(x => x.Login(user))
                .Throws(new InvalidOperationException("Invalid username."));

            var controller = new AuthenticationController(authenticationServiceMock.Object);
            var actualResult = controller.Login(user);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.AreEqual("Invalid username.", deserialized["Error"]);

            authenticationServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Login_InvalidPassword_Test()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            var user = new UserForLoginDto();

            authenticationServiceMock.Setup(x => x.Login(user))
                .Throws(new InvalidOperationException("Invalid password."));

            var controller = new AuthenticationController(authenticationServiceMock.Object);
            var actualResult = controller.Login(user);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.AreEqual("Invalid password.", deserialized["Error"]);

            authenticationServiceMock.VerifyAll();
        }
    }
}
