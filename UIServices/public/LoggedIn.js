let page = 1;
const size = 5;
const date = new Date().toLocaleDateString();

function getMovies() {

    const jwt = window.localStorage.getItem('jwt');

    $('#mainContainer').empty();

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/movies/" + page + "/" + size,
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            const moviesTable = '<p id="moviesOnScreen" class="text-center">This week on screen</p>'
                + '<div id="moviesContainer" class="container"><table class="table table-condensed table-hover float-right rounded" id="movieTable">'
                + '<thead class="thead-dark rounded" id="tableThead"></thead><tbody></tbody></table></div>'
                + '<img id="clapperboarPicture" src="images/rsz_output-onlinepngtools.png">';

            $('#mainContainer').append(moviesTable);

            const pageNavigation = '<div id="paging" class="float-right" aria-label="Page navigation example"><ul class="pagination pagination-sm"><li class="page-item">'
                + '<a id="page" class="btn btn-sm" onclick="getPrevious()" href="#" aria-label="Previous"><span id="page" aria-hidden="true"><b>&laquo; Previous</b></span></a></li>'
                + '<li class="page-item"><a id="page" class="btn btn-sm" onclick="getNext()" href="#" aria-label="Next">'
                + '<span id="page" aria-hidden="true"><b>Next &raquo;</b></span></a></li></ul></div>';

            $('#moviesContainer').append(pageNavigation);

            const tableHeader = '<tr>' + '<th>' + 'Title' + '</th>' + '<th>' + 'Genre' + '</th>' + '<th>' + 'Producer' + '</th>' +
                '<th>' + 'Production' + '</th>' + '<th>' + 'Length(min)' + '</th>' + '<th>' + 'Summary' + '</th>' + '</tr>'

            $('#tableThead').append(tableHeader);

            for (var i = 0; i < response.movies.length; i++) {
                const movie = '<tr id="' + response.movies[i].id + '">' + '<td>' + response.movies[i].title + '</td>' + '<td>' + response.movies[i].genre + '</td>' + '<td>' + response.movies[i].producer +
                    '</td>' + '<td>' + response.movies[i].production + '</td>' + '<td>' + response.movies[i].length + '</td>' +
                    '<td><button id="summaryButton" class="button btn btn-rounded btn-secondary btn-sm m-0" data-toggle="modal" data-target="#modal" style="background-color: #F0F8FF" onclick="popUpSummary(\'' + response.movies[i].id + '\')" >' + 'Read summary' + '</button></td>' + '</tr>';

                $('#movieTable tbody').append(movie);
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

function getPagesCount() {

    const jwt = window.localStorage.getItem('jwt');

    debugger;
    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/pages/" + size,
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            if (page < response.pages) {

                page = page + 1;

                getMovies();
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

function getNext() {

    getPagesCount();
}

function getPrevious() {

    if (page > 1) {

        page = page - 1;

        getMovies();
    }
}

function popUpSummaryContent(title, summary, poster) {

    const jwt = window.localStorage.getItem('jwt');

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/moviePoster/" + poster,
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            const poster = '<img src="images/MoviePosters/' + response.poster + '"></img>';

            $('#modalTitle').append(title);
            $('#modalBody').append(summary);
            $('#modalBody').append(poster);
            $('#modalButton').append('Close');
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

function popUpSummary(rowId) {

    const jwt = window.localStorage.getItem('jwt');

    const row = document.getElementById(rowId);

    $('#modalTitle').empty();
    $('#modalBody').empty();
    $('#modalButton').empty();

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/summary/" + rowId,
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            const title = '<h2>' + row.cells[0].innerHTML.trim() + '</h2>';
            const summary = '<p>' + response.columns.summary + '</p>';

            popUpSummaryContent(title, summary, row.cells[0].innerHTML.trim());
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

function getCities() {

    const jwt = window.localStorage.getItem('jwt');

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Theater/cities",
        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {
            for (var i = 0; i < response.cities.length; i++) {

                var cityOption = new Option(response.cities[i].city, response.cities[i].city);
                $(cityOption).html(response.cities[i].city);

                $('#selectCityDropdown').append(cityOption);
            }
        },

        error: function (response) {

            $('#modalHeader').empty();
            $('#modalBody').empty();
            $('#modalButton').empty();

            $('#modalHeader').append("Error");
            $('#modalBody').append(response.statusText);
            $('#modalButton').append('Close');

            $('#modal').modal('show');
        }
    });
}

function start() {

    getCities();

    getMovies();
}