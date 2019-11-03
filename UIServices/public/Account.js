const todaysDate = new Date().toLocaleDateString();

function getUserEmail(username) {

    const jwt = window.localStorage.getItem('jwt');

    $('#email').empty();

    $.ajax({
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Authentication/email/" + username,
        headers: {
            "Authorization": "Bearer " + jwt,
        },
        dataType: "json",

        success: function (response) {

            $('#email').append(response.email);

            $('#editEmail')[0].placeholder = response.email;
        }
    });
}

function setEmailInEditForm() {

    const jwt = window.localStorage.getItem('jwt');
    const username = parseJwt(jwt).unique_name;

    getUserEmail(username);
}

function changePassword() {

    const jwt = window.localStorage.getItem('jwt');

    const username = parseJwt(jwt).unique_name;
    const password = $('#password')[0].value;
    const newPassword = $('#newPassword')[0].value;
    const confirmPassword = $('#confirmedPassword')[0].value;

    const user = {
        "Username": username,
        "OldPassword": password,
        "NewPassword": newPassword,
        "ConfirmNewPassword": confirmPassword
    }

    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Account/changePassword",
        data: JSON.stringify(user),
        dataType: "json",

        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            $('#modalTitle').empty();
            $('#modalBody').empty();

            $('#modalTitle').append("Successfully changed password.");
            $('#modalBody').append(response.message);
        },

        error: function (response) {

            $('#modalTitle').empty();
            $('#modalBody').empty();

            $('#modalTitle').append("Could not change password.");

            if (!!response.responseJSON.error) {
                $('#modalBody').append(response.responseJSON.error);
            }

            if (response.responseJSON.errors.NewPassword[0] !== "") {
                $('#modalBody').append(response.responseJSON.errors.NewPassword[0]);
                $('#modalBody').append('<p></p>');
            }
            if (response.responseJSON.errors.OldPassword[0] !== "") {
                $('#modalBody').append(response.responseJSON.errors.OldPassword[0]);
                $('#modalBody').append('<p></p>');
            }
            if (response.responseJSON.errors.ConfirmPassword[0] !== "") {
                $('#modalBody').append(response.responseJSON.errorsConfirmPassword[0]);
                $('#modalBody').append('<p></p>');
            }

        }
    });

    $('#editAccountForm')[0].reset();
}

function changeEmail() {

    const jwt = window.localStorage.getItem('jwt');

    const username = parseJwt(jwt).unique_name;
    const newEmail = $('#editEmail')[0].value;

    const user = {
        "Username": username,
        "NewEmail": newEmail
    };

    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Account/changeEmail",
        data: JSON.stringify(user),
        dataType: "json",

        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {

            $('#modalTitle').empty();
            $('#modalBody').empty();

            $('#modalTitle').append("Successfully changed email.");
            $('#modalBody').append(response.message);
        },

        error: function (response) {

            $('#modalTitle').empty();
            $('#modalBody').empty();

            $('#modalTitle').append("Could not change email.");
            $('#modalBody').append(response.responseJson.error);
        }
    });

    $('#editAccountForm')[0].reset();
}

function editAccount() {

    if ($('#editEmail')[0].value !== "" && $('#editEmail')[0].value !== $('#email')[0].innerText && $('#newPassword')[0].value === "") {
        changeEmail();
    }
    else if ($('#editEmail')[0].value !== "" && $('#editEmail')[0].value !== $('#email')[0].innerText && $('#newPassword').val() !== "") {
        changePassword();
        changeEmail();
    }
    else if ($('#editEmail')[0].value === "" && $('#newPassword').val() !== "") {
        changePassword();
    }
}

function deleteAccount() {

    const jwt = window.localStorage.getItem('jwt');

    const username = parseJwt(jwt).unique_name;
    const password = $('#password')[0].value;

    const user = {
        "Username": username,
        "Password": password
    }

    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: getUrl() + "Account/deleteAccount",
        data: JSON.stringify(user),
        dataType: "json",

        headers: {
            "Authorization": "Bearer " + jwt,
        },

        success: function (response) {
        
            $('#modalTitle').empty();
            $('#modalBody').empty();

            $('#modalTitle').append("Successfully deleted account.");
            $('#modalBody').append(response.message);

            window.location = "index.html";
           
            logout();            
        },

        error: function (response) {

            $('#modalTitle').empty();
            $('#modalBody').empty();

            $('#modalTitle').append("Could not delete account.");

            if (!!response.responseJSON.error) {

                $('#modalBody').append(response.responseJSON.error);
            }
            else {

                $('#modalBody').append(response.responseJSON.errors.Password[0]);
            }
        }
    });

    $('#editAccountForm')[0].reset();
}