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
    public class TheaterControllerTests
    {
        [TestMethod]
        public void GetTheatersByCity_Successful_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            string city = "";

            theaterServiceMock.Setup(x => x.GetTheatersByCity(city));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTheaters(city);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTheatersByCity_CityNullValue_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            string city = "";

            theaterServiceMock.Setup(x => x.GetTheatersByCity(city)).Throws<ArgumentNullException>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTheaters(city);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTheatersByCity_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            string city = "";

            theaterServiceMock.Setup(x => x.GetTheatersByCity(city)).Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTheaters(city);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovies_Successful_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            int page = new int();
            int size = new int();

            theaterServiceMock.Setup(x => x.GetMovies(page, size));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovies(page, size);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovies_SizeOrPageLessThanZero_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            int page = new int();
            int size = new int();

            theaterServiceMock.Setup(x => x.GetMovies(page, size)).Throws(new InvalidOperationException("Page and size should be positive numbers."));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovies(page, size);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.AreEqual("Page and size should be positive numbers.", deserialized["Error"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovies_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            int page = new int();
            int size = new int();

            theaterServiceMock.Setup(x => x.GetMovies(page, size)).Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovies(page, size);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovieCategories_Successful_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            theaterServiceMock.Setup(x => x.GetMovieCategories());

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovieCategories();

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovieCategories_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            theaterServiceMock.Setup(x => x.GetMovieCategories()).Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovieCategories();

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetCities_Successful_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            theaterServiceMock.Setup(x => x.GetCities());

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetCities();

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetCities_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            theaterServiceMock.Setup(x => x.GetCities()).Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetCities();

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMoviesByTheater_Successful_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            string theater = "";

            theaterServiceMock.Setup(x => x.GetMoviesByTheater(theater));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMoviesByTheater(theater);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMoviesByTheater_TheaterNullValue_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            string theater = "";

            theaterServiceMock.Setup(x => x.GetMoviesByTheater(theater))
                .Throws<ArgumentNullException>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMoviesByTheater(theater);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMoviesByTheater_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            string theater = "";

            theaterServiceMock.Setup(x => x.GetMoviesByTheater(theater)).Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMoviesByTheater(theater);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTimetableByMovie_Successful_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            string title = "";
            string movieTheater = "";

            theaterServiceMock.Setup(x => x.GetTimetableByMovie(movieTheater, title));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTimetableByMovie(movieTheater, title);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTimetableByMovie_NullValue_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            string title = "";
            string movieTheater = "";

            theaterServiceMock.Setup(x => x.GetTimetableByMovie(movieTheater, title)).Throws<ArgumentNullException>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTimetableByMovie(movieTheater, title);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTimetableByMovie_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            string title = "";
            string movieTheater = "";

            theaterServiceMock.Setup(x => x.GetTimetableByMovie(movieTheater, title)).Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTimetableByMovie(movieTheater, title);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void BookTicket_NullValue_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            Guid id = new Guid();
            UserBookTicketDto user = new UserBookTicketDto();
            int numberOfTickets = new int();

            theaterServiceMock.Setup(x => x.BookTicket(id, numberOfTickets, user)).Throws<ArgumentException>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.BookTicket(id, numberOfTickets, user);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void BookTicket_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            Guid id = new Guid();
            UserBookTicketDto user = new UserBookTicketDto();
            int numberOfTickets = new int();

            theaterServiceMock.Setup(x => x.BookTicket(id, numberOfTickets, user)).Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.BookTicket(id, numberOfTickets, user);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void BookTicket_Successful_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            Guid id = new Guid();
            UserBookTicketDto user = new UserBookTicketDto();
            int numberOfTickets = new int();

            theaterServiceMock.Setup(x => x.BookTicket(id, numberOfTickets, user));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.BookTicket(id, numberOfTickets, user);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovieTitleAndSummaryById_Successful_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            Guid id = new Guid();

            theaterServiceMock.Setup(x => x.GetMovieTitleAndSummaryById(id));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovieTitleAndSummaryById(id);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovieTitleAndSummaryById_NullValue_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            Guid id = new Guid();

            theaterServiceMock.Setup(x => x.GetMovieTitleAndSummaryById(id)).Throws<ArgumentException>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovieTitleAndSummaryById(id);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovieTitleAndSummaryById_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            
            Guid id = new Guid();

            theaterServiceMock.Setup(x => x.GetMovieTitleAndSummaryById(id)).Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovieTitleAndSummaryById(id);

            var result = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(result.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetPagesCount_Successfully_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            int size = new int();

            theaterServiceMock.Setup(x => x.GetPagesCount(size));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetPagesCount(size);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetPagesCount_SizeLessThanZero_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();
            
            int size = new int();

            theaterServiceMock.Setup(x => x.GetPagesCount(size)).Throws(new InvalidOperationException("Size should be positive numbers."));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetPagesCount(size);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.AreEqual("Size should be positive numbers.", deserialized["Error"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetPagesCount_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            int size = new int();

            theaterServiceMock.Setup(x => x.GetPagesCount(size)).Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetPagesCount(size);

            var badRequestResult = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMoviePosterName_Successfully_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            string title = "";

            theaterServiceMock.Setup(x => x.GetMoviePosterName(title));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMoviePosterName(title);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMoviePosterName_NullValueTitle_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            string title = "";

            theaterServiceMock.Setup(x => x.GetMoviePosterName(title))
                .Throws<ArgumentNullException>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMoviePosterName(title);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMoviePosterName_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            string title = "";

            theaterServiceMock.Setup(x => x.GetMoviePosterName(title))
                .Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMoviePosterName(title);

            var badRequestResult = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovieTicketTypes_Successfully_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            theaterServiceMock.Setup(x => x.GetMovieTicketTypes());

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovieTicketTypes();

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovieTicketTypes_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();


            theaterServiceMock.Setup(x => x.GetMovieTicketTypes())
                .Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetMovieTicketTypes();

            var badRequestResult = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTimetableAvailableTickets_Successfully_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            Guid timetableId = new Guid();

            theaterServiceMock.Setup(x => x.GetTimetableAvailableTickets(timetableId));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTimetableAvailableTickets(timetableId);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTimetableAvailableTickets_IdDefaultValue_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            Guid timetableId = new Guid();

            theaterServiceMock.Setup(x => x.GetTimetableAvailableTickets(timetableId))
                .Throws<ArgumentNullException>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTimetableAvailableTickets(timetableId);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTimetableAvailableTickets_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            Guid timetableId = new Guid();

            theaterServiceMock.Setup(x => x.GetTimetableAvailableTickets(timetableId))
                .Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTimetableAvailableTickets(timetableId);

            var badRequestResult = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTicketPriceByType_Successfully_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            string type = "";

            theaterServiceMock.Setup(x => x.GetTicketPriceByType(type));

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTicketPriceByType(type);

            var okResult = (OkObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(okResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"]);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTicketPriceByType_NullValueType_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            string type = "";

            theaterServiceMock.Setup(x => x.GetTicketPriceByType(type))
                .Throws<ArgumentNullException>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTicketPriceByType(type);

            var badRequestResult = (BadRequestObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetTicketPriceByType_InternalServerError_Test()
        {
            var theaterServiceMock = new Mock<ITheaterService>();

            string type = "";

            theaterServiceMock.Setup(x => x.GetTicketPriceByType(type))
                .Throws<Exception>();

            var controller = new TheaterController(theaterServiceMock.Object);
            var actualResult = controller.GetTicketPriceByType(type);

            var badRequestResult = (ObjectResult)actualResult;
            var asJson = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(asJson);

            Assert.IsTrue((bool)deserialized["Success"] == false);

            theaterServiceMock.VerifyAll();
        }
    }
}
