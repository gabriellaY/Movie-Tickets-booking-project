function getMovieCategories() {

    const jwt = window.localStorage.getItem('jwt');

    $('#categoriesDescription').empty();

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/categories",
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            const categoriesBorder = document.getElementById('categoriesDescription');

            if (categoriesBorder.style.display === "none") {

                categoriesBorder.style.display = "block";
            }
            else {
                categoriesBorder.style.display = "none";
            }

            for (var i = 0; i < response.categories.length; i++) {

                const category = '<li>' + response.categories[i].type + " - " + response.categories[i].description + '</li>';

                $('#categoriesDescription').append(category);
            }
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

function listMonthOptions() {

    const jwt = window.localStorage.getItem('jwt');

    const selectMonth = '<p id="chooseTicketHeading" class="h2">Choose your ticket</p>' + '<div id="date" class="row"><div id="monthCol" class="col">'
        + '<p id="dateTag">Month</p><select class="btn btn-secondary btn-sm" id="selectMonthDropdown"></select></div>'
        + '<div id="dayCol" class="col"></div>'
        + '<div id="ticketTypeCol" class="col"></div>'
        + '<div id="numberOfTicketsCol" class="col"></div></div>';

    $('#bookCol').append(selectMonth);

    for (let i = 1; i <= 12; i++) {

        if (i < 10) {
            let monthOption = new Option(i);
        }
        let monthOption = new Option(i, i);
        $(monthOption).html(i);

        $('#selectMonthDropdown').append(monthOption);
    }
}

function listDayOptions() {

    const jwt = window.localStorage.getItem('jwt');

    const dayOption = '<p id="dateTag">Day</p><select class="btn btn-secondary btn-sm" id="selectDayDropdown"></select>';
    $('#dayCol').append(dayOption);

    for (let i = 1; i <= 31; i++) {

        let day = new Option(i, i);
        $(day).html(i);

        $('#selectDayDropdown').append(day);
    }
}

function listTicketOptions() {

    const jwt = window.localStorage.getItem('jwt');

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/tickets",
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            const selectTicketType = '<p><a id="ticketTag" role="button" onclick="getTicketTypes()" href="#">Type of ticket</a></p><select class="btn btn-secondary btn-sm" id="selectTicketDropdown"></select>';
            $('#ticketTypeCol').append(selectTicketType);

            for (var i = 0; i < response.types.length; i++) {

                var ticketOption = new Option(response.types[i].type, response.types[i].type);
                $(ticketOption).html(response.types[i].type);

                $('#selectTicketDropdown').append(ticketOption);
            }

            const ticketTypes = '<div id="ticketTypes" class="border"></div>'
            $('#ticketTypeCol').append(ticketTypes);
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

function getTicketTypes() {

    const jwt = window.localStorage.getItem('jwt');

    $('#ticketTypes').empty();

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/tickets",
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            const typesBorder = document.getElementById('ticketTypes');

            if (typesBorder.style.display === "none") {

                typesBorder.style.display = "block";
            }
            else {
                typesBorder.style.display = "none";
            }

            for (var i = 0; i < response.types.length; i++) {

                const type = '<li>' + response.types[i].type + " - " + response.types[i].price + 'lv.' + '</li>';

                $('#ticketTypes').append(type);
            }
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