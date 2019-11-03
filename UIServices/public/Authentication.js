function register() {
    const username = $('#uname')[0].value;
    const email = $('#email')[0].value;
    const password = $('#password')[0].value;
    const confirmPassword = $('#cpassword')[0].value;

    const person = {
        "Username": username,
        "Email": email,
        "Password": password,
        "ConfirmPassword": confirmPassword
    }
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: "http://localhost:56827/api/Authentication/register",
        data: JSON.stringify(person),
        dataType: "json",

        success: function (response) {

            window.location.href = "index.html";
        },

        error: function (response) {
            $('#modalHeader').empty();
            $('#modalBody').empty();

            $('#modalHeader').append("Could not create an account.");
            if (!!response.responseJSON.error) {
                $('#modalBody').append(response.responseJSON.error);
            }
            else {
                $('#modalBody').append(response.responseJSON.errors.Email[0]);
            }
        }
    });

    document.getElementById('registerForm').reset();
}

function login() {
    const username = $('#uname')[0].value;
    const password = $('#password')[0].value;

    const person = {
        "Username": username,
        "Password": password,
    }

    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Authentication/login",
        data: JSON.stringify(person),
        dataType: "json",

        success: function (response) {

            window.localStorage.setItem('jwt', response.token);

            window.location.href = "LoggedIn.html";
        },

        error: function (response) {
            $('#modalHeader').empty();
            $('#modalBody').empty();

            $('#modalHeader').append("Could not log in.");
            $('#modalBody').append(response.responseJSON.error);
        }
    });

    $('#loginForm')[0].reset();
}

function logout() {

    window.localStorage.clear();
}