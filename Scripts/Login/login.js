$(document).ready(function () {

    verifyToken();
    $('#loginForm').submit(function (e) {
        e.preventDefault(); 

        var rememberSessionValue = $('#remember-session').is(':checked') ? '1' : '0';
        $('[name="rememberSession"]').val(rememberSessionValue);

        var formData = $(this).serialize();
        ajaxCall("/Home/Authenticate", formData)
            .then(function (response) {
                response.status === true ? window.location.href = "/Home/inicio" : $("#validate").fadeIn();
            })
            .catch(function (xhr, status, error) {
                console.error("Error");
            });
    });

    $('#registerForm').submit(function (e) {
        e.preventDefault();
        var formData = $(this).serialize();
        ajaxCall("/Home/SetUsers", formData)
            .then(function (response) {
                console.log("Se guardaron Correctamente los Registros");
                window.location.href = "/Home/Login";
            })
            .catch(function (xhr, status, error) {
                console.error("Error");
            });
    });

});


function ajaxCall(urlMethod, formData) {
    return $.ajax({
        type: 'POST',
        url: urlMethod,
        data: formData,
    });
}

function verifyToken() {
    $.ajax({
        type: 'GET',
        url: '/Home/VerifyToken',
    }).then(function (response) {
        response.status === true ? window.location.href = "/Home/inicio" : null;
    })
    .catch(function (xhr, status, error) {
        console.error("Error");
    });
}

