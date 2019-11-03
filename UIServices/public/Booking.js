let information = "";

function calculatePrice(type, ticketsCount) {

    const jwt = window.localStorage.getItem('jwt');

    $('#ticketTypes').empty();
    $('#priceBox').empty();

    return $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/ticketPrice/" + type,
        headers: {
            "Authorization": "Bearer " + jwt,
        }
    });
}

function setNumberOfTickets(timetableId) {

    const jwt = window.localStorage.getItem('jwt');

    $('#numberOfTicketsCol').empty();
    $('#bookButton').remove();

    const numberOfTickets = '<p id="numberOfTicketsTag">Number of tickets</p><select class="btn btn-secondary btn-sm" id="selectNumberDropdown"></select>';
    $('#numberOfTicketsCol').append(numberOfTickets);

    getAvailableTickets(timetableId);

    const bookButton = '<div id="bookButton" class="btn-group mr-5 float-right"><button type="button" class="btn btn-secondary" data-toggle="modal" data-target="#modal" onclick="selectInformationForBooking()">Book tickets</button></div>';
    $('#bookCol').append(bookButton);
}

function getAvailableTickets(timetableId) {

    const jwt = window.localStorage.getItem('jwt');

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/timetableTickets/" + timetableId,
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            for (var i = 0; i <= response.ticketsAvailable; i++) {

                const ticketsNumberOption = new Option(i, i);
                $(ticketsNumberOption).html(i);

                $('#selectNumberDropdown').append(ticketsNumberOption);
            }
            getTimetableId();
        },

        error: function (response) {

            $('#modalHeader').empty();
            $('#modalBody').empty();
            $('#modalButton').empty();

            $('#modalHeader').append("Error");
            $('#modalBody').append(response.statusText);
            $('#modalButton').append('Close');

            $('#modal').modal('show')
        }
    });
}

function getMovieTheaterName() {

    const jwt = window.localStorage.getItem('jwt');

    const cards = $('#accordion').children();

    for (let i = 0; i < cards.length; i++) {

        const cardChild = $('#' + cards[i].id + '');

        if (cardChild.children()[1].classList.contains('show')) {

            return cardChild.children()[0].innerText;
        }
    }
}

function getTimetableId() {

    const jwt = window.localStorage.getItem('jwt');

    const timetableBody = $('#timetableBodyId').children();

    for (let i = 0; i < timetableBody.length; i++) {

        const timetableRow = $('#' + timetableBody[i].id + '').children();
        const radioButtonColumn = timetableRow[3];

        const button = $('#' + radioButtonColumn.id + '').children()[0];

        if ($('#' + button.id + '').children()[0].checked) {

            return timetableBody[i].id;
        }
    }
}

function selectInformationForBooking() {

    $('#ticketTypes').empty();
    $('#priceBox').empty();

    const selectedCityIndex = $('#selectCityDropdown')[0].options.selectedIndex;
    const city = $('#selectCityDropdown')[0].options[selectedCityIndex].value;

    const movieTheater = getMovieTheaterName() + " " + city;

    const movieTitle = $('#timetableTitle')[0].innerText;

    const selectedMonthIndex = $('#selectMonthDropdown')[0].options.selectedIndex;
    let month = $('#selectMonthDropdown')[0].options[selectedMonthIndex].value;

    const selectedDayIndex = $('#selectDayDropdown')[0].options.selectedIndex;
    let day = $('#selectDayDropdown')[0].options[selectedDayIndex].value;

    const year = new Date();

    if (parseInt(day, 10) < 10) {

        day = "0" + day;
    }

    if (parseInt(month, 10) < 10) {

        month = "0" + month;
    }

    const date = day + "." + month + "." + year.getFullYear();

    const selectedTicketIndex = $('#selectTicketDropdown')[0].options.selectedIndex;
    const ticketType = $('#selectTicketDropdown')[0].options[selectedTicketIndex].value;

    const selectedIndex = $('#selectNumberDropdown')[0].options.selectedIndex;
    const numberOfTickets = $('#selectNumberDropdown')[0].options[selectedIndex].value;

    $.when(calculatePrice(ticketType, numberOfTickets)).done(function (response) {
        
        const timetableCols = $('#' + getTimetableId() + '').children();

        information = '<p>' + "Booked " + numberOfTickets + " tickets for " + '<b>' + date + '</b></p>'
            + '<p>' + "Movie theater: " + '<b>' + movieTheater + '</b></p>'
            + '<p>' + "Movie: " + '<b>' + movieTitle + '</b></p>'
            + '<p>' + "Starts at: " + '<b>' + timetableCols[0].innerText + '</b></p>'
            + '<p id="price" class="float-right">' + "Price: " + '<b>' + numberOfTickets * response.price + "lv." + '</b></p>';

        bookTickets();
    });
}

function sendEmail(information) {

    const email = $('#email')[0].innerText;

    Email.send({
        Host: "smtp.elasticemail.com",
        Username: "book.tickets.movies@gmail.com",
        Password: "996f106f-53a8-490e-91a7-2ef33620d07b",
        To: email,
        From: 'book.tickets.movies@gmail.com',
        Subject: "Booked movie tickets",
        Body: information
    });
}

function bookTickets() {

    const jwt = window.localStorage.getItem('jwt');

    $('#modalTitle').empty();
    $('#modalBody').empty();
    $('#modalButton').empty();
    $('#emailMsg').empty();

    const selectedIndex = $('#selectNumberDropdown')[0].options.selectedIndex;
    const numberOfTickets = $('#selectNumberDropdown')[0].options[selectedIndex].value;

    const user = parseJwt(jwt);

    $.ajax({
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify({
            "Username": user.unique_name
        }),

        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/bookTicket/" + getTimetableId() + "/" + numberOfTickets,
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            $('#modalTitle').append(response.message);

            $('#modalBody').append(information);
            $('#modalButton').append('Ok');
            $('#emailMsg').append('You will recieve an email with the information.');

            sendEmail(information);
        },

        error: function (response) {

            $('#modalHeader').empty();
            $('#modalBody').empty();
            $('#modalButton').empty();

            $('#modalHeader').append("Error");
            $('#modalBody').append(response.statusText);
            $('#modalButton').append('Close');

            $('#modal').modal('show')
        }
    });
}