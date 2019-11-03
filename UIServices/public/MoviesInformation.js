function getTheatersByCity() {

    const jwt = window.localStorage.getItem('jwt');

    $('#mainContainer').empty();

    const selectedIndex = document.getElementById('selectCityDropdown').options.selectedIndex;
    const city = document.getElementById('selectCityDropdown').options[selectedIndex].value;

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/theaters/" + city,
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            const theaters = '<div id="bookingRow" class="row"><div id="timetableCol" class="col"</div><div id="theaterMoviesAndTimetable" class="container"><div class="row"><div id="theatersCol" class="col"><div id="accordion"></div></div>'
                + '<div id="timetables" class="col-md-8"><div id="movieTimetableCol"></div><table class="table table-condensed table-hover" id="timetable">'
                + '<thead class="thead" id="timetableThead"><tr></tr></thead><tbody id="timetableBodyId"></tbody></table>'
                + '<div id="categoriesDescription" class="border"></div></div></div></div></div></div>';

            $('#mainContainer').append(theaters);

            for (var i = 0; i < response.theaters.length; i++) {

                const collapseId = i * 2 + 1;
                let theatersByCity = "";

                if (i == 0) {
                    theatersByCity = '<div id="accordionCard' + i + '" class="card"><div class="card-header" id="' + i + '">'
                        + '<button id="#' + i + '" class="btn btn-secondary collapsed" style="background-color: #F0F8FF; color: black" data-toggle="collapse" data-target="#collapse' + collapseId + '" aria-expanded="true"'
                        + 'aria-controls="collapse' + collapseId + '"> ' + response.theaters[i] + ''
                        + '</button></div>'
                        + '<div id="collapse' + collapseId + '" class="collapse show" aria-labelledby="' + i + '" data-parent="#accordion"><div id="cardBody' + i + '" class="card-body" </div></div></div>';
                }
                else {
                    theatersByCity = '<div id="accordionCard" class="card"><div class="card-header" id="' + i + '">'
                        + '<button class="btn btn-secondary collapsed" style="background-color: #F0F8FF; color: black" data-toggle="collapse" data-target="#collapse' + collapseId + '" aria-expanded="false" aria-controls="collapse' + collapseId + '">'
                        + response.theaters[i]
                        + '</button></div><div id="collapse' + collapseId + '" class="collapse" aria-labelledby="' + i + '" data-parent="#accordion">'
                        + '<div id="cardBody' + i + '" class="card-body" </div></div></div>'
                }
                $('#accordion').append(theatersByCity);

                getMoviesByTheater(response.theaters[i], i);
            }

              const ticketsPicture = '<img id="deadpool" class="float-right" src="images/popcorn.png">';
              $('#mainContainer').append(ticketsPicture);
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

function getMoviesByTheater(theater, id) {

    const jwt = window.localStorage.getItem('jwt');

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/moviesInTheater/" + theater,
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {
            for (var i = 0; i < response.movies.length; i++) {

                const movie = '<ul class="list-group list-group-flush"><a id="title" role="button" onclick="getMovieTimetableByTitle(\'' + response.movies[i].title + '\')" href="#">' + response.movies[i].title + '</a>' + '<p id="genre" class="float-right">' + " -" + response.movies[i].genre + '</p></ul>';

                $('#cardBody' + id + '').append(movie);
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

function getMovieTimetableByTitle(title) {

    const jwt = window.localStorage.getItem('jwt');

    $('#movieTimetableCol').empty();
    $('#timetableThead').empty();
    $('#timetable tbody').empty();

    $.ajax({

        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/timetable/" + getMovieTheaterName() + "/" + title,
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {
            const heading = '<h1 id="timetableHeading">' + "Timetable" + '</h1>';
            const movieTitle = '<h2 id="timetableTitle">' + "\'" + title + "\'" + '</h2>';

            $('#movieTimetableCol').append(heading);
            $('#movieTimetableCol').append(movieTitle);

            const timetableHeader = '<tr>' + '<th>' + 'Starts at' + '</th>'
                + '<th>' + 'Type of projection' + '</th>'
                + '<th><a id="category" role="button" onclick="getMovieCategories()" href="#">' + 'Category' + '</a></th>'
                + '<th>' + "Want to watch" + '</th>' + '</tr>';

            $('#timetableThead').append(timetableHeader);

            for (var i = 0; i < response.timetable.length; i++) {

                const timetableBody = '<tr id="' + response.timetable[i].id + '">' + '<td>' + response.timetable[i].startsAt + '</td>'
                    + '<td>' + response.timetable[i].projectionType + '</td>'
                    + '<td>' + response.timetable[i].movieCategory + '</td>'
                    + '<td id="radioBtnCol' + i + '"><div id="radioBtn' + i + '"><input type="radio" onclick="setNumberOfTickets(\'' + response.timetable[i].id + '\')" name="watch" value="wantToWatch">'
                    + '<label for="radioButton"></label></div></td>' + '</tr>';

                $('#timetable tbody').append(timetableBody);
            }

            $('#bookCol').remove();

            const bookCol = '<div id="bookCol" class="col"></div>';
            $('#bookingRow').append(bookCol);

            listMonthOptions();
            listDayOptions();
            listTicketOptions();
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